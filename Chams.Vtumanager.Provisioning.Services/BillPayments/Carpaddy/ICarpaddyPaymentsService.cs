using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Carpaddy;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Carpaddy
{
    public interface ICarpaddyPaymentsService
    {
        Task<BillPaymentsResponse> CarpaddyPaymentAsync(CarpaddyRequest paymentRequest, CancellationToken cancellationToken);
    }
}