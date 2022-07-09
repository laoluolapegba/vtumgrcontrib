using System;
using System.Collections.Generic;
using System.Text;

namespace SalesMgmt.Services.Evc.Worker.Entities.EtopUp
{
public  class ModifyEvcTransaction
{
        public string transId { get; set; }
        public string msisdn { get; set; }
        public decimal transAmount { get; set; }
        public string transDesc { get; set; }
    }
}
