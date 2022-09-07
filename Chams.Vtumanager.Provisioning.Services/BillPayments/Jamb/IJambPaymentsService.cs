using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec;
using Chams.Vtumanager.Provisioning.Services.BillPayments.Jamb;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Jamb
{
    public interface IJambPaymentsService
    {
        Task<BillPaymentsResponse> JambPINPaymentAsync(JambPINRequest paymentRequest, CancellationToken cancellationToken);
    }
}