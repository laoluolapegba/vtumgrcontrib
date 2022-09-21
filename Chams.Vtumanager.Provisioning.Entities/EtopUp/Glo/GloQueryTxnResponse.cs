using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Glo
{
    public class GloQueryTxnResponse
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

            private executeReportResponse executeReportResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://external.interfaces.ers.seamless.com/")]
            public executeReportResponse executeReportResponse
            {
                get
                {
                    return this.executeReportResponseField;
                }
                set
                {
                    this.executeReportResponseField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://external.interfaces.ers.seamless.com/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://external.interfaces.ers.seamless.com/", IsNullable = false)]
        public partial class executeReportResponse
        {

            private @return returnField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public @return @return
            {
                get
                {
                    return this.returnField;
                }
                set
                {
                    this.returnField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class @return
        {

            private int resultCodeField;

            private string resultDescriptionField;

            private returnReport reportField;

            /// <remarks/>
            public int resultCode
            {
                get
                {
                    return this.resultCodeField;
                }
                set
                {
                    this.resultCodeField = value;
                }
            }

            /// <remarks/>
            public string resultDescription
            {
                get
                {
                    return this.resultDescriptionField;
                }
                set
                {
                    this.resultDescriptionField = value;
                }
            }

            /// <remarks/>
            public returnReport report
            {
                get
                {
                    return this.reportField;
                }
                set
                {
                    this.reportField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnReport
        {

            private string titleField;

            private string contentField;

            private string contentStringField;

            /// <remarks/>
            public string title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            public string content
            {
                get
                {
                    return this.contentField;
                }
                set
                {
                    this.contentField = value;
                }
            }

            /// <remarks/>
            public string contentString
            {
                get
                {
                    return this.contentStringField;
                }
                set
                {
                    this.contentStringField = value;
                }
            }
        }


    }
}
