using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.PortharcourtElectric
{
    public class PortHarcourtElectricPostpaidRequest
    {

        public PortHarcourtElectricPostpaidDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class PortHarcourtElectricPostpaidDetails
        {
            public string customerName { get; set; }
            public string meterNumber { get; set; }
            public string customerPhone { get; set; }
            public string customerNumber { get; set; }
            public int amount { get; set; }
        }

    }
}
