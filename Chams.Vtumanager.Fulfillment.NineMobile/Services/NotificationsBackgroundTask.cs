using AutoMapper;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities;
using Chams.Vtumanager.Provisioning.Entities.BusinessAccount;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.Inventory;
using Chams.Vtumanager.Provisioning.Entities.Partner;
using Chams.Vtumanager.Provisioning.Entities.Subscription;
using Chams.Vtumanager.Provisioning.Hangfire.Services;
using Chams.Vtumanager.Provisioning.Services.GloTopup;
using Chams.Vtumanager.Provisioning.Services.Mtn;
using Chams.Vtumanager.Provisioning.Services.NineMobileEvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationsBackgroundTask
    {
        private readonly ILogger<NotificationsBackgroundTask> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IRepository<NotificationSettings> _notifSettingsRepo;
        private readonly IRepository<StockLevels> _stockLevelsRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<BusinessAccount> _partnerRepo;

        public NotificationsBackgroundTask(
            IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration,
            ILogger<NotificationsBackgroundTask> logger,
            IMailHelper mailHelper



            )
        {
            _logger = logger;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _notifSettingsRepo = unitOfWork.GetRepository<NotificationSettings>();
            _stockLevelsRepo = unitOfWork.GetRepository<StockLevels>();
            _userRepo = unitOfWork.GetRepository<User>();
            _partnerRepo = unitOfWork.GetRepository<BusinessAccount>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ProcessPendingRequests()
        {
            // we just print this message               
            //try
            //{
            int successCount = 0;
            string evctransId = string.Empty;
            InitSettings();

            var pendingJobs = await GetPendingNotifications();
            _logger.LogInformation($"Notification worker found {pendingJobs.Count()} pending partnes at " + DateTime.Now);
            var sb = new System.Text.StringBuilder(187);
            sb.AppendLine(@"Dear Partner, ");
            sb.AppendLine(@"<h3>Reorder Level Notification</h3>");
            sb.AppendLine(@"<p>");
            sb.AppendLine(@"You have reached 50% of your last purchase of {ITEMCODE} stock");
            sb.AppendLine(@"Current Stock Level for {ITEMCODE} is {STOCKLEVEL}, ");
            sb.AppendLine(@"Kindly restock your inventory quantity. ");
            sb.AppendLine(@"</p>");
            sb.AppendLine(@"Thank you.");
            sb.AppendLine(@"Chamsswitch");

            
            //var chamswitchMail = _configuration["SmtpSettings:V1:Username"];

            if (pendingJobs != null && pendingJobs.Count() > 0)
            {
                foreach (var item in pendingJobs)
                {
                    
                    
                    try
                    {
                        _logger.LogInformation($"Emailnotification message processing partnerId: {item.PartnerId}");
                        //get d stock level for the partner and compare
                        var stocklevels = getStockCurrentLevel(item.PartnerId);
                        var partnerEmail = GetPartnerDetails(item.PartnerId);
                        foreach (var partnerStock in stocklevels)
                        {
                            //check for 50% usage
                            //int level = 50;
                            
                            string serviceprovidername = Enum.GetName(typeof(ServiceProvider), partnerStock.ServiceProviderId);
                            
                            double checkLevel = ((double) item.NotifyPct/100) * partnerStock.LastOrder;

                            _logger.LogInformation($"Current stock level for {serviceprovidername} is {partnerStock.Qoh} last order:{partnerStock.LastOrder} : checklevel: {checkLevel}");
                            if (partnerStock.Qoh < checkLevel)
                            {
                                //compose message
                                _logger.LogInformation($"Low stock for {serviceprovidername}");
                                string subject = $"Reorder Level Notification for {serviceprovidername}";

                                string mailbody = sb.ToString();
                                mailbody.Replace("{ITEMCODE}", serviceprovidername);
                                mailbody.Replace("{STOCKLEVEL}", partnerStock.Qoh.ToString());
                                bool mailsent = _mailHelper.SendMail(subject, mailbody, partnerEmail.EmailAddress);
                                if (mailsent)
                                {
                                    UpdateNotificationStatus(item.PartnerId, item.NotifyPct);
                                }
                                _logger.LogInformation($"Emailnotification message for partnerId {item.PartnerId}: {serviceprovidername} sent");
                            }

                        }
                        successCount++;

                    }
                    catch (Exception ex)
                    {
                        
                        _logger.LogError($"Error processing notification message for partnerId {item.PartnerId}: {JsonConvert.SerializeObject(ex)}");
                    }

                }

            }

            _logger.LogInformation($"Notification background worker processed {successCount} successfully" + " at " + DateTime.Now);

        }

        private void InitSettings()
        {
            _logger.LogInformation($"Initializing Notification settings");
            var partners =   GetPartners();
            foreach (var item in partners)
            {
                if( !NotificationSettingsExist(item.PartnerId, 50))
                {
                    _logger.LogInformation($"Initializing Notification settings for partnerId {item.PartnerId}");
                    NotificationSettings notificationSettings = new NotificationSettings
                    {
                        IsProcessed = 0,
                        PartnerId = item.PartnerId,
                        ProcessedDate = DateTime.Now,
                        NotifyPct = 50
                    };
                    _notifSettingsRepo.Add(notificationSettings);
                    _notifSettingsRepo.SaveAsync();
                }
                //if (!NotificationSettingsExist(item.PartnerId, 75))
                //{
                //    NotificationSettings notificationSettings = new NotificationSettings
                //    {
                //        IsProcessed = 0,
                //        PartnerId = item.PartnerId,
                //        NotifyPct = 50
                //    };
                //    _notifSettingsRepo.Add(notificationSettings);
                //    _notifSettingsRepo.SaveAsync();
                //}
                //if (!NotificationSettingsExist(item.PartnerId, 90))
                //{
                //    NotificationSettings notificationSettings = new NotificationSettings
                //    {
                //        IsProcessed = 0,
                //        PartnerId = item.PartnerId,
                //        NotifyPct = 50
                //    };
                //    _notifSettingsRepo.Add(notificationSettings);
                //    _notifSettingsRepo.SaveAsync();
                //}
            }
        }
        private IEnumerable<BusinessAccount> GetPartners()
        {

            var data = _partnerRepo.GetQueryable().Where(a => a.PartnerCategory ==3).ToList();
            return data;
        }
        public bool NotificationSettingsExist(int partnerId, int notifypct)
        {
            
            var data = _notifSettingsRepo.GetQueryable()
                .Where(a => a.PartnerId == partnerId && a.NotifyPct == notifypct).Count()> 0;
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NotificationSettings>> GetPendingNotifications()
        {
            DateTime dt = DateTime.Today;

            //_evctranLogrepo
            var data = _notifSettingsRepo.GetQueryable()
                .Where(a => a.IsProcessed == 0 )
                .OrderBy(a => a.PartnerId).ToList();
            return data;
        }

        private IEnumerable<StockLevels> getStockCurrentLevel(int partnerId)
        {
            
            var data = _stockLevelsRepo.GetQueryable()
                .Where(a => a.PartnerId == partnerId).ToList();

            return data;
        }
        private User GetPartnerDetails(int partnerId)
        {

            var data = _userRepo.GetQueryable()
                .Where(a => a.PartnerId == partnerId).FirstOrDefault();

            return data;
        }
        private void UpdateNotificationStatus(int partnerId, int notify_pct)
        {

            var data = _notifSettingsRepo.GetQueryable()
                .Where(a => a.PartnerId == partnerId && a.NotifyPct == notify_pct).FirstOrDefault();

            data.IsProcessed = 1;
            _notifSettingsRepo.UpdateAsync(data);
            _notifSettingsRepo.SaveAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public async Task UpdateTaskStatusAsync(long taskId, string errorCode, string errorDesc)
        {
            //var _evctranLogrepo = unitOfWork.GetRepository<EvcTransactionLog>();
            var taskEntity = await _notifSettingsRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");

            taskEntity.ProcessedDate = DateTime.Now;
            taskEntity.IsProcessed = 1;
            await _notifSettingsRepo.UpdateAsync(taskEntity);

        }
        
    }
}
