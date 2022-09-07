using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Cornerstone;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.CornerStone
{
    public interface ICornerstonePaymentsService
    {
        Task<BillPaymentsResponse> CornerstonePaymentAsync(CornerstoneRequest paymentRequest, CancellationToken cancellationToken);
    }
}