using System;
using System.Collections.Generic;
using System.Text;

namespace SalesMgmt.Services.Evc.Worker.Entities.Common
{
    public  class EvcSettings
    {
        public PinlessRecharge pinlessRecharge { get; set; }
        public ModifyInventory modifyInventory { get; set; }
        public QueryBalance queryBalance { get; set; }
        public class PinlessRecharge        {
            public string Url { get; set; }
            public string SoapAction { get; set; }
            public string processTypeID { get; set; }
            public string sourceID { get; set; }
            public string RechargeType { get; set; }
            public string Channel_ID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Key { get; set; }
            public string Token { get; set; }
            
        }

        public class QueryBalance
        {
            public string Url { get; set; }
            public string SoapAction { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string UserType { get; set; }
        }

        public class ModifyInventory
        {
            public string Url { get; set; }
            public string SoapAction { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public string Certname { get; set; }
        public string CertPassphrase { get; set; }
    }
}
