using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice.GotvRenew;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Multichoice
{
    public interface IMultichoicePaymentsService
    {
        Task<BillPaymentsResponse> DstvPaymentAsync(DstvRenewRequest dstvRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> DstvRenewRequestAsync(DstvRenewRequest dstvRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> DstvBoxOfficeRequestAsync(DstvBoxOfficeRequest dstvRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> GotvRequestAsync(GotvRequest dstvRequest, CancellationToken cancellationToken);
        Task<BillPaymentsResponse> GotvRenewAsync(GotvRenew dstvRequest, CancellationToken cancellationToken);
    }
}
