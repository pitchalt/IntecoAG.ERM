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
    [MapInheritance(MapInheritanceType.OwnTable)]
    public abstract class FmFinPlanDoc : FmFinPlanBase {
        public FmFinPlanDoc(Session session): base(session) {}

        public override void AfterConstruction() {
            base.AfterConstruction();
            _Journal = new FmFinPlanJournal(this.Session);
            _Journal.FinPlanDoc = this;
        }

//        private FmFinPlanPlan _FinPlan;
//        [Association("FmFinPlanPlan-FmFinPlanDoc")]
        [NonPersistent]
        public abstract FmFinPlanPlan FinPlan {
            get;
            //get { return _FinPlan; }
            //set {
            //    SetPropertyValue<FmFinPlanPlan>("FinPlan", ref _FinPlan, value);
            //    if (!IsLoading) {
            //        Journal.FinPlan = value;
            //    }
            //}
        }
    }

}
