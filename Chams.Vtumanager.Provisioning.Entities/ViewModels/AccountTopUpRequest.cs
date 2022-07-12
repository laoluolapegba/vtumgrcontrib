using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.ViewModels
{
    public  class AccountTopUpRequest
    {
        public string PartnerId { get; set; }
        public decimal Amount { get; set; }
        public int ServiceProviderId { get; set; }
        public string  CreatedBy { get; set; }
        public string AuthorisedBy { get; set; }
    }
}
