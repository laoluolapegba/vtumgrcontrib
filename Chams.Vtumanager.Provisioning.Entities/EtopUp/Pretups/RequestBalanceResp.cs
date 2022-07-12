using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp.Pretups
{
    public class RequestBalanceResp
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class COMMAND
        {

            private string tYPEField;

            private int tXNSTATUSField;

            private string dATEField;

            private string eXTREFNUMField;

            private string tXNIDField;

            private int rEQSTATUSField;

            private string mESSAGEField;

            /// <remarks/>
            public string TYPE
            {
                get
                {
                    return this.tYPEField;
                }
                set
                {
                    this.tYPEField = value;
                }
            }

            /// <remarks/>
            public int TXNSTATUS
            {
                get
                {
                    return this.tXNSTATUSField;
                }
                set
                {
                    this.tXNSTATUSField = value;
                }
            }

            /// <remarks/>
            public string DATE
            {
                get
                {
                    return this.dATEField;
                }
                set
                {
                    this.dATEField = value;
                }
            }

            /// <remarks/>
            public string EXTREFNUM
            {
                get
                {
                    return this.eXTREFNUMField;
                }
                set
                {
                    this.eXTREFNUMField = value;
                }
            }

            /// <remarks/>
            public string TXNID
            {
                get
                {
                    return this.tXNIDField;
                }
                set
                {
                    this.tXNIDField = value;
                }
            }

            /// <remarks/>
            public int REQSTATUS
            {
                get
                {
                    return this.rEQSTATUSField;
                }
                set
                {
                    this.rEQSTATUSField = value;
                }
            }

            /// <remarks/>
            public string MESSAGE
            {
                get
                {
                    return this.mESSAGEField;
                }
                set
                {
                    this.mESSAGEField = value;
                }
            }
        }


    }
}
