using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class FmFinPlanSubjectDocFull : FmFinPlanSubjectDoc {
        public FmFinPlanSubjectDocFull(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _Journal = new FmJournal(this.Session);
            _Journal.JournalTypeAccountingSet(JournalTypeAccounting.FM_JTA_FINANCIAL);
            _Journal.JournalTypeLegalSet(JournalTypeLegal.FM_JTL_COMPANY);
            _Journal.JournalTypeObjectSet(JournalTypeObject.FM_JTO_ORDER);
            _Journal.JournalTypePeriodSet(JournalTypePeriod.FM_JTP_FULL);
            _Journal.JournalTypeSourceSet(JournalTypeSource.FM_JTS_FINPLAN_DOC);
        }

    }

}
