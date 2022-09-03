using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Inventory
{
    [Table("vtu_stock_master")]
    public class StockMaster
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int RecordId { get; set; }
        [Column("tenant_id")]
        public int TenantId { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }
       
        [Column("service_provider_id")]
        public int ServiceProviderId { get; set; }

        [Column("qoh")]
        public int QuantityOnHand { get; set; }


    }
}
