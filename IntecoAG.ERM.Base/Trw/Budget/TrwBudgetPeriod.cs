using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.Trw.Subject;
//
namespace IntecoAG.ERM.Trw.Budget {

    [NavigationItem("Trw")]
    [Persistent("TrwBudgetPeriod")]
    [DefaultProperty("Name")]
    public class TrwBudgetPeriod : BaseObject {

        private Int16 _Year;
        public Int16 Year {
            get { return _Year; }
            set { SetPropertyValue<Int16>("Year", ref _Year, value); }
        }

        public String Name {
            get { return "Á‏הזועû ÒÐÂ חא " + Year.ToString() + "ד."; }
        }

        [Association("TrwBudgetPeriod-TrwSubject"), Aggregated]
        public XPCollection<TrwSubject> TrwSubjects {
            get { return GetCollection<TrwSubject>("TrwSubjects"); }
        }

        [Association("TrwBudgetPeriod-TrwBudgetPeriodValue"), Aggregated]
        public XPCollection<TrwBudgetPeriodValue> TrwBudgetPeriodValues {
            get { return GetCollection<TrwBudgetPeriodValue>("TrwBudgetPeriodValues"); }
        }

        public TrwBudgetPeriod(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            InitPeriodValues();
        }

        public void InitPeriodValues() {
            for (short i = 0; i < 14; i++) {
                TrwBudgetPeriodValue period_value = TrwBudgetPeriodValues.FirstOrDefault(x => x.Month == i);
                if (period_value == null) {
                    period_value = new TrwBudgetPeriodValue(this.Session);
                    period_value.Month = i;
                    TrwBudgetPeriodValues.Add(period_value);
                }
            }
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }
    }

}
