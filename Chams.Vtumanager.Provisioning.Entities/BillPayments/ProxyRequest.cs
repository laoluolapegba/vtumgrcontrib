using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class ProxyRequest
    {
        public ProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class ProxyDetails
        {
            public string requestType { get; set; }
            //public string customerReference { get; set; }
            //public string customerReferenceType { get; set; }
            //public string customerAccountId { get; set; }
            
            //public string smartCardNumber { get; set; }
            
        }
    }
}
