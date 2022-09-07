using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BusinessAccount
{
    [Table("users")]
    public class UserProfile1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public string id { get; set; }

        [Column("firstname")]
        public string Firstname { get; set; }
        [Column("lastname")]
        public string Lastname { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("partner_id")]
        public int PartnerId { get; set; }
    }
}
