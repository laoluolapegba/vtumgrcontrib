using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Jamb
{
    public class JambPINRequest
    {

        public JambCandidatePINDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class JambCandidatePINDetails
        {
            public string confirmationCode { get; set; }
            public string phoneNumber { get; set; }
            public string serviceType { get; set; }
            public int amount { get; set; }
        }

    }
}
