using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
//
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Budget {

    [Persistent("TrwBudgetValue")]
    public abstract class TrwBudgetValue : XPObject {

        private TrwBudgetBase _BudgetBase;
        [Association("TrwBudgetBase-TrwBudgetValue")]
        public TrwBudgetBase BudgetBase {
            get { return _BudgetBase; }
            set { SetPropertyValue<TrwBudgetBase>("BudgetBase", ref _BudgetBase, value); }
        }

        private TrwBudgetPeriodValue _PeriodValue;
        [Association("TrwBudgetPeriodValue-TrwBudgetValue")]
        public TrwBudgetPeriodValue PeriodValue {
            get { return _PeriodValue; }
            set { SetPropertyValue<TrwBudgetPeriodValue>("PeriodValue", ref _PeriodValue, value); }
        }

        private TrwBudgetKey _BudgetKey;
        [Association("TrwBudgetKey-TrwBudgetValue")]
        public TrwBudgetKey BudgetKey {
            get { return _BudgetKey; }
            set { SetPropertyValue<TrwBudgetKey>("BudgetKey", ref _BudgetKey, value); }
        }

        private csValuta _ObligationValuta;
        public csValuta ObligationValuta {
            get { return _ObligationValuta; }
            set { SetPropertyValue<csValuta>("ObligationValuta", ref _ObligationValuta, value); }
        }

        private csValuta _PaymentValuta;
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set { SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value); }
        }

        private Decimal _Count;
        public Decimal Count {
            get { return _Count; }
            set {
                value = Decimal.Round(value, 4);
                SetPropertyValue<Decimal>("Count", ref _Count, value);
            }
        }

        private Decimal _Price;
        public Decimal Price {
            get { return _Price; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("Price", ref _Price, value);
            }
        }

        private Decimal _SummCost;
        public Decimal SummCost {
            get { return _SummCost; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("SummCost", ref _SummCost, value);
            }
        }

        private Decimal _SummVat;
        public Decimal SummVat {
            get { return _SummVat; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("SummVat", ref _SummVat, value);
            }
        }

        private Decimal _SummAll;
        public Decimal SummAll {
            get { return _SummAll; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("SummAll", ref _SummAll, value); 
            }
        }

        public TrwBudgetValue(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
