using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Epurse
{
    [Table("vtu_epurse_account")]
    public class EpurseAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("partner_id")]
        public int PartnerId { get; set; }
        [Column("tenant_id")]
        public int TenantId { get; set; }

        [Column("main_account_balance")]
        public decimal MainAcctBalance { get; set; }
        [Column("commision_account_balance")]
        public decimal CommissionAcctBalance { get; set; }

        [Column("reward_points")]
        public int RewardPoints { get; set; }
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
