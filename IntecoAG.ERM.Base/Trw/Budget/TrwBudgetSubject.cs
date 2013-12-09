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
    public abstract class TrwBudgetSubject : TrwBudgetBase {

        private TrwSubject _TrwSubject;
        [Association("TrwSubject-TrwBudgetSubject")]
        public TrwSubject TrwSubject {
            get { return _TrwSubject; }
            set { 
                SetPropertyValue<TrwSubject>("TrwSubject", ref _TrwSubject, value);
                if (!IsLoading) {
                    NameUpdate();
                }
            }
        }

        [Association("TrwBudgetSubject-TrwBudgetValue"), Aggregated]
        public XPCollection<TrwBudgetValue> BudgetValues {
            get { return GetCollection<TrwBudgetValue>("BudgetValues"); }
        }

        private TrwBudgetMaster _BudgetMaster;
        [Association("TrwBudgetMaster-TrwBudgetSubject")]
        public TrwBudgetMaster BudgetMaster {
            get { return _BudgetMaster; }
            set { SetPropertyValue<TrwBudgetMaster>("BudgetMaster", ref _BudgetMaster, value); }
        }

        public TrwBudgetSubject(Session session) : base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}
