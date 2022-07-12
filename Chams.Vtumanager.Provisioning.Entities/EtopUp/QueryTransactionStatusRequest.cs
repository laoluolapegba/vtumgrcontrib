using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp
{
    public class QueryTransactionStatusRequest
    {
        public int ServiceProviderId { get; set; }
        public string TransactionReference { get; set; }
    }
}
