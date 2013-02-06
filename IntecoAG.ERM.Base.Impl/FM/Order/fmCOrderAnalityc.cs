using System;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.FM.Order {

    [Persistent("fmOrderAnalityc")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
    public abstract class fmCOrderAnalityc : csCCodedComponent {
        public fmCOrderAnalityc(Session session) 
            : base(session) {
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycAccouterType : fmCOrderAnalityc {
        public fm—OrderAnalitycAccouterType(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycAVT : fmCOrderAnalityc {
        public fm—OrderAnalitycAVT(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycWorkType : fmCOrderAnalityc {
        public fm—OrderAnalitycWorkType(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycOrderSource : fmCOrderAnalityc {
        public fm—OrderAnalitycOrderSource(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycFinanceSource : fmCOrderAnalityc {
        public fm—OrderAnalitycFinanceSource(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycMilitary : fmCOrderAnalityc {
        public fm—OrderAnalitycMilitary(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycOKVED : fmCOrderAnalityc {
        public fm—OrderAnalitycOKVED(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycFedProg : fmCOrderAnalityc {
        public fm—OrderAnalitycFedProg(Session session)
            : base(session) {
        }
    }
}