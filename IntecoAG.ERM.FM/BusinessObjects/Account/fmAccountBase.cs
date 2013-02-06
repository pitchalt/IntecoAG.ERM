using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.FM.Account {

    /// <summary>
    /// Признак оплаты под договору или разовому счёту
    /// </summary>
    public enum PaymentRequestTypes {
        CONRACT = 1,
        ONE_TIME_ACCOUNT = 2
    }


    // Счёт абстрактный

    [NavigationItem("Money")]
    [Persistent("fmAccount")]
    public abstract class fmAccountBase : csCComponent
    {
        public fmAccountBase(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmAccountBase);
            this.CID = Guid.NewGuid();

            //PayerBank = new crmBank(this.Session);
            //PayerBankAccount = new crmBankAccount(this.Session);

            //ReceiverBank = new crmBank(this.Session);
            //ReceiverBankAccount = new crmBankAccount(this.Session);

            //Customer = new crmCParty(this.Session);
            //Supplier = new crmCParty(this.Session);
            //Receiver = new crmCParty(this.Session);

            AllowEditProperty = true;
        }

        #region ПОЛЯ КЛАССА

        private bool _AllowEditProperty;   // Разрешение редактирования

        private String _Number; // номер документа
        private DateTime _Date; // дата документа

        private crmBank _PayerBank;     // Банк плательщика
        private crmBankAccount _PayerBankAccount;    // Счёт плательщика

        private DateTime _ValidToDate; // Дата, до которой счёт должен быть оплачен

        private crmCParty _Customer;   // Заказчик
        private crmCParty _Supplier;   // Исполнитель

        private crmCParty _Receiver;   // Получатель, по умолчанию равен Исполнителю

        private crmBank _ReceiverBank;   // Банк получателя
        private crmBankAccount _ReceiverBankAccount;   // Счёт получателя

        private PaymentRequestTypes _PaymentRequestType;   // Вид обязательств. Признак оплаты по договору или разовому счёту

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// AllowEditProperty - Разрешение редактирования
        /// </summary>
        [ImmediatePostData]
        [Browsable(false)]
        public bool AllowEditProperty {
            get { return _AllowEditProperty; }
            set {
                SetPropertyValue<bool>("AllowEditProperty", ref _AllowEditProperty, value);
            }
        }

        /// <summary>
        /// DocNumber - номер платёжного документа
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.DocNumber.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[RuleRequiredField]
        [Size(300)]
        public String Number {
            get { return _Number; }
            set {
                SetPropertyValue<String>("Number", ref _Number, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// DocDate - Дата платежа
        /// </summary>
        //[RuleRequiredField]
        public DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }

        /// <summary>
        /// PayerBank - Банк плательщика
        /// </summary>
        public crmBank PayerBank {
            get { return _PayerBank; }
            set {
                SetPropertyValue<crmBank>("PayerBank", ref _PayerBank, value);
            }
        }

        /// <summary>
        /// PayerBankAccount - Счёт плательщика
        /// </summary>
        public crmBankAccount PayerBankAccount {
            get { return _PayerBankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("PayerBankAccount", ref _PayerBankAccount, value);
            }
        }

        /// <summary>
        /// ReceiverBank - банк получателя
        /// </summary>
        public crmBank ReceiverBank {
            get { return _ReceiverBank; }
            set {
                SetPropertyValue<crmBank>("ReceiverBank", ref _ReceiverBank, value);
            }
        }

        /// <summary>
        /// ReceiverBankAccount - расчётный счёт получателя
        /// </summary>
        public crmBankAccount ReceiverBankAccount {
            get { return _ReceiverBankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("ReceiverBankAccount", ref _ReceiverBankAccount, value);
            }
        }

        /// <summary>
        /// ValidToDate - Дата, до которой счёт должен быть оплачен
        /// </summary>
        //[RuleRequiredField]
        public DateTime ValidToDate {
            get { return _ValidToDate; }
            set {
                SetPropertyValue<DateTime>("ValidToDate", ref _ValidToDate, value);
            }
        }

        /// <summary>
        /// Customer - Заказчик
        /// </summary>
        //[RuleRequiredField]
        public crmCParty Customer {
            get { return _Customer; }
            set {
                SetPropertyValue<crmCParty>("Customer", ref _Customer, value);
            }
        }

        /// <summary>
        /// Supplier - Исполнитель
        /// </summary>
        //[RuleRequiredField]
        public crmCParty Supplier {
            get { return _Supplier; }
            set {
                SetPropertyValue<crmCParty>("Supplier", ref _Supplier, value);
                if (!this.IsLoading) {
                    if (Receiver == null) {
                        Receiver = Supplier;
                        OnChanged("Receiver");
                    }
                }
            }
        }

        /// <summary>
        /// Receiver - Получатель, по умолчанию равен Исполнителю
        /// </summary>
        //[RuleRequiredField]
        public crmCParty Receiver {
            get { return _Receiver; }
            set {
                SetPropertyValue<crmCParty>("Receiver", ref _Receiver, value);
            }
        }

        /// <summary>
        /// PaymentRequestType - Признак оплаты по договору или разовому счёту
        /// </summary>
        public PaymentRequestTypes PaymentRequestType {
            get { return _PaymentRequestType; }
            set {
                SetPropertyValue<PaymentRequestTypes>("PaymentRequestType", ref _PaymentRequestType, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        public void SetAllowEdit() {
            AllowEditProperty = true;
        }

        public void SetDisAllowEdit() {
            AllowEditProperty = false;
        }

        public bool GetAllowEdit() {
            return AllowEditProperty && !ReadOnly;
        }

        protected override void OnSaving() {
            AllowEditProperty = false;
            base.OnSaving();
        }

        #endregion

    }

}
