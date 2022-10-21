using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class BillPaymentsResponse
    {
        public string transactionNumber { get; set; }
        public ResponseDetails details { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
        public class ResponseDetails
        {
            public string customerCareReferenceId { get; set; }
            public string exchangeReference { get; set; }
            
            public string errorId { get; set; }
            public string auditReferenceNumber { get; set; }
            public bool done { get; set; }
            public string status { get; set; }
            public string responseCode { get; set; }
            public string responseMessage { get; set; }
        }


        

    }
}
