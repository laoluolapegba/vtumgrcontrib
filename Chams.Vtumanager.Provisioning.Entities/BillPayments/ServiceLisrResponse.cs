using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class ServiceLisrResponse
    {

        public Services[] serviceList { get; set; }

        public class Services
        {
            public string description { get; set; }
            public string serviceId { get; set; }
            public bool enabled { get; set; }
        }

    }
}
