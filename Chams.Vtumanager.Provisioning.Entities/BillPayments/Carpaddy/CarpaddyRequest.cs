using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Carpaddy
{
    public class CarpaddyRequest
    {
        public int amount { get; set; }
        public string serviceHandlerId { get; set; }
        public string serviceId { get; set; }

    }
}
