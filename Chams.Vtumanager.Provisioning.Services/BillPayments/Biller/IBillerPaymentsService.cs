using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.AbujaDisco
{
    public interface IBillerPaymentsService
    {
        Task<BillPaymentsResponse> BillerPayAsync(BillpaymentRequestRequest paymentRequest, CancellationToken cancellationToken);
        
    }
}