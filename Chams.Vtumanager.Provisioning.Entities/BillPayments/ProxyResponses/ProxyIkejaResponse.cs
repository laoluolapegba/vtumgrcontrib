using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.ProxyResponses
{
    public class ProxyIkejaResponseDetails
    {
        public bool canVend { get; set; }
        public string address { get; set; }
        public string meterNumber { get; set; }
        public decimal? minimumAmount { get; set; }
        public string name { get; set; }
        public string customerAccountType { get; set; }
        public string accountNumber { get; set; }
        public string customerDtNumber { get; set; }
        public decimal debtAmount { get; set; }

        //public class ProxyIkejaResponseDetails
        //{
        //    public bool canVend { get; set; }
        //    public string address { get; set; }
        //    public string meterNumber { get; set; }
        //    public decimal? minimumAmount { get; set; }
        //    public string name { get; set; }
        //    public string customerAccountType { get; set; }
        //    public string accountNumber { get; set; }
        //    public string customerDtNumber { get; set; }
        //    public decimal debtAmount { get; set; }
        //}

    }
}
