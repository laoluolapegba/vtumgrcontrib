using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.IkejaELectric
{
    public interface IIkejaElectricPaymentsService
    {
        Task<BillPaymentsResponse> IkejaElectricPostpaidAsync(IkejaElectricPostpaidRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> IkejaElectricPrepaidAsync(IkejaElectricTokenPurchaseRequest paymentRequest, CancellationToken cancellationToken);
    }
}