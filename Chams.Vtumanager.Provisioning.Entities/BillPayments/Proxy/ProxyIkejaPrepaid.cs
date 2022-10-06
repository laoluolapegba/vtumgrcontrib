using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyIkejaPrepaid
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class ProxyDetails
        {
            public string meterNumber { get; set; }
            
        }


    }
    public class ProxyIkejaPostpaid
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class ProxyDetails
        {
            public string customerNumber { get; set; }

        }


    }
}
