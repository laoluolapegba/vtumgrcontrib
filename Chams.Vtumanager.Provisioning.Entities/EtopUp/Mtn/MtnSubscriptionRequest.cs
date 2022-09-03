using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn
{
    public class MtnSubscriptionRequest
    {
        public string subscriptionId { get; set; }
        public string beneficiaryId { get; set; }
        public string amountCharged { get; set; }
        public string subscriptionProviderId { get; set; }
        public string correlationId { get; set; }

    }
}
