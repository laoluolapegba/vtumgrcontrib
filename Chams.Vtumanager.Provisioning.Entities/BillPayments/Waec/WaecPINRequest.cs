using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec
{
    public class WaecPINRequest
    {

        public WaecPINDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class WaecPINDetails
        {
            public int numberOfPins { get; set; }
            public int pinValue { get; set; }
            public int amount { get; set; }
        }

    }
}
