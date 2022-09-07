using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco
{
    public class AbujaPostpaidRequest
    {

        public AbujaPostpaidRequestDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class AbujaPostpaidRequestDetails
        {
            public string customerReference { get; set; }
            public int amount { get; set; }
            public string customerName { get; set; }
            public string customerAddress { get; set; }
        }

    }
}
