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


        [Aggregated]
        [Browsable(false)]
        [Persistent("TopLine")]
        protected FmFinPlanDocLine _TopLine;

        [PersistentAlias("_TopLine.SubLines")]
        [Aggregated]
        public XPCollection<FmFinPlanDocLine> SubLines {
            get { return _TopLine != null ? _TopLine.SubLines : null; }
        }

        [Association("FmFinPlanDoc-FmFinPlanDocLine"), Aggregated]
        [Browsable(false)]
        public XPCollection<FmFinPlanDocLine> Lines {
            get { return GetCollection<FmFinPlanDocLine>("Lines"); }
        }

        public csValuta Valuta;

        public csNDSRate NdsRate;

        public void Clean() {
            foreach (var line in Lines) {
                line.Clean();
            }
        }

    }

}
