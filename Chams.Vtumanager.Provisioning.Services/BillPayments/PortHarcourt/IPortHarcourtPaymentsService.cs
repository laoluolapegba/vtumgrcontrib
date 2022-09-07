using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.PortharcourtElectric;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.PortHarcourt
{
    public interface IPortHarcourtPaymentsService
    {
        Task<BillPaymentsResponse> PortHarcourtPostpaidAsync(PortHarcourtElectricRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> PortHarcourtPrepaidAsync(PortHarcourtElectricRequest paymentRequest, CancellationToken cancellationToken);
    }
}