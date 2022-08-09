using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Inventory
{
    [Table("vtu_telco_products")]
    public class VtuProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int RecordId { get; set; }
        
        [Column("tenant_id")]
        public int TenantUd { get; set; }
        [Column("product_id")]
        public string ProductId { get; set; }


        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("service_provider_id")]
        public int ServiceProviderId { get; set; }
        [Column("product_type")]
        public int ProductType { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
        [Column("validity")]
        public int Validity { get; set; }
    }
}
