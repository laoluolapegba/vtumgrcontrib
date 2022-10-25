using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyIkejaPrepaid
    {

        public IkejaPrepaidProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class IkejaPrepaidProxyDetails
        {
            public string meterNumber { get; set; }
            
        }


    }
    public class ProxyIkejaPostpaid
    {

        public IkejaPostpaidProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class IkejaPostpaidProxyDetails
        {
            public string customerNumber { get; set; }

        }


    }
}
