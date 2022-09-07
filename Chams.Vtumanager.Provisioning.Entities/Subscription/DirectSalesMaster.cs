using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Subscription
{
    [Table("direct_sales")]
    public class DirectSalesMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }
        [Column("is_approved")] 
        public int IsApproved { get; set; }
        [Column("is_processed")]
        public int IsProcessed { get; set; }
        [Column("updated_at")]
        public DateTime? LastUpdateDate { get; set; }

    }
}
