using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesMgmt.Services.Evc.Worker.Entities.EtopUp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Hangfire.Services
{
    public interface ILightEvcService
    {
        
        Task<RechargeResponseEnvelope.Envelope> PinlessRecharge(PinlessRechargeRequest pinlessRechargeRequest);
        Task<QueryBalanceResponseEnvelope.Envelope> QueryEvcBalance(QueryBalanceRequest queryBalanceRequest);
    }
}
