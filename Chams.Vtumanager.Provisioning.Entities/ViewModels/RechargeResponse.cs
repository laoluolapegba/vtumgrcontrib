using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Entities.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class RechargeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int ResponseCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ResponseDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TransactionReference { get; set; }
    }
}
