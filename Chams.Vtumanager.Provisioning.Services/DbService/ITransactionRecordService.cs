using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.TransactionRecordService
{
    public interface ITransactionRecordService
    {
        Task<bool> IsTransactionExist(string transreference);
        Task<bool> RecordTransaction(RechargeRequest rechargeRequest);
        Task<bool> TopUpBusinessAccount(AccountTopUpRequest accountTopUpRequest);
        Task<List<EpurseAccount>> GetAllEpurseAccounts(string partnerId);
        Task<EpurseAccount> GetEpurseAccountBal(AccountBalRequest accountBalRequest);
        bool IsPartnerExist(string partnerId);

    }
}