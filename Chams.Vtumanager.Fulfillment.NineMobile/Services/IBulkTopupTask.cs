using Chams.Vtumanager.Provisioning.Entities.Subscription;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    public interface IBulkTopupTask
    {
        Task<IEnumerable<DirectSalesDetail>> GetPendingJobs(long requestid);
        Task ProcessPendingRequests();
        Task UpdateFailedTaskStatusAsync(long taskId, string errorCode, string errorDesc);
        Task UpdateMasterTaskStatusAsync(long taskId, string errorCode, string errorDesc);
        Task UpdateTaskStatusAsync(long taskId, string errorCode, string errorDesc);
    }
}