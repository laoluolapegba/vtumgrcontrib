using System;
using System.Collections.Generic;
using System.Text;
using static Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy.ProxyAbujaPostpaid;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyAbujaPrepaid
    {

        public AbujaPrepaidProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class AbujaPrepaidProxyDetails
        {
            public string customerReference { get; set; }
            public string customerReferenceType { get; set; }
        }

    }
}
