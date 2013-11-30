using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw.Budget;

namespace IntecoAG.ERM.Trw.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwSubjectBudgetLine : TrwBudgetLineBase {

        private fmCSubject _Subject;
        public fmCSubject Subject {
            get { return _Subject; }
            set { SetPropertyValue<fmCSubject>("Subject", ref _Subject, value); }
        }

        private fmCOrder _Order;
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        public TrwSubjectBudgetLine(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
