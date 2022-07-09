using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.TransactionRecordService
{
    public interface ITransactionRecordService
    {
        Task<bool> IsTransactionExist(string transreference);
        Task<bool> RecordTransaction(RechargeRequest rechargeRequest);
    }
}