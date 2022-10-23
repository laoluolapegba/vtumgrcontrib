using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Kedco
{
    public class KedcoElectricPostpaidRequest
    {

        public KedcoElectricityPostpaidDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class KedcoElectricityPostpaidDetails
        {
            public string customerReference { get; set; }
            public string tariffCode { get; set; }
            public int amount { get; set; }
        }

    }
}
