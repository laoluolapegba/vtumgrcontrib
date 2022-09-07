using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Smile
{
    public class SmileCommBundleRequest
    {

        public SmileCommBundleDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class SmileCommBundleDetails
        {
            public int bundleTypeCode { get; set; }
            public int customerAccountId { get; set; }
            public int quantity { get; set; }
            public float amount { get; set; }
        }

    }
}
