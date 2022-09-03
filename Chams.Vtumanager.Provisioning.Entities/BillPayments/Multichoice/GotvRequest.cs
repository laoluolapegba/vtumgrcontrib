using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice
{
    public class GotvRequest
    {

        public GotvDetails details { get; set; }
        public int id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class GotvDetails
        {
            public string[] productsCodes { get; set; }
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
