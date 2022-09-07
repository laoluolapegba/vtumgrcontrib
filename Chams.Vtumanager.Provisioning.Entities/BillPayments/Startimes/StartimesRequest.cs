using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Startimes
{
    public class StartimesRequest
    {

        public StartTimesDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class StartTimesDetails
        {
            public string smartCardNumber { get; set; }
            public int amount { get; set; }
        }

    }
}
