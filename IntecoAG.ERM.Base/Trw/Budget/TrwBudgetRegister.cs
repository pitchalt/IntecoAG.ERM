using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw.Budget {

    [Persistent("TrwBudgetRegister")]
    public class TrwBudgetRegister : XPObject {

        private TrwBudgetPeriodValue _TrwPeriodValue;
        public TrwBudgetPeriodValue TrwPeriodValue {
            get { return _TrwPeriodValue; }
            set { SetPropertyValue<TrwBudgetPeriodValue>("TrwPeriodValue", ref _TrwPeriodValue, value); }
        }

        public TrwBudgetRegister(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
