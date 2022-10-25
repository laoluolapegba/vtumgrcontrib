using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.AbujaDisco
{
    public interface IBillerPaymentsService
    {
        Task<BillPaymentsResponse> BillerPayAsync(BillpaymentRequest paymentRequest, CancellationToken cancellationToken);
        Task<ProxyResponse> ProxyAsync(ProxyRequest proxyRequest, CancellationToken cancellationToken);
        Task<List<ServiceListResponse>> ServiceListAsync();
    }
}