using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Multichoice
{
    public interface IBulkSMSPaymentsService
    {
        Task<BillPaymentsResponse> BulkSMSPaymentAsync(BulkSMSRequest paymentRequest, CancellationToken cancellationToken);
    }
}