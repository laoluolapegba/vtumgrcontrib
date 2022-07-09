using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Hangfire.Services
{
    public interface IEvcBackgroundTask
    {
        Task<IEnumerable<TopUpTransactionLog>> GetPendingJobs();
        Task ProcessPendingRequests();
        Task UpdateFailedTaskStatusAsync(long taskId, string errorCode, string errorDesc);
        Task UpdateTaskStatusAsync(long taskId, string errorCode, string errorDesc);
    }
}