using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.JosElectricity;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.JosElectric
{
    public interface IJosElectricPaymentsService
    {
        Task<BillPaymentsResponse> JosElectricPostpaidAsync(JosElectricPostPaidRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> JosElectricPrepaidAsync(JosElectricPrePaidRequest paymentRequest, CancellationToken cancellationToken);
    }
}