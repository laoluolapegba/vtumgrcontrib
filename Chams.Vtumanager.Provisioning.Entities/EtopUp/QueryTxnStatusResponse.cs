using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp
{
    /// <summary>
    /// Response to query an earlier submitted topup transaction
    /// </summary>
    public class QueryTxnStatusResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string statusId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string responseMessage { get; set; }
        /// <summary>
        /// the service provider's reference
        /// </summary>
        public string exchangeReference { get; set; }
        /// <summary>
        /// the client's transation refrence
        /// </summary>

        public string transactionReference { get; set; }

    }
}
