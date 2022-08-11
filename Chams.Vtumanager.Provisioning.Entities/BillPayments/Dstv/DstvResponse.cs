using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Dstv
{
    public class DstvResponse
    {
        public int transactionNumber { get; set; }
        public ResponseDetails details { get; set; }
        public class ResponseDetails
        {
            public string customerCareReferenceId { get; set; }
            public string exchangeReference { get; set; }
            public string errorMessage { get; set; }
            public string errorCode { get; set; }
            public string errorId { get; set; }
            public string auditReferenceNumber { get; set; }
            public bool done { get; set; }
            public string status { get; set; }
        }


    }
}
