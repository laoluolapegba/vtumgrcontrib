using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    public class StockPurchaseRequest
    {
        public int PartnerId { get; set; }
        public int tenantId { get; set; }
        public int ServiceProviderId { get; set; }
        public int Quantity { get; set; }
    }
}
