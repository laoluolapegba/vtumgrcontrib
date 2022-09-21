using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Glo
{
    public class GloQueryTxnRequest
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private object headerField;

            private EnvelopeBody bodyField;

            /// <remarks/>
            public object Header
            {
                get
                {
                    return this.headerField;
                }
                set
                {
                    this.headerField = value;
                }
            }

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

            private executeReport executeReportField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://external.interfaces.ers.seamless.com/")]
            public executeReport executeReport
            {
                get
                {
                    return this.executeReportField;
                }
                set
                {
                    this.executeReportField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://external.interfaces.ers.seamless.com/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://external.interfaces.ers.seamless.com/", IsNullable = false)]
        public partial class executeReport
        {

            private context contextField;

            private string reportIdField;

            private string languageField;

            private parameters parametersField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public context context
            {
                get
                {
                    return this.contextField;
                }
                set
                {
                    this.contextField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public string reportId
            {
                get
                {
                    return this.reportIdField;
                }
                set
                {
                    this.reportIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public string language
            {
                get
                {
                    return this.languageField;
                }
                set
                {
                    this.languageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public parameters parameters
            {
                get
                {
                    return this.parametersField;
                }
                set
                {
                    this.parametersField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class context
        {

            private string channelField;

            private string clientCommentField;

            private string clientIdField;

            private string clientReferenceField;

            private ushort clientRequestTimeoutField;

            private contextInitiatorPrincipalId initiatorPrincipalIdField;

            private ushort passwordField;

            /// <remarks/>
            public string channel
            {
                get
                {
                    return this.channelField;
                }
                set
                {
                    this.channelField = value;
                }
            }

            /// <remarks/>
            public string clientComment
            {
                get
                {
                    return this.clientCommentField;
                }
                set
                {
                    this.clientCommentField = value;
                }
            }

            /// <remarks/>
            public string clientId
            {
                get
                {
                    return this.clientIdField;
                }
                set
                {
                    this.clientIdField = value;
                }
            }

            /// <remarks/>
            public string clientReference
            {
                get
                {
                    return this.clientReferenceField;
                }
                set
                {
                    this.clientReferenceField = value;
                }
            }

            /// <remarks/>
            public ushort clientRequestTimeout
            {
                get
                {
                    return this.clientRequestTimeoutField;
                }
                set
                {
                    this.clientRequestTimeoutField = value;
                }
            }

            /// <remarks/>
            public contextInitiatorPrincipalId initiatorPrincipalId
            {
                get
                {
                    return this.initiatorPrincipalIdField;
                }
                set
                {
                    this.initiatorPrincipalIdField = value;
                }
            }

            /// <remarks/>
            public ushort password
            {
                get
                {
                    return this.passwordField;
                }
                set
                {
                    this.passwordField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class contextInitiatorPrincipalId
        {

            private string idField;

            private string typeField;

            private ushort userIdField;

            /// <remarks/>
            public string id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            public ushort userId
            {
                get
                {
                    return this.userIdField;
                }
                set
                {
                    this.userIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class parameters
        {

            private parametersParameter parameterField;

            /// <remarks/>
            public parametersParameter parameter
            {
                get
                {
                    return this.parameterField;
                }
                set
                {
                    this.parameterField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class parametersParameter
        {

            private parametersParameterEntry entryField;

            /// <remarks/>
            public parametersParameterEntry entry
            {
                get
                {
                    return this.entryField;
                }
                set
                {
                    this.entryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class parametersParameterEntry
        {

            private string keyField;

            private string valueField;

            /// <remarks/>
            public string key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            public string value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }


    }
}
