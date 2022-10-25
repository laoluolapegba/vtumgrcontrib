using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyIbadanPrepaid
    {

        public IbadanPrepaidProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class IbadanPrepaidProxyDetails
        {
            public string customerReference { get; set; }
            
        }


    }
    public class ProxyIbadanPostpaid
    {

        public IbadanPostpaidProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class IbadanPostpaidProxyDetails
        {
            public string customerReference { get; set; }

        }


    }
}
