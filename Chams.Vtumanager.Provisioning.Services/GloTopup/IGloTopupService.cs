using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Glo;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.GloTopup
{
    public interface IGloTopupService
    {
        Task<GloAirtimeResultEnvelope.Envelope> GloAirtimeRecharge(PinlessRechargeRequest pinRechargeRequest);
        Task<GloDataResultEnvelope.Envelope> GloDataRecharge(PinlessRechargeRequest pinRechargeRequest);
    }
}