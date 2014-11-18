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

    /// <summary>
    /// 
    /// </summary>
    public enum JournalType {
        JOURNAL_TYPE_COST_STRUCTURE = 1,
        JOURNAL_TYPE_PERIOD_MONTH = 2,
        JOURNAL_TYPE_PERIOD_KVARTAL = 4,
        JOURNAL_TYPE_PERIOD_YEAR = 8,
        JOURNAL_TYPE_FULL_TIME = 16,
        JOURNAL_TYPE_SUBJECT = 32,
        JOURNAL_TYPE_DOC = 64,
        JOURNAL_TYPE_ALL = 256,

        JOURNAL_TYPE_SUBJECT_DOC_FULL = JOURNAL_TYPE_DOC | JOURNAL_TYPE_SUBJECT | JOURNAL_TYPE_FULL_TIME  
    }


    /// <summary>
    /// 
    /// </summary>
    [Persistent("FmFinPlanJournal")]
    public class FmFinPlanJournal : XPObject {
        public FmFinPlanJournal(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private JournalType _JournalType;
        public JournalType JournalType {
            get { return _JournalType; }
            set { SetPropertyValue<JournalType>("JournalType", ref _JournalType, value); }
        }

        private FmFinPlanPlan _FinPlan; 
        [ExplicitLoading(0)]
        public FmFinPlanPlan FinPlan {
            get { return _FinPlan; }
            set { SetPropertyValue<FmFinPlanPlan>("FinPlan", ref _FinPlan, value); }
        }

        private FmFinPlanDoc _FinPlanDoc;
        [ExplicitLoading(0)]
        public FmFinPlanDoc FinPlanDoc {
            get { return _FinPlanDoc; }
            set { SetPropertyValue<FmFinPlanDoc>("FinPlanDoc", ref _FinPlanDoc, value); }
        }

        [Association("fmFinPlanJournal-fmFinPlanOperation"), Aggregated]
        public XPCollection<FmFinPlanOperation> Operations {
            get {
                return GetCollection<FmFinPlanOperation>("Operations");
            }
        }

        [Persistent("Subject")]
        private fmCSubject _Subject;
        [PersistentAlias("_Subject")]
        public fmCSubject Subject {
            get { return _Subject; }
        }
        public void SubjectSet(fmCSubject value) {
            _Subject = value;
        }

    }
}
