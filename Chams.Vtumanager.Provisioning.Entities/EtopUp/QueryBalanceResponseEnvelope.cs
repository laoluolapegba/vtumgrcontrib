using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp
{
    public   class QueryBalanceResponseEnvelope
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

            private QueryEVCBalanceResultMsg queryEVCBalanceResultMsgField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/evc/businessmgrmsg")]
            public QueryEVCBalanceResultMsg QueryEVCBalanceResultMsg
            {
                get
                {
                    return this.queryEVCBalanceResultMsgField;
                }
                set
                {
                    this.queryEVCBalanceResultMsgField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.huawei.com/bme/evcinterface/evc/businessmgrmsg")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/evc/businessmgrmsg", IsNullable = false)]
        public partial class QueryEVCBalanceResultMsg
        {

            private ResultHeader resultHeaderField;

            private QueryEVCBalanceResult queryEVCBalanceResultField;

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
            public QueryEVCBalanceResult QueryEVCBalanceResult
            {
                get
                {
                    return this.queryEVCBalanceResultField;
                }
                set
                {
                    this.queryEVCBalanceResultField = value;
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

            private string versionField;

            private string transactionIdField;

            private string sequenceIdField;

            private int resultCodeField;

            private string resultDescField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/common")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/common")]
            public string Version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/common")]
            public string TransactionId
            {
                get
                {
                    return this.transactionIdField;
                }
                set
                {
                    this.transactionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/common")]
            public string SequenceId
            {
                get
                {
                    return this.sequenceIdField;
                }
                set
                {
                    this.sequenceIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/common")]
            public int ResultCode
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/common")]
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

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class QueryEVCBalanceResult
        {

            private int dealerAmountField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.huawei.com/bme/evcinterface/evc/businessmgr")]
            public int DealerAmount
            {
                get
                {
                    return this.dealerAmountField;
                }
                set
                {
                    this.dealerAmountField = value;
                }
            }
        }


    }
}
