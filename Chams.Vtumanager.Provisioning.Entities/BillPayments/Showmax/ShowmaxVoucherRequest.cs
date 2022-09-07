using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Showmax
{
    public class ShowmaxVoucherRequest
    {

        public ShowmaxVoucherDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class ShowmaxVoucherDetails
        {
            public int amount { get; set; }
            public string customerPhoneNumber { get; set; }
            public int subscriptionPeriod { get; set; }
            public string subscriptionType { get; set; }
        }

    }
}
