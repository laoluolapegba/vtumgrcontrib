using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS
{
    public class BulkSMSRequest
    {

        public BulkSMSDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class BulkSMSDetails
        {
            public int numberOfPins { get; set; }
            public int pinValue { get; set; }
            public int amount { get; set; }
        }

    }
}
