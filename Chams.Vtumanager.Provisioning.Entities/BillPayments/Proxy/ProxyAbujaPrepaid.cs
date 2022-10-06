using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyAbujaPrepaid
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class ProxyDetails
        {
            public string customerReference { get; set; }
            public string customerReferenceType { get; set; }
        }

    }
}
