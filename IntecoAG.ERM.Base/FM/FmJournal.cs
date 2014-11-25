using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.FinPlan;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM {

    /// <summary>
    /// 
    /// </summary>
    public enum JournalTypeSource {
        FM_JTS_FACT = 1,
        FM_JTS_CONTRACT = 2,
        FM_JTS_FINPLAN = 3,
        FM_JTS_FINPLAN_DOC = 4,
        FM_JTS_COST_STRUCTURE = 5
    }

    /// <summary>
    /// 
    /// </summary>
    public enum JournalTypeLegal {
        FM_JTL_CORPORATION_TOP = 1,
        FM_JTL_CORPORATION = 2,
        FM_JTL_COMPANY = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum JournalTypePeriod {
        FM_JTP_FULL = 1,
        FM_JTP_YEAR3 = 2,
        FM_JTP_YEAR = 3,
        FM_JTP_QUARTER = 4,
        FM_JTP_MONTH = 5,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum JournalTypeObject {
        FM_JTO_ALL = 1,
        FM_JTO_SUBJECT = 2,
        FM_JTO_ORDER = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum JournalTypeAccounting {
        FM_JTA_FINANCIAL = 1,
        FM_JTA_TAX = 2,
        FM_JTA_MANAGEMENT = 3
    }

    /// <summary>
    /// 
    /// </summary>
    [Persistent("FmJournal")]
    public class FmJournal : XPObject {
        public FmJournal(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        //private FmJournalSet _JournalSet;
        //[ExplicitLoading(0)]
        //[Association("FmJournalSet-FmJournal")]
        //public FmJournalSet JournalSet {
        //    get { return _JournalSet; }
        //    set { SetPropertyValue<FmJournalSet>("JournalSet", ref _JournalSet, value); }
        //}

        [Size(64)]
        [Persistent]
        private String _Code;
        [PersistentAlias("_Code")]
        public String Code {
            get { return _Code; }
        }
        public void CodeSet(String code) {
            SetPropertyValue<String>("Code", ref _Code, code);
        }

        [Persistent("JournalTypeSource")]
        private JournalTypeSource _JournalTypeSource;
        [PersistentAlias("_JournalTypeSource")]
        public JournalTypeSource JournalTypeSource {
            get { return _JournalTypeSource; }
        }
        public void JournalTypeSourceSet(JournalTypeSource value) {
            SetPropertyValue<JournalTypeSource>("JournalTypeSource", ref _JournalTypeSource, value); 
        }

        [Persistent("JournalTypeLegal")]
        private JournalTypeLegal _JournalTypeLegal;
        [PersistentAlias("_JournalTypeLegal")]
        public JournalTypeLegal JournalTypeLegal {
            get { return _JournalTypeLegal; }
        }
        public void JournalTypeLegalSet(JournalTypeLegal value) {
            SetPropertyValue<JournalTypeLegal>("JournalTypeLegal", ref _JournalTypeLegal, value);
        }

        [Persistent("JournalTypePeriod")]
        private JournalTypePeriod _JournalTypePeriod;
        [PersistentAlias("_JournalTypePeriod")]
        public JournalTypePeriod JournalTypePeriod {
            get { return _JournalTypePeriod; }
        }
        public void JournalTypePeriodSet(JournalTypePeriod value) {
            SetPropertyValue<JournalTypePeriod>("JournalTypePeriod", ref _JournalTypePeriod, value);
        }

        [Persistent("JournalTypeObject")]
        private JournalTypeObject _JournalTypeObject;
        [PersistentAlias("_JournalTypeObject")]
        public JournalTypeObject JournalTypeObject {
            get { return _JournalTypeObject; }
        }
        public void JournalTypeObjectSet(JournalTypeObject value) {
            SetPropertyValue<JournalTypeObject>("JournalTypeObject", ref _JournalTypeObject, value);
        }

        [Persistent("JournalTypeAccounting")]
        private JournalTypeAccounting _JournalTypeAccounting;
        [PersistentAlias("_JournalTypeAccounting")]
        public JournalTypeAccounting JournalTypeAccounting {
            get { return _JournalTypeAccounting; }
        }
        public void JournalTypeAccountingSet(JournalTypeAccounting value) {
            SetPropertyValue<JournalTypeAccounting>("JournalTypeAccounting", ref _JournalTypeAccounting, value);
        }

        [ExplicitLoading(0)]
        [Persistent("FinPlan")]
        private FmFinPlanPlan _FinPlan;
        [PersistentAlias("_FinPlan")]
        public FmFinPlanPlan FinPlan {
            get { return _FinPlan; }
        }
        public void FinPlanSet(FmFinPlanPlan value) {
            SetPropertyValue<FmFinPlanPlan>("FinPlan", ref _FinPlan, value);
        }

        [ExplicitLoading(0)]
        [Persistent("FinPlanDoc")]
        private FmFinPlanDoc _FinPlanDoc;
        [PersistentAlias("_FinPlanDoc")]
        public FmFinPlanDoc FinPlanDoc {
            get { return _FinPlanDoc; }
        }
        public void FinPlanDocSet(FmFinPlanDoc value) {
            SetPropertyValue<FmFinPlanDoc>("FinPlanDoc", ref _FinPlanDoc, value);
        }

        [ExplicitLoading(0)]
        [Persistent("Accounting")]
        private FmAccounting _Accounting;
        [PersistentAlias("_Accounting")]
        public FmAccounting Accounting {
            get { return _Accounting; }
        }
        public void AccountingSet(FmAccounting value) {
            SetPropertyValue<FmAccounting>("Accounting", ref _Accounting, value);
        }

        [Association("FmJournal-FmJournalOperation"), Aggregated]
        public XPCollection<FmJournalOperation> Operations {
            get {
                return GetCollection<FmJournalOperation>("Operations");
            }
        }

        [Persistent("Subject")]
        private fmCSubject _Subject;
        [PersistentAlias("_Subject")]
        public fmCSubject Subject {
            get { return _Subject; }
        }
        public void SubjectSet(fmCSubject value) {
            SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
            foreach (var oper in Operations) {
                oper.Subject = value;
            }
        }

        [Persistent("Order")]
        private fmCOrder _Order;
        [PersistentAlias("_Order")]
        public fmCOrder Order {
            get { return _Order; }
        }
        public void OrderSet(fmCOrder value) {
            SetPropertyValue<fmCOrder>("Order", ref _Order, value);
            foreach (var oper in Operations) {
                oper.Order = value;
                oper.Subject = value.Subject;
            }
        }

    }
}
