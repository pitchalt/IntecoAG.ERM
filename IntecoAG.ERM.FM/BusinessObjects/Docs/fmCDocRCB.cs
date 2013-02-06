using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;

namespace IntecoAG.ERM.FM.Docs {

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

    [NavigationItem("Money")]
    //[Persistent("fmDocRCB")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [DefaultProperty("DocRCBName")]
    public class fmCDocRCB : csCDocRCB    //csCComponent
    {
        public fmCDocRCB(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCDocRCB);
            this.CID = Guid.NewGuid();

            PaymentPayerRequisites = new fmCDocRCBRequisites(this.Session);
            PaymentPayerRequisites.PaymentDoc = this;
            PaymentReceiverRequisites = new fmCDocRCBRequisites(this.Session);
            PaymentReceiverRequisites.PaymentDoc = this;
            
            AllowEditProperty = true;
        }

        #region ПОЛЯ КЛАССА

        private Decimal _Accuracy = 0.01m;  // Сопоставление с точностью до копейки

        private bool _AllowEditProperty;   // Разрешение редактирования

        private String _PaymentKind; // вид платежа
        private String _PaymentFunction; // назначение платежа
        private Decimal _PaymentCost;  // сумма платежа
        private String _PaymentCostCopybook;  // сумма платежа прописью

        private fmCDocRCBRequisites _PaymentPayerRequisites; // Реквизиты плательщика
        private fmCDocRCBRequisites _PaymentReceiverRequisites; // Реквизиты получателя платежа

        // Квитанция
        private DateTime _TicketDate;  // КвитанцияДата
        private String _TicketTime;  // КвитанцияВремя
        private String _TicketContent;  // КвитанцияВремя

        // Разные поля в платёжке

        private String _OperationKind; // Вид операции
        private String _PaymentFunctionCode; // наз. пл. - Назначение платежа кодовое
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

        // Доп. поля Платёжки в бюджет
        private String _CompilerStatus;   // Статус составителя
        private String _KBKStatus;   // ПоказательКБК
        private String _OKATO;   // ОКАТО
        private String _ReasonIndicator;   // ПоказательОснования
        private String _PeriodIndicator;   // ПоказательПериода
        private String _NumberIndicator;   // ПоказательНомера
        private String _DateIndicator;   // ПоказательДаты
        private String _TypeIndicator;   // ПоказательТипа

        private String _DocType; // Тип документа (Банковский ордер и т.п., кроме тех 4-х, которые явно обозначены в документации 1С)

        private PaymentDocProcessingStates _State; // Состояние обработки платёжного документа

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Разрешение редактирования
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
        /// Вид платежа
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
        /// Назначение платежа
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
        /// Сумма платежа
        /// </summary>
        //[RuleRequiredField]
        public Decimal PaymentCost {
            get { return _PaymentCost; }
            set {
                SetPropertyValue<Decimal>("PaymentCost", ref _PaymentCost, value);
            }
        }

        /// <summary>
        /// Сумма платежа прописью
        /// </summary>
        [Size(300)]
        //[RuleRequiredField]
        public String PaymentCostCopybook {
            get { return _PaymentCostCopybook; }
            set {
                SetPropertyValue<String>("PaymentCostCopybook", ref _PaymentCostCopybook, value == null ? String.Empty : value.Trim());
            }
        }

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
        /// КвитанцияДата
        /// </summary>
        public DateTime TicketDate {
            get { return _TicketDate; }
            set {
                SetPropertyValue<DateTime>("TicketDate", ref _TicketDate, value);
            }
        }

