using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BillPayments
{
    public class ServiceListResponse
    {

        //public Services[] serviceList { get; set; }

        public string description { get; set; }
        public string serviceId { get; set; }
        public bool enabled { get; set; }

        //public class Rootobject
        //{
        //    public Class1[] Property1 { get; set; }
        //}

        //public class Class1
        //{
        //    public string description { get; set; }
        //    public string serviceId { get; set; }
        //    public bool enabled { get; set; }
        //}

    }
}
