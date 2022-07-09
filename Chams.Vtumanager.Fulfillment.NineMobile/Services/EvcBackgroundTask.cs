using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesMgmt.Services.Evc.Worker.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Services.Models.Evc;
using Chams.Vtumanager.Provisioning.Entities.Common;

namespace Chams.Vtumanager.Provisioning.Hangfire.Services
{
    public class EvcBackgroundTask : IEvcBackgroundTask
    {
        private readonly ILogger<EvcBackgroundTask> _logger;
        private readonly IConfiguration _configuration;
        
        private readonly IRepository<TopUpTransactionLog> _topupLogRepo;
        private readonly ILightEvcService _evcService;

        public EvcBackgroundTask(
            IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration,
            ILogger<EvcBackgroundTask> logger,
            ILightEvcService evcService,
            IConfiguration config,
            IHttpClientFactory _httpClientFactory
            )
        {
            _logger = logger;
            _configuration = configuration;
            //_evctranLogrepo = unitOfWork.GetRepository<EvcTransactionLog>();
            _topupLogRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _evcService = evcService;
        }
        public async Task ProcessPendingRequests()
        {
            // we just print this message               
            //try
            //{
            int successCount = 0;
            long evctransId = 0;

            var pendingJobs = await GetPendingJobs();
            _logger.LogInformation($"worker found {pendingJobs.Count()} pending requests at " + DateTime.Now);
            if (pendingJobs != null && pendingJobs.Count() > 0)
            {
                foreach (var item in pendingJobs)
                {
                    try
                    {
                        PinlessRechargeRequest pinlessRechargeRequest = new PinlessRechargeRequest
                        {
                            Amount = item.transamount,
                            Msisdn = item.msisdn,
                            transId = item.transref
                        };
                        RechargeResponseEnvelope.Envelope env1 = new RechargeResponseEnvelope.Envelope();
                        env1 = await _evcService.PinlessRecharge(pinlessRechargeRequest);
                        if (env1.Body != null)
                        {
                            if (env1.Body.SDF_Data.result.statusCode == "0")
                            {
                                await UpdateTaskStatusAsync(item.RecordId,
                                    env1.Body.SDF_Data.result.statusCode.ToString(),
                                    env1.Body.SDF_Data.result.errorDescription
                                    );
                                evctransId = env1.Body.SDF_Data.result.instanceId;
                            }
                            else
                            {
                                await UpdateFailedTaskStatusAsync(item.RecordId,
                                    env1.Body.SDF_Data.result.statusCode.ToString(),
                                    env1.Body.SDF_Data.result.errorDescription
                                    );
                            }
                        }
                        else
                        {
                            await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                        }

                    }
                    catch (Exception ex)
                    {
                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", ex.Message);
                        _logger.LogError($"Error handling message : {ex}");
                    }

                }

            }

            _logger.LogInformation($"topup background worker processed {successCount} successfully" + " at " + DateTime.Now);

        }

        public async Task<IEnumerable<TopUpTransactionLog>> GetPendingJobs()
        {
            DateTime dt = DateTime.Today;

            //_evctranLogrepo
            var data = _topupLogRepo.GetQueryable()
                .Where(a => a.IsProcessed == 0 && a.CountRetries < 1)
                .OrderBy(a => a.tran_date).ToList();
            return data;
        }
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
