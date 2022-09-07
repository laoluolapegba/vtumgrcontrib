using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.IbadanDisco
{
    public class IbadanDiscoRequest
    {

        public IbadanDiscoDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class IbadanDiscoDetails
        {
            public string customerReference { get; set; }
            public string customerType { get; set; }
            public string customerName { get; set; }
            public string thirdPartyCode { get; set; }
            public string serviceBand { get; set; }
            public int amount { get; set; }
        }

    }
}
