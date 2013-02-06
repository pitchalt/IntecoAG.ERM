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
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM.StatementAccount {

    /// <summary>
    /// Выписка
    /// </summary>
    [NavigationItem("Money")]
    [Persistent("fmSAStatementAccount")]
    public class fmCSAStatementAccount : csCComponent
    {
        public fmCSAStatementAccount(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            //AccountInfo = new XPCollection<fmStatementOfAccountInfo>(this.Session);
            //ImportResult = new fmImportResult(this.Session);  // Назначается извне
            this.ComponentType = typeof(fmCSAStatementAccount);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private String _DocNumber; // номер документа
        private DateTime _DocDate; // дата документа

        private DateTime _DateFrom;    // Дата с
        private DateTime _DateTo;   // Дата по

        private crmBank _Bank;
        private crmBankAccount _BankAccount;

        private String _BankName;   // банк файла выписки
        private String _BankAccountText;   // Счёт из файла выписки

        //private String _DataFormat;   // Формат данных: 1C и т.п.
        //private String _CodePage;   // Кодировка файла с данными выписки (по стандартам 1С DOS или WINDOWS)

        //private fmImportResult _ImportResult;   // содержимое пришедшего файла выписки
        //private XPCollection<fmStatementOfAccountInfo> _AccountInfo;   // реквизиты счёта из пришедшего файла выписки


        // ========== Перенесено из AccountInfo ==================
        private Decimal _BalanceOfIncoming;   // Входящее сальдо
        private Decimal _BalanceOfOutgoing;   // Исходящее сальдо

        private Decimal _TotalRecaivedAtAccount;   // Всего поступило
        private Decimal _TotalWriteOfAccount;   // Всего списано
        // =======================================================

        private fmCSAImportResult _ImportResult; // Ссылка на результат импорта
        
        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// DocNumber - номер документа Выписки
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.DocNumber.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[RuleRequiredField]
        [Size(300)]
        public String DocNumber {
            get { return _DocNumber; }
            set {
                SetPropertyValue<String>("DocNumber", ref _DocNumber, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// DocDate - Дата создания документа выписки
        /// </summary>
        //[RuleRequiredField]
        public DateTime DocDate {
            get { return _DocDate; }
            set {
                SetPropertyValue<DateTime>("DocDate", ref _DocDate, value);
            }
        }

        /// <summary>
        /// DateFrom - Дата с
        /// </summary>
        public DateTime DateFrom {
            get { return _DateFrom; }
            set {
                SetPropertyValue<DateTime>("DateFrom", ref _DateFrom, value);
            }
        }

        /// <summary>
        /// DateTo - Дата с
        /// </summary>
        public DateTime DateTo {
            get { return _DateTo; }
            set {
                SetPropertyValue<DateTime>("DateTo", ref _DateTo, value);
            }
        }

        /// <summary>
        /// Bank - банк
        /// </summary>
        public crmBank Bank {
            get { return _Bank; }
            set {
                SetPropertyValue<crmBank>("Bank", ref _Bank, value);
            }
        }

        /// <summary>
        /// BankName - Наименование банка из файла выписки, в настоящий момент (2012-02-13 - это название родительского каталога, в к-м лежит файл выписки)
        /// </summary>
        public string BankName {
            get { return _BankName; }
            set {
                SetPropertyValue<string>("BankName", ref _BankName, value);
            }
        }

        /// <summary>
        /// BankAccount - банковский счёт
        /// </summary>
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
            }
        }

        /// <summary>
        /// BankAccountText - Счёт из файла выписки
        /// </summary>
        public string BankAccountText {
            get { return _BankAccountText; }
            set {
                SetPropertyValue<string>("BankAccountText", ref _BankAccountText, value);
            }
        }

        ///// <summary>
        ///// DataFormat - Формат данных: 1C и т.п.
        ///// </summary>
        //public String DataFormat {
        //    get { return _DataFormat; }
        //    set {
        //        SetPropertyValue<String>("DataFormat", ref _DataFormat, value);
        //    }
        //}

        ///// <summary>
        ///// CodePage - Кодировка файла с данными выписки (по стандартам 1С DOS или WINDOWS)
        ///// </summary>
        //public String CodePage {
        //    get { return _CodePage; }
        //    set {
        //        SetPropertyValue<String>("CodePage", ref _CodePage, value);
        //    }
        //}

        ///// <summary>
        ///// ImportResult - содержимое пришедшего файла выписки
        ///// </summary>
        //[Delayed]
        ////[Aggregated]
        //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        //public fmImportResult ImportResult {
        //    get { return GetDelayedPropertyValue<fmImportResult>("ImportResult"); }
        //    set { SetDelayedPropertyValue<fmImportResult>("ImportResult", value); }
        //}


        //private fmImportResult _ImportResult;   // содержимое пришедшего файла выписки
        ///// <summary>
        ///// ImportResult - содержимое пришедшего файла выписки
        ///// </summary>
        //public fmImportResult ImportResult {
        //    get { return _ImportResult; }
        //    set {
        //        SetPropertyValue<fmImportResult>("ImportResult", ref _ImportResult, value);
        //    }
        //}


        ///// <summary>
        ///// AccountInfo - реквизиты счёта из пришедшего файла выписки
        ///// </summary>
        //[Aggregated]
        //[Association("fmStatementOfAccounts-fmStatementOfAccountInfo", typeof(fmStatementOfAccountInfo))]
        //public XPCollection<fmStatementOfAccountInfo> AccountInfo {
        //    get { return GetCollection<fmStatementOfAccountInfo>("AccountInfo"); }
        //}

        ///// <summary>
        ///// Список документов, полученных по выписке
        ///// </summary>
        //[Aggregated]
        //[Association("fmStatementOfAccounts-fmCDocRCB", typeof(fmCDocRCB))]
        //public XPCollection<fmCDocRCB> RCBDocuments {
        //    get { return GetCollection<fmCDocRCB>("RCBDocuments"); }
        //}


        /// <summary>
        /// Список контрагентов, полученных по выписке
        /// </summary>
        [Aggregated]
        [Association("fmStatementOfAccounts-fmCDocRCBRequisites", typeof(fmCDocRCBRequisites))]
        public XPCollection<fmCDocRCBRequisites> DocRCBRequisites {
            get { return GetCollection<fmCDocRCBRequisites>("DocRCBRequisites"); }
        }

        /*
        XPCollection<fmCSAStatementAccountDoc> _PayInDocs;
        public XPCollection<fmCSAStatementAccountDoc> PayInDocs {
            get { 
                if (_PayInDocs == null) {
                    XPQuery<fmCSAStatementAccountDoc> docq = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
                    var docs = from doc in docq 
                               where doc.PaymentReceiverRequisites.StatementOfAccount == this
                                  && doc.ReceivedByPayerBankDate != DateTime.MinValue
                               select doc;
                    _PayInDocs = new XPCollection<fmCSAStatementAccountDoc>(this.Session, false);
                    foreach (fmCSAStatementAccountDoc doc in docs) {
                        _PayInDocs.Add(doc);
                    }
                }
                return _PayInDocs;
            }
        }
        */

        [Aggregated]
        [Association("fmStatementOfAccount-fmStatementOfAccountInDocs", typeof(fmCSAStatementAccountDoc))]
        public XPCollection<fmCSAStatementAccountDoc> PayInDocs {
            get {
                return GetCollection<fmCSAStatementAccountDoc>("PayInDocs");
            }
        }

        /*
        XPCollection<fmCSAStatementAccountDoc> _PayOutDocs;
        public XPCollection<fmCSAStatementAccountDoc> PayOutDocs {
            get {
                if (_PayOutDocs == null) {
                    XPQuery<fmCSAStatementAccountDoc> docq = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
                    var docs = from doc in docq
                               where doc.PaymentPayerRequisites.StatementOfAccount == this
                                  && doc.DeductedFromPayerAccount != DateTime.MinValue
                               select doc;
                    _PayOutDocs = new XPCollection<fmCSAStatementAccountDoc>(this.Session, false);
                    foreach (fmCSAStatementAccountDoc doc in docs) {
                        _PayOutDocs.Add(doc);
                    }
                }
                return _PayOutDocs;
            }
        }
        */

        [Aggregated]
        [Association("fmStatementOfAccount-fmStatementOfAccountOutDocs", typeof(fmCSAStatementAccountDoc))]
        public XPCollection<fmCSAStatementAccountDoc> PayOutDocs {
            get {
                return GetCollection<fmCSAStatementAccountDoc>("PayOutDocs");
            }
        }


        // ========== Перенесено из AccountInfo ==================

        ///// <summary>
        ///// BankAccountText - Счёт из файла выписки
        ///// </summary>
        //public string BankAccountText {
        //    get { return _BankAccountText; }
        //    set {
        //        SetPropertyValue<string>("BankAccountText", ref _BankAccountText, value);
        //    }
        //}

        /// <summary>
        /// BalanceOfIncoming - Входящее сальдо
        /// </summary>
        public Decimal BalanceOfIncoming {
            get { return _BalanceOfIncoming; }
            set {
                SetPropertyValue<Decimal>("BalanceOfIncoming", ref _BalanceOfIncoming, value);
            }
        }

        /// <summary>
        /// BalanceOfOutgoing - Исходящее сальдо
        /// </summary>
        public Decimal BalanceOfOutgoing {
            get { return _BalanceOfOutgoing; }
            set {
                SetPropertyValue<Decimal>("BalanceOfOutgoing", ref _BalanceOfOutgoing, value);
            }
        }

        /// <summary>
        /// TotalRecaivedAtAccount - Всего поступило
        /// </summary>
        public Decimal TotalRecaivedAtAccount {
            get { return _TotalRecaivedAtAccount; }
            set {
                SetPropertyValue<Decimal>("TotalRecaivedAtAccount", ref _TotalRecaivedAtAccount, value);
            }
        }

        /// <summary>
        /// TotalWriteOfAccount - Всего списано
        /// </summary>
        public Decimal TotalWriteOfAccount {
            get { return _TotalWriteOfAccount; }
            set {
                SetPropertyValue<Decimal>("TotalWriteOfAccount", ref _TotalWriteOfAccount, value);
            }
        }
        // =======================================================

        [Association("fmImportResult-fmStatementOfAccounts")]
        public fmCSAImportResult ImportResult {
            get { return _ImportResult; }
            set { SetPropertyValue<fmCSAImportResult>("ImportResult", ref _ImportResult, value); }
        }



        public void AutoBinding(fmCPRRepaymentTask RepaymentTask) {
            foreach (fmCSAStatementAccountDoc sad in this.PayInDocs) {
                fmCDocRCB paymentDoc = sad.PaymentDocument;
                paymentDoc.AutoBinding(BankAccount, RepaymentTask);
            }
            foreach (fmCSAStatementAccountDoc sad in this.PayOutDocs) {
                fmCDocRCB paymentDoc = sad.PaymentDocument;
                paymentDoc.AutoBinding(BankAccount, RepaymentTask);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
