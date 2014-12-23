using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class FmFinPlanSubjectDoc : FmFinPlanDoc, csIImportSupport {
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
        [DataSourceCriteria("Subject == '@This.Subject'")]
        [Appearance("", AppearanceItemType.ViewItem, "Order != Null", Enabled=false)]
        [RuleRequiredField]
        public fmCOrder Order {
            get { return _Order; }
            set {
                if (!IsLoading)
                    new InvalidOperationException("Изменить нельзя если уже задано");
                SetPropertyValue<fmCOrder>("Order", ref _Order, value);
                if (!IsLoading) {
                    Journal.OrderSet(value);
                    Operations.Criteria = this.OperationsCriteria;
                    Operations.Reload();
//                    OnChanged("Operations");
                }
            }
        }

        protected override FmFinPlanPlan FinPlan {
            get { return FinPlanSubject; }
        }

        [PersistentAlias("Journal.Operations")]
        [Aggregated]
        public XPCollection<FmJournalOperation> DocOperations {
            get {
                return Journal.Operations;
            }
        }
        public void DocOperationsClean() {
            Session.Delete(DocOperations);
        }

        protected override CriteriaOperator OperationsCriteria {
            get {
                return 
                    XPQuery<FmJournalOperation>.TransformExpression(this.Session,
                    x => 
                        x.Subject == FinPlanSubject.Subject &&
                        x.Order == this.Order && (
                            x.Journal == Journal ||
                            x.Journal == FinPlanSubject.Journal ||
                            x.Journal == FinPlanSubject.JournalPlanYear ||
                            x.Journal == FinPlanSubject.AccountingFact.Journal ||
                            x.Journal == FinPlanSubject.AccountingContract.Journal
                        )
                        );
            }
        }

        public abstract void Import(IObjectSpace os, string file_name);
    }

}
