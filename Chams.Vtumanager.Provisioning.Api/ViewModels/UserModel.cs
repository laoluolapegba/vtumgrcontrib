using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Lastname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrincipalCompany { get; set; }
    }
}
