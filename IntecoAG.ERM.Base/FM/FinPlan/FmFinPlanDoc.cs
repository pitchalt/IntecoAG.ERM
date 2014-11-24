using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

//using IntecoAG.ERM.FM.FinPlan.View;

namespace IntecoAG.ERM.FM.FinPlan {

    [Persistent("FmFinPlanDoc")]
//    [MapInheritance(MapInheritanceType.OwnTable)]
    public abstract class FmFinPlanDoc : FmFinPlanBase {
        public FmFinPlanDoc(Session session): base(session) {}

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        //[Persistent("Journal")]
        //[Aggregated]
        //protected FmJournal _Journal;
        //[PersistentAlias("_Journal")]
        //public FmJournal Journal {
        //    get { return _Journal; }
        //}


//        private FmFinPlanPlan _FinPlan;
//        [Association("FmFinPlanPlan-FmFinPlanDoc")]

//        [NonPersistent]
        public abstract FmFinPlanPlan FinPlan { get; }
    }

}