        /// <summary>
        /// КвитанцияВремя
        /// </summary>
        [Size(20)]
        public String TicketTime {
            get { return _TicketTime; }
            set {
                SetPropertyValue<String>("TicketTime", ref _TicketTime, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// КвитанцияВремя
        /// </summary>
        public String TicketContent {
            get { return _TicketContent; }
            set {
                SetPropertyValue<String>("TicketContent", ref _TicketContent, value == null ? String.Empty : value.Trim());
            }
        }


        /// <summary>
        /// Вид операции
        /// </summary>
        [Size(300)]
        public String OperationKind {
            get { return _OperationKind; }
            set {
                SetPropertyValue<String>("OperationKind", ref _OperationKind, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Назначение платежа кодовое
        /// </summary>
        [Size(10)]
        public String PaymentFunctionCode {
            get { return _PaymentFunctionCode; }
            set {
                SetPropertyValue<String>("PaymentFunctionCode", ref _PaymentFunctionCode, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Очерёдность платежа
        /// </summary>
        [Size(300)]
        public String PaymentSequence {
            get { return _PaymentSequence; }
            set {
                SetPropertyValue<String>("PaymentSequence", ref _PaymentSequence, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Срок платежа
        /// </summary>
        [Size(300)]
        public String PaymentDeadLine {
            get { return _PaymentDeadLine; }
            set {
                SetPropertyValue<String>("PaymentDeadLine", ref _PaymentDeadLine, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Рез. поле
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
        /// СрокАкцепта
        /// </summary>
        public Int16 AcceptanceDuration {
            get { return _AcceptanceDuration; }
            set {
                SetPropertyValue<Int16>("AcceptanceDuration", ref _AcceptanceDuration, value);
            }
        }

        /// <summary>
        /// ВидАккредитива
        /// </summary>
        [Size(300)]
        public String AkkreditiveKind {
            get { return _AkkreditiveKind; }
            set {
                SetPropertyValue<String>("AkkreditiveKind", ref _AkkreditiveKind, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// УсловиеОплаты
        /// </summary>
        [Size(300)]
        public String PaymentCondition {
            get { return _PaymentCondition; }
            set {
                SetPropertyValue<String>("PaymentCondition", ref _PaymentCondition, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ПлатежПоПредст
        /// </summary>
        [Size(300)]
        public String PaymentByRepresentation {
            get { return _PaymentByRepresentation; }
            set {
                SetPropertyValue<String>("PaymentByRepresentation", ref _PaymentByRepresentation, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ДополнУсловия
        /// </summary>
        [Size(300)]
        public String AdvancedConditions {
            get { return _AdvancedConditions; }
            set {
                SetPropertyValue<String>("AdvancedConditions", ref _AdvancedConditions, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// НомерСчетаПоставщика
        /// </summary>
        [Size(300)]
        public String AccountNumberSupplier {
            get { return _AccountNumberSupplier; }
            set {
                SetPropertyValue<String>("AccountNumberSupplier", ref _AccountNumberSupplier, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ДатаОтсылкиДок
        /// </summary>
        public DateTime DocumentSendingDate {
            get { return _DocumentSendingDate; }
            set {
                SetPropertyValue<DateTime>("DocumentSendingDate", ref _DocumentSendingDate, value);
            }
        }


        // ============================= Поля, не описанные в Положении, но имеющеся на платёжках ================

        /// <summary>
        /// Поступило в банк плательщика (Поступ. в банк плат.)
        /// </summary>
        public DateTime ReceivedByPayerBankDate {
            get { return _ReceivedByPayerBankDate; }
            set {
                SetPropertyValue<DateTime>("ReceivedByPayerBankDate", ref _ReceivedByPayerBankDate, value);
            }
        }

        /// <summary>
        /// Списано со счета плательщика (Списано со сч. плат.)
        /// </summary>
        public DateTime DeductedFromPayerAccount {
            get { return _DeductedFromPayerAccount; }
            set {
                SetPropertyValue<DateTime>("DeductedFromPayerAccount", ref _DeductedFromPayerAccount, value);
            }
        }



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
        /// Тип документа (Банковский ордер и т.п., кроме тех 4-х, которые явно обозначены в документации 1С)
        /// </summary>
        [Size(100)]
        public String DocType {
            get { return _DocType; }
            set {
                SetPropertyValue<String>("DocType", ref _DocType, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Состояние обработки платёжного документа
        /// </summary>
        public PaymentDocProcessingStates State {
            get {
                return _State;
            }
            set {
                SetPropertyValue<PaymentDocProcessingStates>("State", ref _State, value);
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

        public Decimal AmongRegisterDifferenceOLD() {
            // Вычисляем разность по оборотному регистру и регистру привязок

            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(this.Session);
            decimal operationJournalSumOut = (from operationJournal in operationJournals
                                           where operationJournal.PaymentDocument == this
                                           select operationJournal.SumOut).Sum();

            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session);
            decimal repaymentJournalsSumOut = (from repaymentJournal in repaymentJournals
                                            where repaymentJournal.PaymentDocument == this
                                            select repaymentJournal.SumOut).Sum();

            return operationJournalSumOut - repaymentJournalsSumOut;
        }

        public Decimal AmongRegisterDifference() {
            // Вычисляем разность по оборотному регистру и регистру привязок с учётом валют платежа и договора
            Decimal operationJournalSumOut = 0, repaymentJournalsSumOut = 0;

            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(this.Session);
            var operationJournalQuery = from operationJournal in operationJournals
                                              where operationJournal.PaymentDocument == this
                                              select operationJournal;
            foreach (var operationJournal in operationJournalQuery) {
                DateTime operationDate = operationJournal.OperationDate.Date;
                operationJournalSumOut += operationJournal.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(Session, operationDate, operationJournal.Valuta, GetValutaByCode("RUB"));
            }

            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session);
            var repaymentJournalQuery = from repaymentJournal in repaymentJournals
                                               where repaymentJournal.PaymentDocument == this
                                               select repaymentJournal;
            foreach (var repaymentJournal in repaymentJournalQuery) {
                DateTime operationDate = repaymentJournal.PaymentDate.Date;
                repaymentJournalsSumOut += repaymentJournal.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(Session, operationDate, repaymentJournal.ValutaPayment, GetValutaByCode("RUB"));
            }

            return operationJournalSumOut - repaymentJournalsSumOut;
        }

        private csValuta GetValutaByCode(string code) {
            XPQuery<csValuta> valutas = new XPQuery<csValuta>(Session);
            csValuta Valuta = (from valuta in valutas
                               where valuta.Code.ToUpper() == code.ToUpper()
                               select valuta).FirstOrDefault();
            return Valuta;
        }


        public Decimal AccountBalance() {
            // Вычисляем баланс текущего документа по оборотному регистру

            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(this.Session);
            decimal operationJournalSumOut = (from operationJournal in operationJournals
                                              where operationJournal.PaymentDocument == this
                                              select operationJournal.SumOut).Sum();
            decimal operationJournalSumIn = (from operationJournal in operationJournals
                                             where operationJournal.PaymentDocument == this
                                             select operationJournal.SumIn).Sum();

            return operationJournalSumIn - operationJournalSumOut;
        }

        /// <summary>
        /// Входящая сумма (относительно "нашей" организации)
        /// </summary>
        /// <returns></returns>
        public Decimal GetSumIn(crmBankAccount bankAccount) {
            Decimal Res = 0;

            // Наша организация
            crmCParty OurParty = GetOurParty();

            if (this.PaymentReceiverRequisites.INN == OurParty.INN && this.PaymentReceiverRequisites.StatementOfAccount.BankAccount == bankAccount) {
                Res = this.PaymentCost;
            }

            return Res;
        }

        /// <summary>
        /// Исходящая сумма (относительно "нашей" организации)
        /// </summary>
        /// <returns></returns>
        public Decimal GetSumOut(crmBankAccount bankAccount) {
            Decimal Res = 0;

            // Наша организация
            crmCParty OurParty = GetOurParty();

            if (this.PaymentPayerRequisites.INN == OurParty.INN && this.PaymentPayerRequisites.StatementOfAccount.BankAccount == bankAccount) {
                Res = this.PaymentCost;
            }

            return Res;
        }

        public virtual void AutoBinding(fmCSAStatementAccountDoc StatementAccountDoc, fmCPRRepaymentTask RepaymentTask) {
            fmCSAStatementAccount statement = null;
            statement = StatementAccountDoc.PaymentPayerRequisites.StatementOfAccount;
            if (statement != null && statement.BankAccount != null) {
                AutoBinding(statement.BankAccount, RepaymentTask);
            }
            //if (statement == null) {
            statement = StatementAccountDoc.PaymentReceiverRequisites.StatementOfAccount;
            //}
            if (statement != null && statement.BankAccount != null) {
                AutoBinding(statement.BankAccount, RepaymentTask);
            }
        }

        public virtual void AutoBinding(crmBankAccount BankAccount, fmCPRRepaymentTask RepaymentTask) {
            // Несколько замечаний. 
            // Сопоставление ведётся только с уже существующими заявками в статусе IN_PAY.
            // Для данного платёжного документа выбираются только такие заявки, в которых Плательщик и Получатель 
            // соответствуют таковым в платёжном документе.
            // Часть суммы платёжного документа, не покрытая заявками, должна равняться части суммы Заявки, не истраченной
            // на Платёжные документы.
            // Распределение средств Заявки из предыдущего пункта происходит по дням в операционном журнале.

            // Список счетов "нашей" организации
            XPQuery<crmBankAccount> ourBankAccounts = new XPQuery<crmBankAccount>(Session);
            var OurBankAccounts = (from bankAccount in ourBankAccounts
                                   where bankAccount.PrefferedParty.Code == GetOurParty().Code   //"2518"
                                   select bankAccount).ToList<crmBankAccount>();

            DateTime DateAccountChanged = (this.ReceivedByPayerBankDate != DateTime.MinValue) ? this.ReceivedByPayerBankDate : ((this.DeductedFromPayerAccount != DateTime.MinValue) ? this.DeductedFromPayerAccount : DateTime.MinValue);
            csValuta paymentValuta = crmBankAccount.GetValutaByBankAccount(Session, BankAccount);

            // Сумма Платёжного документа по OperationJournal (в валюте платежа)
            Decimal operationPaymentDocSumIn = 0, operationPaymentDocSumOut = 0;
            GetPaymentDocSumByOperationJournal(this, BankAccount, out operationPaymentDocSumIn, out operationPaymentDocSumOut);     // @@@@@@@@@@@ Добавить проверку по BankAccount
            // Одна из сумм operationPaymentDocSumIn или operationPaymentDocSumOut обязательно равна 0

            // Сумма Платёжного документа по RepaymentJournal (в валюте платежа - это величины SumIn и SumOut)
            Decimal repaymentDocSumIn = 0, repaymentDocSumOut = 0;
            GetPaymentDocSumByRepaymentJournal(this, BankAccount, out repaymentDocSumIn, out repaymentDocSumOut);   // @@@@@@@@@@@ Добавить проверку по BankAccount
            // Одна из сумм repaymentDocSumIn или repaymentDocSumOut также обязательно равна 0

            // Величина непокрытия Платёжного документа Заявками (все суммы в валюте платежа)
            Decimal deltaDocSumIn = operationPaymentDocSumIn - repaymentDocSumIn;
            Decimal deltaDocSumOut = operationPaymentDocSumOut - repaymentDocSumOut;

            if (Decimal.Compare(Math.Abs(deltaDocSumIn) + Math.Abs(deltaDocSumOut), _Accuracy) <= 0)
                return; // Всё сопоставлено уже с точностью до _Accuracy - так условились!

            // Одна (или обе) из сумм deltaDocSumIn или deltaDocSumOut также равна (равны) 0
            Decimal deltaDocSum = deltaDocSumIn + deltaDocSumOut;   // !!!!!!! ПЕРЕСМОТРЕТЬ ?????????

            // Поиск подходящей заявки (Статус, Плательщик, Получатель, остаточные суммы)
            XPQuery<fmCPRPaymentRequest> paymentRequests = new XPQuery<fmCPRPaymentRequest>(Session, true);
            var queryPaymentRequests = from paymentRequest in paymentRequests
                                       where (paymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.State == PaymentRequestStates.IN_BANK)
                                          && paymentRequest.PartyPayReceiver == this.PaymentReceiverRequisites.Party
                                          && paymentRequest.PartyPaySender == this.PaymentPayerRequisites.Party
                                          && OurBankAccounts.Contains<crmBankAccount>(this.PaymentPayerRequisites.BankAccount)   // Означает РАСХОД
                                          && DateAccountChanged.Date >= paymentRequest.Date.Date //&& DateAccountChanged.Date < paymentRequest.Date.AddDays(3).Date
                                       select paymentRequest;

            foreach (var paymentRequest in queryPaymentRequests) {
                // Отбраковка: 
                // (1) сумма заявки не должна быть исчерпана полностью, 
                // (2) остаточная сумма должна равняться величине непокрытия Платёжного документа

                Decimal paymentRequestSumIn = 0, paymentRequestSumOut = 0;   // Эти суммы в валюте платежа
                Decimal paymentRequestSumObligationIn = 0, paymentRequestSumObligationOut = 0;   // Эти суммы в валюте обязательств
                GetPaymentRequestSumByRepaymentJournal(
                        paymentRequest, 
                        BankAccount, 
                        out paymentRequestSumIn, 
                        out paymentRequestSumOut, 
                        out paymentRequestSumObligationIn,
                        out paymentRequestSumObligationOut
                    );

                // Величина неиспользованности Заявки   !!!!!!!! ПЕРЕСМОТРЕТЬ !!!!!!!!!!!
                // Поясение о вычислении неиспользованности. Сумма в заявке - это в валюте обязательств, чтобы её сравнить
                // с суммой платежа, надо перевести по кросс-курсу к валюте платежа и уже полученную сумму сравнить. Вопрос: на
                // какой день брать курс? Предлагается брать DateAccountChanged
                //Decimal deltaRequestSum = GetRequestSumByCourse(paymentRequest, paymentRequest.Valuta);   // В валюте платежа
                Decimal deltaRequestSum = GetRequestSumByCourse(paymentRequest, paymentValuta);   // В валюте платежа
                if (this.PaymentPayerRequisites.BankAccount == BankAccount) {
                    deltaRequestSum -= paymentRequestSumOut;
                } else if (this.PaymentReceiverRequisites.BankAccount == BankAccount) {
                    deltaRequestSum -= paymentRequestSumIn;
                }

                if (Decimal.Compare(Math.Abs(deltaRequestSum), _Accuracy) <= 0)
                    continue;   // Переход к следующей заявке (тогда у заявки должен был бы быть статус PAYED и она не должна была попасть в рассмотрение - это предусловие контракта)

                // Сравнение с точностью до 1 копейки
                if (Decimal.Compare(Math.Abs(deltaDocSum - deltaRequestSum), _Accuracy) <= 0) {
                    // Создаём задачу привязки
                    Session uow = this.Session;
                    //using (UnitOfWork uow = new UnitOfWork(Session.ObjectLayer)) {
                    fmCPRRepaymentTask task = new fmCPRRepaymentTask(uow);
                        task.BankAccount = BankAccount;
                        task.PaymentDocument = this;
                        task.FillRepaymentTaskLines();
                        task.FillRequestList();
                        task.DoBindingRequest(paymentRequest, true, 0);
                        //uow.CommitChanges();
                        //}
                    // Поскольку заявка исчерпана, то меняем ей статус
                    paymentRequest.State = PaymentRequestStates.PAYED;
                    this.State = PaymentDocProcessingStates.PROCESSED;

                    // Заявка и документ выписки полностью взаимопокрылись, поэтому
                    break;
                }
            }
        }


        /// <summary>
        /// Сумма заявки в рублях по курсу указанной в заявке валюты на указанную там же дату
        /// </summary>
        /// <param name="paymentRequest">Заявка</param>
        /// <param name="valutaPayment">Валюта к которой надо привести сумму</param>
        /// <returns>Сумма в валюте valutaPayment</returns>
        private Decimal GetRequestSumByCourse(fmCPRPaymentRequest paymentRequest, csValuta valutaPayment) {
            // Поясение о вычислении. Сумма в заявке - это в валюте обязательств, чтобы её сравнить
            // с суммой платежа, надо перевести по кросс-курсу к валюте платежа и уже полученную сумму сравнить.
            // Предлагается в качестве даты курса брать непустую из двух дат: this.DeductedFromPayerAccount или this.ReceivedByPayerBankDate

            // Тривиальный случай: валюта платежа совпадает с валютой обязательство
            if (paymentRequest.Valuta == valutaPayment) {
                return paymentRequest.Summ;
            }
            
            // Валюты платежа и обязательств не совпадают. Надо вычислять кросс-курс на дату DateAccountChanged (изменения счёта)
            DateTime DateAccountChanged = (this.ReceivedByPayerBankDate != DateTime.MinValue) ? this.ReceivedByPayerBankDate : ((this.DeductedFromPayerAccount != DateTime.MinValue) ? this.DeductedFromPayerAccount : DateTime.MinValue);
            Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(Session, DateAccountChanged, paymentRequest.Valuta, valutaPayment);

            Decimal resultSum = Math.Round(paymentRequest.Summ * crossCource, 4);
            return resultSum;
        }

        private void GetPaymentDocSumByOperationJournal(fmCDocRCB PaymentDocument, crmBankAccount bankAccount, out Decimal SumIn, out Decimal SunOut)
        {
            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(this.Session, true);
            var queryOperationJournals = (from operationJournal in operationJournals
                                         where operationJournal.PaymentDocument == PaymentDocument
                                            && operationJournal.BankAccount == bankAccount
                                          select operationJournal).ToList<fmCSAOperationJournal>();

            // Как я понял, валюта выписки определяется по 3-м знакам счёта выписки, поэтому валюта везде одинаковая и суммирование ниже корректно.
            // В Валюте платежа
            SumIn = queryOperationJournals.Sum(x => x.SumIn);
            SunOut = queryOperationJournals.Sum(x => x.SumOut);
        }

        private void GetPaymentDocSumByRepaymentJournal(fmCDocRCB PaymentDocument, crmBankAccount bankAccount, out Decimal SumIn, out Decimal SunOut)
        {
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
            var queryRepaymentJournals = (from repaymentJournal in repaymentJournals
                                         where repaymentJournal.PaymentDocument == PaymentDocument
                                            && repaymentJournal.BankAccount == bankAccount
                                          select repaymentJournal).ToList<fmCPRRepaymentJurnal>();

            // В журнале привязок хранятся величины сумм для валют - платежа и обязательства. Суммы берём в валюте платежа.
            SumIn = queryRepaymentJournals.Sum(x => x.SumIn);
            SunOut = queryRepaymentJournals.Sum(x => x.SumOut);
        }

        private void GetPaymentRequestSumByRepaymentJournal(fmCPRPaymentRequest PaymentRequest, crmBankAccount bankAccount, out Decimal SumIn, out Decimal SunOut, out Decimal SumObligationIn, out Decimal SunObligationOut)
        {
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
            var queryRepaymentJournals = (from repaymentJournal in repaymentJournals
                                         where repaymentJournal.PaymentRequest == PaymentRequest
                                            && repaymentJournal.BankAccount == bankAccount
                                          select repaymentJournal).ToList<fmCPRRepaymentJurnal>();

            SumIn = queryRepaymentJournals.Sum(x => x.SumIn);
            SunOut = queryRepaymentJournals.Sum(x => x.SumOut);

            SumObligationIn = queryRepaymentJournals.Sum(x => x.SumObligationIn);
            SunObligationOut = queryRepaymentJournals.Sum(x => x.SumObligationOut);
        }


        public virtual void AutoBinding(crmBankAccount BankAccount, fmCPRPaymentRequest PaymentRequest, fmCPRRepaymentTask RepaymentTask) {
            // Привязка с распределением счетов

            // Проверка на уже связанность заявки
            XPQuery<fmCPRRepaymentJurnal> repaymentJurnalExists = new XPQuery<fmCPRRepaymentJurnal>(this.Session);
            var queryRepaymentJurnalExists = from repaymentJurnal in repaymentJurnalExists
                                        where repaymentJurnal.PaymentDocument == this
                                        select repaymentJurnal;
            if (queryRepaymentJurnalExists.Count() > 0) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Заявка уже привязана");
                return;
            }

            // Проверка на превышение сумм
            if (RepaymentTask.RepaymentRegisterSum + PaymentRequest.Summ > RepaymentTask.OperationRegisterSum) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Сумма связанных заявок превысит сумму по документам выписки. Привязка невозможна.");
                return;
            }

            
            // Наша организация
            crmCParty OurParty = GetOurParty();

            // Привязка в принципе возможна, т.е. выбранная заявка добавляется в fmCPRRepaymentJurnal
            fmCPRRepaymentJurnal newRepaymentJournalRecord = new fmCPRRepaymentJurnal(Session);
            newRepaymentJournalRecord.BankAccount = BankAccount;
            //newRepaymentJournalRecord.PaymentDate = 
            newRepaymentJournalRecord.PaymentDocument = this;
            newRepaymentJournalRecord.PaymentRequest = PaymentRequest;
            newRepaymentJournalRecord.PlaneFact = CRM.Contract.Analitic.PlaneFact.FACT;

            if (OurParty.INN == PaymentRequest.PartyPayReceiver.INN) {
                newRepaymentJournalRecord.SumIn = PaymentRequest.Summ;
            }
            if (OurParty.INN == PaymentRequest.PartyPaySender.INN) {
                newRepaymentJournalRecord.SumOut = PaymentRequest.Summ;
            }

            newRepaymentJournalRecord.ValutaObligation = PaymentRequest.Valuta;  // Валюта обязательств
            newRepaymentJournalRecord.ValutaPayment = PaymentRequest.PaymentValuta;   // Валюта платежа


            // Обновление таблицы RepaymentTaskLines

            // Удаление всех записей из fmCPRRepaymentTaskLine
            Session.Delete(RepaymentTask.RepaymentTaskLines);
            //Session.PurgeDeletedObjects();

            // Журнал привязок
            XPQuery<fmCPRRepaymentJurnal> repaymentJurnals = new XPQuery<fmCPRRepaymentJurnal>(this.Session);
            var queryRepaymentJurnals = from repaymentJurnal in repaymentJurnals
                                        where repaymentJurnal.PaymentDocument == this
                                        select repaymentJurnal;
            var queryGroupRepaymentJurnals = from repaymentJurnal in queryRepaymentJurnals
                                             group repaymentJurnal by new { repaymentJurnal.PaymentRequest }
                                                 into grj
                                                 select new {
                                                     PaymentRequest = grj.Key.PaymentRequest,
                                                     GroupSumIn = grj.Sum(row => row.SumIn),
                                                     GroupSumOut = grj.Sum(row => row.SumOut)
                                                 };

            foreach (var grj in queryGroupRepaymentJurnals) {
                // Добавляем запись в fmCPRRepaymentTaskLine
                fmCPRRepaymentTaskLine newLine = new fmCPRRepaymentTaskLine(Session);
                newLine.RepaymentTask = RepaymentTask;
                RepaymentTask.RepaymentTaskLines.Add(newLine);
                newLine.PaymentRequest = PaymentRequest;
                newLine.RequestSum = grj.GroupSumIn + grj.GroupSumOut;   // Одна из них равна 0
            }
        }

        /// <summary>
        /// Ручная привязка по кнопке
        /// </summary>
        /// <param name="RepaymentTask"></param>
        public virtual void ManualBinding(fmCPRRepaymentTask RepaymentTask) {
            // Алгоритм. Из задачи RepaymentTask выбирается Платёжный документ,
        }

        private crmCParty GetOurParty() {
            // Наша организация
            crmCParty _OurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    _OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }
            return _OurParty;
        }

        #region Вспомоательные функции, зависящие от "нашей" партии. Не ясно,правильно ли они булут работать в случае финансового платежа ?????

        /// <summary>
        /// Дата изменения счёта (в зависимости от того, какая сторона является "нашей")
        /// </summary>
        /// <returns></returns>
        public DateTime GetAccountDateChange() {
            crmCParty OurParty = GetOurParty();
            DateTime AccountDateChanged = DeductedFromPayerAccount;
            if (this.PaymentPayerRequisites.Party == OurParty) {
                //return DeductedFromPayerAccount;
                AccountDateChanged = DeductedFromPayerAccount;
            } else {
                //return ReceivedByPayerBankDate;
                AccountDateChanged = ReceivedByPayerBankDate;
            }

            // Если дата не определилась (редкий случай)
            if (AccountDateChanged == DateTime.MinValue) {
                AccountDateChanged = DeductedFromPayerAccount;
            }
            if (AccountDateChanged == DateTime.MinValue) {
                AccountDateChanged = ReceivedByPayerBankDate;
            }
            return AccountDateChanged;
        }

        /// <summary>
        /// Валюта счёта (в зависимости от того, какая сторона является "нашей")
        /// </summary>
        /// <returns></returns>
        public csValuta GetAccountValuta() {
            crmCParty OurParty = GetOurParty();
            if (this.PaymentPayerRequisites.Party == OurParty) {
                return crmBankAccount.GetValutaByBankAccount(Session, PaymentPayerRequisites.BankAccount);
            } else {
                return crmBankAccount.GetValutaByBankAccount(Session, PaymentReceiverRequisites.BankAccount);
            }
        }
        #endregion

        #endregion

    }

}
