using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sales_Mgmt.Services.Entities.EtopUp;
using SalesMgmt.Services.Entities.EtopUp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sales_Mgmt.Services.Services.Evc
{
    public interface ILightEvcService
    {
        Task<ModifyEvcResponseEnvelope.Envelope> ModifyEvcInventory(ModifyEvcTransaction modifyEvcTransaction,
           IConfiguration config,
           ILogger _logger,
           IHttpClientFactory _clientFactory);
    }
}
