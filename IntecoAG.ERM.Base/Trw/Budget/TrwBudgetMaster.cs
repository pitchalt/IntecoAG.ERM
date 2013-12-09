using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.Trw.Subject;

namespace IntecoAG.ERM.Trw.Budget {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class TrwBudgetMaster : TrwBudgetBase {

        protected TrwBudgetPeriod _BudgetPeriod;
        //        [PersistentAlias("_TrwPeriod")]
        [Association("TrwBudgetPeriod-TrwBudgetMaster")]
        public TrwBudgetPeriod BudgetPeriod {
            get { return _BudgetPeriod; }
            set { 
                SetPropertyValue<TrwBudgetPeriod>("BudgetPeriod", ref _BudgetPeriod, value);
                if (!IsLoading) {
                    NameUpdate();
                }
            }
        }

        [Association("TrwBudgetMaster-TrwBudgetSubject"), Aggregated]
        public XPCollection<TrwBudgetSubject> BudgetSubjects {
            get { return GetCollection<TrwBudgetSubject>("BudgetSubjects"); }
        }

        [Association("TrwBudgetMaster-TrwBudgetKey"), Aggregated]
        public XPCollection<TrwBudgetKey> BudgetKeys {
            get {
                return GetCollection<TrwBudgetKey>("BudgetKeys");
            }
        }

        [Association("TrwBudgetMaster-TrwBudgetValue"), Aggregated]
        public XPCollection<TrwBudgetValue> BudgetValues {
            get {
                return GetCollection<TrwBudgetValue>("BudgetValues");
            }
        }

        public TrwBudgetMaster(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }

}
