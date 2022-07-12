using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class StockTopUpRequest
    {
        public string PartnerId { get; set; }
        public decimal Amount { get; set; }
        public int ServiceProviderId { get; set; }

    }
}
