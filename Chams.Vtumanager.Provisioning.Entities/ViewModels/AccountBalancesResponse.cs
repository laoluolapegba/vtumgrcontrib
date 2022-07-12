using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.ViewModels
{
    public class AccountBalancesResponse
    {
        public string AccountCode { get; set; }
        public int ServiceProviderId { get; set; }
        public decimal MainAcctBalance { get; set; }
        public decimal CommissionAcctBalance { get; set; }

    }
}
