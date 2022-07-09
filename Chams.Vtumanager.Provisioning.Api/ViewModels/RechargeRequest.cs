using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chams.Vtumanager.Provisioning.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class RechargeRequest1
    {
        /// <summary>
        /// Unique Transaction identifier
        /// </summary>
        [Required]
        public string TransactionReference { get; set; }
        /// <summary>
        /// Telecommunication Operator 
        /// </summary>
        public int ServiceProviderId { get; set; }
        /// <summary>
        /// Airtime or Data 1= airtime , 2 = data
        /// </summary>
        public int TransactionType { get; set; }
        /// <summary>
        /// Product Id defined by service provider
        /// </summary>
        [Required] 
        public string ProductId { get; set; }
        /// <summary>
        /// Subscriber phonenumber
        /// </summary>
        [Required] 
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Recharge amount
        /// </summary>
        public decimal rechargeAmount { get; set; }
        /// <summary>
        /// Source channel - ussd, sms, web, mobile
        /// </summary>
        public int ChannelId { get; set; }
        /// <summary>
        /// Assigned source system identifier
        /// </summary>
        [Required]
        public string SourceSystemId { get; set; }
        
        

    }
}
