using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Services.GloTopup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MtnProcessor 
    {
        private readonly PinlessRechargeRequest _pinlessRechargeRequest;
        private readonly IGloTopupService _gloTopupService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinlessRechargeRequest"></param>
        /// <param name="gloTopupService"></param>
        public MtnProcessor(PinlessRechargeRequest pinlessRechargeRequest, IGloTopupService gloTopupService)
        {
            _pinlessRechargeRequest = pinlessRechargeRequest;
            _gloTopupService = gloTopupService;
        }
        /// <summary>
        /// 
        /// </summary>
        public void DoProcessing()
        {
        }
    }
}
