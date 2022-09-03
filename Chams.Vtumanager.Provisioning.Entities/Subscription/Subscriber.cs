using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Subscription
{
    [Table("subscribers")]
    public class Subscriber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("email")]
        public string EmailAddress { get; set; }

        [Column("product_code")]
        public string ProductCode { get; set; }

        [Column("monthly_credit")]
        public int CreditAmount { get; set; }
        [Column("partner_id")]
        public int PartnerId { get; set; }        

        [Column("cycle_id")]
        public int CycleId { get; set; }
        [Column("request_type")]
        public int RequestType { get; set; }
        [Column("telco")]
        public int ServiceProviderId { get; set; }
        [Column("phone")]
        public string Phone { get; set; }

        [Column("transaction_status")]
        public int TransactionStatus { get; set; }
        [Column("error_code")]
        public string ErrorCode { get; set; }
        [Column("error_description")]
        public string ErrorDescription { get; set; }
        [Column("transref")]
        public string TransRef { get; set; }
        [Column("updated_at")]
        public DateTime? LastUpdateDate { get; set; }
    }
}
