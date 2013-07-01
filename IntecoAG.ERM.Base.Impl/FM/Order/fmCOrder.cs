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
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.FM.FinAccount;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.Order
{
    /// <summary>
    /// Класс Order
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("fmOrder")]
    [FriendlyKeyProperty("Code")]
    [DefaultProperty("Name")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Code")]
    [Appearance("", AppearanceItemType.ViewItem, "", Enabled = false, TargetItems = "IsClosed")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "OverheadType == 'Standart'", 
            TargetItems="PlanOverheadType,BuhOverheadType,FixKoeff,FixKoeffOZM", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "OverheadType == 'Individual'",
            TargetItems = "OverheadStandart", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "PlanOverheadType == 'NO_OVERHEAD' || PlanOverheadType == 'VARIABLE'",
            TargetItems = "FixKoeff,FixKoeffOZM", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "Status == 'Opened' || Status == 'Closed' || Status == 'Blocked'", TargetItems = "*", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "Status == 'FinOpened'", TargetItems = "*,BuhAccount,AnalitycAVT,AnalitycAccouterType,BuhOverheadType", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "Status == 'FinOpened' && OverheadType != 'Individual'", TargetItems = "BuhOverheadType", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "Status == 'FinClosed'", TargetItems = "*,DateEnd", Enabled = false)]
    [Appearance("", AppearanceItemType.ViewItem, "DateEnd <= LocalDateTimeToday() && Status != 'FinClosed' && Status != 'Closed'", TargetItems = "Code,Name", BackColor = "Blue")]
    //    [RuleCriteria("", DefaultContexts.Save, "FixKoeff == 0 && FixKoeffOZM == 0",
