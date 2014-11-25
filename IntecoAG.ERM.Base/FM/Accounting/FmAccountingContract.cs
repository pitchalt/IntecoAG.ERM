using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.Accounting {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class FmAccountingContract : FmAccounting {
        public FmAccountingContract(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
            _Journal = new FmJournal(this.Session);
            _Journal.AccountingSet(this);
            _Journal.JournalTypeAccountingSet(JournalTypeAccounting.FM_JTA_FINANCIAL);
            _Journal.JournalTypeLegalSet(JournalTypeLegal.FM_JTL_COMPANY);
            _Journal.JournalTypeObjectSet(JournalTypeObject.FM_JTO_ALL);
            _Journal.JournalTypePeriodSet(JournalTypePeriod.FM_JTP_FULL);
            _Journal.JournalTypeSourceSet(JournalTypeSource.FM_JTS_CONTRACT);
            CodeSet("дц");
        }


        protected override CriteriaOperator OperationsCriteria {
            get {
                return XPQuery<FmJournalOperation>.TransformExpression(this.Session,
                        x => x.Journal == Journal);
            }
        }
    }

}
