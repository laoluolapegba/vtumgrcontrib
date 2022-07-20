using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn
{
    public class MtnSettings
{
        public Version1 V1 { get; set; }
        public Version2 V3 { get; set; }

        public class Version1
        {
            public string Url { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Version2
        {
            public string ClientId { get; set; }
            public string TokenUrl { get; set; }
            public string ClientSecret { get; set; }
            public string Url { get; set; }
        }
        public string PartnerMsisdn { get; set; }
    }
}
