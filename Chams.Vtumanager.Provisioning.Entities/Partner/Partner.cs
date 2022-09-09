using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Partner
{
    [Table("users")]
    public class PartnerOld
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Column("partner_name")]
        public string PartnerName { get; set; }
        [Column("code")]
        public string PartnerCode { get; set; }
        
    }
}
