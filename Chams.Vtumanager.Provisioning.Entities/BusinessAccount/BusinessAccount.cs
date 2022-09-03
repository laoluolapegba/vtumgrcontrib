using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BusinessAccount 
{

    [Table("partners")]
    public class BusinessAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int PartnerId { get; set; }
        
        [Column("partner_name")]
        public string PartnerName { get; set; }
        [Column("code")]
        public string PartnerCode { get; set; }
        [Column("business_category_id")]
        public int PartnerCategory { get; set; }


    }
}
