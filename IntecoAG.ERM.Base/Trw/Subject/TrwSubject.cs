using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Budget;
//
namespace IntecoAG.ERM.Trw.Subject {

    [NavigationItem("Trw")]
    [Persistent("TrwSubject")]
    [Appearance("", AppearanceItemType.ViewItem, "Subject != null", TargetItems="NameInternal,Subjects", Enabled=false)]
    public class TrwSubject : BaseObject {

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
        private fmCSubject _Subject;
        public fmCSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading) {
                    if (value != null) {
                        Subjects.Clear();
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
            //DealOtherSale = new TrwContract(Session);
            //DealOtherSale.ContractSuperType = Contract.TrwContractSuperType.DEAL_SALE;
            //DealOtherBay = new TrwContract(Session);
            //DealOtherBay.ContractSuperType = Contract.TrwContractSuperType.DEAL_BAY;
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }

        public void UpdateDeals() {
            foreach (TrwSubjectDealSale deal in DealsSale) {
                deal.TrwContractOrdersUpdate();
            }
        }
    }
}
