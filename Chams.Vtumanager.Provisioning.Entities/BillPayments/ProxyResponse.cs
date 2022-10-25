using System;
using System.Collections.Generic;
using System.Text;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.ProxyResponse.ProxyResponseDetails;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class ProxyResponse
    {
        public string status { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public ProxyResponseDetails details { get; set; }

        public class ProxyResponseDetails
        {
            public string number { get; set; }
            public Bundle[] bundles { get; set; }
            public Pinvalue[] pinValues { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string middleName { get; set; }

            public class Bundle
            {
                public bool isAvailable { get; set; }
                public string name { get; set; }
                public int price { get; set; }
                public string datacode { get; set; }
                public string validity { get; set; }
                public string allowance { get; set; }
            }

            public class Pinvalue
            {
                public int amount { get; set; }
                public int count { get; set; }
            }


            public class Availablepricingoption
            {
                public int? monthsPaidFor { get; set; }
                public int? price { get; set; }
                public int? invoicePeriod { get; set; }
            }
            public string address { get; set; }
            public decimal? outstandingAmount { get; set; }
            public string name { get; set; }
            public string minimumAmount { get; set; }
            public object customerAccountType { get; set; }
            public string accountNumber { get; set; }
            public string customerDtNumber { get; set; }
            public string customerNumber { get; set; }
            public string customerAddress { get; set; }
            public string vendType { get; set; }
            public string meterNumber { get; set; }
            public int? daysSinceLastVend { get; set; }
            public decimal? minVendAmount { get; set; }
            public decimal? maxVendAmount { get; set; }
            public int? freeUnits { get; set; }
            public string company { get; set; }
            public string tariff { get; set; }
            public string responseMessage { get; set; }
            public string customerName { get; set; }
            public string responseCode { get; set; }

            public string businessUnit { get; set; }
            public string phoneNumber { get; set; }

            public string undertaking { get; set; }
            public string tariffCode { get; set; }
            public int customerArrears { get; set; }
            public int? minimumPurchase { get; set; }

            public string email { get; set; }
            public string serviceBand { get; set; }

            public string customerType { get; set; }
            public string phone { get; set; }
            public string customerReference { get; set; }
            public string meterType { get; set; }

            public string thirdPartyCode { get; set; }
            public string otherName { get; set; }

            public int? status { get; set; }
            public bool canVend { get; set; }

            public decimal? debtAmount { get; set; }


            public decimal? outStanding { get; set; }

            public string accessCode { get; set; }
            public string tariffRate { get; set; }
            public string feeder3311dt { get; set; }

            public string customerDetails { get; set; }
            public string referenceId { get; set; }

            public string accountBalance { get; set; }
            public string account { get; set; }
            public string[] addonitems { get; set; }
            public Item[] items { get; set; }
            public string accountStatus { get; set; }

            public int? invoicePeriod { get; set; }
            public DateTime? dueDate { get; set; }

            public int? amount { get; set; }

        }

        public class Item
        {
            public Availablepricingoption[] availablePricingOptions { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public string description { get; set; }

            public int price { get; set; }

        }

    }







}
