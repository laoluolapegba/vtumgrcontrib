using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy.ProxyValidateSmileBundleCustomer;

namespace Chams.Vtumanager.Provisioning.Services.BillPayments.Proxy
{
    public class ProxyCornerstone
    {

        public CornerstoneProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class CornerstoneProxyDetails
        {
            
            public string requestType { get; set; }
        }

    }
    public class ProxyCornerStoneCarModels
    {

        public CornerStoneCarModelsProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class CornerStoneCarModelsProxyDetails
        {
            public string vehicleManufacter { get; set; }
            public string requestType { get; set; }
        }

    }
    public class ProxyCornerStoneGetPolicyDetails
    {

        public GetPolicyDetailsProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class GetPolicyDetailsProxyDetails
        {
            public string policyId { get; set; }
            public string requestType { get; set; }
        }

    }
    public class ProxyCornerSton3rdpartyInsuranceQuotation
    {

        public CornerSton3rdpartyInsuranceQuotationProxyDetails details { get; set; }
        public string serviceId { get; set; }

        public class CornerSton3rdpartyInsuranceQuotationProxyDetails
        {
            public Policy policy { get; set; }
            public string requestType { get; set; }
        }
        public class Policy
        {
            public Client client { get; set; }
            public Vehicle vehicle { get; set; }
        }
        public class Client
        {
            public string address { get; set; }
            public string companyName { get; set; }
            public string dateOfBirth { get; set; }
            public string email { get; set; }
            public string firstName { get; set; }
            public string gender { get; set; }
            public string lastName { get; set; }
            public string mobile { get; set; }
            public string referredBy { get; set; }
            public string referrerDetails { get; set; }
        }

        public class Vehicle
        {
            public string chassisNumber { get; set; }
            public string engineNumber { get; set; }
            public string insuranceType { get; set; }
            public string policyholderType { get; set; }
            public string registrationNumber { get; set; }
            public string vehicleClass { get; set; }
            public string vehicleManufacturer { get; set; }
            public string vehicleModel { get; set; }
            public int yearOfManufacture { get; set; }
            public int yearOfPurchase { get; set; }
        }

    }


}
