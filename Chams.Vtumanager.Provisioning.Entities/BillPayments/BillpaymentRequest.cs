using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class BillpaymentRequest
    {

        public BillpaymentRequestDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }
        public string serviceHandlerId { get; set; }

        public class BillpaymentRequestDetails
        {
            //public BillpaymentDetails()
            //{
            //    meterNumber = "";
            //}
            public string[] productsCodes { get; set; }
            public string customerNumber { get; set; }
            public string smartcardNumber { get; set; }
            public string customerName { get; set; }
            public int invoicePeriod { get; set; }
            public int monthsPaidFor { get; set; }
            public string subscriptionType { get; set; }
            public decimal amount { get; set; }

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
            public string customerType { get; set; }
            public int numberOfPins { get; set; }
            public int pinValue { get; set; }
            public string purchaseType { get; set; }
            public string confirmationCode { get; set; }
            public string serviceType { get; set; }
            public string policyId { get; set; }

            public string address { get; set; }
            public string business { get; set; }
            public string chassisNumber { get; set; }
            public string contactName { get; set; }
            public string dateOfBirth { get; set; }            
            public string engineNumber { get; set; }
            public string gender { get; set; }
            public string insuredName { get; set; }
            public string occupation { get; set; }
            public string operation { get; set; }
            public string phone { get; set; }
            public string sector { get; set; }
            public string subriskCode { get; set; }
            public string tin { get; set; }
            public string vehicleColor { get; set; }
            public string vehicleMake { get; set; }
            public string vehicleModel { get; set; }
            public string vehicleOldRegistrationNumber { get; set; }
            public string vehicleRegistrationNumber { get; set; }
            public string vehicleType { get; set; }
            public string vehicleYear { get; set; }
            

        }

    }
}
