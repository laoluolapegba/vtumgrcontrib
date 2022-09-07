using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Spectranet;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Spectranet
{
    public interface ISpectranetPaymentsService
    {
        Task<BillPaymentsResponse> SpectranetRefillAsync(SpectranetRefillRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> SpectranetPaymentPlanAsync(SpectranetPaymentPlanRequest paymentRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> SpectranetPINAsync(SpectranetPINRequest paymentRequest, CancellationToken cancellationToken);
    }
}