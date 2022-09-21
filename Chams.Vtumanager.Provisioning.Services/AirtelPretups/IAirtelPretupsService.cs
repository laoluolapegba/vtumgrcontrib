using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Pretups;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    public interface IAirtelPretupsService
    {
        Task<PretupsRechargeResponseEnvelope.COMMAND> AirtimeRecharge(PinlessRechargeRequest pinRechargeRequest);
        Task<PretupsRechargeResponseEnvelope.COMMAND> DataRecharge(PinlessRechargeRequest pinRechargeRequest);
        Task<QueryTxnStatusResponse> QueryTransactionStatus(QueryTransactionStatusRequest queryTransactionStatusRequest);
    }
}