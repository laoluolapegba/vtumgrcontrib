using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Kedco;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Kedco
{
    public interface IKedcoPaymentsService
    {
        Task<BillPaymentsResponse> KedcoPostpaidAsync(KedcoRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> KedcoPrepaidAsync(KedcoRequest paymentRequest, CancellationToken cancellationToken);
    }
}