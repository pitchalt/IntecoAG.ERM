using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class FmFinPlanSubjectDoc : FmFinPlanDoc {
        public FmFinPlanSubjectDoc(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        [PersistentAlias("FinPlanSubject.Subject")]
        public fmCSubject Subject {
            get { return FinPlanSubject.Subject; }
        }

        private FmFinPlanSubject _FinPlanSubject;
//        [RuleRequiredField]
        [Association("FmFinPlanSubject-FmFinPlanSubjectDoc")]
        public FmFinPlanSubject FinPlanSubject {
            get { return _FinPlanSubject; }
            set {
                SetPropertyValue<FmFinPlanSubject>("FinPlanSubject", ref _FinPlanSubject, value);
            }
        }

        private fmCOrder _Order;
        [DataSourceCriteria("Subject == This.Subject")]
        [RuleRequiredField]
        public fmCOrder Order {
            get { return _Order; }
            set {
                SetPropertyValue<fmCOrder>("Order", ref _Order, value);
            }
        }

        protected override FmFinPlanPlan FinPlan {
            get { return FinPlanSubject; }
        }

        protected override CriteriaOperator OperationsCriteria {
            get {
                return XPQuery<FmJournalOperation>.TransformExpression(this.Session,
                    x => x.Journal == Journal ||
                         x.Journal == FinPlanSubject.Journal ||
                        x.Journal == FinPlanSubject.JournalPlanYear ||
                        x.Journal == FinPlanSubject.AccountingFact.Journal ||
                        x.Journal == FinPlanSubject.AccountingContract.Journal
                        );
            }
        }
    }

}
