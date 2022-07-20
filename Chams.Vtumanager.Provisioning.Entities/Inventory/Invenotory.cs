using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Inventory
{
    [Table("vtu_stock_details")]
    public class Stock_Details
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idvtu_stock_details")]
        public int RecordId { get; set; }
        [Column("partner_id")]
        public int   partner_id { get; set; }
        [Column("tenant_id")]
        public int tenant_id { get; set; }
        [Column("trans_type_id")]
        public int trans_type_id { get; set; }
        [Column("trans_date")]
        public DateTime trans_date { get; set; }
        [Column("quantity")]
        public int quantity { get; set; }
        [Column("service_provider_id")]
        public int service_provider_id { get; set; }
        [Column("product_id")]
        public string product_id { get; set; }
        [Column("price")]
        public decimal price { get; set; }
        [Column("user_id")]
        public string user_id { get; set; }


    }
}
