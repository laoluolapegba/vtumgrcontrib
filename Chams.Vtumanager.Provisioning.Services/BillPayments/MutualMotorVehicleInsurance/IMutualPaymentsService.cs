using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Mutual;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec;
using Chams.Vtumanager.Provisioning.Services.BillPayments.Jamb;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.MutualMotorVehicleInsurance
{
    public interface IMutualPaymentsService
    {
        Task<BillPaymentsResponse> MutualMVPaymentAsync(MutualMortorInsuranceRequest paymentRequest, CancellationToken cancellationToken);
    }
}