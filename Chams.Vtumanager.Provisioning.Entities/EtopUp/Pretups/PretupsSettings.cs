using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Pretups
{
    /// <summary>
    /// 
    /// </summary>
    public class PretupsSettings
    {
        public TransactionType transactionType { get; set; }
        public class TransactionType
        {
            public string TransactionStatus { get; set; }
            public string AirtimePurchase { get; set; }
            public string DataPurchase { get; set; }
            public string LogicalVoucher { get; set; }
            public string BalanceRequest { get; set; }
        }
        public string Url { get; set; }
        public string PIN { get; set; }
        public string PartnerCode { get; set; }

        public string PartnerMsisdn { get; set; }
    }
}
