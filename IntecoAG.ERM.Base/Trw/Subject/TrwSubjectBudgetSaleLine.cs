using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwSubjectBudgetSaleLine : TrwSubjectBudgetLine {
        public TrwSubjectBudgetSaleLine(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
