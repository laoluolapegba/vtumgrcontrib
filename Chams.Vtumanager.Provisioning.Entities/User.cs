using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities
{
    public class User 
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string PrincipalCompany { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string PasswordSalt { get; set; }

    }
}
