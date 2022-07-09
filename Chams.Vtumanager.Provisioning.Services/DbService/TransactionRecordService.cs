using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.Authentication;
using Chams.Vtumanager.Provisioning.Services.QueService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.TransactionRecordService
{
    public class TransactionRecordService : ITransactionRecordService
    {
        private readonly ILogger<TransactionRecordService> _logger;
        private readonly IRepository<TopUpTransactionLog> _requestsRepo;

        public TransactionRecordService(
            ILogger<TransactionRecordService> logger,
            
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _requestsRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transreference"></param>
        /// <returns></returns>
        public async Task<bool> IsTransactionExist(string transreference)
        {
            _logger.LogInformation($"Checking if transactionexists from Database : {transreference}");
            var requestObkect = _requestsRepo.GetQueryable()

                .Where(a => a.transref == transreference); ;
            return requestObkect.Count() > 0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rechargeRequest"></param>
        /// <returns></returns>
        public async Task<bool> RecordTransaction(RechargeRequest rechargeRequest)
        {
            bool transactionStatus = false;
            _logger.LogInformation($"Saving transaction record : {JsonConvert.SerializeObject(rechargeRequest)}");
            string serviceprovidername = Enum.GetName(typeof(ServiceProvider), rechargeRequest.ServiceProviderId);
            TopUpTransactionLog topUpRequest = new TopUpTransactionLog
            {
                tran_date = DateTime.Now,
                transamount = rechargeRequest.rechargeAmount,
                transref = rechargeRequest.TransactionReference,
                transtype = rechargeRequest.TransactionType,
                channelid = rechargeRequest.ChannelId,
                msisdn = rechargeRequest.PhoneNumber,
                productid = rechargeRequest.ProductId,
                serviceproviderid = rechargeRequest.ServiceProviderId,
                sourcesystem = rechargeRequest.SourceSystemId,
                serviceprovidername = serviceprovidername,
                CountRetries = 0,
                

            };
            var requestObkect = await _requestsRepo.AddAsync(topUpRequest);
            await _requestsRepo.SaveAsync();

            return transactionStatus;

        }
    }
}
