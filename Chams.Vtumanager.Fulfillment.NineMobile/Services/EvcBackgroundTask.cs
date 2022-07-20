using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
using Chams.Vtumanager.Provisioning.Entities.EtopUp.NineMobile;
using Chams.Vtumanager.Provisioning.Services.NineMobileEvc;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Glo;
using Chams.Vtumanager.Provisioning.Services.GloTopup;
using Chams.Vtumanager.Fulfillment.NineMobile.Services;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Pretups;
using Chams.Vtumanager.Provisioning.Services.AirtelPretups;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn;
using Chams.Vtumanager.Provisioning.Services.Mtn;

namespace Chams.Vtumanager.Provisioning.Hangfire.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class EvcBackgroundTask : IEvcBackgroundTask
    {
        private readonly ILogger<EvcBackgroundTask> _logger;
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
        public EvcBackgroundTask(
            IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration,
            ILogger<EvcBackgroundTask> logger,
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
            string evctransId  = string.Empty;

            var pendingJobs = await GetPendingJobs();
            _logger.LogInformation($"worker found {pendingJobs.Count()} pending requests at " + DateTime.Now);
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
                        switch (item.serviceproviderid)
                        {
                            case (int)ServiceProvider.Mtn:
                                if (item.transtype == 1)
                                {
                                    MtnResponseEnvelope.Envelope mtnenv1 = new MtnResponseEnvelope.Envelope();
                                    mtnenv1 = await _mtnToupService.AirtimeRecharge(pinlessRechargeRequest);
                                    if (mtnenv1.body != null)
                                    {
                                        if (mtnenv1.body.vendResponse.responseCode == 0 && mtnenv1.body.vendResponse.statusId == "0")
                                        {
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                mtnenv1.body.vendResponse.responseCode.ToString(),
                                                mtnenv1.body.vendResponse.responseMessage
                                                );
                                            evctransId = mtnenv1.body.vendResponse.txRefId.ToString();
                                        }
                                        else
                                        {
                                            await UpdateFailedTaskStatusAsync(item.RecordId,
                                                mtnenv1.body.vendResponse.responseCode.ToString(),
                                                mtnenv1.body.vendResponse.responseMessage
                                                );
                                        }
                                    }
                                    else
                                    {
                                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                    }
                                }
                                
                                break;
                            case (int)ServiceProvider.Airtel:
                                if (item.transtype == 1)
                                {
                                    PretupsRechargeResponseEnvelope.COMMAND rechargeResponseEnvelope = new PretupsRechargeResponseEnvelope.COMMAND();
                                    rechargeResponseEnvelope = await _airtelPreupsService.AirtimeRecharge(pinlessRechargeRequest);
                                    if (rechargeResponseEnvelope != null)
                                    {
                                        if (rechargeResponseEnvelope.TXNSTATUS == (int)PretupsErrorCodes.RequestSuccessfullyProcessed)
                                        {
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                rechargeResponseEnvelope.TXNSTATUS.ToString(),
                                                rechargeResponseEnvelope.MESSAGE
                                                );
                                            evctransId = rechargeResponseEnvelope.TXNID;
                                        }
                                        else
                                        {
                                            await UpdateFailedTaskStatusAsync(item.RecordId,
                                                rechargeResponseEnvelope.TXNSTATUS.ToString(),
                                                rechargeResponseEnvelope.MESSAGE
                                                );
                                        }
                                    }
                                    else
                                    {
                                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                    }
                                }
                                else
                                {
                                    PretupsRechargeResponseEnvelope.COMMAND rechargeResponseEnvelope1 = new PretupsRechargeResponseEnvelope.COMMAND();
                                    rechargeResponseEnvelope1 = await _airtelPreupsService.DataRecharge(pinlessRechargeRequest);
                                    if (rechargeResponseEnvelope1 != null)
                                    {
                                        if (rechargeResponseEnvelope1.TXNSTATUS == (int)PretupsErrorCodes.RequestSuccessfullyProcessed)
                                        {
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                rechargeResponseEnvelope1.TXNSTATUS.ToString(),
                                                rechargeResponseEnvelope1.MESSAGE
                                                );
                                            evctransId = rechargeResponseEnvelope1.MESSAGE;
                                        }
                                        else
                                        {
                                            await UpdateFailedTaskStatusAsync(item.RecordId,
                                                rechargeResponseEnvelope1.TXNSTATUS.ToString(),
                                                rechargeResponseEnvelope1.MESSAGE
                                                );
                                        }
                                    }
                                    else
                                    {
                                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                    }
                                }
                                break;
                            case (int)ServiceProvider.GLO:
                                if(item.transtype == 1)
                                {
                                    GloAirtimeResultEnvelope.Envelope gloAirtimeResultEnvelope = new GloAirtimeResultEnvelope.Envelope();
                                    gloAirtimeResultEnvelope = await _gloTopupService.GloAirtimeRecharge(pinlessRechargeRequest);
                                    if (gloAirtimeResultEnvelope.Body != null)
                                    {
                                        if (gloAirtimeResultEnvelope.Body.VendResponse.ResponseCode == 0 && gloAirtimeResultEnvelope.Body.VendResponse.StatusId == "00")
                                        {
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseCode.ToString(),
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseMessage
                                                );
                                            evctransId = gloAirtimeResultEnvelope.Body.VendResponse.TxRefId;
                                        }
                                        else
                                        {
                                            await UpdateFailedTaskStatusAsync(item.RecordId,
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseCode.ToString(),
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseMessage
                                                );
                                            evctransId = gloAirtimeResultEnvelope.Body.VendResponse.TxRefId;
                                        }
                                    }
                                    else
                                    {
                                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                    }
                                }
                                else
                                {
                                    GloDataResultEnvelope.Envelope gloDataResultEnvelope1 = new GloDataResultEnvelope.Envelope();
                                    gloDataResultEnvelope1 = await _gloTopupService.GloDataRecharge(pinlessRechargeRequest);
                                    if (gloDataResultEnvelope1.Body != null)
                                    {
                                        if (gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultCode == 0)
                                        {
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultCode.ToString(),
                                                gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultDescription
                                                );
                                            //evctransId = gloAirtimeResultEnvelope.Body.SDF_Data.result.instanceId;
                                        }
                                        else
                                        {
                                            await UpdateFailedTaskStatusAsync(item.RecordId,
                                                gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultCode.ToString(),
                                                gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultDescription
                                                );
                                        }
                                    }
                                    else
                                    {
                                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                    }
                                }
                                break;
                            case (int)ServiceProvider.NineMobile:
                                
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
                                        evctransId = env1.Body.SDF_Data.result.instanceId.ToString();
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
                                break;
                            default:
                                break;
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
