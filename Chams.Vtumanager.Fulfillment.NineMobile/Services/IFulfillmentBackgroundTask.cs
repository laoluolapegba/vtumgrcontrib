using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Hangfire.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFulfillmentBackgroundTask
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TopUpTransactionLog>> GetPendingJobs(int threadNo);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ProcessPendingRequests();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        Task UpdateFailedTaskStatusAsync(long taskId, string errorCode, string errorDesc);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        Task UpdateTaskStatusAsync(long taskId, string errorCode, string errorDesc, string externaltransref);
    }
}