//            TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Individual' && PlanOverheadType != 'VARIABLE' && PlanOverheadType != 'NO_OVERHEAD'",
//            UsedProperties = "FixKoeff,FixKoeffOZM")]
    public abstract class fmCOrder : gfmCAnalyticBase, fmIOrder, IStateMachineProvider
    {
        public fmCOrder(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.OverheadType = fmIOrderOverheadType.Standart;
            this.Status = fmIOrderStatus.Project;
        }

        #region ПОЛЯ КЛАССА
        private fmSubjectSourceType _SourceType;
        private fmCSubject _Subject;
        private hrmStaff _Manager;
        private hrmStaff _ManagerPlanDepartment;
        private crmContractDeal _SourceDeal;
        private String _SourceOther;
        private crmCParty _SourceParty;
        [Persistent("SourceName")]
        private String _SourceName;
        private String _BuhAccountCode;
//        private String _BuhAccount;
        private csNDSRate _AVTRate;
        private Int32 _BuhIntNum;
        private Decimal _KoeffKB;
        private Decimal _KoeffOZM;
        private fmIOrderStatus _Status;
        #endregion

        #region СВОЙСТВА КЛАССА

        public fmIOrderStatus Status {
            get { return _Status; }
            set {
                SetPropertyValue<fmIOrderStatus>("Status", ref _Status, value);
                if (!IsLoading) {
                    if (value == fmIOrderStatus.Closed)
                        IsClosed = true;
                    else
                        IsClosed = false;
                    ReadOnlyUpdate();
                }
            }
        }

        [VisibleInListView(false)]
        public string NameAndCode {
            get {
                if (Name != null && Name.Trim() != "")
                    return string.Concat(Code, " ", Name);
                else
                    return this.Code;
            }
        }

        [Association("fmSubject-Orders")]
        [RuleRequiredField(TargetCriteria = "Status != 'FinClosed' && Status != 'Closed'")]
        public fmCSubject Subject {
            get { return _Subject; }
            set {
                if (IsLoading) {
                    _Subject = value;
                }
                else {
                    fmCSubject old = _Subject;
                    if (value != old) {
                        if (old != null)
                            old.OrdersRemove(this);
                        _Subject = value;
                        if (value != null)
                            value.OrdersAdd(this);
                        OnChanged("Subject", old, value);
                    }
                }
            }
        }

        fmISubject fmIOrder.Subject {
            get { return Subject; }
        }

        ////[PersistentAlias("Subject.SourceName")]
        //public String SourceName {
        //    get {
        //        if (String.IsNullOrEmpty(_SourceName)) {
        //            if (Subject != null)
        //                _SourceName = Subject.SourceName;
        //        }
        //        return _SourceName;
        //    }
        //    set {
        //        SetPropertyValue<String>("SourceName", ref _SourceName, value);
        //    }
        //}

        [DataSourceProperty("ManagerSource")]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened'")]
        public hrmStaff Manager {
            get { return _Manager; }
            set {
                SetPropertyValue<hrmStaff>("Manager", ref _Manager, value);
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfOrderStaffs; }
        }

        hrmIStaff fmIOrder.Manager {
            get { return Manager; }
            set {
                hrmIStaff old = Manager;
                Manager = value as hrmStaff;
                if (Manager != old)
                    OnChanged("Manager", old, Manager);
            }
        }

        [DataSourceProperty("ManagerPlanDepartmentSource")]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened'")]
        public hrmStaff ManagerPlanDepartment {
            get { return _ManagerPlanDepartment; }
            set {
                SetPropertyValue<hrmStaff>("ManagerPlanDepartment", ref _ManagerPlanDepartment, value);
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerPlanDepartmentSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfPlanDepartmentStaffs; }
        }

        hrmIStaff fmIOrder.ManagerPlanDepartment {
            get { return ManagerPlanDepartment; }
            set {
                hrmIStaff old = ManagerPlanDepartment;
                ManagerPlanDepartment = value as hrmStaff;
                if (ManagerPlanDepartment != old)
                    OnChanged("Manager", old, Manager);
            }
        }
//
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened'")]
        public fmSubjectSourceType SourceType {
            get {
                return _SourceType;
            }
            set {
                SetPropertyValue<fmSubjectSourceType>("SourceType", ref _SourceType, value);
            }
        }

        public crmContractDeal SourceDeal {
            get { return _SourceDeal; }
            set {
                SetPropertyValue<crmContractDeal>("SourceDeal", ref _SourceDeal, value);
                if (!IsLoading) {
                    if (value != null)
                        SourceParty = value.Customer;
                    SourceNameUpdate();
                }
            }
        }

        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' && SourceType == 'SOURCE_TYPE_OTHER'")]
        public String SourceOther {
            get { return _SourceOther; }
            set {
                SetPropertyValue<String>("SourceOther", ref _SourceOther, value);
                if (!IsLoading) {
                    SourceNameUpdate();
                }
            }
        }

        public void SourceNameUpdate() {
            if (SourceDeal != null) {
                _SourceName = SourceDeal.Name;
            }
            else {
                _SourceName = _SourceOther;
            }
        }

        [PersistentAlias("_SourceName")]
        public String SourceName {
            get {
                if (_SourceName == null)
                    SourceNameUpdate();
                return _SourceName;
            }
        }
        //[RuleRequiredField]
        public crmCParty SourceParty {
            get { return _SourceParty; }
            set { SetPropertyValue<crmCParty>("SourceParty", ref _SourceParty, value); }
        }

        crmIParty fmIOrder.SourceParty {
            get { return SourceParty; }
            set {
                crmIParty old = SourceParty;
                SourceParty = value as crmCParty;
            }
        }

        [Size(30)]
        public String BuhAccountCode {
            get { return _BuhAccountCode; }
            set { SetPropertyValue<String>("BuhAccountCode", ref _BuhAccountCode, value); }
        }

        //[Size(30)]
        //[Browsable(false)]
        //public String BuhAccount {
        //    get { return _BuhAccount; }
        //    set { SetPropertyValue<String>("BuhAccount", ref _BuhAccount, value); }
        //}

        public csNDSRate AVTRate {
            get { return _AVTRate; }
            set { SetPropertyValue<csNDSRate>("AVTRate", ref _AVTRate, value); }
        }


        [Browsable(false)]
        public Int32 BuhIntNum {
            get { return _BuhIntNum; }
            set { SetPropertyValue<Int32>("BuhIntNum", ref _BuhIntNum, value); }
        }

        private fmСOrderAnalitycCoperatingType _AnalitycCoperatingType;
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycCoperatingType AnalitycCoperatingType {
            get { return _AnalitycCoperatingType; }
            set { SetPropertyValue<fmСOrderAnalitycCoperatingType>("AnalitycCoperatingType", ref _AnalitycCoperatingType, value); }
        }

        private fmСOrderAnalitycAccouterType _AnalitycAccouterType;
        [RuleRequiredField(TargetCriteria = "Status == 'Opened'")]
        public fmСOrderAnalitycAccouterType AnalitycAccouterType {
            get { return _AnalitycAccouterType; }
            set { 
                SetPropertyValue<fmСOrderAnalitycAccouterType>("AnalitycAccouterType", ref _AnalitycAccouterType, value);
                if (!IsLoading && value != null) {
                    BuhAccount = value.DefaultAccount;
                }
            }
        }

        private fmСOrderAnalitycAVT _AnalitycAVT;
        [RuleRequiredField(TargetCriteria = "Status == 'Opened'")]
        public fmСOrderAnalitycAVT AnalitycAVT {
            get { return _AnalitycAVT; }
            set { SetPropertyValue<fmСOrderAnalitycAVT>("AnalitycAccouterType", ref _AnalitycAVT, value); }
        }

        private fmСOrderAnalitycWorkType _AnalitycWorkType;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycWorkType AnalitycWorkType {
            get { return _AnalitycWorkType; }
            set { SetPropertyValue<fmСOrderAnalitycWorkType>("AnalitycWorkType", ref _AnalitycWorkType, value); }
        }

        private fmСOrderAnalitycOrderSource _AnalitycOrderSource;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycOrderSource AnalitycOrderSource {
            get { return _AnalitycOrderSource; }
            set { SetPropertyValue<fmСOrderAnalitycOrderSource>("AnalitycOrderSource", ref _AnalitycOrderSource, value); }
        }

        private fmСOrderAnalitycFinanceSource _AnalitycFinanceSource;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycFinanceSource AnalitycFinanceSource {
            get { return _AnalitycFinanceSource; }
            set { SetPropertyValue<fmСOrderAnalitycFinanceSource>("AnalitycFinanceSource", ref _AnalitycFinanceSource, value); }
        }

        private fmСOrderAnalitycMilitary _AnalitycMilitary;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycMilitary AnalitycMilitary {
            get { return _AnalitycMilitary; }
            set { SetPropertyValue<fmСOrderAnalitycMilitary>("AnalitycMilitary", ref _AnalitycMilitary, value); }
        }
        private fmСOrderAnalitycFedProg _AnalitycFedProg;
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycFedProg AnalitycFedProg {
            get { return _AnalitycFedProg; }
            set { SetPropertyValue<fmСOrderAnalitycFedProg>("AnalitycFedProg", ref _AnalitycFedProg, value); }
        }
        private fmСOrderAnalitycOKVED _AnalitycOKVED;
        public fmСOrderAnalitycOKVED AnalitycOKVED {
            get { return _AnalitycOKVED; }
            set { SetPropertyValue<fmСOrderAnalitycOKVED>("AnalitycOKVED", ref _AnalitycOKVED, value); }
        }
        private fmСOrderAnalitycRegion _AnalitycRegion;
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycRegion AnalitycRegion {
            get { return _AnalitycRegion; }
            set { SetPropertyValue<fmСOrderAnalitycRegion>("AnalitycRegion", ref _AnalitycRegion, value); }
        }
        private fmСOrderAnalitycBigCustomer _AnalitycBigCustomer;
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmСOrderAnalitycBigCustomer AnalitycBigCustomer {
            get { return _AnalitycBigCustomer; }
            set { 
                SetPropertyValue<fmСOrderAnalitycBigCustomer>("AnalitycBigCustomer", ref _AnalitycBigCustomer, value);
                if (!IsLoading && value != null && value.Region != null) {
                    _AnalitycRegion = value.Region;
                }
            }
        }

        #endregion

        #region fmIOrder

        public void CopyTo(fmIOrder to) {
            fmIOrderLogic.CopyTo(this, to);
        }

        #endregion

        private fmIOrderOverheadType _OverheadType;
        private fmCOrderOverheadIndividual _OverheadIndividual;
        private fmCOrderOverheadStandart _OverheadStandart;
        private fmCFAAccount _BuhAccount;

        [DataSourceCriteria("AccountSystem.Code == '1000' && IsSelectabled")]
        [RuleRequiredField(TargetCriteria = "Status == 'Opened'")]
        [Persistent("BuhAccountRef")]
        public fmCFAAccount BuhAccount {
            get { return _BuhAccount; }
            set { SetPropertyValue<fmCFAAccount>("BuhAccount", ref _BuhAccount, value); }
        }

        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' || Status == 'Opened'")]
        public fmIOrderOverheadType OverheadType {
            get {
                return _OverheadType;
            }
            set {
                fmIOrderOverheadType old = OverheadType;
                SetPropertyValue("OverheadType", ref _OverheadType, value);
                if (!IsLoading) {
                    fmCOrderOverheadIndividual oldind = OverheadIndividual;
                    if (value == fmIOrderOverheadType.Standart) {
                        OverheadIndividual = null;
                        if (oldind != null)
                            oldind.Delete();
                    }
                    if (value == fmIOrderOverheadType.Individual) {
                        OverheadStandart = null;
                        if (oldind == null)
                            OverheadIndividual = new fmCOrderOverheadIndividual(Session);
                    }
                    if (old != value) {
                        OnChanged("FixKoeff");
                        OnChanged("FixKoeffOZM");
                        OnChanged("PlanOverheadType");
                        OnChanged("BuhOverheadType");
                    }
                }
            }
        }

        [Browsable(false)]
        [Aggregated]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Individual'")]
        public fmCOrderOverheadIndividual OverheadIndividual {
            get {
                return _OverheadIndividual;
            }
            set {
                if (!IsLoading) {
                    if (OverheadType == fmIOrderOverheadType.Individual && value != null ||
                        OverheadType == fmIOrderOverheadType.Standart && value == null) {
                        SetPropertyValue<fmCOrderOverheadIndividual>("OverheadIndividual", ref _OverheadIndividual, value);
                    }
                    else
                        throw new InvalidOperationException("Overhead Type not is Individual");
                }
                else
                    _OverheadIndividual = value;
            }
        }

        [NonPersistent]
        public fmCOrderOverhead Overhead {
            get {
                if (OverheadType == fmIOrderOverheadType.Individual)
                    return OverheadIndividual;
                if (OverheadType == fmIOrderOverheadType.Standart)
                    return OverheadStandart;
                return null;
            }
        }

        fmIOrderOverheadStandart fmIOrder.OverheadStandart {
            get {
                return OverheadStandart;
            }
            set {
                OverheadStandart = (fmCOrderOverheadStandart) value;
            }
        }

        [ExplicitLoading(1)]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Standart'")]
        public fmCOrderOverheadStandart OverheadStandart {
            get {
                return _OverheadStandart;
            }
            set {
                if (!IsLoading) {
                    if (OverheadType == fmIOrderOverheadType.Standart && value != null ||
                        OverheadType == fmIOrderOverheadType.Individual && value == null) {
                        SetPropertyValue<fmCOrderOverheadStandart>("OverheadStandart", ref _OverheadStandart, value);
                    }
                    else 
                        throw new InvalidOperationException("Overhead Type not is Standart");
                }
                else
                    _OverheadStandart = value;
            }
        }

        [NonPersistent]
        //[RuleCriteria("", DefaultContexts.Save, "PlanOverheadType == 0", TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Individual'")]
        public fmIOrderOverheadValueType PlanOverheadType {
            get { return Overhead == null ? 0 : Overhead.PlanOverheadType; }
            set {
                if (OverheadType == fmIOrderOverheadType.Individual && Overhead != null) {
                    fmIOrderOverheadValueType old = Overhead.PlanOverheadType;
                    Overhead.PlanOverheadType = value;
                    OnChanged("PlanOverheadType", old, value);
                }
                else
                    throw new InvalidOperationException("Overhead Type not is Individual");
            }
        }

        [NonPersistent]
//        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Individual'", TargetContextIDs = "Save")]
        //[RuleCriteria("", DefaultContexts.Save, "BuhOverheadType == 0", TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Individual'")]
        public fmIOrderOverheadValueType BuhOverheadType {
            get { return Overhead == null ? 0 : Overhead.BuhOverheadType; }
            set {
                if (OverheadType == fmIOrderOverheadType.Individual && Overhead != null) {
                    fmIOrderOverheadValueType old = Overhead.BuhOverheadType;
                    Overhead.BuhOverheadType = value;
                    OnChanged("BuhOverheadType", old, value);
                }
                else
                    throw new InvalidOperationException("Overhead Type not is Individual or Overhead is null");
            }
        }

        [Browsable(false)]
        public Decimal KoeffKB {
            get { return _KoeffKB; }
            set { SetPropertyValue<Decimal>("KoeffKB", ref _KoeffKB, value); }
        }

        [Browsable(false)]
        public Decimal KoeffOZM {
            get { return _KoeffOZM; }
            set { SetPropertyValue<Decimal>("KoeffOZM", ref _KoeffOZM, value); }
        }

        [NonPersistent]
//        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Individual'", TargetContextIDs="Save")]
        public Decimal FixKoeff {
            get { return Overhead == null ? 0 : Overhead.FixKoeff; }
            set {
                if (OverheadType == fmIOrderOverheadType.Individual && Overhead != null) {
                    Decimal old = Overhead.FixKoeff;
                    Overhead.FixKoeff = value;
                    OnChanged("FixKoeff", old, value);
                }
                else
                    throw new InvalidOperationException("Overhead Type not is Individual or Overhead is null");
            }
        }

        [NonPersistent]
        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened' && OverheadType == 'Individual'", TargetContextIDs = "Save")]
        public Decimal FixKoeffOZM {
            get { return Overhead == null ? 0 : Overhead.FixKoeffOZM; }
            set {
                if (OverheadType == fmIOrderOverheadType.Individual && Overhead != null) {
                    Decimal old = Overhead.FixKoeffOZM;
                    Overhead.FixKoeffOZM = value;
                    OnChanged("FixKoeffOZM", old, value);
                }
                else
                    throw new InvalidOperationException("Overhead Type not is Individual or Overhead is null");
            }
        }

        //[Action( PredefinedCategory.RecordEdit, Caption="КакВТеме")]
        //public void ActionSetAsSubject() {
        //    this.SourceType = Subject.SourceType;
        //    if (this.SourceDeal == null)
        //        this.SourceDeal = Subject.SourceDeal;
        //    if (String.IsNullOrEmpty(this.SourceOther))
        //        this.SourceOther = Subject.SourceOther;
        //    if (this.SourceParty == null)
        //        this.SourceParty = Subject.SourceParty;
        //    if (this.AnalitycWorkType == null)
        //        this.AnalitycWorkType = Subject.AnalitycWorkType;
        //    if (this.AnalitycFinanceSource == null)
        //        this.AnalitycFinanceSource = Subject.AnalitycFinanceSource;
        //    if (this.AnalitycAVT == null)
        //        this.AnalitycAVT = Subject.AnalitycAVT;
        //    if (this.AnalitycBigCustomer == null)
        //        this.AnalitycBigCustomer = Subject.AnalitycBigCustomer;
        //    if (this.AnalitycCoperatingType == 0)
        //        this.AnalitycCoperatingType = Subject.AnalitycCoperatingType;
        //    if (this.AnalitycFedProg == null)
        //        this.AnalitycFedProg = Subject.AnalitycFedProg;
        //    if (this.AnalitycMilitary == null)
        //        this.AnalitycMilitary = Subject.AnalitycMilitary;
        //    if (this.AnalitycOrderSource == null)
        //        this.AnalitycOrderSource = Subject.AnalitycOrderSource;
        //    if (this.AnalitycRegion == null)
        //        this.AnalitycRegion = Subject.AnalitycRegion;
        //    if (this.Manager == null)
        //        this.Manager = Subject.Manager;
        //    if (this.ManagerPlanDepartment == null)
        //        this.ManagerPlanDepartment = Subject.ManagerPlanDepartment;
        //}

        public IList<IStateMachine> GetStateMachines() {
            List<IStateMachine> result = new List<IStateMachine>();
            result.Add(new fmCOrderSM());
            return result;
        }


    }

}