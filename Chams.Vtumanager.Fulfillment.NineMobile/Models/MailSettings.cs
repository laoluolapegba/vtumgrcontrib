using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SmtpSettings
    {
        public string HostUrl { get; set; }
        public string Port { get; set; }
        public string SenderMail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CSLMail { get; set; }
        public string MailFromFriendlyname { get; set; }
    }
}
