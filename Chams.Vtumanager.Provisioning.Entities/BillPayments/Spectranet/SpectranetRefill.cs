using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Spectranet
{
    public class SpectranetRefillRequest
    {

        public SpectranetRefillDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class SpectranetRefillDetails
        {
            public string accountNumber { get; set; }
            public string customerName { get; set; }
            public string description { get; set; }
            public int amount { get; set; }
        }

    }
}
