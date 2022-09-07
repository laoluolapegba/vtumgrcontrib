using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments.Mutual
{
    public class MutualMortorInsuranceRequest
    {

        public MutualMortorInsurancDetails details { get; set; }
        public string id { get; set; }
        public string paymentCollectorId { get; set; }
        public string paymentMethod { get; set; }
        public string serviceId { get; set; }

        public class MutualMortorInsurancDetails
        {
            public string address { get; set; }
            public string business { get; set; }
            public string chassisNumber { get; set; }
            public string contactName { get; set; }
            public string dateOfBirth { get; set; }
            public string email { get; set; }
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
            public int amount { get; set; }
        }

    }
}
