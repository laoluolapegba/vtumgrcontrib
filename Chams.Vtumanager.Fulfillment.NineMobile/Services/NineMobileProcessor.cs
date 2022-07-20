using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Services.NineMobileEvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class NineMobileProcessor : IServiceProvider
    {
        private PinlessRechargeRequest _pinlessRechargeRequest;
        private readonly ILightEvcService _evcService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinlessRechargeRequest"></param>
        /// <param name="evcService"></param>
        public NineMobileProcessor(PinlessRechargeRequest pinlessRechargeRequest, ILightEvcService evcService)
        {
            _pinlessRechargeRequest = pinlessRechargeRequest;
            _evcService = evcService;
        }
        /// <summary>
        /// 
        /// </summary>
        public void DoProcessing()
        {
            //RechargeResponseEnvelope.Envelope env1 = new RechargeResponseEnvelope.Envelope();
            //env1 = await _evcService.PinlessRecharge(pinlessRechargeRequest);
            //if (env1.Body != null)
            //{
            //    if (env1.Body.SDF_Data.result.statusCode == "0")
            //    {
            //        await UpdateTaskStatusAsync(item.RecordId,
            //            env1.Body.SDF_Data.result.statusCode.ToString(),
            //            env1.Body.SDF_Data.result.errorDescription
            //            );
            //        evctransId = env1.Body.SDF_Data.result.instanceId;
            //    }
            //    else
            //    {
            //        await UpdateFailedTaskStatusAsync(item.RecordId,
            //            env1.Body.SDF_Data.result.statusCode.ToString(),
            //            env1.Body.SDF_Data.result.errorDescription
            //            );
            //    }
            //}
            //else
            //{
            //    await UpdateFailedTaskStatusAsync(item.RecordId, "99", "Web Service Failed");
            //}

        }
    }
}
