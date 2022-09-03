using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class StockPurchaseRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public int PartnerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int tenantId { get; set; }
        /// <summary>
        /// Product category - fund transfer, bill payment, post,  paid pre-pad
        /// </summary>
        public int ProductCategoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Item[] items { get; set;   }
        
        /// <summary>
        /// 
        /// </summary>
        public class Item
        {
            /// <summary>
            /// 
            /// </summary>
            public int ServiceProviderId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Quantity { get; set; }
        }

    }
}
