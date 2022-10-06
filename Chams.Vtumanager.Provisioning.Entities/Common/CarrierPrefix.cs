using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Common
{
    [Table("vtu_carrier_prefix")]
    public class CarrierPrefix
    {        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long RecordId { get; set; }
        [Column("carrier_prefix")]
        public string Prefix { get; set; }
        [Column("service_provider_id")]
        public int ServiceProviderId { get; set; }
        [Column("service_provider_name")]
        public string ServiceProviderName { get; set; }
        
    }
}
