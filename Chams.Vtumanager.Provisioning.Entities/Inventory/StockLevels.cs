using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Inventory
{
    [Table("vw_vtu_stock_levels")]
    public class StockLevels
    {
        [Key]
       
        [Column("partner_id")]
        public int PartnerId { get; set; }
        [Column("service_provider_id")]
        public int ServiceProviderId { get; set; }
        [Column("qoh")]
        public int Qoh { get; set; }
       
        [Column("last_order")]
        public int LastOrder { get; set; }
        

    }
}
