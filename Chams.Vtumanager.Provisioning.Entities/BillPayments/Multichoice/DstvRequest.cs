using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice
{
    public class DstvRequest
    {

        public class Rootobject
        {
            public Details details { get; set; }
            public string id { get; set; }
            public string paymentCollectorId { get; set; }
            public string paymentMethod { get; set; }
            public string serviceId { get; set; }
        }

        public class Details
        {
            public string[] productsCode { get; set; }
            public int customerNumber { get; set; }
            public int smartcardNumber { get; set; }
            public string customerName { get; set; }
            public int invoicePeriod { get; set; }
            public int monthsPaidFor { get; set; }
            public string subscriptionType { get; set; }
            public int amount { get; set; }
        }

    }
}
