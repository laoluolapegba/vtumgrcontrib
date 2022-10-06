using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyKadunaPrepaid
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class ProxyDetails
        {
            public string meterNumber { get; set; }
            
        }


    }
    public class ProxyKadunaPostpaid
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class ProxyDetails
        {
            public string meterNumber { get; set; }

        }


    }
}
