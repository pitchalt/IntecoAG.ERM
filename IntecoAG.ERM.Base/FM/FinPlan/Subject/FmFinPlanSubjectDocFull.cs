using System;
using System.ComponentModel;
using System.IO;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    /// <summary>
    /// 
    /// </summary>
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
            CodeSet("‘œ«" + ".Null");
            //
            _TopLine = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_TOP, null, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                " ÌË„‡", " ÌË„‡", HrmStructItemType.HRM_STRUCT_UNKNOW);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (propertyName == "Order") {
                fmCOrder order = newValue as fmCOrder;
                if (order != null) {
                    CodeSet("‘œ«." + order.Code + ".œ0");
                    Journal.CodeSet(Code);
                }
            }
        }

        public override void Import(IObjectSpace os, String file_name) {
            using (Stream stream = new FileStream(file_name, FileMode.Open)) {
                FmFinPlanSubjectDocFullLogic.LoadDocFromXML(os, this, stream);
            }
        }
    }

}
