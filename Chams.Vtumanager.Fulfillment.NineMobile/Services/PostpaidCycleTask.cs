using AutoMapper;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Hangfire.Services;
using Chams.Vtumanager.Provisioning.Services.GloTopup;
using Chams.Vtumanager.Provisioning.Services.Mtn;
using Chams.Vtumanager.Provisioning.Services.NineMobileEvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    public class PostpaidCycleTask
    {
        private readonly ILogger<FulfillmentBackgroundTask> _logger;
        private readonly IConfiguration _configuration;

        private readonly IRepository<TopUpTransactionLog> _topupLogRepo;
        private readonly ILightEvcService _evcService;
        private readonly IGloTopupService _gloTopupService;
        private readonly IAirtelPretupsService _airtelPreupsService;
        private readonly IMtnTopupService _mtnToupService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="evcService"></param>
        /// <param name="gloTopupService"></param>
        /// <param name="airtelPreupsService"></param>
        /// <param name="mtnToupService"></param>
        /// <param name="config"></param>
        /// <param name="_httpClientFactory"></param>
        public PostpaidCycleTask(
            IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration,
            ILogger<FulfillmentBackgroundTask> logger,
            ILightEvcService evcService,
            IGloTopupService gloTopupService,
            IAirtelPretupsService airtelPreupsService,
            IMtnTopupService mtnToupService,
            IConfiguration config,
            IHttpClientFactory _httpClientFactory
            )
        {
            _logger = logger;
            _configuration = configuration;
            //_evctranLogrepo = unitOfWork.GetRepository<EvcTransactionLog>();
            _topupLogRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _evcService = evcService;
            _gloTopupService = gloTopupService;
            _airtelPreupsService = airtelPreupsService;
            _mtnToupService = mtnToupService;
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
            _logger.LogInformation($"Postpaid worker found {pendingJobs.Count()} pending requests at " + DateTime.Now);
            if (pendingJobs != null && pendingJobs.Count() > 0)
            {
                foreach (var item in pendingJobs)
                {
                    PinlessRechargeRequest pinlessRechargeRequest = new PinlessRechargeRequest
                    {
                        Amount = item.transamount,
                        Msisdn = item.msisdn,
                        transId = item.transref,
                        ProductCode = item.productid
                    };

                    try
                    {



                    }
                    catch (Exception ex)
                    {
                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", ex.Message);
                        _logger.LogError($"Error handling message : {ex}");
                    }

                }

            }

            _logger.LogInformation($"Postpaid topup background worker processed {successCount} successfully" + " at " + DateTime.Now);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TopUpTransactionLog>> GetPendingJobs()
        {
            DateTime dt = DateTime.Today;

            //_evctranLogrepo
            var data = _topupLogRepo.GetQueryable()
                .Where(a => a.IsProcessed == 0 && a.CountRetries < 1)
                .OrderBy(a => a.tran_date).ToList();
            return data;
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
            var taskEntity = await _topupLogRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");

            taskEntity.ProcessedDate = DateTime.Now;
            taskEntity.ErrorCode = errorCode;
            taskEntity.ErrorDesc = errorDesc;
            taskEntity.IsProcessed = 1;
            await _topupLogRepo.UpdateAsync(taskEntity);

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

            var taskEntity = await _topupLogRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");

            taskEntity.ProcessedDate = DateTime.Now;
            taskEntity.ErrorCode = errorCode;
            taskEntity.ErrorDesc = errorDesc;
            taskEntity.CountRetries = taskEntity.CountRetries + 1;
            await _topupLogRepo.UpdateAsync(taskEntity);

        }
    }
}
