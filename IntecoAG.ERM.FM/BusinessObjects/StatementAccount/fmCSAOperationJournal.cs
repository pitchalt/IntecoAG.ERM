#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.ComponentModel;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM.StatementAccount
{
    /// <summary>
    /// Регистр движения денег (Платёжных документов, Документами выписок)
    /// </summary>
    [NavigationItem("Money")]
    [Persistent("fmSAOperationJournal")]
    public class fmCSAOperationJournal : DevExpress.Xpo.XPLiteObject
    {
        public fmCSAOperationJournal(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            DateRecord = DateTime.Now;
        }

        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public Guid Oid;

        #region ПОЛЯ КЛАССА

        private DateTime _DateRecord;   // Дата создания текущей записи регистра

        private fmCDocRCB _PaymentDocument;   // Платёжный документ
        private fmCSAStatementAccountDoc _StatementAccountDoc;   // Основание изменения в регистре
        private crmBankAccount _BankAccount;   // Счёт
        private decimal _SumIn;   // Сумма поступления в указанной валюте
        private decimal _SumOut;   // Сумма убытия в указанной валюте
        private decimal _SumBalance;   // SumIn - SumOut
        private csValuta _Valuta;   // Валюта расчёта
        //[Persistent("OperationDate")] /* Закомментарил код Павла, т.к. не отрабатывает, времени выяснять нет */
        private DateTime _OperationDate;   // Дата операции по выписке
        //private String _PaymentDocumentNumber;   // Номер документа
        //private DateTime _DeductedFromPayerAccount;   // Дата изменния (списания со счёта Плательщика) состояния счёта согласно Документу выписки

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата создания текущей записи регистра
        /// </summary>
        public DateTime DateRecord {
            get { return _DateRecord; }
            set { SetPropertyValue<DateTime>("DateRecord", ref _DateRecord, value); }
        }

        /// <summary>
        /// Платёжный документ
        /// </summary>
        public fmCDocRCB PaymentDocument {
            get { return _PaymentDocument; }
            set { SetPropertyValue<fmCDocRCB>("PaymentDocument", ref _PaymentDocument, value); }
        }

        /// <summary>
        /// Документ выписки (основание изменения в регистре)
        /// </summary>
        public fmCSAStatementAccountDoc StatementAccountDoc {
            get { return _StatementAccountDoc; }
            set { 
                SetPropertyValue<fmCSAStatementAccountDoc>("StatementAccountDoc", ref _StatementAccountDoc, value);
                if (!IsLoading && value != null) { 
                    UpdateOperationDate();
                }
            }
        }

        /// <summary>
        /// Счёт
        /// </summary>
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set { 
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
                if (!IsLoading && value != null) {
                    UpdateOperationDate();
                }
            }
        }

        /// <summary>
        /// Банк
        /// </summary>
        [PersistentAlias("BankAccount.Bank")]
        public crmBank Bank {
            get { return BankAccount.Bank; }
        }

        /// <summary>
        /// Сумма поступления в указанной валюте
        /// </summary>
        public decimal SumIn {
            get { return _SumIn; }
            set {
                SetPropertyValue<decimal>("SumIn", ref _SumIn, value);
                if (!IsLoading) {
                    UpdateBalance();
                }
            }
        }

        /// <summary>
        /// Сумма убытия в указанной валюте
        /// </summary>
        public decimal SumOut {
            get { return _SumOut; }
            set {
                SetPropertyValue<decimal>("SumOut", ref _SumOut, value);
                if (!IsLoading) {
                    UpdateBalance();
                }
            }
        }

        /// <summary>
        /// Баланс SumIn - SumOut
        /// </summary>
        public decimal SumBalance {
            get { return _SumBalance; }
            set { SetPropertyValue<decimal>("SumBalance", ref _SumBalance, value); }
        }

        /// <summary>
        /// Валюта расчёта (определяется по 6-8 знакам номера счёта BankAccount)
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        /// <summary>
        /// Дата документа
        /// </summary>
        [PersistentAlias("PaymentDocument.DocDate")]
        public DateTime PaymentDocumentDate {
            get { return PaymentDocument.DocDate; }
        }

        /*
        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime PaymentDocumentDate {
            get { return _PaymentDocumentDate; }
            set {
                SetPropertyValue<DateTime>("PaymentDocumentDate", ref _PaymentDocumentDate, value);
            }
        }
        */

        /// <summary>
        /// Номер документа
        /// </summary>
        [PersistentAlias("PaymentDocument.DocNumber")]
        public String PaymentDocumentNumber {
            get { return PaymentDocument.DocNumber; }
        }

        /*
        /// <summary>
        /// Номер документа
        /// </summary>
        public String PaymentDocumentNumber {
            get { return _PaymentDocumentNumber; }
            set {
                SetPropertyValue<String>("PaymentDocumentNumber", ref _PaymentDocumentNumber, value);
            }
        }
        */

        /*
        /// <summary>
        /// Дата изменния (списания со счёта Плательщика) состояния счёта согласно Документу выписки
        /// </summary>
        public DateTime DeductedFromPayerAccount {
            get { return _DeductedFromPayerAccount; }
            set {
                SetPropertyValue<DateTime>("DeductedFromPayerAccount", ref _DeductedFromPayerAccount, value);
            }
        }
        */

        /* Закомментарил это определение OperationDate Павла, см. ниже определение UpdateOperationDate
        /// <summary>
        /// Дата изменния состояния счёта согласно Документу выписки
        /// </summary>
        [PersistentAlias("_OperationDate")]
        public DateTime OperationDate {
            get { return _OperationDate; }
        }
        */
        
        /// <summary>
        /// Дата изменния состояния счёта согласно Документу выписки
        /// </summary>
        public DateTime OperationDate {
            get { return _OperationDate; }
            set {
                SetPropertyValue<DateTime>("OperationDate", ref _OperationDate, value);
            }
        }

        #endregion


        #region МЕТОДЫ

        private void UpdateBalance() {
            SumBalance = SumIn - SumOut;
        }

        protected void UpdateOperationDate() { 

            return;

            /* Закомментарил код Павла, т.к. не отрабатывает, времени выяснять нет
            DateTime date_old = _OperationDate;
            DateTime date_new = date_old;
            if (BankAccount != null && StatementAccountDoc != null) {
                if (BankAccount == StatementAccountDoc.PaymentReceiverRequisites.BankAccount)
                    date_new = StatementAccountDoc.ReceivedByPayerBankDate;
                if (BankAccount == StatementAccountDoc.PaymentPayerRequisites.BankAccount)
                    date_new = StatementAccountDoc.DocumentSendingDate;
                if (date_old != date_new) {
                    _OperationDate = date_new;
                    OnChanged("OperationDate", date_old, date_new);
                }
            }
            */

            //if (StatementAccountDoc != null)
            //{
            //    if (StatementAccountDoc.PaymentReceiverRequisites.INN == GetOurParty().INN)
            //        date_new = StatementAccountDoc.ReceivedByPayerBankDate;
            //    if (StatementAccountDoc.PaymentPayerRequisites.INN == GetOurParty().INN)
            //        date_new = StatementAccountDoc.DocumentSendingDate;
            //    if (date_old != date_new)
            //    {
            //        _OperationDate = date_new;
            //        OnChanged("OperationDate", date_old, date_new);
            //    }
            //}

        }


        private crmCParty GetOurParty() {
            // Наша организация
            crmCParty OurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }
            return OurParty;
        }

        #endregion

    }
}