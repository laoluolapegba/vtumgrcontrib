using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.ViewModels
{
    public class StockPurchaseOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public int PartnerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int tenantId { get; set; }
        public string UserId { get; set; }
        public List<Item> items { get; set; }
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
