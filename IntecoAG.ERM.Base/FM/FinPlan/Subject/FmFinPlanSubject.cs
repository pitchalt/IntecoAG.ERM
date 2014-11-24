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
using IntecoAG.ERM.XafExt;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    [NavigationItem("FinPlan")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class FmFinPlanSubject : FmFinPlanPlan {
        public FmFinPlanSubject(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
            _Journal = new FmJournal(this.Session);
            _Journal.JournalTypeAccountingSet(JournalTypeAccounting.FM_JTA_FINANCIAL);
            _Journal.JournalTypeLegalSet(JournalTypeLegal.FM_JTL_COMPANY);
            _Journal.JournalTypeObjectSet(JournalTypeObject.FM_JTO_SUBJECT);
            _Journal.JournalTypePeriodSet(JournalTypePeriod.FM_JTP_FULL);
            _Journal.JournalTypeSourceSet(JournalTypeSource.FM_JTS_FINPLAN);
            //
            _JournalPlanYear = new FmJournal(this.Session);
            _JournalPlanYear.JournalTypeAccountingSet(JournalTypeAccounting.FM_JTA_FINANCIAL);
            _JournalPlanYear.JournalTypeLegalSet(JournalTypeLegal.FM_JTL_COMPANY);
            _JournalPlanYear.JournalTypeObjectSet(JournalTypeObject.FM_JTO_SUBJECT);
            _JournalPlanYear.JournalTypePeriodSet(JournalTypePeriod.FM_JTP_YEAR);
            _JournalPlanYear.JournalTypeSourceSet(JournalTypeSource.FM_JTS_FINPLAN);
        }

        private fmCSubject _Subject;
        [RuleRequiredField]
        public fmCSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading) {
                    //Journal.SubjectSet(value);
                    //foreach (var doc in FinPlanSubjectFullDocs) {
                    //    doc.SubjectSet(value);
                    //}
                    //if (value != null)
                    //    CodeSet("SFP." + value.Code);
                    //else
                    //    CodeSet("SFP.Null");
                }
            } 
        }

        [Association("FmFinPlanSubject-FmFinPlanSubjectDoc"), Aggregated]
        public XPCollection<FmFinPlanSubjectDoc> FinPlanSubjectDocs {
            get { return GetCollection<FmFinPlanSubjectDoc>("FinPlanSubjectDocs"); }
        }

        [Persistent("JournalPlanYear")]
        [Aggregated]
        protected FmJournal _JournalPlanYear;
        [PersistentAlias("_JournalPlanYear")]
        public FmJournal JournalPlanYear {
            get { return _JournalPlanYear; }
        }

        public override CriteriaOperator OperationsCriteria {
            get {  
                return XPQuery<FmJournalOperation>.TransformExpression(this.Session, 
                    x => x.Journal == Journal ||
                        x.Journal == JournalPlanYear
                    );
            }
        }
    }

}
