using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyValidateSmileBundleCustomer
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class ProxyDetails
        {
            public string customerAccountId { get; set; }
            public string requestType { get; set; }
        }

        

    }
    public class ProxySmileGetBundles
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class ProxyDetails
        {
            public string customerAccountId { get; set; }
            public string requestType { get; set; }
        }



    }

}
