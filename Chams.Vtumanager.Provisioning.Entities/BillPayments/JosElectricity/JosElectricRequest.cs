using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.JosElectricity
{
    public class JosElectricRequest
    {

        public JosElectricity details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class JosElectricity
        {
            public string accessCode { get; set; }
            public string customerPhoneNumber { get; set; }
            public int amount { get; set; }
        }

    }
}
