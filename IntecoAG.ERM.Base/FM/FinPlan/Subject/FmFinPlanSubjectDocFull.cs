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
            _Journal.FinPlanDocSet(this);
            _Journal.JournalTypeAccountingSet(JournalTypeAccounting.FM_JTA_FINANCIAL);
            _Journal.JournalTypeLegalSet(JournalTypeLegal.FM_JTL_COMPANY);
            _Journal.JournalTypeObjectSet(JournalTypeObject.FM_JTO_ORDER);
            _Journal.JournalTypePeriodSet(JournalTypePeriod.FM_JTP_FULL);
            _Journal.JournalTypeSourceSet(JournalTypeSource.FM_JTS_FINPLAN_DOC);
            CodeSet("тог" + ".Null");

            FmFinPlanDocLine line = null;
            line = new FmFinPlanDocLine(this.Session);
            Lines.Add(line);
            line.SheetSet(FmFinPlanSheetType.FMFPS_COST);
            line = new FmFinPlanDocLine(this.Session);
            Lines.Add(line);
            line.SheetSet(FmFinPlanSheetType.FMFPS_CASH);
            line = new FmFinPlanDocLine(this.Session);
            Lines.Add(line);
            line.SheetSet(FmFinPlanSheetType.FMFPS_PARTY);
            line = new FmFinPlanDocLine(this.Session);
            Lines.Add(line);
            line.SheetSet(FmFinPlanSheetType.FMFPS_MATERIAL);
            line = new FmFinPlanDocLine(this.Session);
            Lines.Add(line);
            line.SheetSet(FmFinPlanSheetType.FMFPS_NORMATIV);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (propertyName == "Order") {
                fmCOrder order = newValue as fmCOrder;
                if (order != null) {
                    CodeSet("тог." + order.Code + ".о0");
                    Journal.CodeSet(Code);
                }
            }
        }
    }

}
