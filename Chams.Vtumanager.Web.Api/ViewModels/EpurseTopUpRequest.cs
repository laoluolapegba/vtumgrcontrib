using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class EpurseTopUpRequest
    {
        [Required]
        public int TenantId { get; set; }
        [Required]
        public int PartnerId { get; set; }
        [Required]
        public int ProductcategoryId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        

    }
}
