using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn
{
    public class MtnResponseEnvelope
    {

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement("Body")]
            public Body body { get; set; }
        }
        public class Body
        {
            [XmlElement(ElementName = "vendResponse", Namespace = "http://hostif.vtm.prism.co.za/xsd")]
            public VendResponse vendResponse { get; set; }
        }
        public class VendResponse
        {
            public string sequence { get; set; }
            public string statusId { get; set; }
            public string txRefId { get; set; }
            public string origBalance { get; set; }
            public string origMsisdn { get; set; }
            public string destMsisdn { get; set; }
            public int responseCode { get; set; }
            public string responseMessage { get; set; }
        }
    }

}

