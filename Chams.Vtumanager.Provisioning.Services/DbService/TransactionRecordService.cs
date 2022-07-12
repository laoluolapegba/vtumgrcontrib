using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities.BusinessAccount;
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
        private readonly IRepository<EpurseAccount> _epurserepo;
        private readonly IRepository<BusinessAccount> _partnerRepo;

        public TransactionRecordService(
            ILogger<TransactionRecordService> logger,
            
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _requestsRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _epurserepo = unitOfWork.GetRepository<EpurseAccount>();
            _partnerRepo = unitOfWork.GetRepository<BusinessAccount>();
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
        public async Task<bool> TopUpBusinessAccount(AccountTopUpRequest accountTopUpRequest)
        {
            bool successful = false;

            int acctcount = _epurserepo.GetQueryable()
                .Where(a => a.partner_id == accountTopUpRequest.PartnerId && a.serviceprovider_id == accountTopUpRequest.ServiceProviderId).Count();
            if(acctcount > 0)
            {
                var epurseacct = _epurserepo.GetQueryable()
                .Where(a => a.partner_id == accountTopUpRequest.PartnerId && a.serviceprovider_id == accountTopUpRequest.ServiceProviderId).FirstOrDefault();

                epurseacct.LastCreditDate = DateTime.Now;
                epurseacct.CreatedBy = accountTopUpRequest.CreatedBy;
                epurseacct.AuthorisedBy = epurseacct.AuthorisedBy;
                epurseacct.main_account_balance = epurseacct.main_account_balance + accountTopUpRequest.Amount;

                await _epurserepo.UpdateAsync(epurseacct);
            }
            else
            {
                EpurseAccount epurseAcct = new EpurseAccount
                {
                    LastCreditDate = DateTime.Now,
                    CreatedBy = accountTopUpRequest.CreatedBy,
                    AuthorisedBy = accountTopUpRequest.AuthorisedBy,
                    main_account_balance =  accountTopUpRequest.Amount,
                    serviceprovider_id = accountTopUpRequest.ServiceProviderId,
                    partner_id = accountTopUpRequest.PartnerId
                 };

                await _epurserepo.AddAsync(epurseAcct);
            }

            return successful;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="serviceProviderId"></param>
        /// <returns></returns>
        public async Task<EpurseAccount> GetEpurseAccountBal(AccountBalRequest accountBalRequest)
        {
            
            EpurseAccount epurseAccount = _epurserepo.GetQueryable()
                .Where(a => a.partner_id == accountBalRequest.PartnerId && a.serviceprovider_id == accountBalRequest.ServiceProviderId).FirstOrDefault();
            

            return epurseAccount;
        }
       

        public async Task<List<EpurseAccount>> GetAllEpurseAccounts(string partnerId)
        {

             var epurseAccounts = _epurserepo.GetQueryable()
                .Where(a => a.partner_id == partnerId).ToList();


            return epurseAccounts;
        }
        public bool IsPartnerExist(string partnerId)
        {
            _logger.LogInformation($"Checking if IspartnerExists from Database : {partnerId}");
            var requestObkect = _partnerRepo.GetQueryable()

                .Where(a => a.AccountCode == partnerId); ;
            return requestObkect.Count() > 0;

        }
    }
}
