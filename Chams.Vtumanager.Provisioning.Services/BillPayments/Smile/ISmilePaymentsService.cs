using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Smile;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Smile
{
    public interface ISmilePaymentsService
    {
        Task<BillPaymentsResponse> SmileBundlePaymentAsync(SmileCommBundleRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> SmileRechargePaymentAsync(SmileCommRechargeRequest paymentRequest, CancellationToken cancellationToken);
    }
}