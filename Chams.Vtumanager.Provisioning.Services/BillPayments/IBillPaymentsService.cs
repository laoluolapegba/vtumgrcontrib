using Chams.Vtumanager.Provisioning.Entities.BillPayments.Dstv;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments
{
    public interface IBillPaymentsService
    {
        Task<DstvResponse> DstvPaymentAsync(DstvRequest dstvRequest, CancellationToken cancellationToken);
    }
}
