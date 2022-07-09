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
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string PrincipalCompany { get; set; }
    }
}
