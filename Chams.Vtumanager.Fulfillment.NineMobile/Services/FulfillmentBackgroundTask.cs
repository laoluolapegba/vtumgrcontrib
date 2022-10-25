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
using Chams.Vtumanager.Provisioning.Entities.Inventory;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;

namespace Chams.Vtumanager.Provisioning.Hangfire.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class FulfillmentBackgroundTask : IFulfillmentBackgroundTask
    {
        private readonly ILogger<FulfillmentBackgroundTask> _logger;
        private readonly IConfiguration _configuration;
        
        private readonly IRepository<TopUpTransactionLog> _topupLogRepo;
        private readonly IRepository<StockMaster> _stockmaster;
        private readonly ILightEvcService _evcService;
        private readonly IGloTopupService _gloTopupService;
        private readonly IAirtelPretupsService _airtelPreupsService;
        private readonly IMtnTopupService _mtnToupService;
        private readonly ITransactionRecordService _transactionRecordingService;
        private readonly IConfiguration _config;

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
        public FulfillmentBackgroundTask(
            IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration,
            ILogger<FulfillmentBackgroundTask> logger,
            ILightEvcService evcService,
            IGloTopupService gloTopupService,
            IAirtelPretupsService airtelPreupsService,
            IMtnTopupService mtnToupService,
            IConfiguration config,
            IHttpClientFactory _httpClientFactory,
            ITransactionRecordService transactionRecoringService
            
            )
        {
            _logger = logger;
            _configuration = configuration;
            //_evctranLogrepo = unitOfWork.GetRepository<EvcTransactionLog>();
            _topupLogRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _stockmaster = unitOfWork.GetRepository<StockMaster>();
            _evcService = evcService;
            _gloTopupService = gloTopupService;
            _airtelPreupsService = airtelPreupsService;
            _mtnToupService = mtnToupService;
            _transactionRecordingService = transactionRecoringService;
            _config = config;
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
            int _threadNo = int.Parse(_config["ThreadNo"]);
            int mtnVersion = int.Parse(_config["MtnTopupSettings:Version"]);
            string externaltransref  = string.Empty;

            var pendingJobs = await GetPendingJobs(_threadNo);
            _logger.LogInformation($"Fulfillment worker found {pendingJobs.Count()} pending requests at " + DateTime.Now);
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
                    bool isSuccess = false;
                    ///Check balance

                    _logger.LogInformation($"Checking current stock balance for partner:{item.PartnerId} telco: {item.serviceproviderid}");

                    int balance = 0;
                    var stockdata = await _transactionRecordingService.GetPartnerStockBalance(item.PartnerId, item.serviceproviderid);

                    if (stockdata != null)
                    {
                        balance = stockdata.QuantityOnHand;
                    }
                    _logger.LogInformation($"Current stock balance for partner:{item.PartnerId} telco: {item.serviceproviderid} is {balance}");

                    if (balance < item.transamount)
                    {
                        _logger.LogInformation($"Not sufficient stock balance for partner:{item.PartnerId} telco: {item.serviceproviderid} is {balance}");
                        string errorMessage = $"Insufficient {item.serviceprovidername} Stock Balance for {item.sourcesystem} ";
                        await UpdateTaskStatusAsync(item.RecordId, "20014", errorMessage, "0");
                        continue;

                    }
                    ///end check balance
                    try
                    {
                        _logger.LogInformation($"Start processing request {pinlessRechargeRequest.transId} ");
                        switch (item.serviceproviderid)
                        {
                            case (int)ServiceProvider.MTN:
                                if(mtnVersion == 1)
                                {
                                    if (item.transtype == 1)
                                    {
                                        MtnResponseEnvelope.Envelope mtnenv1 = new MtnResponseEnvelope.Envelope();
                                        mtnenv1 = await _mtnToupService.AirtimeRecharge(pinlessRechargeRequest);
                                        if (mtnenv1.body != null)
                                        {
                                            if (mtnenv1.body.vendResponse.responseCode == 0 && mtnenv1.body.vendResponse.statusId == "0")
                                            {
                                                isSuccess = true;
                                                externaltransref = mtnenv1.body.vendResponse.txRefId;
                                                await UpdateTaskStatusAsync(item.RecordId,
                                                    mtnenv1.body.vendResponse.responseCode.ToString(),
                                                    mtnenv1.body.vendResponse.responseMessage,
                                                    externaltransref
                                                    );
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
                                            _logger.LogInformation($"Mtn airtime Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
                                            await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                        }

                                    }
                                    else
                                    {
                                        MtnResponseEnvelope.Envelope mtnenv1 = new MtnResponseEnvelope.Envelope();
                                        mtnenv1 = await _mtnToupService.AirtimeRecharge(pinlessRechargeRequest);
                                        if (mtnenv1.body != null)
                                        {
                                            if (mtnenv1.body.vendResponse.responseCode == 0 && mtnenv1.body.vendResponse.statusId == "0")
                                            {
                                                isSuccess = true;
                                                externaltransref = mtnenv1.body.vendResponse.txRefId;
                                                await UpdateTaskStatusAsync(item.RecordId,
                                                    mtnenv1.body.vendResponse.responseCode.ToString(),
                                                    mtnenv1.body.vendResponse.responseMessage,
                                                    externaltransref
                                                    );
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
                                            _logger.LogInformation($"Mtn Data Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
                                            await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                        }
                                    }
                                }
                                else
                                {
                                    MtnSubscriptionResponse mtnresult = new MtnSubscriptionResponse();
                                    mtnresult = await _mtnToupService.MtnSubscription(pinlessRechargeRequest);
                                    if (mtnresult != null)
                                    {
                                        if (mtnresult.statusCode == "0000" && mtnresult.statusMessage == "Successful")
                                        {
                                            externaltransref = mtnresult.subscriptionDescription;
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                mtnresult.statusCode,
                                                mtnresult.statusMessage,
                                                externaltransref
                                                );

                                        }
                                        else
                                        {
                                            await UpdateFailedTaskStatusAsync(item.RecordId,
                                                mtnresult.status.ToString(),
                                                mtnresult.message
                                                );
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogInformation($"Mtn version 3 Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
                                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                    }
                                }
                                
                                
                                /*

                                
                                */

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
                                            isSuccess = true;
                                            externaltransref = rechargeResponseEnvelope.TXNID;
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                rechargeResponseEnvelope.TXNSTATUS.ToString(),
                                                rechargeResponseEnvelope.MESSAGE,
                                                externaltransref
                                                );
                                            
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
                                        _logger.LogInformation($"Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
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
                                            isSuccess = true;
                                            externaltransref = rechargeResponseEnvelope1.EXTREFNUM;
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                rechargeResponseEnvelope1.TXNSTATUS.ToString(),
                                                rechargeResponseEnvelope1.MESSAGE,
                                                externaltransref
                                                );
                                            
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
                                        _logger.LogInformation($"Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
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
                                            isSuccess = true;
                                            externaltransref = gloAirtimeResultEnvelope.Body.VendResponse.TxRefId;
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseCode.ToString(),
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseMessage,
                                                externaltransref
                                                );
                                           
                                        }
                                        else
                                        {
                                            externaltransref = gloAirtimeResultEnvelope.Body.VendResponse.TxRefId;
                                            await UpdateFailedTaskStatusAsync(item.RecordId,
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseCode.ToString(),
                                                gloAirtimeResultEnvelope.Body.VendResponse.ResponseMessage
                                                );
                                            
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogInformation($"Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
                                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                    }
                                }
                                else
                                {
                                    GloDataResultEnvelope.Envelope gloDataResultEnvelope1 = new GloDataResultEnvelope.Envelope();
                                    gloDataResultEnvelope1 = await _gloTopupService.GloDataRecharge(pinlessRechargeRequest);
                                    if (gloDataResultEnvelope1.Body != null)
                                    {
                                        if (gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultCode == 0 )
                                        {
                                            externaltransref = gloDataResultEnvelope1.Body.requestTopupResponse.@return.ersReference;
                                            isSuccess = true;
                                            await UpdateTaskStatusAsync(item.RecordId,
                                                gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultCode.ToString(),
                                                gloDataResultEnvelope1.Body.requestTopupResponse.@return.resultDescription,
                                                externaltransref
                                                );
                                            //
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
                                        _logger.LogInformation($"Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
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
                                        isSuccess = true;
                                        externaltransref = env1.Body.SDF_Data.result.instanceId.ToString();
                                        await UpdateTaskStatusAsync(item.RecordId,
                                            env1.Body.SDF_Data.result.statusCode.ToString(),
                                            env1.Body.SDF_Data.result.errorDescription,
                                            externaltransref
                                            );
                                        
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
                                    _logger.LogInformation($"9Mobile Web Service Failed : {item.serviceproviderid} for request {pinlessRechargeRequest.transId}");
                                    await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
                                }
                                break;
                            default:
                                break;
                        }

                        //Debit the dealer
                        if(isSuccess)
                        {
                            await _transactionRecordingService.StockSales(item.PartnerId, item.serviceproviderid, item.transamount, item.transref);
                        }
                        
                        successCount++;
                        _logger.LogInformation($"End processing request {pinlessRechargeRequest.transId} ");
                    }
                    catch (Exception ex)
                    {
                        await UpdateFailedTaskStatusAsync(item.RecordId, "99", ex.Message);
                        _logger.LogError($"Error handling message : {ex}");
                    }

                }

            }

            _logger.LogInformation($"Fulfillment topup background worker processed {successCount} successfully" + " at " + DateTime.Now);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TopUpTransactionLog>> GetPendingJobs(int threadNo)
        {
            DateTime dt = DateTime.Today;

            //_evctranLogrepo
            var data = _topupLogRepo.GetQueryable(a => a.IsProcessed == 0 && a.ThreadNo == threadNo)
                //.Where()  //|| a.CountRetries < 6
                .OrderBy(a => a.tran_date).ToList();
            return data;
        }
        //private StockMaster GetPartnerStockBalance(int partnerId, int serviceProviderId)
        //{
            
        //    var stockData = _stockmaster.GetQueryable()
        //        .Where(a => a.PartnerId == partnerId && a.ServiceProviderId == serviceProviderId).FirstOrDefault();
                
        //    return stockData;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public async Task UpdateTaskStatusAsync(long taskId, string errorCode, string errorDesc, string externaltransref)
        {
            //var _evctranLogrepo = unitOfWork.GetRepository<EvcTransactionLog>();
            var taskEntity = await _topupLogRepo.GetByIdAsync(taskId);

            if (taskEntity == null)
                throw new ArgumentNullException("taskEntity");

            taskEntity.ProcessedDate = DateTime.Now;
            taskEntity.ErrorCode = errorCode;
            taskEntity.ErrorDesc = errorDesc;
            taskEntity.IsProcessed = 1;
            taskEntity.TransactionStatus = 1;
            taskEntity.external_transref = externaltransref;
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
            taskEntity.IsProcessed = 1;
            taskEntity.TransactionStatus = 2;
            taskEntity.CountRetries = taskEntity.CountRetries + 1;
            await _topupLogRepo.UpdateAsync(taskEntity);

        }

    }


}
