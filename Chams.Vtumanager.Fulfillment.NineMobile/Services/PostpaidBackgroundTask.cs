using AutoMapper;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities.BusinessAccount;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.Subscription;
using Chams.Vtumanager.Provisioning.Hangfire.Services;
using Chams.Vtumanager.Provisioning.Services.GloTopup;
using Chams.Vtumanager.Provisioning.Services.Mtn;
using Chams.Vtumanager.Provisioning.Services.NineMobileEvc;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PostpaidBackgroundTask : IPostpaidSubscriptionTask
    {
        private readonly ILogger<PostpaidBackgroundTask> _logger;
        private readonly IConfiguration _configuration;

        private readonly IRepository<TopUpTransactionLog> _topupLogRepo;
        private readonly IRepository<Subscriber> _postpaidSubsRepo;
        private readonly ITransactionRecordService _transaactionRecordService;
        private readonly IRepository<PartnerServiceProvider> _partnerServicesRepo;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        /// <param name="_httpClientFactory"></param>
        /// <param name="transaactionRecordService"></param>
        public PostpaidBackgroundTask(
            IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration,
            ILogger<PostpaidBackgroundTask> logger,

            IConfiguration config,
            IHttpClientFactory _httpClientFactory,
            ITransactionRecordService transaactionRecordService

            )
        {
            _logger = logger;
            _configuration = configuration;
            //_evctranLogrepo = unitOfWork.GetRepository<EvcTransactionLog>();
            _topupLogRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _postpaidSubsRepo = unitOfWork.GetRepository<Subscriber>();
            _transaactionRecordService = transaactionRecordService;
            _partnerServicesRepo = unitOfWork.GetRepository<PartnerServiceProvider>();

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

            var pendingJobs = await GetPendingJobs();
            _logger.LogInformation($"Postpaid worker found {pendingJobs.Count()} pending postpaid requests at " + DateTime.Now);
            if (pendingJobs != null && pendingJobs.Count() > 0)
            {
                foreach (var item in pendingJobs)
                {
                    try
                    {
                        _logger.LogInformation($"Logging Postpaid transaction record : {JsonConvert.SerializeObject(item.Id)}");
                        string serviceprovidername = Enum.GetName(typeof(ServiceProvider), item.ServiceProviderId);
                        int channelId = (int)Channel.Web;
                        var partner = await _transaactionRecordService.GetPatnerById(item.PartnerId);
                        string partnerCode = partner.PartnerCode;
                        string transref = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

                        bool isdup = _transaactionRecordService.IsTransactionExist(transref, partner.PartnerId);
                        if (isdup)
                        {
                            continue;
                        }


                        int prepaidpayentsCategory = (int)ProductCategory.Prepaid;
                        var partnerService = _partnerServicesRepo.GetQueryable()
                        .Where(a => a.PartnerId == partner.PartnerId && a.ServiceProviderId == item.ServiceProviderId
                        && a.ProductCategoryid == prepaidpayentsCategory).FirstOrDefault();

                        //validate the number 
                        //int serviceproviderId = _transaactionRecordService.CarrierLookup(item.Phone);


                        decimal commission = 0;

                        if (partnerService != null)
                        {
                            _logger.LogInformation($"available prod categories for  partner : {partner.PartnerName} ");
                            commission = partnerService.CommissionPct == null ? 0 : (decimal)partnerService.CommissionPct;
                        }
                        decimal settlementAmt = commission == 0 ? item.CreditAmount : item.CreditAmount - (commission / 100 * item.CreditAmount);
                        TopUpTransactionLog topUpRequest = new TopUpTransactionLog
                        {
                            tran_date = DateTime.Now,
                            transamount = item.CreditAmount,
                            transref = transref,
                            transtype = item.RequestType,
                            channelid = channelId,
                            msisdn = item.Phone,
                            productid = item.ProductCode,
                            serviceproviderid = item.ServiceProviderId,
                            sourcesystem = partnerCode,
                            serviceprovidername = serviceprovidername,
                            PartnerId = item.PartnerId,
                            CountRetries = 0,
                            IsProcessed = 0,
                            TransactionStatus = 0,
                            SettlementAmount = settlementAmt,
                            ThreadNo = 1
                        };
                        var requestObkect = await _topupLogRepo.AddAsync(topUpRequest);
                        await _topupLogRepo.SaveAsync();
                        _logger.LogInformation($"Updating task postpaid record : {JsonConvert.SerializeObject(item.Id)}");
                        await UpdateTaskStatusAsync(item.Id, transref, "", "");
                        _logger.LogInformation($"Updated task postpaid record : {JsonConvert.SerializeObject(item.Id)}");

                    }
                    catch (Exception ex)
                    {
                        //await UpdateFailedTaskStatusAsync(item.Id, "99", ex.Message);
                        _logger.LogError($"Error handling postpaid requests {item.Id}: {ex}");
                    }

                }

            }

            


            _logger.LogInformation($"Postpaid background worker processed {successCount} successfully" + " at " + DateTime.Now);

        }
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public async Task<IEnumerable<Subscriber>> GetPendingJobs()
        {
            DateTime dt = DateTime.Today;
            var details = _postpaidSubsRepo.GetQueryable().Where(a => a.TransactionStatus == 0).ToList();
            
            return details;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public async Task UpdateTaskStatusAsync(long taskId, string transref,  string errorCode, string errorDesc)
        {

            var taskEntity = await _postpaidSubsRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");
            taskEntity.TransactionStatus = 1;
            taskEntity.TransRef = transref;
            taskEntity.LastUpdateDate = DateTime.Now;
            await _postpaidSubsRepo.UpdateAsync(taskEntity);

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public async Task UpdateFailedTaskStatusAsync(long taskId, string errorCode, string errorDesc)
        {

            var taskEntity = await _postpaidSubsRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");

            taskEntity.TransactionStatus = 1;
            taskEntity.ErrorCode = errorCode;
            taskEntity.ErrorDescription = errorDesc;
            taskEntity.LastUpdateDate = DateTime.Now;

            await _postpaidSubsRepo.UpdateAsync(taskEntity);

        }
    }
}
