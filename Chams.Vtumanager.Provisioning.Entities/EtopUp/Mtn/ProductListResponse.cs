using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn
{
    public class ProductListResponse
    {

        public class Rootobject
        {
            public string statusCode { get; set; }
            public string transactionId { get; set; }
            public Data data { get; set; }
            public Link[] links { get; set; }
        }

        public class Data
        {
            public Others others { get; set; }
        }

        public class Others
        {
            public Daily[] daily { get; set; }
            public object[] weekend { get; set; }
            public Weekly[] weekly { get; set; }
            public Monthly[] monthly { get; set; }
            public Day[] days { get; set; }
            public Link[] links { get; set; }
        }

        public class Daily
        {
            public string id { get; set; }
            public string name { get; set; }
            public string currency { get; set; }
            public int amount { get; set; }
            public string bundleCategory { get; set; }
            public string subCategory { get; set; }
            public string bundleValidity { get; set; }
            public string description { get; set; }
            public string[] paymentMode { get; set; }
            public bool buyForOthers { get; set; }
            public bool promotionApplicable { get; set; }
            public string[] actions { get; set; }
            public bool renewal { get; set; }
            public string activationId { get; set; }
        }

        public class Weekly
        {
            public string id { get; set; }
            public string name { get; set; }
            public string currency { get; set; }
            public int amount { get; set; }
            public string bundleCategory { get; set; }
            public string subCategory { get; set; }
            public string bundleValidity { get; set; }
            public string description { get; set; }
            public string[] paymentMode { get; set; }
            public bool buyForOthers { get; set; }
            public bool promotionApplicable { get; set; }
            public string[] actions { get; set; }
            public bool renewal { get; set; }
            public string activationId { get; set; }
        }

        public class Monthly
        {
            public string id { get; set; }
            public string name { get; set; }
            public string currency { get; set; }
            public int amount { get; set; }
            public string bundleCategory { get; set; }
            public string subCategory { get; set; }
            public string bundleValidity { get; set; }
            public string description { get; set; }
            public string[] paymentMode { get; set; }
            public bool buyForOthers { get; set; }
            public bool promotionApplicable { get; set; }
            public string[] actions { get; set; }
            public bool renewal { get; set; }
            public string activationId { get; set; }
        }

        public class Day
        {
            public string id { get; set; }
            public string name { get; set; }
            public string currency { get; set; }
            public int amount { get; set; }
            public string bundleCategory { get; set; }
            public string subCategory { get; set; }
            public string bundleValidity { get; set; }
            public string description { get; set; }
            public string[] paymentMode { get; set; }
            public bool buyForOthers { get; set; }
            public bool promotionApplicable { get; set; }
            public string[] actions { get; set; }
            public bool renewal { get; set; }
            public string activationId { get; set; }
        }

        public class Link
        {
            public string rel { get; set; }
            public string href { get; set; }
        }

    }
}
