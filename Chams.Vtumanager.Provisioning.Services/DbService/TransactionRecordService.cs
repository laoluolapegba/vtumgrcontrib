using Chams.Vtumanager.Provisioning.Data;
using Chams.Vtumanager.Provisioning.Entities;
using Chams.Vtumanager.Provisioning.Entities.BusinessAccount;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.Epurse;
using Chams.Vtumanager.Provisioning.Entities.Inventory;
using Chams.Vtumanager.Provisioning.Entities.Partner;
using Chams.Vtumanager.Provisioning.Entities.Product;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.Authentication;
using Chams.Vtumanager.Provisioning.Services.QueService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.TransactionRecordService
{
    public class TransactionRecordService : ITransactionRecordService
    {
        private readonly ILogger<TransactionRecordService> _logger;
        private readonly IRepository<TopUpTransactionLog> _requestsRepo;
        private readonly IRepository<EpurseAccountMaster> _epurserepo;
        private readonly IRepository<BusinessAccount> _partnerRepo;
        private readonly IRepository<StockDetails> _stockRepo;
        private readonly IRepository<VtuProducts> _productsRepo;
        private readonly IRepository<EpurseAcctTransactions> _epursetransRepo;
        private readonly IRepository<ApiCredentials> _apicredRepo;
        private readonly IRepository<StockMaster> _stockmasterRepo;
        private readonly IRepository<PartnerServiceProvider> _partnerServicesRepo;

        public TransactionRecordService(
            ILogger<TransactionRecordService> logger,

            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _requestsRepo = unitOfWork.GetRepository<TopUpTransactionLog>();
            _epurserepo = unitOfWork.GetRepository<EpurseAccountMaster>();
            _partnerRepo = unitOfWork.GetRepository<BusinessAccount>();
            _stockRepo = unitOfWork.GetRepository<StockDetails>();
            _productsRepo = unitOfWork.GetRepository<VtuProducts>();
            _epursetransRepo = unitOfWork.GetRepository<EpurseAcctTransactions>();
            _apicredRepo = unitOfWork.GetRepository<ApiCredentials>();
            _stockmasterRepo = unitOfWork.GetRepository<StockMaster>();
            _partnerServicesRepo = unitOfWork.GetRepository<PartnerServiceProvider>();

            //_productsRepo = unitOfWork.GetRepository<Product>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transreference"></param>
        /// <returns></returns>
        public bool IsTransactionExist(string transreference, int partnerId)
        {
            _logger.LogInformation($"Checking if transactionexists from Database : {transreference}");
            var requestObkect = _requestsRepo.GetQueryable()

                .Where(a => a.transref == transreference && a.PartnerId == partnerId);
            return requestObkect.Count() > 0;

        }
        public int GetPartnerIdbykey(string apiKey)
        {
            int partnerid = 0;
            try
            {
                _logger.LogInformation($"Checking if GetPartnerIdbykey from Database");
                var requestObject = _apicredRepo.GetQueryable()

                    .Where(a => a.ApiKey == apiKey && a.Active == true).FirstOrDefault();
                partnerid = requestObject.Id;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get partnerId by apikey");
                partnerid = 0;
            }

            return partnerid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rechargeRequest"></param>
        /// <returns></returns>
        public async Task<bool> RecordTransaction(RechargeRequest rechargeRequest, int partnerId)
        {
            bool transactionStatus = false;
            _logger.LogInformation($"Saving transaction record : {JsonConvert.SerializeObject(rechargeRequest)}");
            string serviceprovidername = Enum.GetName(typeof(ServiceProvider), rechargeRequest.ServiceProviderId);
            var partner = await GetPatnerById(partnerId);
            string partnerCode = partner.PartnerCode;

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
                sourcesystem = partnerCode,
                serviceprovidername = serviceprovidername,
                PartnerId = partnerId,
                CountRetries = 0,


            };
            var requestObkect = await _requestsRepo.AddAsync(topUpRequest);
            await _requestsRepo.SaveAsync();

            return transactionStatus;

        }
        public async Task<EpurseAccountMaster> CreditEpurseAccount(AccountTopUpRequest accountTopUpRequest)
        {


            //int acctcount = _epurserepo.GetQueryable()
            //    .Where(a => a.PartnerId == accountTopUpRequest.PartnerId).Count();
            var epurseacct = _epurserepo.GetQueryable()
                .Where(a => a.PartnerId == accountTopUpRequest.PartnerId && a.ProductcategoryId == accountTopUpRequest.ProductCategoryId).FirstOrDefault();
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

        #region Epurse
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PartnerId"></param>
        /// <returns></returns>
        public List<EpurseAccountMaster> GetEpurseByPartnerId(int PartnerId)
        {

            var epurseAccounts = _epurserepo.GetQueryable()
                .Where(a => a.PartnerId == PartnerId ).ToList();


            return epurseAccounts;
        }
        public EpurseAccountMaster GetEpurseByPartnerIdCategoryId(int PartnerId, int productcategoryId)
        {

            var epurseAccounts = _epurserepo.GetQueryable()
                .Where(a => a.PartnerId == PartnerId && a.ProductcategoryId == productcategoryId).FirstOrDefault();


            return epurseAccounts;
        }
        public async Task<IEnumerable<EpurseAccountMaster>> GetEpurseAccounts()
        {

            var epurseAccounts = await _epurserepo.GetAllAsync();

            return epurseAccounts;
        }



        public bool IsPartnerExist(int partnerId)
        {
            _logger.LogInformation($"Checking if IspartnerExists from Database : {partnerId}");
            var requestObkect = _partnerRepo.GetQueryable()

                .Where(a => a.PartnerId == partnerId); ;
            return requestObkect.Count() > 0;

        }
        public bool IsEpurseExist(int partnerId, int tenantId)
        {
            _logger.LogInformation($"Checking if IsEpurseExist from Database : {partnerId}");
            var requestObkect = _epurserepo.GetQueryable()

                .Where(a => a.PartnerId == partnerId && a.TenantId == tenantId); 
            return requestObkect.Count() > 0;

        }
        public async Task<EpurseAccountMaster> CreateEpurseAccount(EpurseAccountMaster epurseAccount, int prodCat)
        {
            var lastacct = _epurserepo.GetQueryable().OrderByDescending(a => a.AcctNo).FirstOrDefault();
               // .Where(a => a.PartnerId == epurseAccount.PartnerId ).OrderByDescending(a=>a.AcctNo).FirstOrDefault(); //&& a.ProductcategoryId == prodCat
            int acctNo = 0;

            
            if(lastacct == null)
            {
                _logger.LogInformation($"No  account found for partnerId:{epurseAccount.PartnerId}");
                epurseAccount.AcctNo = "1000001";
            }
            else
            {
                acctNo = int.Parse(lastacct.AcctNo) + 1;
                epurseAccount.AcctNo = acctNo.ToString();
                _logger.LogInformation($"Next  account number for partnerId:{epurseAccount.PartnerId} : acctNo");
            }

            epurseAccount.MainAcctBalance = 0;
            epurseAccount.RewardPoints = 0;
            epurseAccount.CommissionAcctBalance = 0;
            epurseAccount.CreatedBy = epurseAccount.CreatedBy;
            epurseAccount.AuthorisedBy = epurseAccount.AuthorisedBy;
            epurseAccount.ProductcategoryId = prodCat;
            epurseAccount.CreatedDate = DateTime.Now;
            _logger.LogInformation($"Creating account number: {epurseAccount.AcctNo}");

            await _epurserepo.AddAsync(epurseAccount);
            await _epurserepo.SaveAsync();


            return epurseAccount;
        }
        #endregion
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
                    ProductCategoryId = 2,
                    ServiceProviderId = g.Key,
                    PartnerId = partnerId,
                    Qoh = g.Sum( a=>a.quantity) 
                });

            return balances;
        }

        public async Task<bool> PurchaseStock(StockPurchaseOrder stockPurchaseRequest, bool addCommision)
        {
            int rowcount = 0;
            var epurseacctObj = _epurserepo.GetQueryable()
                .Where(a => a.PartnerId == stockPurchaseRequest.PartnerId && a.ProductcategoryId == stockPurchaseRequest.ProductCategoryId).FirstOrDefault();

            

            foreach (var orderLine in stockPurchaseRequest.items)
            {
                var product = _productsRepo.GetQueryable()
                .Where(a => a.ServiceProviderId == orderLine.ServiceProviderId && a.ProductType == 1).FirstOrDefault();

               

                //Create a transaction on the partner's account
                var epurseaccttrans = new EpurseAcctTransactions
                {
                    PartnerId = stockPurchaseRequest.PartnerId,
                    DrCr = "D",
                    TranDate = DateTime.Now,
                    TranDesc = "Stock Purchase",
                    TenantId = stockPurchaseRequest.tenantId,
                    TranAmount = orderLine.Quantity * product.Price,
                    UserId = stockPurchaseRequest.UserId,
                    AccountNo = epurseacctObj.AcctNo,
                    ProductCode = product.ProductId,
                    ServiceProviderId = orderLine.ServiceProviderId

                };
                _epursetransRepo.Add(epurseaccttrans);
                //create an inventory details record

                var stockentry = new StockDetails
                {
                    partner_id = stockPurchaseRequest.PartnerId,
                    price = 0,
                    quantity = orderLine.Quantity,
                    service_provider_id = orderLine.ServiceProviderId,
                    tenant_id = stockPurchaseRequest.tenantId,
                    trans_date = DateTime.Now,
                    trans_type_id = 1,
                    product_id = product.ProductId,
                    user_id = stockPurchaseRequest.UserId
                };
                _stockRepo.Add(stockentry);

                //Increase the QOH
                StockMaster stockObj = _stockmasterRepo.GetQueryable(a => a.PartnerId == stockPurchaseRequest.PartnerId && a.ServiceProviderId == orderLine.ServiceProviderId).FirstOrDefault();
                if (stockObj == null)
                {
                    StockMaster masterrecord = new StockMaster
                    {
                        ServiceProviderId = orderLine.ServiceProviderId,
                        PartnerId = stockPurchaseRequest.PartnerId,
                        TenantId = stockPurchaseRequest.tenantId,
                        QuantityOnHand = orderLine.Quantity
                    };
                    //stockObj.partner_id = stockPurchaseRequest.PartnerId;
                    //stockObj.tenant_id = stockPurchaseRequest.tenantId;
                    //stockObj.service_provider_id = orderLine.ServiceProviderId;
                    //stockObj.QuantityOnHand = orderLine.Quantity;

                    _logger.LogInformation($"transactionrecord adding stock master: {JsonConvert.SerializeObject(masterrecord)}");
                    await _stockmasterRepo.AddAsync(masterrecord);

                }
                else
                {
                    
                    stockObj.QuantityOnHand = stockObj.QuantityOnHand + orderLine.Quantity;
                    _logger.LogInformation($"transactionrecord updating stock master: {JsonConvert.SerializeObject(stockObj)}");
                    await _stockmasterRepo.UpdateAsync(stockObj);
                }



                if (addCommision)
                {
                    decimal commisionpct = 0;
                    var commisionObj = _partnerServicesRepo.GetQueryable()
                   .Where(a => a.PartnerId == stockPurchaseRequest.PartnerId && a.ServiceProviderId == orderLine.ServiceProviderId).FirstOrDefault();

                    if(commisionObj != null && commisionObj.CommissionPct != null )
                    {
                        commisionpct = (decimal)commisionObj.CommissionPct;

                        if(commisionpct > 0)
                        {
                            int commisionQty = Convert.ToInt32(Math.Round((orderLine.Quantity * (commisionpct / 100))));
                            decimal commisionAmt = (orderLine.Quantity * (commisionpct / 100));
                            //Add the  partner's commision
                            var epurseCommitssion = new EpurseAcctTransactions
                            {
                                PartnerId = stockPurchaseRequest.PartnerId,
                                DrCr = "C",
                                TranDate = DateTime.Now,
                                TranDesc = "Commission for Stock Purchase",
                                TenantId = stockPurchaseRequest.tenantId,
                                TranAmount = commisionAmt * product.Price, //GetHashCode rate Laolu
                                UserId = stockPurchaseRequest.UserId,
                                AccountNo = epurseacctObj.AcctNo,
                                ProductCode = product.ProductId,
                                ServiceProviderId = orderLine.ServiceProviderId

                            };
                            _epursetransRepo.Add(epurseCommitssion);

                            var commisionEntry = new StockDetails
                            {
                                partner_id = stockPurchaseRequest.PartnerId,
                                price = 0,
                                quantity = commisionQty,
                                service_provider_id = orderLine.ServiceProviderId,
                                tenant_id = stockPurchaseRequest.tenantId,
                                trans_date = DateTime.Now,
                                trans_type_id = 1,
                                product_id = product.ProductId,
                                user_id = stockPurchaseRequest.UserId
                            };
                            _stockRepo.Add(commisionEntry);

                            var commisionStock = _stockmasterRepo.GetQueryable(a => a.PartnerId == stockPurchaseRequest.PartnerId && a.ServiceProviderId == orderLine.ServiceProviderId).FirstOrDefault();

                            if (commisionStock == null)
                            {
                                StockMaster masterrecord = new StockMaster
                                {
                                    ServiceProviderId = orderLine.ServiceProviderId,
                                    PartnerId = stockPurchaseRequest.PartnerId,
                                    TenantId = stockPurchaseRequest.tenantId,
                                    QuantityOnHand = commisionQty  //get commiison #tage
                                };
                                await _stockmasterRepo.AddAsync(masterrecord);

                            }
                            else
                            {
                                commisionStock.QuantityOnHand = stockObj.QuantityOnHand + commisionQty;  //get commiison #tage
                                await _stockmasterRepo.UpdateAsync(commisionStock);
                            }
                        }
                    }

                   

                }
                
                
                


                //debit the main account
                epurseacctObj.MainAcctBalance = epurseacctObj.MainAcctBalance - (orderLine.Quantity * product.Price);
                await _epurserepo.UpdateAsync(epurseacctObj);

            }

            rowcount = await _epursetransRepo.SaveAsync(); rowcount = 0;
            rowcount = await _epurserepo.SaveAsync(); rowcount = 0;
            rowcount = await _stockRepo.SaveAsync();
            rowcount = await _stockmasterRepo.SaveAsync();

            return true;


        }
        
        public async Task<BusinessAccount> GetPatnerById(int partnerId)
        {
            var partner = _partnerRepo.GetQueryable().Where(a => a.PartnerId == partnerId).FirstOrDefault();
            return partner;
        }

        public async Task<IEnumerable<VtuProducts>> ProductList(int serviceProviderId)
        {
            var products = _productsRepo.GetQueryable().Where(a => a.ServiceProviderId == serviceProviderId).ToList();
            return products;
        }

        public async Task<TopUpTransactionLog> GetTransactionById(int serviceproviderId, string transactionReference)
        {

            var transaction = _requestsRepo.GetQueryable().Where(a => a.serviceproviderid == serviceproviderId && a.transref == transactionReference).FirstOrDefault();
            return transaction;
        }
    }
}
