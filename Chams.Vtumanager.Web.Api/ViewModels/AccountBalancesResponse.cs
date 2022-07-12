using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Web.Api.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountBalancesResponseView
    {
        public string AccountCode { get; set; }
        public int ServiceProviderId { get; set; }
        public decimal MainAcctBalance { get; set; }
        public decimal CommissionAcctBalance { get; set; }

    }
}
