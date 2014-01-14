using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Budget;
//
namespace IntecoAG.ERM.Trw.Subject {

    public enum TrwSubjectStatus {
        TRW_SUBJECT_INIT = 1,
        TRW_SUBJECT_CONF_SUBJECT_LIST = 2,
        TRW_SUBJECT_CONF_DEAL_LIST = 3,
        TRW_SUBJECT_DELETE = 16
    }

    public enum TrwSubjectType { 
        TRW_SUBJECT_TYPE_UNDEFINED = 1,
        TRW_SUBJECT_TYPE_REAL = 2,
        TRW_SUBJECT_TYPE_CONSOLIDATE = 3
    }

    [NavigationItem("Trw")]
    [Persistent("TrwSubject")]
    [Appearance(null, AppearanceItemType.ViewItem, "SubjectType == 'TRW_SUBJECT_TYPE_REAL'", 
        TargetItems = "SubjectType,NameInternal,Subjects", Enabled = false)]
    [Appearance(null, AppearanceItemType.ViewItem, "SubjectType == 'TRW_SUBJECT_TYPE_CONSOLIDATE'",
        TargetItems = "SubjectType,Subject", Enabled = false)]
    [Appearance(null, AppearanceItemType.ViewItem, "SubjectType == 'TRW_SUBJECT_TYPE_UNDEFINED'",
        TargetItems = "*,SubjectType", Enabled = false)]
    [Appearance(null, AppearanceItemType.ViewItem, "Status != 'TRW_SUBJECT_INIT'", TargetItems = "SubjectType,Subject,NameInternal,Subjects", Enabled = false)]
    [Appearance(null, AppearanceItemType.ViewItem, "Status != 'TRW_SUBJECT_CONF_SUBJECT_LIST'", TargetItems = "DealsBay,DealsSale", Enabled = false)]
    [Appearance(null, AppearanceItemType.LayoutItem, "Status != 'TRW_SUBJECT_CONF_DEAL_LIST'", TargetItems = "SubjectItems", Visibility = ViewItemVisibility.Hide)]
    public class TrwSubject : BaseObject, IStateMachineProvider {

        private TrwSubjectStatus _Status;
        public TrwSubjectStatus Status {
            get { return _Status; }
            set { SetPropertyValue<TrwSubjectStatus>("Status", ref _Status, value); }
        }

        private TrwSubjectType _SubjectType;
        [ImmediatePostData]
        [RuleValueComparison(null, "Save;Approve", ValueComparisonType.NotEquals, TrwSubjectType.TRW_SUBJECT_TYPE_UNDEFINED)]
        public TrwSubjectType SubjectType {
            get { return _SubjectType; }
            set { 
                SetPropertyValue<TrwSubjectType>("SubjectType", ref _SubjectType, value);
                if (!IsLoading) {
                    if (value == TrwSubjectType.TRW_SUBJECT_TYPE_REAL) {
                        NameInternal = null;
                        Subjects.Clear();
                    }
                    if (value == TrwSubjectType.TRW_SUBJECT_TYPE_CONSOLIDATE) {
                        Subject = null;
                    }
                }
            }
        }

        private TrwBudgetPeriod _Period;
        [RuleRequiredField]
        [VisibleInDetailView(true)]
        [Association("TrwBudgetPeriod-TrwSubject")]
        public TrwBudgetPeriod Period {
            get { return _Period; }
            set { 
                SetPropertyValue<TrwBudgetPeriod>("Period", ref _Period, value);
                if (!IsLoading) {
                    if (value != null) {
                        CodeSet(value.Code + value.SubjectNumberNextGet().ToString("D3"));
                    }
                }
            }
        }

