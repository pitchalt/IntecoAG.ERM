using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM.FinPlan {

    [NavigationItem("FinPlan")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class FmFinPlanSubjectFull : FmFinPlanPlan {
        public FmFinPlanSubjectFull(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private fmCSubject _Subject;
        [RuleRequiredField]
        public fmCSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading) {
                    Journal.SubjectSet(value);
                    foreach (var doc in FinPlanSubjectFullDocs) {
                        doc.Subject = value;
                    }
                    if (value != null)
                        CodeSet("SFP." + value.Code);
                    else
                        CodeSet("SFP.Null");
                }
            } 
        }

        [Association("FmFinPlanSubjectFull-FmFinPlanSubjectFullDoc"), Aggregated]
        public XPCollection<FmFinPlanSubjectFullDoc> FinPlanSubjectFullDocs {
            get { return GetCollection<FmFinPlanSubjectFullDoc>("FinPlanSubjectFullDocs"); }
        }

        [Browsable(false)]
        public override XPCollection<FmFinPlanDoc> FinPlanDocs {
            get { return new XPCollection<FmFinPlanDoc>(FinPlanSubjectFullDocs); }
        }
    }

}
