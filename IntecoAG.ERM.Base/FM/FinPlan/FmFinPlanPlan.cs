using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Accounting;

namespace IntecoAG.ERM.FM.FinPlan {

    [Persistent("FmFinPlanPlan")]
//    [MapInheritance(MapInheritanceType.OwnTable)]
    public abstract class FmFinPlanPlan : FmFinPlanBase {
        public FmFinPlanPlan(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
            //_JournalPlan0 = new FmJournal(this.Session);
            //_JournalPlan0.FinPlanSet(this);
        }

        protected FmAccountingFinancial _AccountingFact;
//        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public FmAccountingFinancial AccountingFact {
            get { return _AccountingFact; }
            set { SetPropertyValue<FmAccountingFinancial>("AccountingFact", ref _AccountingFact, value); }
        }

        protected FmAccountingContract _AccountingContract;
//        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public FmAccountingContract AccountingContract {
            get { return _AccountingContract; }
            set { SetPropertyValue<FmAccountingContract>("AccountingContract", ref _AccountingContract, value); }
        }

        //[Persistent("Journal")]
        //[Aggregated]
        //protected FmJournal _Journal;
        //[PersistentAlias("_Journal")]
        //public FmJournal Journal {
        //    get { return _Journal; }
        //}
        //[Persistent("JournalPlanFull")]
        //[Aggregated]
        //protected FmJournal _JournalPlanFull;
        //[PersistentAlias("_JournalPlanFull")]
        //public FmJournal JournalPlanFull {
        //    get { return _JournalPlanFull; }
        //}
       // [Persistent("JournalPlan0")]
       // [Aggregated]
       // protected FmJournal _JournalPlan0;
       // [PersistentAlias("_JournalPlan0")]
       // public FmJournal JournalPlan0 {
       //     get { return _JournalPlan0; }
       // }
       ////[Association("FmFinPlanPlan-FmFinPlanDoc"), Aggregated]
//        [Aggregated]
//        public abstract XPCollection<FmFinPlanDoc> FinPlanDocs { 
//            get ;
////            get { return GetCollection<FmFinPlanDoc>("FinPlanDocs"); }
//        } 

    }

}
