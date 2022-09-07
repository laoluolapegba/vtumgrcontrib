using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Showmax;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Showmax
{
    public interface IShowmaxPaymentsService
    {
        Task<BillPaymentsResponse> ShowmaxVoucehrPaymentAsync(ShowmaxVoucherRequest paymentRequest, CancellationToken cancellationToken);
    }
}