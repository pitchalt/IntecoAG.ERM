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
    //[NavigationItem]
    [MapInheritance(MapInheritanceType.ParentTable)]
    //[Persistent("fmDocRCBPayment")]
    public class fmCDocRCBPayment : fmCDocRCB
    {
        public fmCDocRCBPayment(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            PaymentPayerRequisites = new fmCDocRCBRequisites(this.Session);
            PaymentReceiverRequisites = new fmCDocRCBRequisites(this.Session);
            this.ComponentType = typeof(fmCDocRCBPaymentOrder);
            this.CID = Guid.NewGuid();

            AllowEditProperty = true;
        }

        #region ПОЛЯ КЛАССА

        private bool _AllowEditProperty;   // Разрешение редактирования

        //private String _DocNumber; // номер платёжного документа
        //private DateTime _DocDate; // дата плтежа
        private String _PaymentKind; // вид платежа
        private String _PaymentFunction; // назначение платежа
        private Decimal _PaymentCost;  // сумма платежа
        private String _PaymentCostCopybook;  // сумма платежа прописью
        //private Decimal _PaymentTax;  // налог

        private fmCDocRCBRequisites _PaymentPayerRequisites; // Реквизиты плательщика
        private fmCDocRCBRequisites _PaymentReceiverRequisites; // Реквизиты получателя платежа

        // Разные поля в платёжке

        private String _OperationKind; // Вид операции
        private String _PaymentFunctionCode; // наз. пл. - Назначение платежа кодовое
        private String _PaymentCode; // Код
        private String _PaymentSequence; // Очерёдность платежа
        private String _PaymentDeadLine; // Срок платежа
        private String _PaymentResField; // Рез. поле


        private DateTime _ReceivedByPayerBankDate;    // Поступило в банк плательщика (Поступ. в банк плат.)
        private DateTime _DeductedFromPayerAccount;   // Списано со счета плательщика (Списано со сч. плат.)

        private fmStatementOfAccounts _StatementAccount;   // Ссылка на выписку, если экзмпляр загружен из выписки

        // Доп. поля из файла 1С
        private String _CompilerStatus;   // Статус составителя
        private String _KBKStatus;   // ПоказательКБК
        private String _OKATO;   // ОКАТО
        private String _ReasonIndicator;   // ПоказательОснования
        private String _PeriodIndicator;   // ПоказательПериода
        private String _NumberIndicator;   // ПоказательНомера
        private String _DateIndicator;   // ПоказательДаты
        private String _TypeIndicator;   // ПоказательТипа

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
        /// PaymentKind - вид платежа
        /// </summary>
        //[Appearance("fmCDocRCBPayment.PaymentKind.Enabled", Method = "AllowEditPayer", Enabled = false)]
        [Size(300)]
        public String PaymentKind {
            get { return _PaymentKind; }
            set {
                SetPropertyValue<String>("PaymentKind", ref _PaymentKind, value == null ? String.Empty : value.Trim()); }
        }

        /// <summary>
        /// PaymentFunction - назначение платежа
        /// </summary>
        //[Appearance("fmCDocRCBPayment.PaymentFunction.Enabled", Method = "AllowEditPayer", Enabled = false)]
        [Size(300)]
        public String PaymentFunction {
            get { return _PaymentFunction; }
            set {
                SetPropertyValue<String>("PaymentFunction", ref _PaymentFunction, value == null ? String.Empty : value.Trim()); }
        }

        /// <summary>
        /// PaymentCost - сумма платежа
        /// </summary>
        //[RuleRequiredField]
        public Decimal PaymentCost {
            get { return _PaymentCost; }
            set {
                SetPropertyValue<Decimal>("PaymentCost", ref _PaymentCost, value); }
        }

        /// <summary>
        /// PaymentCostCopybook - сумма платежа прописью
        /// </summary>
        [Size(300)]
        //[RuleRequiredField]
        public String PaymentCostCopybook {
            get { return _PaymentCostCopybook; }
            set {
                SetPropertyValue<String>("PaymentCostCopybook", ref _PaymentCostCopybook, value == null ? String.Empty : value.Trim()); }
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

        /// <summary>
        /// PaymentCode - Код
        /// </summary>
        [Size(300)]
        public String PaymentCode {
            get { return _PaymentCode; }
            set {
                SetPropertyValue<String>("PaymentCode", ref _PaymentCode, value == null ? String.Empty : value.Trim());
            }
        }

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


        // ============================= Поля, не описанные в Положении, но имеющеся на платёжках (может быть они и не нужны) ================
        
        /// <summary>
        /// ReceivedByPayerBankDate - Поступило в банк плательщика (Поступ. в банк плат.)
        /// </summary>
        [Browsable(false)]
        public DateTime ReceivedByPayerBankDate {
            get { return _ReceivedByPayerBankDate; }
            set {
                SetPropertyValue<DateTime>("ReceivedByPayerBankDate", ref _ReceivedByPayerBankDate, value); }
        }
        
        /// <summary>
        /// DeductedFromPayerAccount - Списано со счета плательщика (Списано со сч. плат.)
        /// </summary>
        [Browsable(false)]
        public DateTime DeductedFromPayerAccount {
            get { return _DeductedFromPayerAccount; }
            set {
                SetPropertyValue<DateTime>("DeductedFromPayerAccount", ref _DeductedFromPayerAccount, value); }
        }

        /// <summary>
        /// StatementAccount - Ссылка на выписку, если экзмпляр загружен из выписки
        /// </summary>
        public fmStatementOfAccounts StatementAccount {
            get { return _StatementAccount; }
            set {
                SetPropertyValue<fmStatementOfAccounts>("StatementAccount", ref _StatementAccount, value);
            }
        }



        // Доп. поля из файла 1С

        /// <summary>
        /// CompilerStatus - Статус составителя
        /// </summary>
        [Size(300)]
        public String CompilerStatus {
            get { return _CompilerStatus; }
            set {
                SetPropertyValue<String>("CompilerStatus", ref _CompilerStatus, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// KBKStatus - ПоказательКБК
        /// </summary>
        [Size(300)]
        public String KBKStatus {
            get { return _KBKStatus; }
            set {
                SetPropertyValue<String>("KBKStatus", ref _KBKStatus, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// OKATO - ОКАТО
        /// </summary>
        [Size(300)]
        public String OKATO {
            get { return _OKATO; }
            set {
                SetPropertyValue<String>("OKATO", ref _OKATO, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// ReasonIndicator - ПоказательОснования
        /// </summary>
        [Size(300)]
        public String ReasonIndicator {
            get { return _ReasonIndicator; }
            set {
                SetPropertyValue<String>("ReasonIndicator", ref _ReasonIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// PeriodIndicator - ПоказательПериода
        /// </summary>
        [Size(300)]
        public String PeriodIndicator {
            get { return _PeriodIndicator; }
            set {
                SetPropertyValue<String>("PeriodIndicator", ref _PeriodIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// NumberIndicator - ПоказательНомера
        /// </summary>
        [Size(300)]
        public String NumberIndicator {
            get { return _NumberIndicator; }
            set {
                SetPropertyValue<String>("NumberIndicator", ref _NumberIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// DateIndicator - ПоказательДаты
        /// </summary>
        [Size(300)]
        public String DateIndicator {
            get { return _DateIndicator; }
            set {
                SetPropertyValue<String>("DateIndicator", ref _DateIndicator, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// TypeIndicator - ПоказательТипа
        /// </summary>
        [Size(300)]
        public String TypeIndicator {
            get { return _TypeIndicator; }
            set {
                SetPropertyValue<String>("TypeIndicator", ref _TypeIndicator, value == null ? String.Empty : value.Trim());
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
            return AllowEditProperty && !ReadOnly;
        }

        protected override void OnSaving() {
            AllowEditProperty = false;
            base.OnSaving();
        }

        #endregion

    }

}
