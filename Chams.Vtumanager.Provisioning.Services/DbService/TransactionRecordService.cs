using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities.BusinessAccount;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.Epurse;
using Chams.Vtumanager.Provisioning.Entities.Inventory;
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
        private readonly IRepository<Stock_Details> _stockRepo;

        public TransactionRecordService(
            ILogger<TransactionRecordService> logger,

            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _requestsRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _epurserepo = unitOfWork.GetRepository<EpurseAccount>();
            _partnerRepo = unitOfWork.GetRepository<BusinessAccount>();
            _stockRepo = unitOfWork.GetRepository<Stock_Details>();
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
        public async Task<EpurseAccount> CreditEpurseAccount(AccountTopUpRequest accountTopUpRequest)
        {


            //int acctcount = _epurserepo.GetQueryable()
            //    .Where(a => a.PartnerId == accountTopUpRequest.PartnerId).Count();
            var epurseacct = _epurserepo.GetQueryable()
                .Where(a => a.PartnerId == accountTopUpRequest.PartnerId && a.TenantId == accountTopUpRequest.TenantId).FirstOrDefault();
            if(epurseacct != null)
            {
                epurseacct.LastCreditDate = DateTime.Now;
                epurseacct.CreatedBy = accountTopUpRequest.CreatedBy;
                epurseacct.AuthorisedBy = epurseacct.AuthorisedBy;
                epurseacct.MainAcctBalance = epurseacct.MainAcctBalance + accountTopUpRequest.Amount;

                await _epurserepo.UpdateAsync(epurseacct);
                await _epurserepo.SaveAsync();
            }
            
            return epurseacct;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PartnerId"></param>
        /// <returns></returns>
        public async Task<EpurseAccount> GetEpurseByPartnerId(int PartnerId)
        {

            EpurseAccount epurseAccount = _epurserepo.GetQueryable()
                .Where(a => a.PartnerId == PartnerId).FirstOrDefault();


            return epurseAccount;
        }
        public async Task<IEnumerable<EpurseAccount>> GetEpurseAccounts()
        {

            var epurseAccounts = _epurserepo.GetAll();

            return epurseAccounts;
        }



        public bool IsPartnerExist(string partnerId)
        {
            _logger.LogInformation($"Checking if IspartnerExists from Database : {partnerId}");
            var requestObkect = _partnerRepo.GetQueryable()

                .Where(a => a.AccountCode == partnerId); ;
            return requestObkect.Count() > 0;

        }
        public bool IsEpurseExist(int partnerId, int tenantId)
        {
            _logger.LogInformation($"Checking if IsEpurseExist from Database : {partnerId}");
            var requestObkect = _epurserepo.GetQueryable()

                .Where(a => a.PartnerId == partnerId && a.TenantId == tenantId); 
            return requestObkect.Count() > 0;

        }
        public async Task<EpurseAccount> CreateEpurseAccount(EpurseAccount epurseAccount)
        {
            epurseAccount.MainAcctBalance = 0;
            epurseAccount.RewardPoints = 0;
            epurseAccount.CommissionAcctBalance = 0;
            epurseAccount.CreatedBy = epurseAccount.CreatedBy;
            epurseAccount.AuthorisedBy = epurseAccount.AuthorisedBy;


            await _epurserepo.AddAsync(epurseAccount);
            await _epurserepo.SaveAsync();


            var acctcount = _epurserepo.GetQueryable()
                .Where(a => a.PartnerId == epurseAccount.PartnerId && a.TenantId == epurseAccount.TenantId).FirstOrDefault();


            return acctcount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StockBalanceView>> GetStockbalancesbyPartnerId(int partnerId)
        {
            //var list1 = dbEntities.People.
            //GroupBy(m => m.PersonType).
            //Select(c =>
            //    new
            //    {
            //        Type = c.Key,
            //        Max = c.Max(),
            //    });

            var balances = _stockRepo.GetQueryable()
                .Where(a => a.partner_id == partnerId)
                .GroupBy(a => a.service_provider_id)
                .Select( g => 
                new StockBalanceView
                { 
                    ServiceProviderId = g.Key,
                    PartnerId = partnerId,
                    Qoh = g.Sum( a=>a.quantity)
                });

            return balances;
        }
    }
}
