﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.ConditionalAppearance;
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
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.Order {
    //[Appearance("", AppearanceItemType.ViewItem, "Status != 'Execution'", TargetItems = "*", Enabled = false)]
    [VisibleInReports(true)]
    [DefaultProperty("DocName")]
    [NavigationItem("Finance")]
    [Persistent("fmOrderManageDoc")]
    public class fmCOrderManageDoc : csCCodedComponent, fmIOrderManageDoc { //, IStateMachineProvider {
        public fmCOrderManageDoc(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.Status = fmIOrderManageDocStatus.Execution;
            this._DocDate = DateTime.Now;
        }

        #region fmCOrderManageDocFinIndexStructureItem

//        [VisibleInReports(true)]
        [MapInheritance(MapInheritanceType.ParentTable)]
        public class fmCOrderManageDocFinIndexStructureItem : fmCFinIndexStructureItem {
            public fmCOrderManageDocFinIndexStructureItem(Session session)
                : base(session) {
            }

            private fmCOrderManageDoc _OrderManageDoc;

            [Association("fmOrderManageDoc-fmOrderManageDocFinIndexes")]
            public fmCOrderManageDoc OrderManageDoc {
                get { return _OrderManageDoc; }
                set { SetPropertyValue<fmCOrderManageDoc>("OrderManageDoc", ref _OrderManageDoc, value); }
            }
        }
        #endregion

        #region ПОЛЯ КЛАССА
        private fmCFAAccount _BuhAccount;
        private String _BuhAccountCode;
        private fmIOrderManageDocStatus _Status;
        private fmCSubjectExt _Subject;
        private fmCOrderExt _Order;
        private hrmStaff _Manager;
        private hrmStaff _ManagerPlanDepartment;
        private hrmStaff _ManagerSignAccount;
        private hrmStaff _ManagerSignPlan;
        private String _NameFull;
        private csNDSRate _AVTRate;
        private DateTime _DateBegin;
        private DateTime _DateEnd;
        private Boolean _IsClosed;
        private DateTime _DocDate;
        //
        private crmContractDeal _SourceDeal;
        private String _SourceOther;
        private crmCParty _SourceParty;
        [Persistent("SourceName")]
        private String _SourceName;
        //
        private Int32 _BuhIntNum;
        //private Decimal _KoeffKB;
        //private Decimal _KoeffOZM;
        //        [Persistent("DocName")]
//        private DateTime _DocName;
        #endregion

        //        public IList<IStateMachine> GetStateMachines() {
        //            List<IStateMachine> result = new List<IStateMachine>();
        //            result.Add(new fmCOrderManageDocSM());
        //            return result;
        //        }

        #region СВОЙСТВА КЛАССА

        [DataSourceCriteria("AccountSystem.Code == '1000' && IsSelectabled")]
        [RuleRequiredField(TargetCriteria = "Status == 'Opened'")]
        public fmCFAAccount BuhAccount {
            get { return _BuhAccount; }
            set { SetPropertyValue<fmCFAAccount>("BuhAccount", ref _BuhAccount, value); }
        }

        public override String Code {
            get { return base.Code; }
            set {
                base.Code = value;
                if (!IsLoading) {
                    OnChanged("DocName");
                }
            }
        }

        public DateTime DocDate {
            get { return _DocDate; }
            set { 
                SetPropertyValue<DateTime>("DocDate", ref _DocDate, value);
                if (!IsLoading) {
                    OnChanged("DocName");
                }
            }
        }

//        [PersistentAlias("_DocName")]
        public String DocName {
            get { return String.Concat(Code, " от ", this.DocDate.ToString("d")); }
        }

        public fmIOrderManageDocStatus Status {
            get { return _Status; }
            set {
                fmIOrderManageDocStatus old = _Status;
                if (old == value) return;
                _Status = value;
                if (!IsLoading) {
                    switch (value) {
                        case fmIOrderManageDocStatus.Rejected:
                            if (Order != null)
                                Order.ManageDocCancel(this);
                            break;
                        case fmIOrderManageDocStatus.AcceptMaker:
                            break;
                        case fmIOrderManageDocStatus.AcceptPlanDepartment:
                            break;
                        case fmIOrderManageDocStatus.AcceptAccountDepartment:
                            if (Order != null)
                                Order.ManageDocComplete(this);
                            break;
                        default:
                            break;
                    }
                    ReadOnlyUpdate();
                    OnChanged("Status", old, value);
                }
            }
        }

        public override Boolean ReadOnlyGet() {
            return Status != fmIOrderManageDocStatus.Execution;
        }

        [Association("fmOrder-OrderManageDoc")]
        [Appearance("", Criteria = "Order != null", Enabled = false)]
        [DataSourceCriteria("Status = 'Project' or Status = 'Accepted'")]
        [DataSourceProperty("Subject.OrderExts")]
        [RuleRequiredField]
        public fmCOrderExt Order {
            get { return _Order; }
            set {
                if (_Order != null)
                    return;
                SetPropertyValue<fmCOrderExt>("Order", ref _Order, value);
                if (!IsLoading) {
                    if (value != null) {
                        value.ManageDocNew(this);
                        this.CopyFrom(value);
                    } else {
                        value.ManageDocCancel(this);
                    }
                }
            }
        }

        fmIOrderExt fmIOrderManageDoc.Order {
            get { return this.Order; }
            set {
                fmCOrderExt old = Order;
                this.Order = value as fmCOrderExt;
                if (old != this.Order)
                    OnChanged("Order", old, this.Order);
            }
        }

        [Appearance("", Criteria = "Order != null", Enabled = false)]
        [RuleRequiredField]
        public fmCSubjectExt Subject {
            get { return _Subject; }
            set {
                SetPropertyValue("Subject", ref _Subject, value);
            }
        }

        fmISubject fmIOrder.Subject {
            get { return Subject; }
        }

        [Size(30)]
        public String BuhAccountCode {
            get { return _BuhAccountCode; }
            set { SetPropertyValue<String>("BuhAccountCode", ref _BuhAccountCode, value); }
        }

        public csNDSRate AVTRate {
            get { return _AVTRate; }
            set { SetPropertyValue<csNDSRate>("AVTRate", ref _AVTRate, value); }
        }

        [Browsable(false)]
        public Int32 BuhIntNum {
            get { return _BuhIntNum; }
            set { SetPropertyValue<Int32>("BuhIntNum", ref _BuhIntNum, value); }
        }

        [DataSourceProperty("ManagerSource")]
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

        [DataSourceProperty("ManagerSignAccountSource")]
        public hrmStaff ManagerSignAccount {
            get { return _ManagerSignAccount; }
            set {
                SetPropertyValue<hrmStaff>("ManagerSignAccount ", ref _ManagerSignAccount, value);
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerSignAccountSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfSignAccountDepartmentStaffs; }
        }

        [DataSourceProperty("ManagerSignPlanSource")]
        public hrmStaff ManagerSignPlan {
            get { return _ManagerSignPlan; }
            set {
                SetPropertyValue<hrmStaff>("ManagerSignPlan", ref _ManagerSignPlan, value);
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerSignPlanSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfSignPlanDepartmentStaffs; }
        }

        [Size(250)]
        public virtual String NameFull {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }
        //
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
            }
        }
        public Boolean IsClosed {
            get { return _IsClosed; }
            set {
                SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value);
            }
        }

        [Aggregated]
        [Association("fmOrderManageDoc-fmOrderManageDocFinIndexes", typeof(fmCOrderManageDocFinIndexStructureItem))]
        public XPCollection<fmCOrderManageDocFinIndexStructureItem> FinIndexes {
            get {
                XPCollection<fmCOrderManageDocFinIndexStructureItem> col = GetCollection<fmCOrderManageDocFinIndexStructureItem>("FinIndexes");
                //                col.Sorting.Add(new SortProperty("SortOrder", DevExpress.Xpo.DB.SortingDirection.Ascending));
                return col;
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
            } else {
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


        private fmСOrderAnalitycAccouterType _AnalitycAccouterType;
        public fmСOrderAnalitycAccouterType AnalitycAccouterType {
            get { return _AnalitycAccouterType; }
            set { SetPropertyValue<fmСOrderAnalitycAccouterType>("AnalitycAccouterType", ref _AnalitycAccouterType, value); }
        }

        private fmСOrderAnalitycAVT _AnalitycAVT;
        public fmСOrderAnalitycAVT AnalitycAVT {
            get { return _AnalitycAVT; }
            set { SetPropertyValue<fmСOrderAnalitycAVT>("AnalitycAccouterType", ref _AnalitycAVT, value); }
        }

        private fmСOrderAnalitycWorkType _AnalitycWorkType;
        public fmСOrderAnalitycWorkType AnalitycWorkType {
            get { return _AnalitycWorkType; }
            set { SetPropertyValue<fmСOrderAnalitycWorkType>("AnalitycWorkType", ref _AnalitycWorkType, value); }
        }

        private fmСOrderAnalitycOrderSource _AnalitycOrderSource;
        public fmСOrderAnalitycOrderSource AnalitycOrderSource {
            get { return _AnalitycOrderSource; }
            set { SetPropertyValue<fmСOrderAnalitycOrderSource>("AnalitycOrderSource", ref _AnalitycOrderSource, value); }
        }

        private fmСOrderAnalitycFinanceSource _AnalitycFinanceSource;
        public fmСOrderAnalitycFinanceSource AnalitycFinanceSource {
            get { return _AnalitycFinanceSource; }
            set { SetPropertyValue<fmСOrderAnalitycFinanceSource>("AnalitycFinanceSource", ref _AnalitycFinanceSource, value); }
        }

        private fmСOrderAnalitycMilitary _AnalitycMilitary;
        public fmСOrderAnalitycMilitary AnalitycMilitary {
            get { return _AnalitycMilitary; }
            set { SetPropertyValue<fmСOrderAnalitycMilitary>("AnalitycMilitary", ref _AnalitycMilitary, value); }
        }
        private fmСOrderAnalitycOKVED _AnalitycOKVED;
        public fmСOrderAnalitycOKVED AnalitycOKVED {
            get { return _AnalitycOKVED; }
            set { SetPropertyValue<fmСOrderAnalitycOKVED>("AnalitycOKVED", ref _AnalitycOKVED, value); }
        }
        private fmСOrderAnalitycFedProg _AnalitycFedProg;
        public fmСOrderAnalitycFedProg AnalitycFedProg {
            get { return _AnalitycFedProg; }
            set { SetPropertyValue<fmСOrderAnalitycFedProg>("AnalitycFedProg", ref _AnalitycFedProg, value); }
        }
        private fmСOrderAnalitycRegion _AnalitycRegion;
        public fmСOrderAnalitycRegion AnalitycRegion {
            get { return _AnalitycRegion; }
            set { SetPropertyValue<fmСOrderAnalitycRegion>("AnalitycRegion", ref _AnalitycRegion, value); }
        }
        private fmСOrderAnalitycBigCustomer _AnalitycBigCustomer;
        public fmСOrderAnalitycBigCustomer AnalitycBigCustomer {
            get { return _AnalitycBigCustomer; }
            set { SetPropertyValue<fmСOrderAnalitycBigCustomer>("AnalitycBigCustomer", ref _AnalitycBigCustomer, value); }
        }


        #endregion

        #region fmIFinStructure

        IList<fmIFinIndexStructureItem> fmIFinIndexStructure.FinIndexes {
            get { return new ListConverter<fmIFinIndexStructureItem, fmCOrderManageDocFinIndexStructureItem>(FinIndexes); }
        }

        /// <summary>
        /// Паша!!! Пока работаем с сессией потом нужен будет ObjectSpace
        /// </summary>
        /// <param name="fin_index"></param>
        /// <returns></returns>
        public fmIFinIndexStructureItem FinIndexesCreateItem(fmCFinIndex fin_index) {
            fmCOrderManageDocFinIndexStructureItem item = new fmCOrderManageDocFinIndexStructureItem(this.Session) {
                FinIndex = fin_index
            };
            FinIndexes.Add(item);
            return item;
        }

        public void UpdateIndexStructure(IList<fmCFinIndex> index_col) {
            fmIFinIndexStructureLogic.UpdateIndexStructure(this, index_col);
        }

        void fmIFinIndexStructure.Copy(fmIFinIndexStructure from) {
            fmIFinIndexStructureLogic.Copy(this, from);
        }

        #endregion

        public void CopyTo(fmCOrderExt to) {
            this.CopyTo((fmIOrder)to);
        }

        public void CopyFrom(fmCOrderExt from) {
            from.CopyTo(this);
            ((fmIFinIndexStructure)this).Copy(from);
            this.Subject = from.Subject as fmCSubjectExt;
            if (this.Subject != null) {
                this.SourceDeal = this.Subject.SourceDeal;
                if (this.SourceDeal == null)
                    this.SourceParty = this.Subject.SourceParty;
                this.SourceOther = from.SourceOther;
            }
        }

        #region fmIOrder

        public void CopyTo(fmIOrder to) {
            fmIOrderLogic.CopyTo(this, to);
        }

        #endregion


        private fmIOrderOverheadType _OverheadType;
        private fmCOrderOverheadIndividual _OverheadIndividual;
        private fmCOrderOverheadStandart _OverheadStandart;

        [RuleRequiredField(TargetCriteria = "Status == 'FinOpened'")]
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
                OverheadStandart = (fmCOrderOverheadStandart)value;
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

        //[Browsable(false)]
        //public Decimal KoeffKB {
        //    get { return _KoeffKB; }
        //    set { SetPropertyValue<Decimal>("KoeffKB", ref _KoeffKB, value); }
        //}

        //[Browsable(false)]
        //public Decimal KoeffOZM {
        //    get { return _KoeffOZM; }
        //    set { SetPropertyValue<Decimal>("KoeffOZM", ref _KoeffOZM, value); }
        //}

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

        //public fmIOrderOverheadType OverheadType {
        //    get {
        //        throw new NotImplementedException();
        //    }
        //    set {
        //        throw new NotImplementedException();
        //    }
        //}

        //public fmIOrderOverheadStandart OverheadStandart {
        //    get {
        //        throw new NotImplementedException();
        //    }
        //    set {
        //        throw new NotImplementedException();
        //    }
        //}

        //public fmIOrderOverheadValueType PlanOverheadType {
        //    get {
        //        throw new NotImplementedException();
        //    }
        //    set {
        //        throw new NotImplementedException();
        //    }
        //}


        //public fmIOrderOverheadValueType BuhOverheadType {
        //    get {
        //        throw new NotImplementedException();
        //    }
        //    set {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
