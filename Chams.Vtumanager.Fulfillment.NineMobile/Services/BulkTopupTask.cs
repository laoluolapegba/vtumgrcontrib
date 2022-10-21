using AutoMapper;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities.BusinessAccount;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.Partner;
using Chams.Vtumanager.Provisioning.Entities.Subscription;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
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
    public class BulkTopupTask : IBulkTopupTask
    {
        private readonly ILogger<BulkTopupTask> _logger;
        private readonly IConfiguration _configuration;

        private readonly IRepository<TopUpTransactionLog> _topupLogRepo;
        private readonly IRepository<DirectSalesDetail> _bulkrequestDetailsRepo;
        private readonly IRepository<DirectSalesMaster> _bulkrequestMasterRepo;
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
        public BulkTopupTask(
            IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration,
            ILogger<BulkTopupTask> logger,

            IConfiguration config,
            IHttpClientFactory _httpClientFactory,
            ITransactionRecordService transaactionRecordService

            )
        {
            _logger = logger;
            _configuration = configuration;
            //_evctranLogrepo = unitOfWork.GetRepository<EvcTransactionLog>();
            _topupLogRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _bulkrequestDetailsRepo = unitOfWork.GetRepository<DirectSalesDetail>();
            _bulkrequestMasterRepo = unitOfWork.GetRepository<DirectSalesMaster>();
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

            var master = _bulkrequestMasterRepo.GetQueryable().Where(a => a.IsApproved == 1 && a.IsProcessed == 0).ToList();
            foreach (var mst in master)
            {
                var pendingJobs = await GetPendingJobs(mst.Id);
                _logger.LogInformation($"BulkTopup Prepaid worker found {pendingJobs.Count()} pending  requests at " + DateTime.Now);
                try
                {
                    if (pendingJobs != null && pendingJobs.Count() > 0)
                    {
                        foreach (var item in pendingJobs)
                        {
                            try
                            {
                                

                                _logger.LogInformation($"Logging BulkTopup Prepaid transaction record : {JsonConvert.SerializeObject(item.Id)}");
                                string serviceprovidername = Enum.GetName(typeof(ServiceProvider), item.ServiceProviderId);
                                int channelId = (int)Channel.Web;
                                var partner = await _transaactionRecordService.GetPatnerById(mst.PartnerId);
                                string partnerCode = partner.PartnerCode;
                                //string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

                                _logger.LogInformation($"Checking for Duplicate transaction record : {item.TransRef} partmer: {partner.PartnerId}");

                                bool isdup = _transaactionRecordService.IsTransactionExist(item.TransRef, partner.PartnerId);
                                if(isdup)
                                {
                                    _logger.LogInformation($"{item.TransRef} already exists");
                                    continue;
                                }

                                _logger.LogInformation($"transaction record : {item.TransRef} not found");

                                //validate the number 
                                //int serviceproviderId = _transaactionRecordService.CarrierLookup(item.Msisdn);

                                int prepaidpayentsCategory = (int)ProductCategory.Prepaid;
                                var partnerService = _partnerServicesRepo.GetQueryable()
                                .Where(a => a.PartnerId == partner.PartnerId && a.ServiceProviderId == item.ServiceProviderId
                                && a.ProductCategoryid == prepaidpayentsCategory).FirstOrDefault();

                                decimal commission = 0;

                                if (partnerService != null)
                                {
                                    _logger.LogInformation($"available prod categories for  partner : {partner.PartnerName} ");
                                    commission = partnerService.CommissionPct == null ? 0 : (decimal)partnerService.CommissionPct;
                                }
                                decimal settlementAmt = commission == 0 ? item.Amount : item.Amount - (commission / 100 * item.Amount);

                                TopUpTransactionLog topUpRequest = new TopUpTransactionLog
                                {
                                    tran_date = DateTime.Now,
                                    transamount = item.Amount,
                                    transref = item.TransRef,
                                    transtype = item.RequestType,
                                    channelid = channelId,
                                    msisdn = item.Msisdn,
                                    productid = item.ProductCode,
                                    serviceproviderid = item.ServiceProviderId,
                                    sourcesystem = partnerCode,
                                    serviceprovidername = serviceprovidername,
                                    PartnerId = mst.PartnerId,
                                    CountRetries = 0,
                                    IsProcessed = 0,
                                    TransactionStatus = 0,
                                    SettlementAmount = settlementAmt,
                                    ThreadNo = 1
                                };
                                var requestObkect = await _topupLogRepo.AddAsync(topUpRequest);
                                await _topupLogRepo.SaveAsync();

                                _logger.LogInformation($"Updating task detail record : {JsonConvert.SerializeObject(item.Id)}");
                                await UpdateTaskStatusAsync(item.Id, "", "");
                                _logger.LogInformation($"Updated task detail record : {JsonConvert.SerializeObject(item.Id)}");

                            }
                            catch (Exception ex)
                            {

                                _logger.LogError($"Error processing BulkTopup Prepaid message direct_sales_record id:{item.Id}: {ex}");
                                continue;
                            }

                        }

                    }
                    _logger.LogInformation($"Updating task master record : {JsonConvert.SerializeObject(mst.Id)}");
                    await UpdateMasterTaskStatusAsync(mst.Id, "", "");
                    _logger.LogInformation($"Updated task master record : {JsonConvert.SerializeObject(mst.Id)}");
                }
                catch (Exception ex)
                {

                    _logger.LogError($"Error processing BulkTopup Prepaid message direct_sales id:  {mst.Id}: {ex}");
                    continue;
                }
               
            }


            _logger.LogInformation($"BulkTopup Prepaid background worker processed {successCount} successfully" + " at " + DateTime.Now);

        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DirectSalesDetail>> GetPendingJobs(long requestid)
        {
            DateTime dt = DateTime.Today;

            /*
            var sb = new System.Text.StringBuilder(179);
            sb.AppendLine(@"select b.id, a.partner_id, b.product_code, b.telco, b.phone, b.request_type, b.amount");
            sb.AppendLine(@"from  direct_sales a,  direct_sales_record b ");
            sb.AppendLine(@"where a.id = b.request_id");
            sb.AppendLine(@"and a.is_approved = 1");

            #pragma warning disable EF1000 // Possible SQL injection vulnerability.
            newMatches = await _matchRepo.FromSql(sql, parameters.ToArray()).ToListAsync();
            #pragma warning restore EF1000 // Possible SQL injection vulnerability.
            */


            var details = _bulkrequestDetailsRepo.GetQueryable().Where(a => a.request_id == requestid).ToList();
            //    .Join(_bulkrequestMasterRepo)
            return details;
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

            var taskEntity = await _bulkrequestDetailsRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");
            taskEntity.IsProcessed = 1;
            taskEntity.LastUpdateDate = DateTime.Now;
            await _bulkrequestDetailsRepo.UpdateAsync(taskEntity);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public async Task UpdateMasterTaskStatusAsync(long taskId, string errorCode, string errorDesc)
        {

            var taskEntity = await _bulkrequestMasterRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");
            taskEntity.IsProcessed = 1;
            taskEntity.LastUpdateDate = DateTime.Now;

            await _bulkrequestMasterRepo.UpdateAsync(taskEntity);

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

            var taskEntity = await _bulkrequestDetailsRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");

            taskEntity.IsProcessed = 1;
            await _bulkrequestDetailsRepo.UpdateAsync(taskEntity);

        }
    }
}
