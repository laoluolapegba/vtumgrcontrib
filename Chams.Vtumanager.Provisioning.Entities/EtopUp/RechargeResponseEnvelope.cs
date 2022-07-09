using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp
{
    public class RechargeResponseEnvelope
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

            private SDF_Data sDF_DataField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://sdf.cellc.net/commonDataModel")]
            public SDF_Data SDF_Data
            {
                get
                {
                    return this.sDF_DataField;
                }
                set
                {
                    this.sDF_DataField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://sdf.cellc.net/commonDataModel")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://sdf.cellc.net/commonDataModel", IsNullable = false)]
        public partial class SDF_Data
        {

            private SDF_DataHeader headerField;

            private SDF_DataParameters parametersField;

            private SDF_DataResult resultField;

            private string processIDField;

            /// <remarks/>
            public SDF_DataHeader header
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
            public SDF_DataParameters parameters
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

            /// <remarks/>
            public SDF_DataResult result
            {
                get
                {
                    return this.resultField;
                }
                set
                {
                    this.resultField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string processID
            {
                get
                {
                    return this.processIDField;
                }
                set
                {
                    this.processIDField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://sdf.cellc.net/commonDataModel")]
        public partial class SDF_DataHeader
        {

            private int processTypeIDField;

            private string externalReferenceField;

            private int sourceIDField;

            private string usernameField;

            private string passwordField;

            private int processFlagField;

            /// <remarks/>
            public int processTypeID
            {
                get
                {
                    return this.processTypeIDField;
                }
                set
                {
                    this.processTypeIDField = value;
                }
            }

            /// <remarks/>
            public string externalReference
            {
                get
                {
                    return this.externalReferenceField;
                }
                set
                {
                    this.externalReferenceField = value;
                }
            }

            /// <remarks/>
            public int sourceID
            {
                get
                {
                    return this.sourceIDField;
                }
                set
                {
                    this.sourceIDField = value;
                }
            }

            /// <remarks/>
            public string username
            {
                get
                {
                    return this.usernameField;
                }
                set
                {
                    this.usernameField = value;
                }
            }

            /// <remarks/>
            public string password
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

            /// <remarks/>
            public int processFlag
            {
                get
                {
                    return this.processFlagField;
                }
                set
                {
                    this.processFlagField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://sdf.cellc.net/commonDataModel")]
        public partial class SDF_DataParameters
        {

            private SDF_DataParametersParameter[] parameterField;

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("parameter")]
            public SDF_DataParametersParameter[] parameter
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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://sdf.cellc.net/commonDataModel")]
        public partial class SDF_DataParametersParameter
        {

            private string nameField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
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

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://sdf.cellc.net/commonDataModel")]
        public partial class SDF_DataResult
        {

            private string statusCodeField;

            private string errorCodeField;

            private string errorDescriptionField;

            private long instanceIdField;

            /// <remarks/>
            public string statusCode
            {
                get
                {
                    return this.statusCodeField;
                }
                set
                {
                    this.statusCodeField = value;
                }
            }

            /// <remarks/>
            public string errorCode
            {
                get
                {
                    return this.errorCodeField;
                }
                set
                {
                    this.errorCodeField = value;
                }
            }

            /// <remarks/>
            public string errorDescription
            {
                get
                {
                    return this.errorDescriptionField;
                }
                set
                {
                    this.errorDescriptionField = value;
                }
            }

            /// <remarks/>
            public long instanceId
            {
                get
                {
                    return this.instanceIdField;
                }
                set
                {
                    this.instanceIdField = value;
                }
            }
        }


    }
}
