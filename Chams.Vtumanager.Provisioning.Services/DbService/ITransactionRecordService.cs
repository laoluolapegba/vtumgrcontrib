using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.Epurse;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.TransactionRecordService
{
    public interface ITransactionRecordService
    {
        Task<EpurseAccountMaster> CreateEpurseAccount(EpurseAccountMaster epurseAccount);
        
        bool IsPartnerExist(int partnerId);
        bool IsTransactionExist(string transreference);
        Task<bool> RecordTransaction(RechargeRequest rechargeRequest);
        Task<EpurseAccountMaster> CreditEpurseAccount(AccountTopUpRequest accountTopUpRequest);
        
        Task<IEnumerable<EpurseAccountMaster>> GetEpurseAccounts();
        EpurseAccountMaster GetEpurseByPartnerId(int PartnerId);
        bool IsEpurseExist(int partnerId, int tenantId);
        Task<IEnumerable<StockBalanceView>> GetStockbalancesbyPartnerId(int partnerId);
        Task<bool> PurchaseStock(StockPurchaseOrder stockPurchaseRequest);
    }
}