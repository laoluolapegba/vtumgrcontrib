using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Waec
{
    public interface IWaecPaymentsService
    {
        Task<BillPaymentsResponse> WaecPINPaymentAsync(WaecPINRequest paymentRequest, CancellationToken cancellationToken);
    }
}