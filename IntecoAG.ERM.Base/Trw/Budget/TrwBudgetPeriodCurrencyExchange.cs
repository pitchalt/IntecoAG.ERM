using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Persistent;

using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.Trw.Budget {

    [Persistent("TrwBudgetPeriodCurrencyExchange")]
    public class TrwBudgetPeriodCurrencyExchange: XPObject {

        private TrwBudgetPeriod _TrwPeriod;
        [Association("TrwBudgetPeriod-TrwBudgetPeriodCurrencyExchange")]
        public TrwBudgetPeriod TrwPeriod {
            get { return _TrwPeriod; }
            set { SetPropertyValue<TrwBudgetPeriod>("TrwPeriod", ref _TrwPeriod, value); }
        }

        private Decimal _Rate;
        public Decimal Rate {
            get { return _Rate; }
            set { SetPropertyValue<Decimal>("Rate", ref _Rate, value); }
        }

        private csValuta _Valuta;
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<csValuta>("Valuta", ref _Valuta, value); }
        }

        public TrwBudgetPeriodCurrencyExchange(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }
}
