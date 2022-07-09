using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp
{
    public class PinlessRechargeRequest
    {
        public string Msisdn { get; set; }
        public decimal Amount { get; set; }
        public string transId { get; set; }
        public int rechargeType { get; set; }

    }
}
