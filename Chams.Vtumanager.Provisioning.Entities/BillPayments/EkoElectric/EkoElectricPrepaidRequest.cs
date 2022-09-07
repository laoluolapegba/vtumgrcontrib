using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric
{
    public class EkoElectricPrepaidRequest
    {

        public EkoElectricPrepaidRequestDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class EkoElectricPrepaidRequestDetails
        {
            public string customerAddress { get; set; }
            public string customerDistrict { get; set; }
            public string customerName { get; set; }
            public string meterNumber { get; set; }
            public int amount { get; set; }
        }

    }
}
