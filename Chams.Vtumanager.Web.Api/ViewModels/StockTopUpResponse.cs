using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class StockTopUpResponse
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
