using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Subscription
{
    [Table("direct_sales_record")]
    public class DirectSalesDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Int64 Id { get; set; }

        [Column("request_id")]
        public Int64 request_id { get; set; }
        [Column("telco")]
        public int ServiceProviderId { get; set; }
        [Column("phone")]
        public string Msisdn { get; set; }
        [Column("product_code")]
        public string ProductCode { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("is_processed")]
        public int IsProcessed { get; set; }
        [Column("request_type")]
        public int RequestType { get; set; }
        [Column("transref")]
        public string TransRef { get; set; }

        [Column("updated_at")]
        public DateTime? LastUpdateDate { get; set; }
        //[Column("retry_count")]
        //public int Retries { get; set; }
        //[Column("error_code")]
        //public string ErrorCode { get; set; }
        //[Column("error_description")]
        //public string ErrorDesc { get; set; }
    }
}
