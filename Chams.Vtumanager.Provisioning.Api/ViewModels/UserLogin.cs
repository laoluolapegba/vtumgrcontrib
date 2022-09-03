using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
