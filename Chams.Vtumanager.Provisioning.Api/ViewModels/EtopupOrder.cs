using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales_Mgmt.Services.Simplex.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class EtopupOrder
    {
        /// <summary>
        /// unique transaction identifier
        /// </summary>
        [Required]
        public string transId { get; set; }
        /// <summary>
        /// related order number
        /// </summary>
        [Required]
        public string OrderNo { get; set; }
        /// <summary>
        /// evc acct code to debit
        /// </summary>
        [Required]
        public string PartnerEvcAcctCode { get; set; }
        /// <summary>
        /// subscriber phone number to credit
        /// </summary>
        [Required]
        public string SubscriberMsisdn { get; set; }
        /// <summary>
        /// Amount of etopup to purchase
        /// </summary>
        [Required]
        public decimal TranAmount { get; set; }


    }
}
