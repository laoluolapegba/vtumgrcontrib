using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.Epurse;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.TransactionRecordService
{
    public interface ITransactionRecordService
    {
        Task<EpurseAccount> CreateEpurseAccount(EpurseAccount epurseAccount);
        
        bool IsPartnerExist(string partnerId);
        Task<bool> IsTransactionExist(string transreference);
        Task<bool> RecordTransaction(RechargeRequest rechargeRequest);
        Task<EpurseAccount> CreditEpurseAccount(AccountTopUpRequest accountTopUpRequest);
        
        Task<IEnumerable<EpurseAccount>> GetEpurseAccounts();
        Task<EpurseAccount> GetEpurseByPartnerId(int PartnerId);
        bool IsEpurseExist(int partnerId, int tenantId);
        Task<IEnumerable<StockBalanceView>> GetStockbalancesbyPartnerId(int partnerId);
    }
}