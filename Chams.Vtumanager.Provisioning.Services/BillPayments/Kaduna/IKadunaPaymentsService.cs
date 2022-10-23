using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Kaduna;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Kedco;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Kaduna
{
    public interface IKadunaPaymentsService
    {
        Task<BillPaymentsResponse> KadunaPostpaidAsync(KadunaElectricPrepaidRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> KadunaPrepaidAsync(KadunaElectricPrepaidRequest paymentRequest, CancellationToken cancellationToken);
    }
}