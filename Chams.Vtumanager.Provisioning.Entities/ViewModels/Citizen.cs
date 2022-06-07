using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.ViewModels
{
 public   class Citizen
{
        public string NIN { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string MothersMaidenname { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string MobileNumber { get; set; }
        public string PhotoBase64 { get; set; }
    }
}
