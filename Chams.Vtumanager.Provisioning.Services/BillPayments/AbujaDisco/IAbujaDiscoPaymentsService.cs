using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Multichoice
{
    public interface IAbujaDiscoPaymentsService
    {
        Task<BillPaymentsResponse> AbujaPostpaidAsync(AbujaPostpaidRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> AbujaPrepaidAsync(AbujaPrepaidRequest paymentRequest, CancellationToken cancellationToken);
    }
}