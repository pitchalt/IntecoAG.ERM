using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CS.Settings;
using IntecoAG.ERM.CS.Security;

namespace IntecoAG.ERM.FM.PaymentRequest {

    /// <summary>
    /// Виды служебных записок
    /// </summary>
    public enum PaymentRequestMemorandumKinds {
        /// <summary>
        /// Командировочные
        /// </summary>
        BUSINESS_TRIP = 1,
        /// <summary>
        /// Бюджетные
        /// </summary>
        BUDGET = 2,
        /// <summary>
        /// Перечисление на базы
        /// </summary>
        TRANSFER_TO_BASE = 3,
        /// <summary>
        /// Зарплата
        /// </summary>
        SALARY = 4,
        /// <summary>
        /// Прочее
        /// </summary>
        OTHER = 5
    }

    /// <summary>
    /// Заявка на оплату: служебная записка на оплату
    /// </summary>
    [Appearance("fmPaymentRequestMemorandum.Persons.Visible",
        AppearanceItemType = "LayoutItem", Criteria = "MemorandumKind != 'SALARY' AND MemorandumKind != 'BUSINESS_TRIP'",
        TargetItems = "Persons", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.MemorandumSumm.Visible",
        AppearanceItemType = "LayoutItem", Criteria = "PaySettlmentOfObligations.Count > 1",
        TargetItems = "MemorandumSumm, MemorandumCostItem, MemorandumOrder", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.BUSINESS_TRIP.LayoutItem.Visible",
        AppearanceItemType = "LayoutItem", Criteria = "MemorandumKind == 'BUSINESS_TRIP'",
        TargetItems = "Parameters", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.BUSINESS_TRIP.Visible",
        AppearanceItemType = "ViewItem", Criteria = "MemorandumKind == 'BUSINESS_TRIP'",
        TargetItems = "PaymentBase, PaymentKBK, PaymentKind, PaymentTaxPeriod, PaymentTaxPeriodDate, PaymentTaxPeriodPart2, PaymentTaxPeriodPart4, ОКАТО, PayUpDate, PaymentStatus, PaymentPurpose, PersonalAccount, ReceiverData", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.TRANSFER_TO_BASE.LayoutItem.Visible",
        AppearanceItemType = "LayoutItem", Criteria = "MemorandumKind == 'TRANSFER_TO_BASE'",
        TargetItems = "Parameters", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.BUDGET.Visible",
        AppearanceItemType = "ViewItem", Criteria = "MemorandumKind == 'BUDGET'",
        TargetItems = "TabNo, ReceiverData", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.TRANSFER_TO_BASE.Visible",
        AppearanceItemType = "ViewItem", Criteria = "MemorandumKind == 'TRANSFER_TO_BASE'",
        TargetItems = "PayUpDate, PersonalAccount", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.SALARY.Visible",
        AppearanceItemType = "ViewItem", Criteria = "MemorandumKind == 'SALARY'",
        TargetItems = "Reason, PayUpDate, TabNo, FBKManager", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.OTHER.Visible",
        AppearanceItemType = "ViewItem", Criteria = "MemorandumKind == 'OTHER'",
        TargetItems = "Reason, PayUpDate, TabNo, PersonalAccount", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("fmPaymentRequestMemorandum.TemplateName.Visible",
    AppearanceItemType = "ViewItem", Criteria = "State != 'TEMPLATE'",
    TargetItems = "TemplateName", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [VisibleInReports]
    public class fmPaymentRequestMemorandum : fmCPRPaymentRequest
    {
        public fmPaymentRequestMemorandum(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmPaymentRequestMemorandum);
            this.CID = Guid.NewGuid();

            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            user = this.Session.GetObjectByKey<csCSecurityUser>(user.Oid);
            if (user != null) {
                this.Creator = user;
                //this.FBKManager = user.Staff;
                this.Requester = user.Staff;
                this.Curator = user.Department;
                this.SecondSignaturePerson = user.Staff;

                IList<csCSecurityRole> roles = GetActualRoles(user);
                //csCSecurityRole mainBuhRole = fmCSettingsFinance.GetInstance(Session).MainBuhRole;
                if (roles.Count == 1) {
                    this.OwnerRole = roles[0];
                }
            }

            // Подразделение-адресат
            this.FBKDepartment = fmCSettingsFinance.GetInstance(Session).FBKDepartment;

            // Номер и дата документа по умолчанию
            this.ExtDocNumber = "-";
            this.ExtDocDate = DateTime.Now;   //.Date;

            this.PaymentTaxPeriodPart4 = Convert.ToInt16(DateTime.Now.Year);
        }


        static public IList<csCSecurityRole> GetActualRoles(Session session, csCSecurityUser user) {
            if (user.Session != session) throw new ArgumentException("Not valid Session for user object");
            IList<csCSecurityRole> roles = new List<csCSecurityRole>(user.Roles.Count);
            csCSecurityRole mainBuhRole = fmCSettingsFinance.GetInstance(session).MainBuhRole;
            if (mainBuhRole != null) {
                IList<csCSecurityRole> valid_roles = mainBuhRole.ChildRoles;
                foreach (csCSecurityRole role in user.Roles) {
                    if (valid_roles.Contains(role)) {
                        roles.Add(role);
                    }
                }
            }
            return roles;
        }

        public IList<csCSecurityRole> GetActualRoles(csCSecurityUser user) {
            return GetActualRoles(this.Session, user);
        }

        public IList<csCSecurityRole> GetActualRoles() {
            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            user = this.Session.GetObjectByKey<csCSecurityUser>(user.Oid);
            return GetActualRoles(user);
        }

        #region ПОЛЯ КЛАССА

        private PaymentRequestMemorandumKinds _MemorandumKind; // Вид служебной записки

        private decimal _MemorandumSumm; // Сумма служебной записки (преобразуется в сумму обязательства)
        private fmCostItem _MemorandumCostItem;   // Статья затрат
        private fmCOrderExt _MemorandumOrder;  // Заказ

        private String _Subject;   // Предмет оплаты, описание
        private String _ReceiverData;   // Данные о получателе в свободном формате
        //private crmCParty _Receiver;   // Получатель
        private crmBank _ReceiverBank;   // Банк получателя
        private crmBankAccount _ReceiverBankAccount;   // Счёт получателя

        //private String _SumInString; // Сумма заявки прописью


        //private hrmStaff _FBKManager; // Управленец ФБК, кому обращена служебная записка
        private hrmDepartment _FBKDepartment; // Подразделение, в которое направлена служебная записка
        private hrmStaff _FBKReceiver; // Управленец, которому направлена служебная записка

        private hrmStaff _Requester; // Управленец-заявитель
        private csCSecurityUser _Creator; // Пользователь, который создал объект заявки
        //private hrmDepartment _Curator; // Подразделение-заявитель

        private hrmStaff _FirstSignaturePerson; // Подписант 1
        private hrmStaff _SecondSignaturePerson; // Подписант 2

        private String _BalanceAccount;   // Балансовый счёт
        private String _PaymentStatus;   // Статус в платёжке (01, ...)
        private String _PaymentPurpose;   // Назначение платежа
        private String _TabNo;   // Табельный номер
        private DateTime _PayUpDate;   // Оплатить ДО

        private String _Reason;   // Основание, например: "В соответствии с документом № ...."
        private String _ОКАТО;   // ОКАТО

        private fmCPaymentBase _PaymentBase;   // Код основания платежа
        private fmCPaymentKBK _PaymentKBK;   // КБК
        private fmCPaymentKind _PaymentKind;   // Тип платежа
        private fmCPaymentTaxPeriod _PaymentTaxPeriod;   // Период уплаты налога (сбора)
        private DateTime _PaymentTaxPeriodDate;   // Период уплаты налога (сбора) - календарная часть
        private Int16 _PaymentTaxPeriodPart2;   // Период уплаты налога (сбора) - только месяц, квартал и т.д.
        private Int16 _PaymentTaxPeriodPart4;   // Период уплаты налога (сбора) - только год
        private String _PaymentTaxPeriodPrint;   // Период уплаты налога (сбора) - вид для печати

        private String _TemplateName;   // Наименование шаблона
        private String _PersonalAccount;   // Лицевой счёт

        //private csCSecurityRole _OwnerRole;   // Конкретная роль создателя объекта из числа всех ролей, приписанная объекту

        #endregion


        #region СВОЙСТВА КЛАССА

        /*
        /// <summary>
        /// Общая сумма заявки
        /// </summary>
        public override decimal Summ {
            get {
                return base.Summ;
            }
            set {
                //Decimal old = base.Summ;
                base.Summ = value;
                //if (!IsLoading && old != value) {
                //    this.SumInString = SumInWord.RurPhrase(value);
                //}
            }
        }
        */

        /// <summary>
        /// Вид служебной записки
        /// </summary>
        //[ImmediatePostData]
        [Indexed]
        [Custom("AllowEdit", "False")]
        public PaymentRequestMemorandumKinds MemorandumKind {
            get {
                return _MemorandumKind;
            }
            set {
                SetPropertyValue<PaymentRequestMemorandumKinds>("MemorandumKind", ref _MemorandumKind, value);
            }
        }


        /// <summary>
        /// Сумма служебной записки (преобразуется в сумму обязательства)
        /// </summary>
        //[ImmediatePostData]
        //[RuleValueComparison("fmPaymentRequestMemorandum.MemorandumSumm_GreaterThanOrEqual_0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public decimal MemorandumSumm {
            get {
                if (!IsLoading) {
                    if (this.PaySettlmentOfObligations.Count != 0) {
                        fmCPRPaymentRequestObligation pro = this.PaySettlmentOfObligations[0];
                        _MemorandumSumm = pro.Summ;
                    }
                }
                return _MemorandumSumm;
            }
            set {
                decimal old = _MemorandumSumm;
                if (old != value) {
                    _MemorandumSumm = value;
                    if (!IsLoading) {
                        if (this.PaySettlmentOfObligations.Count == 0) {
                            fmCPRPaymentRequestObligation pro = new fmCPRPaymentRequestObligation(Session);
                            pro.Summ = value;
                            //pro.Valuta = this.Valuta;   // Валюта обязательств
                            pro.Order = this.MemorandumOrder;
                            pro.CostItem = this.MemorandumCostItem;
                            this.PaySettlmentOfObligations.Add(pro);
                        }
                        else {
                            fmCPRPaymentRequestObligation pro = this.PaySettlmentOfObligations[0];
                            pro.Summ = value;
                            //pro.Valuta = this.Valuta;   // Валюта обязательств
                            pro.Order = this.MemorandumOrder;
                            pro.CostItem = this.MemorandumCostItem;
                        }
                        OnChanged("MemorandumSumm", old, value);
                    }
                }
            }
        }

        /// <summary>
        /// Статья
        /// </summary>
        //[ImmediatePostData]
        [RuleRequiredField]
        public fmCostItem MemorandumCostItem {
            get {
                if (!IsLoading) {
                    if (this.PaySettlmentOfObligations.Count != 0) {
                        fmCPRPaymentRequestObligation pro = this.PaySettlmentOfObligations[0];
                        _MemorandumCostItem = pro.CostItem;
                    }
                }
                return _MemorandumCostItem;
            }
            set {
                fmCostItem old = MemorandumCostItem;
                SetPropertyValue<fmCostItem>("MemorandumCostItem", ref _MemorandumCostItem, value);
                if (!IsLoading && old != value) {
                    if (this.PaySettlmentOfObligations.Count == 0) {
                        fmCPRPaymentRequestObligation pro = new fmCPRPaymentRequestObligation(Session);
                        pro.Summ = this.MemorandumSumm;
                        //pro.Valuta = this.Valuta;   // Валюта обязательств
                        pro.Order = this.MemorandumOrder;
                        pro.CostItem = value;
                        this.PaySettlmentOfObligations.Add(pro);
                    } else {
                        fmCPRPaymentRequestObligation pro = this.PaySettlmentOfObligations[0];
                        pro.Summ = this.MemorandumSumm;
                        //pro.Valuta = this.Valuta;   // Валюта обязательств
                        pro.Order = this.MemorandumOrder;
                        pro.CostItem = value;
                    }
                }
            }
        }

        /// <summary>
        /// Заказ
        /// </summary>
        //[ImmediatePostData]
        [RuleRequiredField]
        public fmCOrderExt MemorandumOrder {
            get {
                if (!IsLoading) {
                    if (this.PaySettlmentOfObligations.Count != 0) {
                        fmCPRPaymentRequestObligation pro = this.PaySettlmentOfObligations[0];
                        _MemorandumOrder = pro.Order;
                    }
                }
                return _MemorandumOrder;
            }
            set {
                fmCOrderExt old = MemorandumOrder;
                SetPropertyValue<fmCOrderExt>("MemorandumOrder", ref _MemorandumOrder, value);
                if (!IsLoading && old != value) {
                    if (this.PaySettlmentOfObligations.Count == 0) {
                        fmCPRPaymentRequestObligation pro = new fmCPRPaymentRequestObligation(Session);
                        pro.Summ = this.MemorandumSumm;
                        //pro.Valuta = this.Valuta;   // Валюта обязательств
                        pro.Order = value;
                        pro.CostItem = this.MemorandumCostItem;
                        this.PaySettlmentOfObligations.Add(pro);
                    } else {
                        fmCPRPaymentRequestObligation pro = this.PaySettlmentOfObligations[0];
                        pro.Summ = this.MemorandumSumm;
                        //pro.Valuta = this.Valuta;   // Валюта обязательств
                        pro.Order = value;
                        pro.CostItem = this.MemorandumCostItem;
                    }
                }
            }
        }

        /// <summary>
        /// Subject - Предмет оплаты, описание
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String Subject {
            get { return _Subject; }
            set {
                SetPropertyValue<String>("Subject", ref _Subject, value);
            }
        }

        ///// <summary>
        ///// Receiver - Получатель
        ///// </summary>
        //public crmCParty Receiver {
        //    get { return _Receiver; }
        //    set {
        //        SetPropertyValue<crmCParty>("Receiver", ref _Receiver, value);
        //    }
        //}

        /// <summary>
        /// Данные о получателе в свободном формате
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String ReceiverData {
            get {
                return _ReceiverData;
            }
            set {
                SetPropertyValue<String>("ReceiverData", ref _ReceiverData, value);
            }
        }

        /// <summary>
        /// ReceiverBank - банк получателя
        /// </summary>
        public crmBank ReceiverBank {
            get { return _ReceiverBank; }
            set {
                crmBank oldBank = _ReceiverBank;
                SetPropertyValue<crmBank>("ReceiverBank", ref _ReceiverBank, value);
                if (!IsLoading && _ReceiverBank != oldBank) {
                    ReceiverBankAccount = null;
                    OnChanged("ReceiverBankAccount");
                }
            }
        }

        /// <summary>
        /// ReceiverBankAccount - расчётный счёт получателя
        /// </summary>
        [DataSourceProperty("BankAccounts")]
        public crmBankAccount ReceiverBankAccount {
            get { return _ReceiverBankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("ReceiverBankAccount", ref _ReceiverBankAccount, value);
            }
        }

        private XPCollection<crmBankAccount> BankAccounts {
            get {
                XPCollection<crmBankAccount> bankAccountCol = new XPCollection<crmBankAccount>(Session, false);
                if (!IsLoading) {
                    XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(Session);
                    var queryBankAccounts = from bankAccount in bankAccounts
                                            where bankAccount.Bank == this.ReceiverBank
                                            select bankAccount;
                    foreach (var bankAccount in queryBankAccounts) {
                        bankAccountCol.Add(bankAccount);
                    }
                }
                return bankAccountCol;
            }
        }


        ///// <summary>
        ///// Сумма заявки прописью
        ///// </summary>
        //public String SumInString {
        //    get {
        //        return _SumInString;
        //    }
        //    set {
        //        SetPropertyValue<String>("SumInString", ref _SumInString, value);
        //    }
        //}

        /// <summary>
        /// Заявитель
        /// </summary>
        //[RuleRequiredField]
        public hrmStaff Requester {
            get {
                return _Requester;
            }
            set {
                SetPropertyValue<hrmStaff>("Requester", ref _Requester, value);
                if (!IsLoading && value != null) {
                    Curator = value.Department;
                }
            }
        }

        /// <summary>
        /// Управленец, которому направлена служебная записка
        /// </summary>
        //[RuleRequiredField]
        public hrmStaff FBKReceiver {
            get {
                return _FBKReceiver;
            }
            set {
                SetPropertyValue<hrmStaff>("FBKReceiver", ref _FBKReceiver, value);
            }
        }

        /// <summary>
        /// Пользователь, который создал объект заявки
        /// </summary>
        [Browsable(false)]
        public csCSecurityUser Creator {
            get {
                return _Creator;
            }
            set {
                SetPropertyValue<csCSecurityUser>("Creator", ref _Creator, value);
            }
        }

        /// <summary>
        /// Конкретная роль создателя объекта из числа всех ролей, приписанная объекту
        /// </summary>
        [RuleRequiredField]
        [DataSourceProperty("ActualRoles")]
        public csCSecurityRole OwnerRole {
            get {
                return _OwnerRole;
            }
            set {
                SetPropertyValue<csCSecurityRole>("OwnerRole", ref _OwnerRole, value);
            }
        }

        public IList<csCSecurityRole> ActualRoles {
            get {
                return GetActualRoles();
            }
        }
/*
        private XPCollection<csCSecurityRole> OwnerRoles {
            get {
                XPCollection<csCSecurityRole> roleCol = new XPCollection<csCSecurityRole>(Session, false);
                if (!IsLoading) {
                    foreach (SecurityStrategyRole role in this.Creator.Roles) {
                        roleCol.Add(role as csCSecurityRole);
                    }
                }
                return roleCol;
            }
        }
*/

        /// <summary>
        /// Подразделение, в которое направлена служебная записка
        /// </summary>
        public hrmDepartment FBKDepartment {
            get {
                return _FBKDepartment;
            }
            set {
                SetPropertyValue<hrmDepartment>("FBKDepartment", ref _FBKDepartment, value);
            }
        }

        /// <summary>
        /// Подписант 1
        /// </summary>
        public hrmStaff FirstSignaturePerson {
            get {
                return _FirstSignaturePerson;
            }
            set {
                SetPropertyValue<hrmStaff>("FirstSignaturePerson", ref _FirstSignaturePerson, value);
            }
        }

        /// <summary>
        /// Подписант 2
        /// </summary>
        public hrmStaff SecondSignaturePerson {
            get {
                return _SecondSignaturePerson;
            }
            set {
                //SetPropertyValue<hrmStaff>("SecondSignaturePerson", ref _SecondSignaturePerson, value);
                hrmStaff old = _SecondSignaturePerson;
                if (old != value) {
                    _SecondSignaturePerson = value;
                    if (!IsLoading) {
                        if (this.Requester == null) {
                            this.Requester = value;
                        }
                        OnChanged("SecondSignaturePerson", old, value);
                    }
                }
            }
        }

        /// <summary>
        /// Табельный номер
        /// </summary>
        public String TabNo {
            get {
                return _TabNo;
            }
            set {
                SetPropertyValue<String>("TabNo", ref _TabNo, value);
            }
        }

        /// <summary>
        /// Балансовый счёт
        /// </summary>
        public String BalanceAccount {
            get {
                return _BalanceAccount;
            }
            set {
                SetPropertyValue<String>("BalanceAccount", ref _BalanceAccount, value);
            }
        }

        /// <summary>
        /// Статус в платёжке (01, ...)
        /// </summary>
        public String PaymentStatus {
            get {
                return _PaymentStatus;
            }
            set {
                SetPropertyValue<String>("PaymentStatus", ref _PaymentStatus, value);
            }
        }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String PaymentPurpose {
            get {
                return _PaymentPurpose;
            }
            set {
                SetPropertyValue<String>("PaymentPurpose", ref _PaymentPurpose, value);
            }
        }

        /// <summary>
        /// Оплатить ДО
        /// </summary>
        public DateTime PayUpDate {
            get {
                return _PayUpDate;
            }
            set {
                SetPropertyValue<DateTime>("PayUpDate", ref _PayUpDate, value);
            }
        }

        /// <summary>
        /// Основание, например: "В соответствии с документом № ...."
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String Reason {
            get {
                return _Reason;
            }
            set {
                SetPropertyValue<String>("Reason", ref _Reason, value);
            }
        }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public String ОКАТО {
            get {
                return _ОКАТО;
            }
            set {
                SetPropertyValue<String>("ОКАТО", ref _ОКАТО, value);
            }
        }

        /// <summary>
        /// Код основания платежа
        /// </summary>
        public fmCPaymentBase PaymentBase {
            get {
                return _PaymentBase;
            }
            set {
                SetPropertyValue<fmCPaymentBase>("PaymentBase", ref _PaymentBase, value);
            }
        }

        /// <summary>
        /// КБК
        /// </summary>
        public fmCPaymentKBK PaymentKBK {
            get {
                return _PaymentKBK;
            }
            set {
                SetPropertyValue<fmCPaymentKBK>("PaymentKBK", ref _PaymentKBK, value);
            }
        }

        /// <summary>
        /// Тип платежа
        /// </summary>
        public fmCPaymentKind PaymentKind {
            get {
                return _PaymentKind;
            }
            set {
                SetPropertyValue<fmCPaymentKind>("PaymentKind", ref _PaymentKind, value);
            }
        }

        /// <summary>
        /// Период уплаты налога (сбора)
        /// </summary>
        public fmCPaymentTaxPeriod PaymentTaxPeriod {
            get {
                return _PaymentTaxPeriod;
            }
            set {
                //SetPropertyValue<fmCPaymentTaxPeriod>("PaymentTaxPeriod", ref _PaymentTaxPeriod, value);
                //if (!IsLoading) {
                //    SetTaxPeriod();
                //    OnChanged("PaymentTaxPeriodPrint");
                //}
                fmCPaymentTaxPeriod old = _PaymentTaxPeriod;
                if (old != value) {
                    _PaymentTaxPeriod = value;
                    PaymentTaxPeriodPart2 = 0;
                    if (!IsLoading) {
                        SetTaxPeriod();
                        OnChanged("PaymentTaxPeriod", old, value);
                    }
                }
            }
        }

        /// <summary>
        /// Период уплаты налога (сбора) - календарная часть
        /// </summary>
        [Browsable(false)]
        public DateTime PaymentTaxPeriodDate {
            get {
                return _PaymentTaxPeriodDate;
            }
            set {
                //SetPropertyValue<DateTime>("PaymentTaxPeriodDate", ref _PaymentTaxPeriodDate, value);
                DateTime old = _PaymentTaxPeriodDate;
                if (old != value) {
                    _PaymentTaxPeriodDate = value;
                    if (!IsLoading) {
                        SetTaxPeriod();
                        OnChanged("PaymentTaxPeriodDate");
                    }
                }
                //if (!IsLoading) {
                //    SetTaxPeriod();
                //    OnChanged("PaymentTaxPeriodPrint");
                //}
            }
        }

        /// <summary>
        /// Период уплаты налога (сбора) - только месяц, квартал и т.д.
        /// </summary>
        //[RuleRange("fmPaymentRequestMemorandum.PaymentTaxPeriodPart2.month.Range", DefaultContexts.Save, 0, 12)]
        public Int16 PaymentTaxPeriodPart2 {
            get {
                return _PaymentTaxPeriodPart2;
            }
            set {
                //SetPropertyValue<Int16>("PaymentTaxPeriodPart2", ref _PaymentTaxPeriodPart2, value);
                Int16 old = _PaymentTaxPeriodPart2;
                if (old != value) {
                    _PaymentTaxPeriodPart2 = value;
                    if (!IsLoading) {
                        SetTaxPeriod();
                        OnChanged("PaymentTaxPeriodPart2", old, value);
                    }
                }
            }
        }

        [RuleFromBoolProperty("fmPaymentRequestMemorandum.PaymentTaxPeriodPart2", DefaultContexts.Save, "Номер периода не соответствует коду налогового периода")]
        protected bool IsPaymentTaxPeriodValid {
            get {
                /*
                if (PaymentTaxPeriod == null)
                    return true;
                if (PaymentTaxPeriod.Name == "МС") {
                    if (PaymentTaxPeriodPart2 < 1 || PaymentTaxPeriodPart2 > 12) {
                        return false;
                    }
                } else if (PaymentTaxPeriod.Name == "KB") {
                    if (PaymentTaxPeriodPart2 < 1 || PaymentTaxPeriodPart2 > 4) {
                        return false;
                    }
                } else if (PaymentTaxPeriod.Name == "ГД") {
                    if (PaymentTaxPeriodPart2 != 0) {
                        return false;
                    }
                } else if (PaymentTaxPeriod.Name == "ПЛ") {
                    if (PaymentTaxPeriodPart2 < 1 || PaymentTaxPeriodPart2 > 2) {
                        return false;
                    }
                }
                */
                return true;
            }
        }


        /// <summary>
        /// Период уплаты налога (сбора) - только год
        /// </summary>
        //[RuleRange("fmPaymentRequestMemorandum.PaymentTaxPeriodPart4.month.Range", DefaultContexts.Save, 2000, 2100)]
        public Int16 PaymentTaxPeriodPart4 {
            get {
                return _PaymentTaxPeriodPart4;
            }
            set {
                //SetPropertyValue<Int16>("PaymentTaxPeriodPart4", ref _PaymentTaxPeriodPart4, value);
                Int16 old = _PaymentTaxPeriodPart4;
                if (old != value) {
                    _PaymentTaxPeriodPart4 = value;
                    if (!IsLoading) {
                        SetTaxPeriod();
                        OnChanged("PaymentTaxPeriodPart4", old, value);
                    }
                }
            }
        }

        /// <summary>
        /// Период уплаты налога (сбора) - вид для печати: МС.05.2012 и т.п.
        /// </summary>
        //[Browsable(false)]
        //[PersistentAlias("_PaymentTaxPeriodPrint")]
        public String PaymentTaxPeriodPrint {
            get {
                //if (_PaymentTaxPeriodPrint == null)
                //_PaymentTaxPeriodPrint = PaymentTaxPeriod.ToString() + PaymentTaxPeriodDate.ToString(".MM.yyyy");
                //SetTaxPeriod();
                return _PaymentTaxPeriodPrint;
            }
            set {
                SetPropertyValue<String>("PaymentTaxPeriodPrint", ref _PaymentTaxPeriodPrint, value);
            }
        }

        /// <summary>
        /// Наименование шаблона
        /// </summary>
        [Size(250)]
        public String TemplateName {
            get {
                return _TemplateName;
            }
            set {
                SetPropertyValue<String>("TemplateName", ref _TemplateName, value);
            }
        }

        /// <summary>
        /// Лицевой счёт
        /// </summary>
        [Size(30)]
        public String PersonalAccount {
            get {
                return _PersonalAccount;
            }
            set {
                SetPropertyValue<String>("PersonalAccount", ref _PersonalAccount, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Создать шаблон на основе данного документа
        /// </summary>
        /// <returns></returns>
        public override fmCPRPaymentRequest CreateTemplate() {
            return base.CreateTemplate() as fmPaymentRequestMemorandum;
        }

        public override fmCPRPaymentRequest CloneRequest() {
            fmCPRPaymentRequest req = base.CloneRequest();
            return req;
        }

        private bool ManyObligations() {
            if (this.PaySettlmentOfObligations.Count > 1) {
                return true;
            }
            return false;
        }

        private void SetTaxPeriod() {
            //if (PaymentTaxPeriod == null || PaymentTaxPeriodDate == DateTime.MinValue) {
            //    _PaymentTaxPeriodPrint = null;
            //    return;
            //}
            //_PaymentTaxPeriodPrint = PaymentTaxPeriod.ToString() + PaymentTaxPeriodDate.ToString(".MM.yyyy");

            if (PaymentTaxPeriod == null) {
                _PaymentTaxPeriodPrint = null;
                return;
            }
            _PaymentTaxPeriodPrint = PaymentTaxPeriod.ToString() + "." + PaymentTaxPeriodPart2.ToString("0#") + "." + PaymentTaxPeriodPart4.ToString("000#");
        }

        /// <summary>
        /// Список персон
        /// </summary>
        [Aggregated]
        [Association("fmPRPaymentRequestMemorandum-fmCPRPaymentRequestMemorandumPeson", typeof(fmCPRPaymentRequestMemorandumPeson))]
        public XPCollection<fmCPRPaymentRequestMemorandumPeson> Persons {
            get {
                return GetCollection<fmCPRPaymentRequestMemorandumPeson>("Persons");
            }
        }

        #endregion

    }

}
