using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class EpurseAccountModel
    {
        [Required]
        public int PartnerId { get; set; }
        [Required]
        public int TenantId { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string AuthorisedBy { get; set; }
        //public decimal MainAcctBalance { get; set; }        
        //public decimal CommissionAcctBalance { get; set; }
        //public int RewardPoints { get; set; }
    }
}
