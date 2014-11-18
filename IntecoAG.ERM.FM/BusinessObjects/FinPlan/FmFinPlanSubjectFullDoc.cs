using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM.FinPlan {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class FmFinPlanSubjectFullDoc : FmFinPlanDoc {
        public FmFinPlanSubjectFullDoc(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private fmCSubject _Subject;
        public fmCSubject Subject {
            get { return _Subject; }
            set {
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading) {
                    Journal.SubjectSet(value);
                }
            }
        }

        private FmFinPlanSubjectFull _FinPlanSubjectFull;
        [Association("FmFinPlanSubjectFull-FmFinPlanSubjectFullDoc")]
        public FmFinPlanSubjectFull FinPlanSubjectFull {
            get { return _FinPlanSubjectFull; }
            set {
                SetPropertyValue<FmFinPlanSubjectFull>("FinPlanSubjectFull", ref _FinPlanSubjectFull, value);
                if (!IsLoading) {
                    Journal.FinPlan = value;
                }
            }
        }

        [Browsable(false)]
        public override FmFinPlanPlan FinPlan {
            get {
                return FinPlanSubjectFull;
            }
        }
    }

}
