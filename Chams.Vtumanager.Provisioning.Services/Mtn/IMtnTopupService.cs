using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.NineMobile;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.Mtn
{
    public interface IMtnTopupService
    {
        Task<MtnResponseEnvelope.Envelope> AirtimeRecharge(PinlessRechargeRequest pinlessRechargeRequest);
        Task<RechargeResponseEnvelope.Envelope> DataRecharge(PinlessRechargeRequest pinlessRechargeRequest);
        Task<AccessTokenResponse> GetAccessToken();
        Task<MtnSubscriptionResponse> MtnSubscription(PinlessRechargeRequest pinlessRechargeRequest);
        Task<QueryTxnStatusResponse> QueryTransactionStatus(QueryTransactionStatusRequest statusRequest);
        Task<MtnQTxResponseEnvelope.Envelope> QueryTransactionStatusbyERSRef(QueryTransactionStatusRequest queryTransaction);
        Task<MtnQTxResponseEnvelope.Envelope> QueryTransactionStatusbyClientRef(QueryTransactionStatusRequest queryTransaction);
    }
}