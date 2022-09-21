using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class BillpaymentRequestRequest
    {

        public BillpaymentDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class BillpaymentDetails
        {
            public string customerReference { get; set; }
            public decimal amount { get; set; }
            public string customerName { get; set; }
            public string customerAddress { get; set; }
            public string[] productsCodes { get; set; }
            public int customerNumber { get; set; }
            public long smartcardNumber { get; set; }
            
            public int invoicePeriod { get; set; }
            public int monthsPaidFor { get; set; }
            public string subscriptionType { get; set; }
            public string email { get; set; }
            public string customerAccountType { get; set; }
            public string contactType { get; set; }
            public string customerDtNumber { get; set; }
            public string phoneNumber { get; set; }
            
        }

    }
}
