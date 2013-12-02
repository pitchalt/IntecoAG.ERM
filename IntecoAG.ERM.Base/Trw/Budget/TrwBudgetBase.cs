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

    [Persistent("TrwBudget")]
    public abstract class TrwBudgetBase : BaseObject {

        [Persistent("Name")]
        protected String _Name;
        [PersistentAlias("_Name")]
        public String Name {
            get { return _Name; }
//            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        protected abstract void NameUpdate();

        [Association("TrwBudgetBase-TrwBudgetKey"), Aggregated]
        public XPCollection<TrwBudgetKey> Keys {
            get {
                return GetCollection<TrwBudgetKey>("Keys");
            }
        }

        [Association("TrwBudgetBase-TrwBudgetValue"), Aggregated]
        public XPCollection<TrwBudgetValue> BudgetValues {
            get {
                return GetCollection<TrwBudgetValue>("BudgetValues");
            }
        }

        [Persistent("TrwPeriod")]
        protected TrwBudgetPeriod _TrwPeriod;
        [PersistentAlias("_TrwPeriod")]
        public TrwBudgetPeriod TrwPeriod {
            get { return _TrwPeriod; }
//            set { SetPropertyValue<TrwPeriod>("TrwPeriod", ref _TrwPeriod, value); }
        }

        public abstract void Calculate(IObjectSpace os);

        public TrwBudgetBase(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            NameUpdate();
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }
    }

}
