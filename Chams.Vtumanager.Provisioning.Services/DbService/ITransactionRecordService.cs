using Chams.Vtumanager.Provisioning.Entities;
using Chams.Vtumanager.Provisioning.Entities.BusinessAccount;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.Epurse;
using Chams.Vtumanager.Provisioning.Entities.Inventory;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.TransactionRecordService
{
    public interface ITransactionRecordService
    {
        Task<EpurseAccountMaster> CreateEpurseAccount(EpurseAccountMaster epurseAccount, int prodCat);
        
        bool IsPartnerExist(int partnerId);
        bool IsTransactionExist(string transreference,int partnerId);
        Task<bool> RecordTransaction(RechargeRequest rechargeRequest, int partnerId);
        Task<EpurseAccountMaster> CreditEpurseAccount(AccountTopUpRequest accountTopUpRequest);
        
        Task<IEnumerable<EpurseAccountMaster>> GetEpurseAccounts();
        List<EpurseAccountMaster> GetEpurseByPartnerId(int PartnerId);
        EpurseAccountMaster GetEpurseByPartnerIdCategoryId(int PartnerId, int productcategoryId);
        bool IsEpurseExist(int partnerId, int tenantId);
        Task<IEnumerable<StockBalanceView>> GetStockbalancesbyPartnerId(int partnerId);
        Task<bool> PurchaseStock(StockPurchaseOrder stockPurchaseRequest, bool addCommision);
        //int GetPartnerIdbykey(string apiKey);
        Task<ApiCredentials> GetPartnerbyAPIkey(string apiKey);
        Task<BusinessAccount> GetPatnerById(int partnerId);
        Task<IEnumerable<VtuProducts>> ProductList(int serviceProviderId);
        Task<TopUpTransactionLog> GetTransactionById(int serviceproviderId, string transactionReference);
        CarrierPrefix GetServiceProviderByPrefix(string prefix);
        Task<StockMaster> GetPartnerStockBalance(int partnerId, int serviceProviderId);
        int CarrierLookup1(string msisdn);
        Task<bool> StockSales(int partnerId, int serviceproviderId, decimal txnAmt, string transref);
    }
}