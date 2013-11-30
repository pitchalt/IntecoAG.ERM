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
namespace IntecoAG.ERM.Trw {

    [NavigationItem("Trw")]
    [Persistent("TrwPeriod")]
    [DefaultProperty("Name")]
    public class TrwPeriod : BaseObject {

        private Int16 _Year;
        public Int16 Year {
            get { return _Year; }
            set { SetPropertyValue<Int16>("Year", ref _Year, value); }
        }

        public String Name {
            get { return "Á‏הזועû ÒÐÂ חא " + Year.ToString() + "ד."; }
        }

        [Association("TrwPeriod-TrwSubject"), Aggregated]
        public XPCollection<TrwSubject> TrwSubjects {
            get { return GetCollection<TrwSubject>("TrwSubjects"); }
        }

        [Association("TrwPeriod-TrwPeriodValue"), Aggregated]
        public XPCollection<TrwPeriodValue> TrwPeriodValues {
            get { return GetCollection<TrwPeriodValue>("TrwPeriodValues"); }
        }

        public TrwPeriod(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            InitPeriodValues();
        }

        public void InitPeriodValues() {
            for (short i = 0; i < 14; i++) {
                TrwPeriodValue period_value = TrwPeriodValues.FirstOrDefault(x => x.Month == i);
                if (period_value == null) {
                    period_value = new TrwPeriodValue(this.Session);
                    period_value.Month = i;
                    TrwPeriodValues.Add(period_value);
                }
            }
        }
    }

}
