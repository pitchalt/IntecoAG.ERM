using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Subject;
//
namespace IntecoAG.ERM.Trw.Subject {

    [NavigationItem("Trw")]
    [Persistent("TrwSubject")]
    public class TrwSubject : BaseObject {

        private TrwPeriod _Period;

        [RuleRequiredField]
        [VisibleInDetailView(true)]
        [Association("TrwPeriod-TrwSubject")]
        public TrwPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<TrwPeriod>("Period", ref _Period, value); }
        }

        private fmCSubject _Subject;
        [RuleRequiredField]
        public fmCSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading) {
                    DealOtherSale.Subject = value;
                    DealOtherBay.Subject = value;
                }
            }
        }

        public TrwContract DealOtherSale;
        public TrwContract DealOtherBay;
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

        [Association("TrwSubject-TrwSubjectBudget"), Aggregated]
        public XPCollection<TrwSubjectBudget> Budgets {
            get { return GetCollection<TrwSubjectBudget>("Budgets"); }
        }

        public TrwSubject(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            DealOtherSale = new TrwContract(Session);
            DealOtherSale.ContractSuperType = Contract.TrwContractSuperType.DEAL_SALE;
            DealOtherBay = new TrwContract(Session);
            DealOtherBay.ContractSuperType = Contract.TrwContractSuperType.DEAL_BAY;
        }
    }
}
