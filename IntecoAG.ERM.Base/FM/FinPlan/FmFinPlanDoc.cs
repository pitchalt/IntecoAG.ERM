using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;

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
        protected abstract FmFinPlanPlan FinPlan { get; }

        [Association("FmFinPlanDoc-FmFinPlanDocLine"), Aggregated]
        public XPCollection<FmFinPlanDocLine> Lines {
            get { return GetCollection<FmFinPlanDocLine>("Lines"); }
        }

        public csValuta Valuta;

        public csNDSRate NdsRate;

    }

}
