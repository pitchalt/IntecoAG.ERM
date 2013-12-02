using System;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
namespace IntecoAG.ERM.Trw.Budget {

    [Persistent("TrwBudgetPeriodValue")]
    public class TrwBudgetPeriodValue : XPObject {

        private TrwBudgetPeriod _TrwPeriod;
        [Association("TrwBudgetPeriod-TrwBudgetPeriodValue")]
        public TrwBudgetPeriod TrwPeriod {
            get { return _TrwPeriod; }
            set { SetPropertyValue<TrwBudgetPeriod>("TrwPeriod", ref _TrwPeriod, value); }
        }

        [Association("TrwBudgetPeriodValue-TrwBudgetValue"), Aggregated]
        public XPCollection<TrwBudgetValue> BudgetValues {
            get { return GetCollection<TrwBudgetValue>("BudgetValues"); }
        }


        private Int16 _Month;
        public Int16 Month {
            get { return _Month; }
            set { SetPropertyValue<Int16>("Month", ref _Month, value); }
        }

        public TrwBudgetPeriodValue(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
