using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyAbujaPostpaid
    {

        public AbujaPostpaidProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class AbujaPostpaidProxyDetails
        {
            public string customerReference { get; set; }
            public string customerReferenceType { get; set; }
        }

    }
}
