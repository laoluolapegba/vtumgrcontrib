using System;
using System.Collections.Generic;
using System.Text;

namespace SalesMgmt.Services.Evc.Worker.Entities.EtopUp
{
    public class ModifyEvcResponseEnvelope
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

            private ModifyEVCInventoryResultMsg modifyEVCInventoryResultMsgField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/evcinterface/websmapmgrmsg")]
            public ModifyEVCInventoryResultMsg ModifyEVCInventoryResultMsg
            {
                get
                {
                    return this.modifyEVCInventoryResultMsgField;
                }
                set
                {
                    this.modifyEVCInventoryResultMsgField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.huawei.com/evcinterface/websmapmgrmsg")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.huawei.com/evcinterface/websmapmgrmsg", IsNullable = false)]
        public partial class ModifyEVCInventoryResultMsg
        {

            private ResultHeader resultHeaderField;

            private object modifyEVCInventoryResultField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public ResultHeader ResultHeader
            {
                get
                {
                    return this.resultHeaderField;
                }
                set
                {
                    this.resultHeaderField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public object ModifyEVCInventoryResult
            {
                get
                {
                    return this.modifyEVCInventoryResultField;
                }
                set
                {
                    this.modifyEVCInventoryResultField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class ResultHeader
        {

            private string commandIdField;

            private string resultCodeField;

            private string serialNoField;

            private string resultDescField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/evcinterface/common")]
            public string CommandId
            {
                get
                {
                    return this.commandIdField;
                }
                set
                {
                    this.commandIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/evcinterface/common")]
            public string ResultCode
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/evcinterface/common")]
            public string SerialNo
            {
                get
                {
                    return this.serialNoField;
                }
                set
                {
                    this.serialNoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/evcinterface/common")]
            public string ResultDesc
            {
                get
                {
                    return this.resultDescField;
                }
                set
                {
                    this.resultDescField = value;
                }
            }
        }
    }

}
