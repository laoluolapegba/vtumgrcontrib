using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyPortharcoutPostpaid
    {

        public PortharcoutPostpaidProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class PortharcoutPostpaidProxyDetails
        {
            public string requestType { get; set; }
            public string accountType { get; set; }
            public string meterNumber { get; set; }
            public string phone { get; set; }
        }


    }
}
