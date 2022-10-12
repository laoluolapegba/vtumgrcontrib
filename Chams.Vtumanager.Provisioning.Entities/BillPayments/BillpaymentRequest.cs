using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class BillpaymentRequest
    {

        public BillpaymentDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class BillpaymentDetails
        {
            
            public string[] productsCodes { get; set; }
            public int customerNumber { get; set; }
            public long smartcardNumber { get; set; }
            public string customerName { get; set; }
            public int invoicePeriod { get; set; }
            public int monthsPaidFor { get; set; }
            public string subscriptionType { get; set; }
            public int amount { get; set; }

            /// <summary>
            /// /others
            /// </summary>
            public string email { get; set; }
            public string customerAccountType { get; set; }
            public string contactType { get; set; }
            public string customerDtNumber { get; set; }
            public string phoneNumber { get; set; }
            public string customerReference { get; set; }
            public string customerAddress { get; set; }
            public string billingMethod { get; set; }
            public string description { get; set; }
            public string accountNumber { get; set; }
            public string tariff { get; set; }
            public string customerMobileNumber { get; set; }
            public string meterNumber { get; set; }
            public string customerPhone { get; set; }
            public string accessCode { get; set; }
            public string customerPhoneNumber { get; set; }
            public int numberOfPins { get; set; }
            public int pinValue { get; set; }
            public string purchaseType { get; set; }
            public string confirmationCode { get; set; }
            public string serviceType { get; set; }
        }

    }
}
