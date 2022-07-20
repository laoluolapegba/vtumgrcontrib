using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Glo
{
    public class GloAirtimeResultEnvelope
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private EnvelopeBody bodyField;

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private VendResponse vendResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public VendResponse VendResponse
            {
                get
                {
                    return this.vendResponseField;
                }
                set
                {
                    this.vendResponseField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class VendResponse
        {

            private string destAccountField;

            private decimal amountField;

            private string statusIdField;

            private string statusMessageField;

            private string txRefIdField;

            private int responseCodeField;

            private string responseMessageField;

            /// <remarks/>
            public string DestAccount
            {
                get
                {
                    return this.destAccountField;
                }
                set
                {
                    this.destAccountField = value;
                }
            }

            /// <remarks/>
            public decimal Amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }

            /// <remarks/>
            public string StatusId
            {
                get
                {
                    return this.statusIdField;
                }
                set
                {
                    this.statusIdField = value;
                }
            }

            /// <remarks/>
            public string StatusMessage
            {
                get
                {
                    return this.statusMessageField;
                }
                set
                {
                    this.statusMessageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string TxRefId
            {
                get
                {
                    return this.txRefIdField;
                }
                set
                {
                    this.txRefIdField = value;
                }
            }

            /// <remarks/>
            public int ResponseCode
            {
                get
                {
                    return this.responseCodeField;
                }
                set
                {
                    this.responseCodeField = value;
                }
            }

            /// <remarks/>
            public string ResponseMessage
            {
                get
                {
                    return this.responseMessageField;
                }
                set
                {
                    this.responseMessageField = value;
                }
            }
        }


    }
}
