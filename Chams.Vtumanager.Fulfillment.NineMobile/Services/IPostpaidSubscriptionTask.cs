using Chams.Vtumanager.Provisioning.Entities.Subscription;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    public interface IPostpaidSubscriptionTask
    {
        Task<IEnumerable<Subscriber>> GetPendingJobs();
        Task ProcessPendingRequests();
        Task UpdateFailedTaskStatusAsync(long taskId, string errorCode, string errorDesc);
        
        Task UpdateTaskStatusAsync(long taskId, string transref, string errorCode, string errorDesc);
    }
}