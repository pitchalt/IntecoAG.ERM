using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;

//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.Trw.Party;

namespace IntecoAG.ERM.FM.PaymentRequest {

    /// <summary>
    /// Статусы заявки
    /// </summary>
    public enum PaymentRequestStates {
        /// <summary>
        /// Создана
        /// </summary>
        OPEN = 1,   
        /// <summary>
        /// Получена ФБК
        /// </summary>
        REGISTERED = 2, 
        /// <summary>
        /// Утверждена дог./план. отделом
        /// </summary>
        ACCEPTED = 3,  
        /// <summary>
        /// Отклонена
        /// </summary>
        DELETED = 4,
        /// <summary>
        /// Не использовать
        /// </summary>
        OLD_APPROVED_PLAN = 5,  
        /// <summary>
        /// Не использовать
        /// </summary>
        OLD_DECLINED_PLAN = 6,  
        /// <summary>
        /// Утверждена бюджетным отделом
        /// </summary>
        IN_BUDGET = 7,  
        /// <summary>
        /// Не использовать
        /// </summary>
        OLD_DECLINED_BUDGET = 8,
        /// <summary>
        /// Утверждена финансовым отделом для оплаты
        /// </summary>
        IN_PAYMENT = 9,  
        /// <summary>
        /// Не использовать
        /// </summary>
        OLD_DECLINED_FINANCE = 10,  
        /// <summary>
        /// Отложена
        /// </summary>
        SUSPENDED = 11,  
        /// <summary>
        /// Оплачена
        /// </summary>
        PAYED = 12,
        /// <summary>
        /// Отложена до особого распоряжения
        /// </summary>
        SUSPENDED_BUDGET = 13,
        /// <summary>
        /// Отменена
        /// </summary>
        DECLINED = 14,
        /// <summary>
        /// Признак финансового платежа
        /// </summary>
        FINANCE_PAYMENT = 15,
        /// <summary>
        /// Платёжка отправлена в банк (уточнение для статуса "Утверждена финансовым отделом для оплаты - IN_PAYMENT")
        /// </summary>
        IN_BANK = 16,
        /// <summary>
        /// Документ является шаблоном
        /// </summary>
        TEMPLATE = 17  
    }

    /// <summary>
    ///  Заявка на оплату
    /// </summary>
    [NavigationItem("Money")]
    [Persistent("fmPRPaymentRequest")]
    [LikeSearchPathList(new String[] {
        "PartyPaySender.INN",
        "PartyPaySender.Name",
        "PartyPayReceiver.INN",
        "PartyPayReceiver.Name",
        "ExtDocNumber",
        "Number"})]
    [DefaultProperty("Name")]
    [Appearance("fmCPRPaymentRequest.ApproveAction.Desable", AppearanceItemType = "Action", Method = "ApproveActionEnabling", TargetItems = "fmCPRPaymentRequestViewController_ApproveAction" /*, Visibility = ViewItemVisibility.Hide*/, Enabled = false, Context = "Any")]
    [Appearance("fmCPRPaymentRequest.DeclinAction.Desable", AppearanceItemType = "Action", Method = "DeclinActionEnabling", TargetItems = "fmCPRPaymentRequestViewController_DeclineAction" /*, Visibility = ViewItemVisibility.Hide*/, Enabled = false, Context = "Any")]
    [Appearance("fmCPRPaymentRequest.SuspendAction.Desable", AppearanceItemType = "Action", Method = "SuspendActionEnabling", TargetItems = "fmCPRPaymentRequestViewController_SuspendAction" /*, Visibility = ViewItemVisibility.Hide*/, Enabled = false, Context = "Any")]
    public abstract class fmCPRPaymentRequest : csCComponent
    {
        protected static BinaryOperator ValRub = new BinaryOperator("Code", "RUB");

        public fmCPRPaymentRequest(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRPaymentRequest);
            this.CID = Guid.NewGuid();

            Date = DateTime.Now;
            DateRegister = Date;
            State = PaymentRequestStates.REGISTERED;

