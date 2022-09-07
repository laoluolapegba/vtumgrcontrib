using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.IkejaELectric
{
    public interface IIkejaElectricPaymentsService
    {
        Task<BillPaymentsResponse> IkejaElectricPostpaidAsync(IkejaElectricRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> IkejaElectricPrepaidAsync(IkejaElectricRequest paymentRequest, CancellationToken cancellationToken);
    }
}