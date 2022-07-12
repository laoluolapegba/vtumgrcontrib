using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Common
{
    [Table("vtu_epurse_accounts")]
    public class EpurseAccount
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("account_id")]
        public long RecordId { get; set; }
        [Column("partner_id")]
        public string   partner_id { get; set; }
        [Column("serviceprovider_id")]
        public int serviceprovider_id { get; set; }
        [Column("main_account_balance")]
        public decimal main_account_balance { get; set; }
        [Column("commision_account_balance")]
        public int commision_account_balance { get; set; }
        [Column("dat_last_credit")]
        public DateTime? LastCreditDate { get; set; }
        [Column("dat_last_debit")]
        public DateTime? LastDebitDate { get; set; }
        [Column("requested_by")]
        public string CreatedBy { get; set; }
        [Column("authorised_by")]
        public string AuthorisedBy { get; set; }

    }
}
