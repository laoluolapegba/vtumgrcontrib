using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities
{
    [Table("vtu_api_credentials")]
    public class ApiCredentials
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }

        [Column("api_key")]
        public string ApiKey { get; set; }
        [Column("active")]
        public bool Active { get; set; }
    }
}
