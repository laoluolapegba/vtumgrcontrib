using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
   

    public class StockPurchaseRequest1
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PartnerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Required]
        public int tenantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Required]
        public int ServiceProviderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Required]
        [Range( 1,999999)]
        public int Quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string UserId { get; set; }
    }
}
