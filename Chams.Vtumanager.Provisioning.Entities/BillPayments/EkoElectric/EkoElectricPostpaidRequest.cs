using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric
{
    public class EkoElectricPostpaidRequest
    {

        public EkoElectricPostpaidDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class EkoElectricPostpaidDetails
        {
            public string customerReference { get; set; }
            public int amount { get; set; }
        }

    }
}
