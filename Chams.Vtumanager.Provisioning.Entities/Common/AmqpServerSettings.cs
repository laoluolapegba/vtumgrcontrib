using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Common
{
    public class AmqpServerSettings
    {
        public string HostName { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
