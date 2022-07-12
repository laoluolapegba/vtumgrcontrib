using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.ViewModels
{
    public class AccountTopUpResponse
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
