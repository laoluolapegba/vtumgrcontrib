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

            private requestTopupResponse requestTopupResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://external.interfaces.ers.seamless.com/")]
            public requestTopupResponse requestTopupResponse
            {
                get
                {
                    return this.requestTopupResponseField;
                }
                set
                {
                    this.requestTopupResponseField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://external.interfaces.ers.seamless.com/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://external.interfaces.ers.seamless.com/", IsNullable = false)]
        public partial class requestTopupResponse
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

            private string ersReferenceField;

            private int resultCodeField;

            private string resultDescriptionField;

            private returnRequestedTopupAmount requestedTopupAmountField;

            private returnSenderPrincipal senderPrincipalField;

            private returnTopupAccountSpecifier topupAccountSpecifierField;

            private returnTopupAmount topupAmountField;

            private returnTopupPrincipal topupPrincipalField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
            public string ersReference
            {
                get
                {
                    return this.ersReferenceField;
                }
                set
                {
                    this.ersReferenceField = value;
                }
            }

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
            public returnRequestedTopupAmount requestedTopupAmount
            {
                get
                {
                    return this.requestedTopupAmountField;
                }
                set
                {
                    this.requestedTopupAmountField = value;
                }
            }

            /// <remarks/>
            public returnSenderPrincipal senderPrincipal
            {
                get
                {
                    return this.senderPrincipalField;
                }
                set
                {
                    this.senderPrincipalField = value;
                }
            }

            /// <remarks/>
            public returnTopupAccountSpecifier topupAccountSpecifier
            {
                get
                {
                    return this.topupAccountSpecifierField;
                }
                set
                {
                    this.topupAccountSpecifierField = value;
                }
            }

            /// <remarks/>
            public returnTopupAmount topupAmount
            {
                get
                {
                    return this.topupAmountField;
                }
                set
                {
                    this.topupAmountField = value;
                }
            }

            /// <remarks/>
            public returnTopupPrincipal topupPrincipal
            {
                get
                {
                    return this.topupPrincipalField;
                }
                set
                {
                    this.topupPrincipalField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnRequestedTopupAmount
        {

            private string currencyField;

            private decimal valueField;

            /// <remarks/>
            public string currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }

            /// <remarks/>
            public decimal value
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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnSenderPrincipal
        {

            private returnSenderPrincipalPrincipalId principalIdField;

            private string principalNameField;

            private returnSenderPrincipalAccounts accountsField;

            private string statusField;

            private string msisdnField;

            /// <remarks/>
            public returnSenderPrincipalPrincipalId principalId
            {
                get
                {
                    return this.principalIdField;
                }
                set
                {
                    this.principalIdField = value;
                }
            }

            /// <remarks/>
            public string principalName
            {
                get
                {
                    return this.principalNameField;
                }
                set
                {
                    this.principalNameField = value;
                }
            }

            /// <remarks/>
            public returnSenderPrincipalAccounts accounts
            {
                get
                {
                    return this.accountsField;
                }
                set
                {
                    this.accountsField = value;
                }
            }

            /// <remarks/>
            public string status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }

            /// <remarks/>
            public string msisdn
            {
                get
                {
                    return this.msisdnField;
                }
                set
                {
                    this.msisdnField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnSenderPrincipalPrincipalId
        {

            private string idField;

            private string typeField;

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
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnSenderPrincipalAccounts
        {

            private returnSenderPrincipalAccountsAccount accountField;

            /// <remarks/>
            public returnSenderPrincipalAccountsAccount account
            {
                get
                {
                    return this.accountField;
                }
                set
                {
                    this.accountField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnSenderPrincipalAccountsAccount
        {

            private returnSenderPrincipalAccountsAccountAccountSpecifier accountSpecifierField;

            private returnSenderPrincipalAccountsAccountBalance balanceField;

            private returnSenderPrincipalAccountsAccountCreditLimit creditLimitField;

            /// <remarks/>
            public returnSenderPrincipalAccountsAccountAccountSpecifier accountSpecifier
            {
                get
                {
                    return this.accountSpecifierField;
                }
                set
                {
                    this.accountSpecifierField = value;
                }
            }

            /// <remarks/>
            public returnSenderPrincipalAccountsAccountBalance balance
            {
                get
                {
                    return this.balanceField;
                }
                set
                {
                    this.balanceField = value;
                }
            }

            /// <remarks/>
            public returnSenderPrincipalAccountsAccountCreditLimit creditLimit
            {
                get
                {
                    return this.creditLimitField;
                }
                set
                {
                    this.creditLimitField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnSenderPrincipalAccountsAccountAccountSpecifier
        {

            private string accountIdField;

            private string accountTypeIdField;

            /// <remarks/>
            public string accountId
            {
                get
                {
                    return this.accountIdField;
                }
                set
                {
                    this.accountIdField = value;
                }
            }

            /// <remarks/>
            public string accountTypeId
            {
                get
                {
                    return this.accountTypeIdField;
                }
                set
                {
                    this.accountTypeIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnSenderPrincipalAccountsAccountBalance
        {

            private string currencyField;

            private decimal valueField;

            /// <remarks/>
            public string currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }

            /// <remarks/>
            public decimal value
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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnSenderPrincipalAccountsAccountCreditLimit
        {

            private string currencyField;

            private decimal valueField;

            /// <remarks/>
            public string currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }

            /// <remarks/>
            public decimal value
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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupAccountSpecifier
        {

            private string accountIdField;

            private string accountTypeIdField;

            /// <remarks/>
            public string accountId
            {
                get
                {
                    return this.accountIdField;
                }
                set
                {
                    this.accountIdField = value;
                }
            }

            /// <remarks/>
            public string accountTypeId
            {
                get
                {
                    return this.accountTypeIdField;
                }
                set
                {
                    this.accountTypeIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupAmount
        {

            private string currencyField;

            private decimal valueField;

            /// <remarks/>
            public string currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }

            /// <remarks/>
            public decimal value
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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupPrincipal
        {

            private returnTopupPrincipalPrincipalId principalIdField;

            private object principalNameField;

            private returnTopupPrincipalAccounts accountsField;

            /// <remarks/>
            public returnTopupPrincipalPrincipalId principalId
            {
                get
                {
                    return this.principalIdField;
                }
                set
                {
                    this.principalIdField = value;
                }
            }

            /// <remarks/>
            public object principalName
            {
                get
                {
                    return this.principalNameField;
                }
                set
                {
                    this.principalNameField = value;
                }
            }

            /// <remarks/>
            public returnTopupPrincipalAccounts accounts
            {
                get
                {
                    return this.accountsField;
                }
                set
                {
                    this.accountsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupPrincipalPrincipalId
        {

            private string idField;

            private string typeField;

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
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupPrincipalAccounts
        {

            private returnTopupPrincipalAccountsAccount accountField;

            /// <remarks/>
            public returnTopupPrincipalAccountsAccount account
            {
                get
                {
                    return this.accountField;
                }
                set
                {
                    this.accountField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupPrincipalAccountsAccount
        {

            private returnTopupPrincipalAccountsAccountAccountSpecifier accountSpecifierField;

            private returnTopupPrincipalAccountsAccountBalance balanceField;

            /// <remarks/>
            public returnTopupPrincipalAccountsAccountAccountSpecifier accountSpecifier
            {
                get
                {
                    return this.accountSpecifierField;
                }
                set
                {
                    this.accountSpecifierField = value;
                }
            }

            /// <remarks/>
            public returnTopupPrincipalAccountsAccountBalance balance
            {
                get
                {
                    return this.balanceField;
                }
                set
                {
                    this.balanceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupPrincipalAccountsAccountAccountSpecifier
        {

            private string accountIdField;

            private string accountTypeIdField;

            /// <remarks/>
            public string accountId
            {
                get
                {
                    return this.accountIdField;
                }
                set
                {
                    this.accountIdField = value;
                }
            }

            /// <remarks/>
            public string accountTypeId
            {
                get
                {
                    return this.accountTypeIdField;
                }
                set
                {
                    this.accountTypeIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class returnTopupPrincipalAccountsAccountBalance
        {

            private string currencyField;

            private decimal valueField;

            /// <remarks/>
            public string currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }

            /// <remarks/>
            public decimal value
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
