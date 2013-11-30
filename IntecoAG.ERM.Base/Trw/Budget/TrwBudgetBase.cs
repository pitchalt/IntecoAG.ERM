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
    public abstract class TrwBudgetBase : BaseObject {

        [Persistent("Name")]
        protected String _Name;
        [PersistentAlias("_Name")]
        public String Name {
            get { return _Name; }
//            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        protected abstract void NameUpdate();

        [Browsable(false)]
        [Association("TrwBudgetBase-TrwBudgetLineBase"), Aggregated]
        public XPCollection<TrwBudgetLineBase> Lines {
            get {
                return GetCollection<TrwBudgetLineBase>("Lines");
            }
        }

        [Persistent("TrwPeriod")]
        protected TrwPeriod _TrwPeriod;
        [PersistentAlias("_TrwPeriod")]
        public TrwPeriod TrwPeriod {
            get { return _TrwPeriod; }
//            set { SetPropertyValue<TrwPeriod>("TrwPeriod", ref _TrwPeriod, value); }
        }

        public abstract void Calculate(IObjectSpace os);

        public TrwBudgetBase(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            NameUpdate();
        }
    }

}
