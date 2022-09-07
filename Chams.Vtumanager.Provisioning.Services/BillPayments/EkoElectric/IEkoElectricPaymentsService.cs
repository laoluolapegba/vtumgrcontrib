using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.EkoElectric
{
    public interface IEkoElectricPaymentsService
    {
        Task<BillPaymentsResponse> EkoElectricPostpaidAsync(EkoElectricPostpaidRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> EkoElectricPrepaidAsync(EkoElectricPrepaidRequest paymentRequest, CancellationToken cancellationToken);
    }
}