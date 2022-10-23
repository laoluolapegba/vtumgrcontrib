using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Kaduna
{
    public class KadunaElectricPostpaidRequest
    {

        public KadunaElectricPostpaidDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class KadunaElectricPostpaidDetails
        {
            public string meterNumber { get; set; }
            public int amount { get; set; }
            public string customerName { get; set; }
            public string customerAddress { get; set; }
            public string tariff { get; set; }
            public string customerMobileNumber { get; set; }
        }

    }
}
