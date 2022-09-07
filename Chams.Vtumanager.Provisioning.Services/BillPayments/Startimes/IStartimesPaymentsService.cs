using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Startimes;

using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Startimes
{
    public interface IStartimesPaymentsService
    {
        Task<BillPaymentsResponse> StartimesPaymentAsync(StartimesRequest paymentRequest, CancellationToken cancellationToken);
    }
}