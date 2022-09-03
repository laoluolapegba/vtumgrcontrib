using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Subscription
{
    [Table("notification_settings")]
    public class NotificationSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }

        [Column("is_processed")]
        public int IsProcessed { get; set; }
        [Column("created_date")]
        public DateTime? ProcessedDate { get; set; }
        [Column("notify_pct")]
        public int NotifyPct { get; set; }

        //public string ErrorDesc { get; set; }
    }
}
