using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyWAECPIN
    {

        public string serviceId { get; set; }

    }
    public class ProxyBulkSMS
    {

        public string serviceId { get; set; }

    }
    public class ProxySpectranetPIN
    {

        public string serviceId { get; set; }

    }
    public class ProxyShowmaxVouchers
    {

        public string serviceId { get; set; }

    }
    public class ProxySpectranetPaymentPlan
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class ProxyDetails
        {
            public string accountNumber { get; set; }
            public string requestType { get; set; }

        }

    }
    public class ProxyCarpaddy
    {

        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }


        public class ProxyDetails
        {
            public string registrationNumber { get; set; }
            

        }

    }
    public class ProxySmileRecharge
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