            this.Valuta = Session.FindObject<csValuta>(ValRub);
            this.PaymentValuta = this.Valuta;

            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    this.PartyPaySender  = crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }

            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            user = this.Session.GetObjectByKey<csCSecurityUser>(user.Oid);
            if (user != null) {
                this.FBKManager = user.Staff;
            }
        }

        #region ПОЛЯ КЛАССА

        private String _Number; // номер документа 
        private DateTime _Date; // дата создания документа
        private DateTime _PayDate; // Дата к оплате

        private String _ExtDocNumber; // номер документа 
        private DateTime _ExtDocDate; // дата создания документа

        private crmCParty _PartyPaySender;
        private crmCParty _PartyPayReceiver;
        private crmCParty _PartyPayDebitor;
        private crmCParty _PartyPayCreditor;

        private PaymentRequestStates _State; // Состояние заявки
        // Даты утверждений статусов соответствующими отделами
        private hrmDepartment _DepartmentOfState; // Подразделение, которое выставило текущий статус
        private DateTime _DateRegister; // Дата регистрации заявки курирующим подразделением
        private DateTime _DateContractOrPlan; // Дата утверждения или отклонения заявки договорным или плановым подразделением
        private DateTime _DateBudget; // Дата утверждения или отклонения заявки бюджетным подразделением
        private DateTime _DateFinance; // Дата утверждения или отклонения заявки финансовым подразделением

        private decimal _Summ; // Сумма заявки
        private String _SumInString; // Сумма заявки прописью
        private csValuta _Valuta; // Валюта заявки, т.е. ОБЯЗАТЕЛЬСТВ
        private DateTime _DateInPayment; // Дата к оплате
        private csValuta _PaymentValuta; // Валюта платежа

        private string _Description; // Краткое описание
        private string _Comment; // Комментарий

        private hrmDepartment _Curator; // Подразделение-заявитель (курирующее подразделение)
        private hrmStaff _FBKManager;

        protected csCSecurityRole _OwnerRole;   // Роль создателя записки

        #endregion


        #region СВОЙСТВА КЛАССА

        //================

        /// <summary>
        /// Номер документа заявки
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.DocNumber.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[RuleRequiredField]
        [Size(20)]
        public String Number {
            get { return _Number; }
            set {
                SetPropertyValue<String>("Number", ref _Number, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Дата создания документа заявки
        /// </summary>
        [RuleRequiredField]
        public DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
                if (!IsLoading) {
                    this.PayDate = Date.AddDays(10);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public String Name {
            get { return this.ExtDocNumber + " от " + this.ExtDocDate.ToString("dd.MM.yyyy"); }
        }

        /// <summary>
        /// Состояние заявки (положение в бизнес-процессе)
        /// </summary>
        [Indexed]
        public PaymentRequestStates State {
            get { return _State; }
            set {
                PaymentRequestStates old = _State;
                SetPropertyValue<PaymentRequestStates>("State", ref _State, value);
                if (!IsLoading) {
                    if (value == PaymentRequestStates.ACCEPTED) {
                        UseCounter++;
                        TrwPartyParty trw_party;
                        trw_party = TrwPartyParty.LocateTrwParty(ObjectSpace.FindObjectSpaceByObject(this), 
                            this.PartyPayCreditor);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                        trw_party = TrwPartyParty.LocateTrwParty(ObjectSpace.FindObjectSpaceByObject(this), 
                            this.PartyPayDebitor);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                        trw_party = TrwPartyParty.LocateTrwParty(ObjectSpace.FindObjectSpaceByObject(this), 
                            this.PartyPayReceiver);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                        trw_party = TrwPartyParty.LocateTrwParty(ObjectSpace.FindObjectSpaceByObject(this), 
                            this.PartyPaySender);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                    }
                    ReadOnlyUpdate();
                }
            }
        }

        /// <summary>
        /// Дата платежа
        /// </summary>
        [RuleRequiredField]
        public DateTime PayDate {
            get { return _PayDate; }
            set {
                SetPropertyValue<DateTime>("PayDate", ref _PayDate, value);
            }
        }

        /// <summary>
        /// Номер документа основания
        /// </summary>
        [Size(50)]
        [RuleRequiredField]
        public String ExtDocNumber {
            get { return _ExtDocNumber; }
            set {
                SetPropertyValue<String>("ExtDocNumber", ref _ExtDocNumber, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Дата документа основания
        /// </summary>
        [RuleRequiredField]
        public DateTime ExtDocDate {
            get { return _ExtDocDate; }
            set {
                SetPropertyValue<DateTime>("ExtDocDate", ref _ExtDocDate, value);
            }
        }
        /// <summary>
        /// Плательщик
        /// </summary>
        [Browsable(false)]
        public crmCParty PartyPaySender {
            get { return _PartyPaySender; }
            set { 
                crmCParty old = _PartyPaySender;
                if (old != value) {
                    _PartyPaySender = value;
                    if (!IsLoading) {
                        if (PartyPayCreditor == null || PartyPayCreditor == old) {
                            PartyPayCreditor = value;
                        }
                        OnChanged("PartyPaySender", old, value);
                        OnChanged("IPartyPaySender");
                    }
                }
            }
        }

        /// <summary>
        /// Плательщик
        /// </summary>
        [PersistentAlias("PartyPaySender")]
        [RuleRequiredField]
        public crmIParty IPartyPaySender {
            get { return PartyPaySender; }
            set {
                crmCParty old = PartyPaySender;
                if (value != null && old != value.Party) {
                    PartyPaySender = value.Party;
                } else if (value == null) {
                    PartyPaySender = null;
                }
            }
        }

        /// <summary>
        /// Получатель
        /// </summary>
        [Browsable(false)]
        public crmCParty PartyPayReceiver {
            get { return _PartyPayReceiver; }
            set {
                crmCParty old = _PartyPayReceiver;
                if (old != value) {
                    _PartyPayReceiver = value;
                    if (!IsLoading ) {
                        if (PartyPayDebitor == null || PartyPayDebitor == old) {
                            PartyPayDebitor = value;
                        }
                        OnChanged("PartyPayReceiver", old, value);
                        OnChanged("IPartyPayReceiver");
                    }
                }
            }
        }

        /// <summary>
        /// Получатель
        /// </summary>
        [PersistentAlias("PartyPayReceiver")]
        [RuleRequiredField]
        public crmIParty IPartyPayReceiver {
            get { return PartyPayReceiver; }
            set {
                crmCParty old = PartyPayReceiver;
                if (value != null && old != value.Party) {
                    PartyPayReceiver = value.Party;
                } else if (value == null) {
                    PartyPayReceiver = null;
                }
            }
        }

        /// <summary>
        /// Получатель
        /// </summary>
        [Browsable(false)]
        public virtual crmCParty PartyPayDebitor {
            get { return _PartyPayDebitor; }
            set { 
                crmCParty old = _PartyPayDebitor;
                if (old != value) {
                    _PartyPayDebitor = value;
                    if (!IsLoading) {
                        OnChanged("PartyPayDebitor", old, value);
                        OnChanged("IPartyPayDebitor");
                    }
                }
            }
        }

        /// <summary>
        /// Получатель
        /// </summary>
        [PersistentAlias("PartyPayDebitor")]
        [RuleRequiredField]
        public crmIParty IPartyPayDebitor {
            get { return PartyPayDebitor; }
            set {
                crmCParty old = PartyPayDebitor;
                if (value != null && old != value.Party) {
                    PartyPayDebitor = value.Party;
                } else if (value == null) {
                    PartyPayDebitor = null;
                }
            }
        }

        /// <summary>
        /// Плательщик
        /// </summary>
        [Browsable(false)]
        public virtual crmCParty PartyPayCreditor {
            get { return _PartyPayCreditor; }
            set {
                crmCParty old = _PartyPayCreditor;
                if (old != value) {
                    _PartyPayCreditor = value;
                    if (!IsLoading) {
                        OnChanged("PartyPayCreditor", old, value);
                        OnChanged("IPartyPayCreditor");
                    }
                }
            }
        }

        /// <summary>
        /// Плательщик
        /// </summary>
        [PersistentAlias("PartyPayCreditor")]
        [RuleRequiredField]
        public crmIParty IPartyPayCreditor {
            get { return PartyPayCreditor; }
            set {
                crmCParty old = PartyPayCreditor;
                if (value != null && old != value.Party) {
                    PartyPayCreditor = value.Party;
                } else if (value == null) {
                    PartyPayCreditor = null;
                }
            }
        }

        /// <summary>
        /// Подразделение, которое поставило текущий статус заявке
        /// </summary>
        public hrmDepartment DepartmentOfState {
            get { return _DepartmentOfState; }
            set { SetPropertyValue<hrmDepartment>("DepartmentOfState", ref _DepartmentOfState, value); }
        }

        /// <summary>
        /// Дата регистрации заявки курирующим подразделением
        /// </summary>
        //[RuleRequiredField]
        public DateTime DateRegister {
            get { return _DateRegister; }
            set {
                SetPropertyValue<DateTime>("DateRegister", ref _DateRegister, value);
            }
        }

        /// <summary>
        /// Дата утверждения или отклонения заявки договорным или плановым подразделением
        /// </summary>
        //[RuleRequiredField]
        public DateTime DateContractOrPlan {
            get { return _DateContractOrPlan; }
            set {
                SetPropertyValue<DateTime>("DateContractOrPlan", ref _DateContractOrPlan, value);
            }
        }

        /// <summary>
        /// Дата утверждения или отклонения заявки бюджетным подразделением
        /// </summary>
        //[RuleRequiredField]
        public DateTime DateBudget {
            get { return _DateBudget; }
            set {
                SetPropertyValue<DateTime>("DateBudget", ref _DateBudget, value);
            }
        }

        /// <summary>
        /// Дата утверждения или отклонения заявки финансовым подразделением
        /// </summary>
        //[RuleRequiredField]
        public DateTime DateFinance {
            get { return _DateFinance; }
            set {
                SetPropertyValue<DateTime>("DateFinance", ref _DateFinance, value);
            }
        }


        /// <summary>
        /// Сумма заявки
        /// </summary>
        [RuleValueComparison("fmCPRPaymentRequest.Summ_GreaterThanOrEqual_0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public virtual decimal Summ {
            get { return _Summ; }
            set { 
                Decimal old = _Summ;
                if (old != value) {
                    _Summ = value;
                    if (!IsLoading ) {
                        if (Valuta.Code == "RUB") {
                            SumInString = SumInWord.RurPhrase(value);
                        }
                        else
                            SumInString = "";
                        OnChanged("SumInString", old, value);
                    }
                }
            }
        }

        /// <summary>
        /// Сумма заявки прописью
        /// </summary>
        [Appearance("fmCPRPaymentRequest.SumInString.Enabled", Criteria = "true", Enabled = false)]
        //[Appearance("fmCPRPaymentRequest.SumInString.Enabled", Method = "AllowEditSumInString", Enabled = false)]
        [Size(300)]
        public String SumInString {
            get {
                return _SumInString;
            }
            set {
                SetPropertyValue<String>("SumInString", ref _SumInString, value);
            }
        }

        /// <summary>
        /// Валюта обязательств, указывается согласно Договору и т.п. Сумма заявки Summ указывается в валюте обязательств
        /// </summary>
        [RuleRequiredField]
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<csValuta>("Valuta", ref _Valuta, value); }
        }

        
        /// <summary>
        /// Валюта расчёта (платежа)
        /// </summary>
        [RuleRequiredField]
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set { SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value); }
        }        

        /// <summary>
        /// Дата к оплате
        /// </summary>
        //[RuleRequiredField]
        public DateTime DateInPayment {
            get { return _DateInPayment; }
            set {
                SetPropertyValue<DateTime>("DateInPayment", ref _DateInPayment, value);
            }
        }

        /// <summary>
        /// Краткое описание
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment {
            get { return _Comment; }
            set { SetPropertyValue<string>("Comment", ref _Comment, value); }
        }

        /// <summary>
        /// Подразделение-заявитель
        /// </summary>
        [RuleRequiredField]
        public hrmDepartment Curator {
            get { return _Curator; }
            set { SetPropertyValue<hrmDepartment>("Curator", ref _Curator, value); }
        }
        /// <summary>
        /// Ответственный ФБК
        /// </summary>
        //[RuleRequiredField]
        public hrmStaff FBKManager {
            get { return _FBKManager; }
            set { 
                SetPropertyValue<hrmStaff>("FBKManager", ref _FBKManager, value);
                if (!IsLoading && value != null) {
                    DepartmentOfState = value.Department;
                }
            }
        }
        /// <summary>
        /// Список оплачиваемых расчётных обязательств
        /// </summary>
        [Aggregated]
        [Association("fmPRPaymentRequest-fmPRPaymentRequestObligation", typeof(fmCPRPaymentRequestObligation))]
        public XPCollection<fmCPRPaymentRequestObligation> PaySettlmentOfObligations {
            get { return GetCollection<fmCPRPaymentRequestObligation>("PaySettlmentOfObligations"); }
        }

        /// <summary>
        /// Коллекция связей с документами по журналу сопоставления
        /// </summary>
        public XPCollection<fmCPRRepaymentJurnal> Repayments {
            get {
                return new XPCollection<fmCPRRepaymentJurnal>( PersistentCriteriaEvaluationBehavior.InTransaction, this.Session, new BinaryOperator("PaymentRequest", this));
            }
        }

        /// <summary>
        /// Сумма оплаты
        /// </summary>
        public Decimal PaymentSumm {
            get {
                return Repayments.Sum(rep => rep.SumBalance);
            }
        }

        /// <summary>
        /// Список заказов
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public String OrderList {
            get { 
                String [] array = new String [PaySettlmentOfObligations.Count];
                int index = 0;
                foreach (fmCPRPaymentRequestObligation obl in PaySettlmentOfObligations) {
                    array[index++] = obl.Order.Code;
                }
                return String.Join(", ", array);
            }
        }

        /// <summary>
        /// Список заказов
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(true)]
        public String TrwRefCode {
            get {
                if (PaySettlmentOfObligations.Count == 1 && PaySettlmentOfObligations[0].TrwRefCashFlow != null)
                    return PaySettlmentOfObligations[0].TrwRefCashFlow.Code;
                else
                    return null;
            }
        }

        #endregion

        #region МЕТОДЫ

        public void UpdateSumm() {
            Decimal col_summ = 0;
            foreach (fmCPRPaymentRequestObligation obl in PaySettlmentOfObligations)
                col_summ += obl.Summ;
            Summ = col_summ;
        }

        public override bool ReadOnlyGet() {
            return State != PaymentRequestStates.OPEN && 
                State != PaymentRequestStates.REGISTERED &&
                State != PaymentRequestStates.FINANCE_PAYMENT &&
                State != PaymentRequestStates.TEMPLATE;
        }

        public virtual fmCPRPaymentRequest CloneRequest() {
            fmCPRPaymentRequest req = null;
            DevExpress.Persistent.Base.Cloner cloner = new Cloner();

            // Копирование полей из данного документа в новый
            req = cloner.CloneTo(this, typeof(fmPaymentRequestMemorandum)) as fmPaymentRequestMemorandum;
            req.State = PaymentRequestStates.OPEN;
            return req;
        }

        public virtual fmCPRPaymentRequest CreateTemplate() {
            fmCPRPaymentRequest req = this.CloneRequest();
            req.State = PaymentRequestStates.TEMPLATE;
            return req;
        } 

        /// <summary>
        /// Определение доступности действия "Утердить" заявку
        /// </summary>
        /// <returns></returns>
        public virtual bool ApproveActionEnabling() {
            return fmCPRPaymentRequestBusinesLogic.EnableApproveAction(this);
        }

        /// <summary>
        /// Определение доступности действия "Отклонить" заявку
        /// </summary>
        /// <returns></returns>
        public virtual bool DeclinActionEnabling() {
            return fmCPRPaymentRequestBusinesLogic.EnableDeclinAction(this);
        }

        /// <summary>
        /// Определение доступности действия "Отложить" заявку
        /// </summary>
        /// <returns></returns>
        public virtual bool SuspendActionEnabling() {
            return fmCPRPaymentRequestBusinesLogic.EnableSuspendAction(this);
        }

        #endregion

    }

}
