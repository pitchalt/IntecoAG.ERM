using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw.Budget {
    [Persistent("TrwBudget")]
    public class TrwBudgetBudget : BaseObject {

        private String _Code;
        public String Code {
            get { return _Code; }
        }

        public TrwBudgetBudget(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
