using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities
{
    [Table("users")]
    public class User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Username { get; set; }

        [Column("lastname")]
        public string Lastname { get; set; }
        [Column("email")]
        public string EmailAddress { get; set; }
        [Column("email_verified_at")]
        public DateTime? EmailVerifiedDate { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("remember_token")]
        public string RememberToken { get; set; }

        [Column("has_change_password")]
        public int FlgPassword_Changed { get; set; }
        [Column("partner_id")]
        public int PartnerId { get; set; }

        [Column("phone")]
        public string PhoneNumber { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
        [Column("isactive")]
        public bool IsActive { get; set; }

        //public string Role { get; set; }
        //public string Lastname { get; set; }
        //public string Firstname { get; set; }
        //public string PrincipalCompany { get; set; }
        //public string PasswordSalt { get; set; }




    }
}
