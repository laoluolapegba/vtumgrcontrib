using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Glo
{
    public class GloTopupSettings
    {
        public InitiatorPrincipal Initiator{ get; set; }
        public class InitiatorPrincipal
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string Password { get; set; }
        }
        public string Url { get; set; }
    }
}
