using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.ViewModels
{
    public class StockBalanceView
    {
        public int PartnerId { get; set; }
        public int ProductCategoryId { get; set; }
        public int ServiceProviderId { get; set; }
        public decimal Qoh { get; set; }

        
    }
}
