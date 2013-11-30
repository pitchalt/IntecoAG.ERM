using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw {

    [Persistent("TrwPeriodValue")]
    public class TrwPeriodValue : XPObject {

        private TrwPeriod _TrwPeriod;
        [Association("TrwPeriod-TrwPeriodValue")]
        public TrwPeriod TrwPeriod {
            get { return _TrwPeriod; }
            set { SetPropertyValue<TrwPeriod>("TrwPeriod", ref _TrwPeriod, value); }
        }

        private Int16 _Month;
        public Int16 Month {
            get { return _Month; }
            set { SetPropertyValue<Int16>("Month", ref _Month, value); }
        }

        public TrwPeriodValue(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
