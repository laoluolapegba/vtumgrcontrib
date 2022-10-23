using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.IbadanDisco;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.IbadanDisco
{
    public interface IIbadanDiscoPaymentsService
    {
        Task<BillPaymentsResponse> IbadanDiscoPostpaidAsync(IbadanDiscoPostpaidRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> IbadanDiscoPrepaidAsync(IbadanDiscoPrepaidRequest paymentRequest, CancellationToken cancellationToken);
    }
}