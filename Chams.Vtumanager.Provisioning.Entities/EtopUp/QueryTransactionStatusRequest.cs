using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp
{
    public class QueryTransactionStatusRequest
    {
        /// <summary>
        /// Telco service provider Enum
        /// </summary>
        [Required]
        public int ServiceProviderId { get; set; }
        /// <summary>
        /// an identified for this request
        /// </summary>
        [Required]
        public string transactionId { get; set; }
        /// <summary>
        /// The reference number of the transaction being queried
        /// </summary>
        [Required]
        public string TransactionReference { get; set; }
    }
}
