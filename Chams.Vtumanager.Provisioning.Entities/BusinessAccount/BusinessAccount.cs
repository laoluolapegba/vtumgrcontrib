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
        [Column("partner_code")]
        public string AccountCode { get; set; }




    }
}
