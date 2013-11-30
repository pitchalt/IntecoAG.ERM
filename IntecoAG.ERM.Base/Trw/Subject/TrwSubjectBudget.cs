using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.Trw.Budget;

namespace IntecoAG.ERM.Trw.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class TrwSubjectBudget : TrwBudgetBase {

        private TrwSubject _TrwSubject;
        [Association("TrwSubject-TrwSubjectBudget")]
        public TrwSubject TrwSubject {
            get { return _TrwSubject; }
            set { 
                SetPropertyValue<TrwSubject>("TrwSubject", ref _TrwSubject, value);
                if (!IsLoading) {
                    if (TrwSubject != null) {
                        _TrwPeriod = TrwSubject.Period;
                    }
                    NameUpdate();
                }
            }
        }

        public TrwSubjectBudget(Session session) : base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}
