using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Common
{
    [Table("SLP_CONNECTION_PARAMS")]
    public class ConfigParameters
    {
        [Key]
        public string id { get; set; }
        public string PARAM_NAME { get; set; }
        public string PARAM_VALUE { get; set; }
    }
}
