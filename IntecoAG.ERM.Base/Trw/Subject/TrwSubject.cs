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

        private fmCSubject _Subject;
        public fmCSubject Subject {
            get { return _Subject; }
            set { SetPropertyValue<fmCSubject>("Subject", ref _Subject, value); }
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

        public TrwSubject(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
