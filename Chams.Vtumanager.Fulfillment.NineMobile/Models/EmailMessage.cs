using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Models
{
    public class EmailMessage
    {
        public EmailAddress ToAddress { get; set; }
        public EmailAddress FromAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
