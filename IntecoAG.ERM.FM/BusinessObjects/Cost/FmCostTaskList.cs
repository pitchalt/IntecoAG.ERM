using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.Cost {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class FmCostTaskList : FmCostTask {

        [Association("FmCostTaskList-FmCostTask"), Aggregated]
        public XPCollection<FmCostTask> SubTasks {
            get { return GetCollection<FmCostTask>("SubTasks"); }
        }

        public FmCostTaskList(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private IBindingList _TreeChildren;
        protected override IBindingList TreeChildren {
            get {
                if (_TreeChildren == null)
                    _TreeChildren = new BindingList<FmCostTask>(SubTasks);
                return _TreeChildren; 
            }
        }
    }

}