        [Persistent("Code")]
        [Size(16)]
        private String _Code;
        [PersistentAlias("_Code")]
        public String Code {
            get { return _Code; }
        }
        public void CodeSet(String code) {
            SetPropertyValue<String>("Code", ref _Code, code);
        }
        //
        private IList<TrwSubjectItem> _SubjectItems;
        public IList<TrwSubjectItem> SubjectItems {
            get {
                if (_SubjectItems == null) {
                    _SubjectItems = new List<TrwSubjectItem>();
                    TrwSubjectItemSaleDeals sale_item = new TrwSubjectItemSaleDeals(this.Session);
                    sale_item.Subject = this;
                    _SubjectItems.Add(sale_item);
                    TrwSubjectItemBayDeals bay_item = new TrwSubjectItemBayDeals(this.Session);
                    bay_item.Subject = this;
                    _SubjectItems.Add(bay_item);
                }
                return _SubjectItems; 
            }
        }
        //
        private fmCSubject _Subject;
        [RuleRequiredField(null, "Save;Approve", TargetCriteria = "SubjectType == 'TRW_SUBJECT_TYPE_REAL'")]
        public fmCSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading) {
                    if (value != null) {
                        Subjects.Add(value);
                    }
                }
            }
        }

        public String Name {
            get {
                if (Subject != null) {
                    return Subject.Name;
                }
                else {
                    return NameInternal;
                }
            }
        }

        private String _NameInternal;
        [RuleRequiredField(null, "Save;Approve", TargetCriteria = "SubjectType == 'TRW_SUBJECT_TYPE_CONSOLIDATE'")]
        public String NameInternal {
            get { return _NameInternal; }
            set { SetPropertyValue<String>("NameInternal", ref _NameInternal, value); }
        }

        //[Association("TrwSubject-fmCSubject")]
        //public XPCollection<fmCSubject> Subjects {
        //    get { return GetCollection<fmCSubject>("Subjects"); }
        //}

        [Aggregated]
        [Browsable(false)]
        [Association("TrwSubject-TrwSubjectSubjectLink")]
        public XPCollection<TrwSubjectSubjectLink> SubjectLinks {
            get { return GetCollection<TrwSubjectSubjectLink>("SubjectLinks"); }
        }

        [ManyToManyAlias("SubjectLinks", "FmSubject")]
        public IList<fmCSubject> Subjects {
            get {
                return GetList<fmCSubject>("Subjects");
            }
        }

        private Int32 _SubjectDealNumberCurrent;
        [Browsable(false)]
        public Int32 SubjectDealNumberCurrent {
            get { return _SubjectDealNumberCurrent; }
            set { SetPropertyValue<Int32>("SubjectDealNumberCurrent", ref _SubjectDealNumberCurrent, value); }
        }
        public Int32 SubjectDealNumberGet() { 
            SubjectDealNumberCurrent++;
            return SubjectDealNumberCurrent;
        }

        private TrwContract _DealOtherSale;
        [Browsable(false)]
        public TrwContract DealOtherSale {
            get { return _DealOtherSale; }
            set { SetPropertyValue<TrwContract>("DealOtherSale", ref _DealOtherSale, value); }
        }

        private TrwContract _DealOtherBay;
        [Browsable(false)]
        public TrwContract DealOtherBay {
            get { return _DealOtherBay; }
            set { SetPropertyValue<TrwContract>("DealOtherBay", ref _DealOtherBay, value); }
        }

//        public XPCollection<TrwBudgetBudget> SubjectBudgets {
//            get { return GetCollection<TrwBudgetBudget>("SubjectBudgets"); }
//        }

        [Association("TrwSubject-TrwSubjectDealBay"), Aggregated]
        public XPCollection<TrwSubjectDealBay> DealsBay {
            get { return GetCollection<TrwSubjectDealBay>("DealsBay"); }
        }

        [Association("TrwSubject-TrwSubjectDealSale"), Aggregated]
        public XPCollection<TrwSubjectDealSale> DealsSale {
            get { return GetCollection<TrwSubjectDealSale>("DealsSale"); }
        }

        [Association("TrwSubject-TrwBudgetSubject"), Aggregated]
        public XPCollection<TrwBudgetSubject> Budgets {
            get { return GetCollection<TrwBudgetSubject>("Budgets"); }
        }


        public TrwSubject(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Status = TrwSubjectStatus.TRW_SUBJECT_INIT;
            SubjectType = TrwSubjectType.TRW_SUBJECT_TYPE_UNDEFINED;
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }

        //public void UpdateDeals() {
        //    foreach (TrwSubjectDealSale deal in DealsSale) {
        //        deal.TrwContractOrdersUpdate();
        //    }
        //}

        public IList<IStateMachine> GetStateMachines() {
            IList<IStateMachine> sml = new List<IStateMachine>(1);
            sml.Add(new TrwSubjectSM());
            return sml;
        }
    }

}
