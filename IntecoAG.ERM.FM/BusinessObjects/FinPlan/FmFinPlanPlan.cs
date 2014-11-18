using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.FinPlan {

    [Persistent("FmFinPlanPlan")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public abstract class FmFinPlanPlan : FmFinPlanBase {
        public FmFinPlanPlan(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
            _Journal = new FmFinPlanJournal(this.Session);
            _Journal.FinPlan = this;
        }

        //[Association("FmFinPlanPlan-FmFinPlanDoc"), Aggregated]
        [Aggregated]
        public abstract XPCollection<FmFinPlanDoc> FinPlanDocs { 
            get ;
//            get { return GetCollection<FmFinPlanDoc>("FinPlanDocs"); }
        } 

    }

}
