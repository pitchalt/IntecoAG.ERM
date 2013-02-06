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
//using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM.StatementAccount {

    // Так как платёжки все почти одинаковы, то этот класс содержит общую их часть

    // Описание смысла полей см. http://sprbuh.systecs.ru/uchet/bank/platezhnoe_poruchenie.html

    // Правила редактирования платёжки.
    // 0. Редактирование платёжки предполагает редактирование любого её поля. Таким образом, достаточно одной кнопки на форме, 
    //    позволяющей начать редактирование.
    // 1. Если платёжка только что создана, то платёжка доступна для редактирования.
    // 2. Если платёжка загружена в систему извне, то при её открытии в системе, она недоступна для редактирования.
    // 3. Если платёжка образована путём копирования другой платёжки, то при первом открытии она доступна для редактирования.
    // 4. Если платёжка создана в системе и не утверждена, то при каждом её открытии, она недоступна для редактирования.

    // 5. Утоверждённая платёжка никаким образом не может быть отредактирована (недоступны поля и недоступна кнопка редактирования). Утверждённая платёжка имеет значением
    //    ReadOnly = true
    // 6. Во всех прочих случаях платёжка может быть отредактирована после нажития кнопки редактирования на форме.
    // 7. Если реквизиты плательщика/получателя уже введены, а пользователь меняет выбор партнёра или его расчётный счёт, то реквизиты обновляются.
    // 8. Подтверждением изменений является сохранение формы.

    //[Appearance("fmCDocRCBPaymentOrder.PaymentPayer.NameParty.Enabled", AppearanceItemType = "LayoutItem", TargetItems = "PaymentPayer.NameParty", Criteria = "0=1", Visibility = ViewItemVisibility.Show, Enabled = true)]
    //[Appearance("fmCDocRCBPaymentOrder.PaymentPayer.NameParty.Enabled", AppearanceItemType = "LayoutItem", TargetItems = "DocNumber", Method = "AllowEditPayer", Enabled = false)]
    //[Appearance("fmCDocRCBPaymentOrder.PaymentPayer.NameParty.Enabled", AppearanceItemType = "LayoutItem", TargetItems = "DocNumber", Criteria = "False", Enabled = false)]

    [Persistent("fmSAStatementAccountDoc")]
    public class fmCSAStatementAccountDoc : csCComponent
    {
        public fmCSAStatementAccountDoc(Session session)
            : base(session) {
        }

        //public fmCDocRCB(Session session, fmStatementOfAccounts statementAccounts)
        //    : base(session) {
        //    StatementAccount = statementAccounts;
        //}

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCSAStatementAccountDoc);
            this.CID = Guid.NewGuid();

            PaymentPayerRequisites = new fmCDocRCBRequisites(this.Session);
            PaymentPayerRequisites.StatementOfAccountDoc = this;
            PaymentReceiverRequisites = new fmCDocRCBRequisites(this.Session);
            PaymentReceiverRequisites.StatementOfAccountDoc = this;

            AllowEditProperty = true;
        }

        #region ПОЛЯ КЛАССА

        private String _DocNumber; // номер документа
        private DateTime _DocDate; // дата документа

        // ------------------------------------------


        private bool _AllowEditProperty;   // Разрешение редактирования

        private String _PaymentKind; // вид платежа
        private String _PaymentFunction; // назначение платежа
        private Decimal _PaymentCost;  // сумма платежа
        private String _PaymentCostCopybook;  // сумма платежа прописью
        //private Decimal _PaymentTax;  // налог

        private fmCDocRCBRequisites _PaymentPayerRequisites; // Реквизиты плательщика
        private fmCDocRCBRequisites _PaymentReceiverRequisites; // Реквизиты получателя платежа

        // Квитанция
        private DateTime _TicketDate;  // КвитанцияДата
        private String _TicketTime;  // КвитанцияВремя
        private String _TicketContent;  // КвитанцияВремя

        // Разные поля в платёжке

        private String _OperationKind; // Вид операции
        private String _PaymentFunctionCode; // наз. пл. - Назначение платежа кодовое
        //private String _PaymentCode; // Код
        private String _PaymentSequence; // Очерёдность платежа
        private String _PaymentDeadLine; // Срок платежа
        private String _PaymentResField; // Рез. поле

        // Ещё раные поля

        private Int16 _AcceptanceDuration; // СрокАкцепта
        private String _AkkreditiveKind; // ВидАккредитива
        private String _PaymentCondition; // УсловиеОплаты
        private String _PaymentByRepresentation; // ПлатежПоПредст
        private String _AdvancedConditions; // ДополнУсловия
        private String _AccountNumberSupplier; // НомерСчетаПоставщика
        private DateTime _DocumentSendingDate; // ДатаОтсылкиДок


        private DateTime _ReceivedByPayerBankDate;    // Поступило в банк плательщика (Поступ. в банк плат.)
        private DateTime _DeductedFromPayerAccount;   // Списано со счета плательщика (Списано со сч. плат.)

        //private fmStatementOfAccounts _StatementAccount;   // Ссылка на выписку, если экзмпляр загружен из выписки
        //private fmStatementOfAccounts _StatementOfAccounts;   // Ссылка на выписку, если экзмпляр загружен из выписки



        // Доп. поля Платёжки в бюджет
        private String _CompilerStatus;   // Статус составителя
        private String _KBKStatus;   // ПоказательКБК
        private String _OKATO;   // ОКАТО
        private String _ReasonIndicator;   // ПоказательОснования
        private String _PeriodIndicator;   // ПоказательПериода
        private String _NumberIndicator;   // ПоказательНомера
        private String _DateIndicator;   // ПоказательДаты
        private String _TypeIndicator;   // ПоказательТипа

        // Результат исхода (успешно - true, неуспешно - false) постобработки документа при его загрузке
        private bool _PostProcessResult;

        //private bool _StatementAccountIndicator;   // true, если документ является документом выписки (признак выписки)
        private fmCDocRCB _PaymentDocument;   // Ссылка на платёжный документ для документа выписки

        private String _DocType; // Тип документа (Банковский ордер и т.п., кроме тех 4-х, которые явно обозначены в документации 1С)
        //private Type _TypeOfDocument; // Тип документа fmCDocRCBPaymentOrder и т.п.
        private String _NameTypeOfRCBDocument; // Наименование типа документа fmCDocRCBPaymentOrder и т.п.

        private fmCSAImportResult _ImportResult; // Ссылка на результат импорта, по которому документ получен

        //private Decimal _SumIn;  // сумма, равная PaymentCost, если PaymentCost - это приход
        //private Decimal _SumOut;  // сумма, равная PaymentCost, если PaymentCost - это расход

        private fmCSAStatementAccount _StatementAccountIn;   // Ссылка на выписку (случай поступления средств)
        private fmCSAStatementAccount _StatementAccountOut;   // Ссылка на выписку (случай списания средств)

        #endregion



        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// DocNumber - номер платёжного документа
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.DocNumber.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[RuleRequiredField]
        [Size(300)]
        public String DocNumber {
            get { return _DocNumber; }
            set {
                SetPropertyValue<String>("NameParty", ref _DocNumber, value == null ? String.Empty : value.Trim()); }
        }

        /// <summary>
        /// DocDate - Дата платежа
        /// </summary>
        //[RuleRequiredField]
        public DateTime DocDate {
            get { return _DocDate; }
            set {
                SetPropertyValue<DateTime>("DocDate", ref _DocDate, value);
            }
        }


        // -------------------------------------------------



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
        /// PaymentKind - вид платежа
        /// </summary>
        //[Appearance("fmCDocRCBPayment.PaymentKind.Enabled", Method = "AllowEditPayer", Enabled = false)]
        [Size(300)]
        public String PaymentKind {
            get { return _PaymentKind; }
            set {
                SetPropertyValue<String>("PaymentKind", ref _PaymentKind, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PaymentFunction - назначение платежа
        /// </summary>
        //[Appearance("fmCDocRCBPayment.PaymentFunction.Enabled", Method = "AllowEditPayer", Enabled = false)]
        [Size(300)]
        public String PaymentFunction {
            get { return _PaymentFunction; }
            set {
                SetPropertyValue<String>("PaymentFunction", ref _PaymentFunction, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PaymentCost - сумма платежа
        /// </summary>
        //[RuleRequiredField]
        public Decimal PaymentCost {
            get { return _PaymentCost; }
            set {
                SetPropertyValue<Decimal>("PaymentCost", ref _PaymentCost, value);
            }
        }

        /// <summary>
        /// PaymentCostCopybook - сумма платежа прописью
        /// </summary>
        [Size(300)]
        //[RuleRequiredField]
        public String PaymentCostCopybook {
            get { return _PaymentCostCopybook; }
            set {
                SetPropertyValue<String>("PaymentCostCopybook", ref _PaymentCostCopybook, value == null ? String.Empty : value.Trim());
            }
        }

        ///// <summary>
        ///// PaymentTax - налог
        ///// </summary>
        //public Decimal PaymentTax {
        //    get { return _PaymentTax; }
        //    set {
        //        SetPropertyValue<Decimal>("PaymentTax", ref _PaymentTax, value); }
        //}

        /// <summary>
        /// Реквизиты плательщика
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.PaymentPayerRequisites.Enabled", Criteria = "AllowEditPayerReq", Enabled = false)]
        //[Appearance("fmCDocRCBPaymentOrder.PaymentPayerRequisites.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[Appearance("fmCDocRCBPayment.PaymentPayerRequisites.Enabled", Method = "AllowEditPayer", Enabled = false)]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public fmCDocRCBRequisites PaymentPayerRequisites {
            get { return _PaymentPayerRequisites; }
            set { SetPropertyValue<fmCDocRCBRequisites>("PaymentPayerRequisites", ref _PaymentPayerRequisites, value); }
        }

        /// <summary>
        /// Реквизиты получателя платежа
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.PaymentReceiverRequisites.Enabled", Method = "AllowEditReceiver", Enabled = false)]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public fmCDocRCBRequisites PaymentReceiverRequisites {
            get { return _PaymentReceiverRequisites; }
            set { SetPropertyValue<fmCDocRCBRequisites>("PaymentReceiverRequisites", ref _PaymentReceiverRequisites, value); }
        }


        // КВИТАНЦИЯ

        /// <summary>
        /// TicketDate - КвитанцияДата
        /// </summary>
        public DateTime TicketDate {
            get { return _TicketDate; }
            set {
                SetPropertyValue<DateTime>("TicketDate", ref _TicketDate, value);
            }
        }

        /// <summary>
        /// TicketTime - КвитанцияВремя
        /// </summary>
        [Size(20)]
        public String TicketTime {
            get { return _TicketTime; }
            set {
                SetPropertyValue<String>("TicketTime", ref _TicketTime, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// TicketContent - КвитанцияВремя
        /// </summary>
        public String TicketContent {
            get { return _TicketContent; }
            set {
                SetPropertyValue<String>("TicketContent", ref _TicketContent, value == null ? String.Empty : value.Trim());
            }
        }


        // =============== Доп поля, которые есть в платёжке и не имеют ясной выраженности ===============

        /// <summary>
        /// OperationKind - Вид операции
        /// </summary>
        [Size(300)]
        public String OperationKind {
            get { return _OperationKind; }
            set {
                SetPropertyValue<String>("OperationKind", ref _OperationKind, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PaymentFunctionCode - наз. пл. - Назначение платежа кодовое
        /// </summary>
        [Size(10)]
        public String PaymentFunctionCode {
            get { return _PaymentFunctionCode; }
            set {
                SetPropertyValue<String>("PaymentFunctionCode", ref _PaymentFunctionCode, value == null ? String.Empty : value.Trim());
            }
        }

        ///// <summary>
        ///// PaymentCode - Код
        ///// </summary>
        //[Size(300)]
        //public String PaymentCode {
        //    get { return _PaymentCode; }
        //    set {
        //        SetPropertyValue<String>("PaymentCode", ref _PaymentCode, value == null ? String.Empty : value.Trim());
        //    }
        //}

        /// <summary>
        /// PaymentSequence - Очерёдность платежа
        /// </summary>
        [Size(300)]
        public String PaymentSequence {
            get { return _PaymentSequence; }
            set {
                SetPropertyValue<String>("PaymentSequence", ref _PaymentSequence, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PaymentDeadLine - Срок платежа
        /// </summary>
        [Size(300)]
        public String PaymentDeadLine {
            get { return _PaymentDeadLine; }
            set {
                SetPropertyValue<String>("PaymentDeadLine", ref _PaymentDeadLine, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PaymentResField - Рез. поле
        /// </summary>
        [Browsable(false)]
        [Size(300)]
        public String PaymentResField {
            get { return _PaymentResField; }
            set {
                SetPropertyValue<String>("PaymentResField", ref _PaymentResField, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// AcceptanceDuration - СрокАкцепта
        /// </summary>
        public Int16 AcceptanceDuration {
            get { return _AcceptanceDuration; }
            set {
                SetPropertyValue<Int16>("AcceptanceDuration", ref _AcceptanceDuration, value);
            }
        }

        /// <summary>
        /// AkkreditiveKind - ВидАккредитива
        /// </summary>
        [Size(300)]
        public String AkkreditiveKind {
            get { return _AkkreditiveKind; }
            set {
                SetPropertyValue<String>("AkkreditiveKind", ref _AkkreditiveKind, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PaymentCondition - УсловиеОплаты
        /// </summary>
        [Size(300)]
        public String PaymentCondition {
            get { return _PaymentCondition; }
            set {
                SetPropertyValue<String>("PaymentCondition", ref _PaymentCondition, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PaymentByRepresentation - ПлатежПоПредст
        /// </summary>
        [Size(300)]
        public String PaymentByRepresentation {
            get { return _PaymentByRepresentation; }
            set {
                SetPropertyValue<String>("PaymentByRepresentation", ref _PaymentByRepresentation, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// AdvancedConditions - ДополнУсловия
        /// </summary>
        [Size(300)]
        public String AdvancedConditions {
            get { return _AdvancedConditions; }
            set {
                SetPropertyValue<String>("AdvancedConditions", ref _AdvancedConditions, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// AccountNumberSupplier - НомерСчетаПоставщика
        /// </summary>
        [Size(300)]
        public String AccountNumberSupplier {
            get { return _AccountNumberSupplier; }
            set {
                SetPropertyValue<String>("AccountNumberSupplier", ref _AccountNumberSupplier, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// DocumentSendingDate - ДатаОтсылкиДок
        /// </summary>
        public DateTime DocumentSendingDate {
            get { return _DocumentSendingDate; }
            set {
                SetPropertyValue<DateTime>("DocumentSendingDate", ref _DocumentSendingDate, value);
            }
        }


        // ============================= Поля, не описанные в Положении, но имеющеся на платёжках (может быть они и не нужны) ================

        /// <summary>
        /// ReceivedByPayerBankDate - Поступило в банк плательщика (Поступ. в банк плат.)
        /// </summary>
        //[Browsable(false)]
        public DateTime ReceivedByPayerBankDate {
            get { return _ReceivedByPayerBankDate; }
            set {
                SetPropertyValue<DateTime>("ReceivedByPayerBankDate", ref _ReceivedByPayerBankDate, value);
            }
        }

        /// <summary>
        /// DeductedFromPayerAccount - Списано со счета плательщика (Списано со сч. плат.)
        /// </summary>
        //[Browsable(false)]
        public DateTime DeductedFromPayerAccount {
            get { return _DeductedFromPayerAccount; }
            set {
                SetPropertyValue<DateTime>("DeductedFromPayerAccount", ref _DeductedFromPayerAccount, value);
            }
        }

        /*
        /// <summary>
        /// StatementAccount - Ссылка на выписку, если экзмпляр загружен из выписки
        /// </summary>
        public fmStatementOfAccounts StatementAccount {
            get { return _StatementAccount; }
            set {
                SetPropertyValue<fmStatementOfAccounts>("StatementAccount", ref _StatementAccount, value);
            }
        }
        */
        // ---

        //[Association("fmStatementOfAccounts-fmCDocRCB")]
        //public fmStatementOfAccounts StatementOfAccounts {
        //    get { return _StatementOfAccounts; }
        //    set { SetPropertyValue<fmStatementOfAccounts>("StatementOfAccounts", ref _StatementOfAccounts, value); }
        //}



        #region Дополнительные поля Платёжки в бюджет

        /// <summary>
        /// Статус составителя
        /// </summary>
        [Size(300)]
        public String CompilerStatus {
            get { return _CompilerStatus; }
            set {
                SetPropertyValue<String>("CompilerStatus", ref _CompilerStatus, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ПоказательКБК
        /// </summary>
        [Size(300)]
        public String KBKStatus {
            get { return _KBKStatus; }
            set {
                SetPropertyValue<String>("KBKStatus", ref _KBKStatus, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ОКАТО
        /// </summary>
        [Size(300)]
        public String OKATO {
            get { return _OKATO; }
            set {
                SetPropertyValue<String>("OKATO", ref _OKATO, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ПоказательОснования
        /// </summary>
        [Size(300)]
        public String ReasonIndicator {
            get { return _ReasonIndicator; }
            set {
                SetPropertyValue<String>("ReasonIndicator", ref _ReasonIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ПоказательПериода
        /// </summary>
        [Size(300)]
        public String PeriodIndicator {
            get { return _PeriodIndicator; }
            set {
                SetPropertyValue<String>("PeriodIndicator", ref _PeriodIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ПоказательНомера
        /// </summary>
        [Size(300)]
        public String NumberIndicator {
            get { return _NumberIndicator; }
            set {
                SetPropertyValue<String>("NumberIndicator", ref _NumberIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ПоказательДаты
        /// </summary>
        [Size(300)]
        public String DateIndicator {
            get { return _DateIndicator; }
            set {
                SetPropertyValue<String>("DateIndicator", ref _DateIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ПоказательТипа
        /// </summary>
        [Size(300)]
        public String TypeIndicator {
            get { return _TypeIndicator; }
            set {
                SetPropertyValue<String>("TypeIndicator", ref _TypeIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        #endregion


        /// <summary>
        /// TypeIndicator - Результат исхода (успешно - true, неуспешно - false) постобработки документа при его загрузке
        /// </summary>
        public bool PostProcessResult {
            get { return _PostProcessResult; }
            set {
                SetPropertyValue<bool>("PostProcessResult", ref _PostProcessResult, value);
            }
        }

        ///// <summary>
        ///// true, если документ является документом выписки (признак выписки)
        ///// </summary>
        //public bool StatementAccountIndicator {
        //    get { return _StatementAccountIndicator; }
        //    set {
        //        SetPropertyValue<bool>("StatementAccountIndicator", ref _StatementAccountIndicator, value);
        //    }
        //}

        /// <summary>
        /// Ссылка на платёжный документ для документа выписки
        /// </summary>
        public fmCDocRCB PaymentDocument {
            get { return _PaymentDocument; }
            set {
                SetPropertyValue<fmCDocRCB>("PaymentDocument", ref _PaymentDocument, value);
            }
        }

        /// <summary>
        /// DocType - // Тип документа (Банковский ордер и т.п., кроме тех 4-х, которые явно обозначены в документации 1С)
        /// </summary>
        [Size(100)]
        public String DocType {
            get { return _DocType; }
            set {
                SetPropertyValue<String>("DocType", ref _DocType, value == null ? String.Empty : value.Trim());
            }
        }

        ///// <summary>
        ///// Тип документа fmCDocRCBPaymentOrder и т.п.
        ///// </summary>
        //[ValueConverter(typeof(ConverterType2String))]
        //public Type TypeOfDocument
        //{
        //    get { return _TypeOfDocument; }
        //    set
        //    {
        //        SetPropertyValue<Type>("TypeOfDocument", ref _TypeOfDocument, value);
        //    }
        //}

        /// <summary>
        /// Наименование типа документа fmCDocRCBPaymentOrder и т.п.
        /// </summary>
        public String NameTypeOfRCBDocument {
            get { return _NameTypeOfRCBDocument; }
            set {
                SetPropertyValue<String>("NameTypeOfRCBDocument", ref _NameTypeOfRCBDocument, value);
            }
        }

        /// <summary>
        /// Ссылка на результат импорта, по которому документ получен
        /// </summary>
        [Browsable(false)]
        [Association("fmImportResult-fmStatementAccountDoc")]
        public fmCSAImportResult ImportResult {
            get { return _ImportResult; }
            set {
                SetPropertyValue<fmCSAImportResult>("ImportResult", ref _ImportResult, value);
            }
        }

        ///// <summary>
        ///// Сумма, равная PaymentCost, если PaymentCost - это приход
        ///// </summary>
        //[Browsable(false)]
        //public Decimal SumIn {
        //    get { return _SumIn; }
        //    set {
        //        SetPropertyValue<Decimal>("SumIn", ref _SumIn, value);
        //    }
        //}

        ///// <summary>
        ///// Сумма, равная PaymentCost, если PaymentCost - это расход
        ///// </summary>
        //[Browsable(false)]
        //public Decimal SumOut {
        //    get { return _SumOut; }
        //    set {
        //        SetPropertyValue<Decimal>("SumOut", ref _SumOut, value);
        //    }
        //}

        /// <summary>
        /// Признак того, что для связанного Платёжного документа обработка завершена
        /// </summary>
        public bool SignProcessing {
            get {
                if (PaymentDocument != null && PaymentDocument.AmongRegisterDifference() == 0) 
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Ссылка на выписку (случай поступления средств)
        /// </summary>
        [Association("fmStatementOfAccount-fmStatementOfAccountInDocs")]
        public fmCSAStatementAccount StatementAccountIn {
            get {
                return _StatementAccountIn;
            }
            set {
                SetPropertyValue<fmCSAStatementAccount>("StatementAccountIn", ref _StatementAccountIn, value);
            }
        }

        /// <summary>
        /// Ссылка на выписку (случай списания средств)
        /// </summary>
        [Association("fmStatementOfAccount-fmStatementOfAccountOutDocs")]
        public fmCSAStatementAccount StatementAccountOut {
            get {
                return _StatementAccountOut;
            }
            set {
                SetPropertyValue<fmCSAStatementAccount>("StatementAccountOut", ref _StatementAccountOut, value);
            }
        }


        #endregion

        #region МЕТОДЫ

        public void SetAllowEdit() {
            AllowEditProperty = true;
            PaymentReceiverRequisites.SetAllowEdit();
        }

        public void SetDisAllowEdit() {
            AllowEditProperty = false;
            PaymentReceiverRequisites.SetDisAllowEdit();
        }

        public bool GetAllowEdit() {
            //return AllowEditProperty && !ReadOnly;
            return (AllowEditProperty && !ReadOnly) || this.PaymentPayerRequisites.ImportErrorStates > 0 || this.PaymentReceiverRequisites.ImportErrorStates > 0;
        }

        protected override void OnSaving() {
            AllowEditProperty = false;
            base.OnSaving();
        }

        #endregion

    }

}
