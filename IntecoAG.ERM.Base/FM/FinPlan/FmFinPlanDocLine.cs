using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.FinPlan {

    public enum FmFinPlanSheetType { 
        FMFPS_COST      = 1,
        FMFPS_CASH      = 2,
        FMFPS_PARTY     = 3,
        FMFPS_MATERIAL  = 4,
        FMFPS_NORMATIV  = 5
    }

    public enum FmFinPlanTotalType {
        FMFPT_NOTOTAL  = 1,
        FMFPT_HIERARCHICAL  = 2,
        FMFPT_ACCUMULATED  = 3
    }

    [Appearance("", AppearanceItemType.ViewItem, "IsError", TargetItems="*", BackColor = "Red")]
    [Persistent("FmFinPlanDocLine")]
    public class FmFinPlanDocLine : XPObject, ITreeNode {
        public FmFinPlanDocLine(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TotalType = FmFinPlanTotalType.FMFPT_NOTOTAL;
            _LineTime = new FmFinPlanDocTime(this.Session);
            _LineTime.TimeTypeSet(FmFinPlanTimeType.FMFPT_TOTAL);
        }
        [Persistent("LineTime")]
        [Aggregated]
        protected FmFinPlanDocTime _LineTime;
        [PersistentAlias("_LineTime")]
        public FmFinPlanDocTime LineTime {
            get { return _LineTime; }
//            set { SetPropertyValue<FmFinPlanDocTime>("LineTime", ref _LineTime, value); }
        }

        protected FmFinPlanDoc _FinPlanDoc;
        [Association("FmFinPlanDoc-FmFinPlanDocLine")]
        [RuleRequiredField]
        public FmFinPlanDoc FinPlanDoc {
            get { return _FinPlanDoc; }
            set { SetPropertyValue<FmFinPlanDoc>("FinPlanDoc", ref _FinPlanDoc, value); }
        }

        [Persistent("Sheet")]
        private FmFinPlanSheetType _Sheet;
        [PersistentAlias("_Sheet")]
        public FmFinPlanSheetType Sheet {
            get { return _Sheet; }
        }
        public void SheetSet(FmFinPlanSheetType value) {
            SetPropertyValue<FmFinPlanSheetType>("Sheet", ref _Sheet, value);
        }
//
        private FmFinPlanDocLine _TopLine;
        [Association("FmFinPlanDocLine-SubLines")]
        public FmFinPlanDocLine TopLine {
            get { return _TopLine; }
            set { 
                SetPropertyValue<FmFinPlanDocLine>("TopLine", ref _TopLine, value);
                if (!IsLoading && value != null) {
                    SheetSet(value.Sheet);
                    FinPlanDoc = value.FinPlanDoc;
                }
            }
        }
        //
        [Association("FmFinPlanDocLine-SubLines"), Aggregated]
        public XPCollection<FmFinPlanDocLine> SubLines {
            get { return GetCollection<FmFinPlanDocLine>("SubLines"); }
        }

        [Association("FmFinPlanDocLine-FmFinPlanDocTime"), Aggregated]
        public XPCollection<FmFinPlanDocTime> LineTimes {
            get { return GetCollection<FmFinPlanDocTime>("SubTimes"); }
        }

//        [RuleRequiredField]
        [Size(32)]
        public String LineCode;

        [Size(256)]
        public String LineName;

        public HrmStructItemType DepStruct;

        private String _PartyName;
        [Size(1024)]
        public String PartyName {
            get { return _PartyName; }
            set { SetPropertyValue<String>("PartyName", ref _PartyName, value); }
        }

        private crmCParty _Party;
        public crmCParty Party {
            get { return _Party; }
            set { 
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
            }
        }

        public csValuta Valuta;

        public csNDSRate NdsRate;

        public fmPRPayType PayType;

        public FmFinPlanTotalType TotalType;

        [PersistentAlias("LineTime.ValueManual")]
        public Decimal ValueManual {
            get {
                return LineTime.ValueManual;
            }
        }

        [PersistentAlias("LineTime.ValueAutomatic")]
        public Decimal ValueAutomatic {
            get {
                return LineTime.ValueManual;
            }
        }

        public Boolean IsError {
            get { return false; }
        }
        public override string ToString() {
            return LineCode;
        }

        public IBindingList Children {
            get { return SubLines; }
        }

        public String Name {
            get { return ToString(); }
        }

        public ITreeNode Parent {
            get { return TopLine; }
        }
    }

}
