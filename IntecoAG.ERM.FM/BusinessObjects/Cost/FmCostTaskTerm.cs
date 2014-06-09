using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.Cost {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class FmCostTaskTerm : FmCostTask {

        public FmCostTaskTerm(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private static IBindingList _TreeChildren = new BindingList<FmCostTask>();
        protected override IBindingList TreeChildren {
            get {
                return _TreeChildren;
            }
        }

    }

}
