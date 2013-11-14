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
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.FM.Subject
{

    public enum DealInfoDealType {
        DEAL_INFO_PROCEEDS = 1,
        DEAL_INFO_EXPENDITURE = 2
    }
    public enum DealInfoNomType {
        DEAL_INFO_DELIVERY = 1,
        DEAL_INFO_PAYMENT = 2
    }

    /// <summary>
    /// Класс Subject
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    [MiniNavigation("This", "Тема", TargetWindow.Default, 1)]
    [MiniNavigation("SourceDeal.Current", "Договор", TargetWindow.Default, 2)]
    [FriendlyKeyProperty("Code")]
    [DefaultProperty("Name")]
    [Persistent("fmSubject")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Code")]
    [Appearance("", AppearanceItemType.ViewItem, "", Enabled = false, TargetItems = "IsClosed")]
    [Appearance("", AppearanceItemType.Action, "Status != 'PROJECT' || !IsDeleteAllow", Enabled = false, TargetItems = "Delete")]
    public abstract class fmCSubject : gfmCAnalyticBase, fmISubject, IStateMachineProvider
    {
        public fmCSubject(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Status = fmISubjectStatus.PROJECT;
        }

        protected override void OnDeleting() {
            if (this.IsSaving && (Status != fmISubjectStatus.PROJECT || !IsDeleteAllow)) {
                throw new InvalidOperationException("Delete is not allowed");
            }
        }

        #region ПОЛЯ КЛАССА
        [Persistent("TrwCode")]
        private String _TrwCode;
        private Int16 _TrwNumber;
        private fmISubjectStatus _Status;
        private fmCSubjectGroup _SubjectGroup;
        private fmCDirection _Direction;
        private fmСOrderAnalitycWorkType _AnalitycWorkType;
        private fmСOrderAnalitycFinanceSource _AnalitycFinanceSource;
        private crmContractDeal _SourceDeal;
        private String _SourceOther;
        private crmCParty _SourceParty;
        [Persistent("SourceName")]
        private String _SourceName;
        private hrmStaff _Manager;
        private hrmStaff _ManagerCurator;
        private hrmStaff _ManagerPlanDepartment;
        private fmSubjectSourceType _SourceType;
        #endregion

        #region Associations

        [Size(7)]
        [PersistentAlias("_TrwCode")]
        public String TrwCode {
            get { return _TrwCode; }
//            set { SetPropertyValue<String>("TrwCode", ref _TrwCode, value); }
        }
        [Browsable(false)]
        public Int16 TrwNumber {
            get { return _TrwNumber; }
            set { SetPropertyValue<Int16>("TrwNumber", ref _TrwNumber, value); }
        }
        public void TrwCodeNew() {
            if (Direction == null) 
                throw new InvalidOperationException();
            TrwNumber = Direction.TrwCodeSubjectNumberNew();
            TrwCodeReNumber();
        }
        public void TrwCodeReNumber() {
            if (Direction == null)
                throw new InvalidOperationException();
            _TrwCode = "Z" + Direction.TrwCode + TrwNumber.ToString("D4");
            OnChanged("TrwCode");
        }

        [Association("fmDirection-Subjects")]
        public fmCDirection Direction {
            get { return _Direction; }
            set { 
                SetPropertyValue<fmCDirection>("Direction", ref _Direction, value);
                if (!IsLoading && value != null) {
                    this.ManagerCurator = value.Manager;
                    this.Manager = value.Manager;
                    this.TrwCodeNew();
                }
            }
        }
        fmIDirection fmISubject.Direction {
            get {
                return this.Direction;
            }
            set {
                this.Direction = (fmCDirection) value;
            }
        }
        [RuleRequiredField]
        [Association("fmCSubject-fmCSubjectGroup")]
        public fmCSubjectGroup SubjectGroup {
            get { return _SubjectGroup; }
            set {
                SetPropertyValue<fmCSubjectGroup>("SubjectGroup", ref _SubjectGroup, value);
            }
        }
        //
        [Association("fmSubject-Orders", typeof(Order.fmCOrder))]
        //[Aggregated]
        public XPCollection<fmCOrder> Orders {
            get {
                return GetCollection<Order.fmCOrder>("Orders");
            }
        }
        //[Aggregated]
        IList<fmIOrder> fmISubject.Orders {
            get {
                return new ListConverter<fmIOrder, fmCOrder>(this.Orders);
            }
        }

        public void OrdersAdd(fmCOrder order) {
            order.SourceType = this.SourceType;
            order.SourceDeal = this.SourceDeal;
            order.SourceOther = this.SourceOther;
            order.SourceParty = this.SourceParty;
            order.AnalitycWorkType = this.AnalitycWorkType;
            order.AnalitycFinanceSource = this.AnalitycFinanceSource;
            order.AnalitycAVT = this.AnalitycAVT;
            order.AnalitycBigCustomer = this.AnalitycBigCustomer;
            order.AnalitycCoperatingType = this.AnalitycCoperatingType;
            order.AnalitycFedProg = this.AnalitycFedProg;
            order.AnalitycMilitary = this.AnalitycMilitary;
            order.AnalitycOrderSource = this.AnalitycOrderSource;
            order.AnalitycRegion = this.AnalitycRegion;
            order.Manager = this.Manager;
            order.ManagerPlanDepartment = this.ManagerPlanDepartment;
        }

        public void OrdersRemove(fmCOrder order) { 
        }

        [Association("fmSubjects-crmDeals")]
        public XPCollection<crmContractDeal> Deals {
            get {
                return GetCollection<crmContractDeal>("Deals");
            }
        }

        public class DealsCollection<T> : XPCollection<T> {
            private Object _Owner;
            public DealsCollection(Session session, object owner, XPMemberInfo property) :
                base(session, owner, property) {
                    _Owner = owner;
            }
            public override int BaseAdd(object newObject) {
                Int32 index = base.BaseAdd(newObject);
                fmCSubject owner = _Owner as fmCSubject;
                if (owner != null && newObject is crmContractDeal)
                    owner.DealsAdd(newObject as crmContractDeal);
                return index;
            }
        }

        protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property) {
            if (property.Name != "Deals")
                return base.CreateCollection<T>(property);
            else {
                XPCollection<T> col = new DealsCollection<T>(this.Session, this, property);
			    GC.SuppressFinalize(col);
			    return col;
            }
        }

        [NonPersistent]
        public class DealInfo: BaseObject {
            public DealInfoDealType DealType;
            public DealInfoNomType NomType;
            public crmContractDeal Deal;
            public fmCSubject Subject;
            public fmCOrder Order;
            public csValuta Valuta;
            public Int32 Year {
                get {
                    return Date.Year;
                }
            }
            public Int32 Month {
                get {
                    return Date.Month;
                }
            }
            public Decimal SummCost;
            public Decimal SummVat;
            public Decimal SummFull;

            [VisibleInDetailView(false)]
            [VisibleInListView(false)]
            public Decimal PaySummCost {
                get { return NomType == DealInfoNomType.DEAL_INFO_PAYMENT ? SummCost : 0; }
            }
            [VisibleInDetailView(false)]
            [VisibleInListView(false)]
            public Decimal PaySummVat {
                get { return NomType == DealInfoNomType.DEAL_INFO_PAYMENT ? SummVat : 0; }
            }
            [VisibleInDetailView(false)]
            [VisibleInListView(false)]
            public Decimal PaySummFull {
                get { return NomType == DealInfoNomType.DEAL_INFO_PAYMENT ? SummFull : 0; }
            }

            [VisibleInDetailView(false)]
            [VisibleInListView(false)]
            public Decimal DeliverySummCost {
                get { return NomType == DealInfoNomType.DEAL_INFO_DELIVERY ? SummCost : 0; }
            }
            [VisibleInDetailView(false)]
            [VisibleInListView(false)]
            public Decimal DeliverySummVat {
                get { return NomType == DealInfoNomType.DEAL_INFO_DELIVERY ? SummVat : 0; }
            }
            [VisibleInDetailView(false)]
            [VisibleInListView(false)]
            public Decimal DeliverySummFull {
                get { return NomType == DealInfoNomType.DEAL_INFO_DELIVERY ? SummFull : 0; }
            }

            public DateTime Date;
            public DealInfo(Session session) : base(session) { }
        }

        public class DealInfoCollection : List<DealInfo> {
            public fmCSubject Subject;

            [Action(Caption="Reload")]
            public void ReloadAction() {
                Subject.ReloadDealInfos();
            }
        }

        private DealInfoCollection _DealInfos;
        public DealInfoCollection DealInfos {
            get {
                if (_DealInfos == null) 
                    ReloadDealInfos();
                return _DealInfos;
            }
        }

        public void ReloadDealInfos() {
            if (_DealInfos == null)  {
                _DealInfos = new DealInfoCollection() {
                    Subject = this
                };
            }
            _DealInfos.Clear();
            foreach(crmContractDeal deal in Deals) {
                DealInfoDealType deal_type;
                if (deal.TRVType == null || deal.TRVType.TrwContractSuperType != TrwContractSuperType.DEAL_SALE)
                    deal_type = DealInfoDealType.DEAL_INFO_EXPENDITURE;
                else
                    deal_type = DealInfoDealType.DEAL_INFO_PROCEEDS;
                if (deal is crmDealWithoutStage)
                    ReloadDealInfos(deal_type, (crmDealWithoutStage)deal);
                if (deal is crmDealWithStage)
                    ReloadDealInfos(deal_type, (crmDealWithStage)deal);
            }
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmDealWithStage deal) {
            foreach (crmStage stage in deal.Current.StageStructure.Stages) {
                ReloadDealInfos(deal_type, deal, stage.DeliveryPlan);
                ReloadDealInfos(deal_type, deal, stage.PaymentPlan);
            }
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmDealWithoutStage deal) {
            ReloadDealInfos(deal_type, deal, ((crmDealWithoutStageVersion)deal.Current).DeliveryPlan);
            ReloadDealInfos(deal_type, deal, ((crmDealWithoutStageVersion)deal.Current).PaymentPlan);
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmContractDeal deal, crmDeliveryPlan plan) { 
            foreach (crmDeliveryUnit unit in plan.DeliveryUnits) {
                ReloadDealInfos(deal_type, deal, DealInfoNomType.DEAL_INFO_DELIVERY, unit, new ListConverter<crmObligation, crmDeliveryItem>( unit.DeliveryItems));
            }
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmContractDeal deal, crmPaymentPlan plan) {
            foreach (crmPaymentUnit unit in plan.PaymentUnits) {
                ReloadDealInfos(deal_type, deal, DealInfoNomType.DEAL_INFO_PAYMENT, unit, new ListConverter<crmObligation, crmPaymentItem>(unit.PaymentItems));
            }
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type,  crmContractDeal deal, DealInfoNomType nom_type, crmObligationUnit unit, IList<crmObligation> items) {
            foreach (crmObligation obl in items) {
                var deal_info = _DealInfos.FirstOrDefault(
                    x => x.Deal == deal &&
                        x.DealType == deal_type &&
                        x.NomType == nom_type &&
                        x.Order == obl.Order &&
                        x.Valuta == obl.Valuta &&
                        x.Date == unit.DatePlane
                    );
                if (deal_info == null) {
                    deal_info = new DealInfo(this.Session) {
                        DealType = deal_type,
                        Deal = deal,
                        NomType = nom_type,
                        Subject = obl.Order != null ? obl.Order.Subject : null,
                        Order = obl.Order,
                        Valuta = obl.Valuta,
                        Date = unit.DatePlane
                    };
                    _DealInfos.Add(deal_info);
                }
                deal_info.SummCost += obl.SummCost;
                deal_info.SummVat += obl.SummNDS;
                deal_info.SummFull += obl.SummFull;
            }
        }

        [Browsable(false)]
        public Boolean IsDeleteAllow {
            get {
                return Orders.Count == 0;
            }
        }

        [DataSourceProperty("ManagerSource")]
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public hrmStaff ManagerCurator {
            get { return _ManagerCurator; }
            set {
                SetPropertyValue<hrmStaff>("ManagerCurator", ref _ManagerCurator, value);
            }
        }
        [DataSourceProperty("ManagerSource")]
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public hrmStaff Manager {
            get { return _Manager; }
            set {
                SetPropertyValue<hrmStaff>("Manager", ref _Manager, value);
            }
        }
        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfSubjectStaffs; }
        }

        hrmIStaff fmISubject.Manager {
            get { return Manager; }
            set {
                hrmIStaff old = Manager;
                Manager = value as hrmStaff;
//                if (Manager != old)
//                    OnChanged("Manager", old, Manager);
            }
        }

        [DataSourceProperty("ManagerPlanDepartmentSource")]
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
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

        hrmIStaff fmISubject.ManagerPlanDepartment {
            get { return Manager; }
            set {
                hrmIStaff old = ManagerPlanDepartment;
                ManagerPlanDepartment = value as hrmStaff;
            }
        }

        public crmCParty SourceParty {
            get { return _SourceParty; }
            set { SetPropertyValue<crmCParty>("SourceParty", ref _SourceParty, value); }
        }

        crmIParty fmISubject.SourceParty {
            get { return SourceParty; }
            set {
                crmIParty old = SourceParty;
                SourceParty = value as crmCParty;
            }
        }

        #endregion

        #region СВОЙСТВА КЛАССА

        [RuleRequiredField]
        public fmISubjectStatus Status {
            get { return _Status; }
            set {
                SetPropertyValue<fmISubjectStatus>("Status", ref _Status, value);
                if (!IsLoading) {
                    if (value == fmISubjectStatus.CLOSED) 
                        IsClosed = true;
                    else
                        IsClosed = false;
                }
            }
        }

        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
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
                SetPropertyValue<crmContractDeal >("SourceDeal", ref _SourceDeal, value );
                if (!IsLoading) {
                    if (value != null) {
                        SourceParty = value.Customer;
                        Deals.Add(value);
                    }
                    SourceNameUpdate();
                }
            }
        }

        [RuleRequiredField(TargetCriteria = "Status == 'OPENED' && SourceType == 'SOURCE_TYPE_OTHER'")]
        public String SourceOther {
            get { return _SourceOther ; }
            set { 
                SetPropertyValue<String>("SourceOther", ref _SourceOther, value );
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

        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycWorkType AnalitycWorkType {
            get { return _AnalitycWorkType; }
            set { SetPropertyValue<fmСOrderAnalitycWorkType>("AnalitycWorkType", ref _AnalitycWorkType, value); }
        }

        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycFinanceSource AnalitycFinanceSource {
            get { return _AnalitycFinanceSource; }
            set { SetPropertyValue<fmСOrderAnalitycFinanceSource>("AnalitycFinanceSource", ref _AnalitycFinanceSource, value); }
        }

        private fmСOrderAnalitycAVT _AnalitycAVT;
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycAVT AnalitycAVT {
            get { return _AnalitycAVT; }
            set { SetPropertyValue<fmСOrderAnalitycAVT>("AnalitycAccouterType", ref _AnalitycAVT, value); }
        }
        private fmСOrderAnalitycCoperatingType _AnalitycCoperatingType;
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycCoperatingType AnalitycCoperatingType {
            get { return _AnalitycCoperatingType; }
            set { SetPropertyValue<fmСOrderAnalitycCoperatingType>("AnalitycCoperatingType", ref _AnalitycCoperatingType, value); }
        }

        private fmСOrderAnalitycOrderSource _AnalitycOrderSource;
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycOrderSource AnalitycOrderSource {
            get { return _AnalitycOrderSource; }
            set { SetPropertyValue<fmСOrderAnalitycOrderSource>("AnalitycOrderSource", ref _AnalitycOrderSource, value); }
        }

        private fmСOrderAnalitycMilitary _AnalitycMilitary;
        //        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycMilitary AnalitycMilitary {
            get { return _AnalitycMilitary; }
            set { SetPropertyValue<fmСOrderAnalitycMilitary>("AnalitycMilitary", ref _AnalitycMilitary, value); }
        }
        private fmСOrderAnalitycFedProg _AnalitycFedProg;
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycFedProg AnalitycFedProg {
            get { return _AnalitycFedProg; }
            set { SetPropertyValue<fmСOrderAnalitycFedProg>("AnalitycFedProg", ref _AnalitycFedProg, value); }
        }
        private fmСOrderAnalitycRegion _AnalitycRegion;
        [RuleRequiredField(TargetCriteria = "Status == 'OPENED'")]
        public fmСOrderAnalitycRegion AnalitycRegion {
            get { return _AnalitycRegion; }
            set { 
                SetPropertyValue<fmСOrderAnalitycRegion>("AnalitycRegion", ref _AnalitycRegion, value);
                if (!IsLoading && value != null) {
                    if (value.BigCustomers.Count == 1) {
                        AnalitycBigCustomer = value.BigCustomers[0];
                    }
                }
            }
        }
        private fmСOrderAnalitycBigCustomer _AnalitycBigCustomer;
        [RuleRequiredField(TargetCriteria="Status == 'OPENED'")]
        public fmСOrderAnalitycBigCustomer AnalitycBigCustomer {
            get { return _AnalitycBigCustomer; }
            set { 
                SetPropertyValue<fmСOrderAnalitycBigCustomer>("AnalitycBigCustomer", ref _AnalitycBigCustomer, value);
                if (!IsLoading && value != null && value.Region != null) {
                    AnalitycRegion = value.Region;
                }
            }
        }
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Int32 OrderOpendCount {
            get {
                Int32 count = 0;
                foreach (fmCOrder order in Orders) {
                    if (order.Status != fmIOrderStatus.Closed &&
                        order.Status != fmIOrderStatus.Loaded &&
                        order.Status != fmIOrderStatus.Project &&
                        order.Status != fmIOrderStatus.Deleting)
                        count++;
                }
                return count;
            }
        }

        private Int32 _OrderNumberCurrent;

        [Browsable(false)]
        public Int32 OrderNumberCurrent {
            get { return _OrderNumberCurrent; }
            set { SetPropertyValue<Int32>("OrderNumberCurrent", ref _OrderNumberCurrent, value); }
        }

        [Aggregated]
        [Association("fmSubject-TrwOrders")]
        public XPCollection<TrwOrder> TrwOrders {
            get { return GetCollection<TrwOrder>("TrwOrders"); }
        }
        #endregion

        public Int32 GetNextOrderNumber() { 
            OrderNumberCurrent++;
//            return TrwCode + "/" + OrderNumberCurrent;
            return OrderNumberCurrent;
        }

        public void DealsAdd(crmContractDeal deal) {
            if (deal.TRVType == null || deal.TRVType.TrwContractSuperType != TrwContractSuperType.DEAL_SALE)
                    return;
            TrwOrder cur_order = null;
            foreach (TrwOrder trw_order in this.TrwOrders) {
               if (trw_order.Deal == deal)
                   cur_order = trw_order;
            }
            if (cur_order == null) {
                cur_order = new TrwOrder(this.Session);
                cur_order.Subject = this;
                cur_order.Deal = deal;
            }
        }

        [Action()]
        public void RefreshDeals() {
            foreach (crmContractDeal deal in Deals) {
                DealsAdd(deal);
            }
        }
        //
        public IList<IStateMachine> GetStateMachines() {
            List<IStateMachine> result = new List<IStateMachine>();
            result.Add(new fmCSubjectSM());
            return result;
        }

    }

}