using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Epurse
{
    [Table("vtu_epurse_acct_transactions")]
    public class EpurseAcctTransactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("transaction_id")]
        public int TranId { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }
        [Column("tenant_id")]
        public int TenantId { get; set; }
        [Column("account_no")]
        public string AccountNo { get; set; }


        [Column("tran_date")]
        public DateTime TranDate { get; set; }

        [Column("tran_amount")]
        public decimal TranAmount { get; set; }
        [Column("tran_desc")]
        public string TranDesc { get; set; }
        [Column("cod_dr_cr")]
        public string DrCr { get; set; }

        [Column("product_code")]
        public string ProductCode { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }


        [Column("auth_id")]
        public string AuthId { get; set; }

        [Column("service_provider_id")]
        public int ServiceProviderId { get; set; }

    }
}
