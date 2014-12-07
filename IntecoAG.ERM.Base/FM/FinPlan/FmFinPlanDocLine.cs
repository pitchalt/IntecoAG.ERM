using System;
using System.ComponentModel;
using System.Collections.Generic;

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
        FMFPS_UNKNOW    = 0,
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

    public enum FmFinPlanLineType {
        FMFPL_UNKNOW = 0,
        FMFPL_TOP = 1,
        FMFPL_COST_TOP = 100,
        FMFPL_COST_SALE = 110,
        FMFPL_COST_SALE_COUNT = 111,
        FMFPL_COST_SALE_SALE_NOVAT_VAL = 112,
        FMFPL_COST_SALE_SALE_NOVAT_VAL_CONS = 113,
        FMFPL_COST_SALE_SALE_NOVAT_RUB = 114,
        FMFPL_COST_SALE_SALE_NOVAT_RUB_CONS = 115,
        FMFPL_COST_TOTAL = 120,
        FMFPL_COST_MATERIAL = 121,
        FMFPL_COST_WORK_TOTAL = 122,
        FMFPL_COST_WORK_ITEM_TOTAL = 123,
        FMFPL_COST_WORK_ITEM_FOT_TOTAL = 124,
        FMFPL_COST_WORK_ITEM_FOT_FOT = 125,
        FMFPL_COST_WORK_ITEM_FOT_CF = 126,
        FMFPL_COST_WORK_ITEM_FOT_TIME = 127,
        FMFPL_COST_WORK_ITEM_SOCIAL = 128,
        FMFPL_COST_WORK_ITEM_ADDITION = 129,
        FMFPL_COST_PARTY_TOTAL = 130,
        FMFPL_COST_PARTY_NPO = 131,
        FMFPL_COST_PARTY_TRW = 132,
        FMFPL_COST_PARTY_OTHER = 133,
        FMFPL_COST_OTHER_TOTAL = 134,
        FMFPL_COST_OTHER = 135,
        FMFPL_COST_TOTAL_CONS = 150,
        FMFPL_CASH_TOP = 200,
        FMFPL_CASH_IN_TOTAL = 210,
        FMFPL_CASH_IN_VAL_CASH = 211,
        FMFPL_CASH_IN_VAL_CASH_PRE = 212,
        FMFPL_CASH_IN_VAL_CASH_POST = 213,
        FMFPL_CASH_IN_VAL_SALE = 214,
        FMFPL_CASH_IN_RUB_CASH = 215,
        FMFPL_CASH_IN_RUB_CASH_PRE = 216,
        FMFPL_CASH_IN_RUB_CASH_POST = 217,
        FMFPL_CASH_IN_RUB_SALE = 218,
        FMFPL_CASH_OUT_TOTAL = 220,
        FMFPL_CASH_OUT_INTERNAL_TOTAL = 221,
        FMFPL_CASH_OUT_INTERNAL_MATERIAL = 222,
        FMFPL_CASH_OUT_INTERNAL_WORK_ITEM_TOTAL = 223,
        FMFPL_CASH_OUT_INTERNAL_WORK_ITEM = 224,
        FMFPL_CASH_OUT_INTERNAL_OTHER_TOTAL = 225,
        FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM = 226,
        FMFPL_CASH_OUT_PARTY_TOTAL = 227,
        FMFPL_CASH_OUT_PARTY_PRE_TOTAL = 228,
        FMFPL_CASH_OUT_PARTY_POST_TOTAL = 229,
        FMFPL_PARTY_TOP = 300,
        FMFPL_MATERIAL_TOP = 400,
        FMFPL_NORMATIV_TOP = 500
        //FMFPL_CASH = 2,
        //FMFPL_PARTY = 3,
        //FMFPL_MATERIAL = 4,
        //FMFPL_NORMATIV = 5
    }

    [Appearance("", AppearanceItemType.ViewItem, "IsError", TargetItems="*", BackColor = "Red")]
    [Persistent("FmFinPlanDocLine")]
    public class FmFinPlanDocLine : XPObject, ITreeNode {
        public FmFinPlanDocLine(Session session): base(session) { }
        public FmFinPlanDocLine(Session session, FmFinPlanLineType line_type, 
            FmFinPlanDocLine top_line, FmFinPlanTotalType total_type,
            String code, String name, HrmStructItemType struct_item) : base(session) {
            _LineType = line_type;
            TopLine = top_line;
            TotalType = total_type;
            LineCode = code;
            LineName = name;
            DepStruct = struct_item;
            switch (LineType) {
                case FmFinPlanLineType.FMFPL_TOP:
                    _Sheet = FmFinPlanSheetType.FMFPS_UNKNOW;
                    break;
                case FmFinPlanLineType.FMFPL_COST_TOP:
                    _Sheet = FmFinPlanSheetType.FMFPS_COST;
                    break;
                case FmFinPlanLineType.FMFPL_CASH_TOP:
                    _Sheet = FmFinPlanSheetType.FMFPS_CASH;
                    break;
                case FmFinPlanLineType.FMFPL_PARTY_TOP:
                    _Sheet = FmFinPlanSheetType.FMFPS_PARTY;
                    break;
                case FmFinPlanLineType.FMFPL_MATERIAL_TOP:
                    _Sheet = FmFinPlanSheetType.FMFPS_MATERIAL;
                    break;
                case FmFinPlanLineType.FMFPL_NORMATIV_TOP:
                    _Sheet = FmFinPlanSheetType.FMFPS_NORMATIV;
                    break;
                default:
                    _Sheet = TopLine.Sheet;
                    break;
            }
            UpdateSubLines();
        }
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
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public FmFinPlanDocTime LineTime {
            get { return _LineTime; }
//            set { SetPropertyValue<FmFinPlanDocTime>("LineTime", ref _LineTime, value); }
        }

        protected FmFinPlanDoc _FinPlanDoc;
        [Association("FmFinPlanDoc-FmFinPlanDocLine")]
        [RuleRequiredField]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
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

        [Persistent("LineType")]
        private FmFinPlanLineType _LineType;
        [PersistentAlias("_LineType")]
        public FmFinPlanLineType LineType {
            get { return _LineType; }
        }
        public void LineTypeSet(FmFinPlanLineType value) {
            SetPropertyValue<FmFinPlanLineType>("LineType", ref _LineType, value);
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
            get { 
                XPCollection<FmFinPlanDocLine> result = GetCollection<FmFinPlanDocLine>("SubLines");
                if (Sheet == FmFinPlanSheetType.FMFPS_NORMATIV)
                    result.BindingBehavior = CollectionBindingBehavior.AllowNone;
                return result;
            }
        }

        [Association("FmFinPlanDocLine-FmFinPlanDocTime"), Aggregated]
        public XPCollection<FmFinPlanDocTime> LineTimes {
            get { return GetCollection<FmFinPlanDocTime>("LineTimes"); }
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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public Boolean IsError {
            get { return false; }
        }
        public override string ToString() {
            return LineCode;
        }

        [Browsable(false)]
        public IBindingList Children {
            get { return SubLines; }
        }

        [Browsable(false)]
        public String Name {
            get { return ToString(); }
        }

        [Browsable(false)]
        public ITreeNode Parent {
            get { return TopLine; }
        }

        public void Clean() {
            IList<FmFinPlanDocTime> times = new List<FmFinPlanDocTime>(LineTimes);
            times.Remove(this.LineTime);
            this.Session.Delete(times);
        }

        public void UpdateSubLines() {
            FmFinPlanDocLine sub_line = null;
            switch (LineType) {
                case FmFinPlanLineType.FMFPL_UNKNOW:
                    break;
                case FmFinPlanLineType.FMFPL_TOP:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_TOP, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL, 
                        "Л1", "Затраты", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_TOP, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "Л2", "ДДС", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_PARTY_TOP, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "Л3", "Смежники", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_MATERIAL_TOP, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "Л4", "Материалы", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_NORMATIV_TOP, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "Л5", "Нормативы", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_COST_TOP:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_SALE, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1", "Выручка", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2", "Совокупные затраты", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_TOTAL_CONS, this, FmFinPlanTotalType.FMFPT_ACCUMULATED,
                        "2н", "Совокупные затраты нарастающим итогом", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_COST_SALE:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_SALE_COUNT, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.1", "- Выручка в штуках", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_VAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.2", "- Выручка от продаж  в валюте контракта (без НДС)", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_VAL_CONS, this, FmFinPlanTotalType.FMFPT_ACCUMULATED,
                        "1.2н", "- Нарастающим итогом", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_RUB, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.3", "- Выручка от продаж  в рублях", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_RUB_CONS, this, FmFinPlanTotalType.FMFPT_ACCUMULATED,
                        "1.3н", "- Нарастающим итогом", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_COST_SALE_COUNT:
                case FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_VAL:
                case FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_VAL_CONS:
                case FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_RUB:
                case FmFinPlanLineType.FMFPL_COST_SALE_SALE_NOVAT_RUB_CONS:
                    break;
                case FmFinPlanLineType.FMFPL_COST_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_MATERIAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1", "Прямые затраты на материалы и ПКИ", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2", "Прямые затраты труда", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_PARTY_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.3", "Затраты соисполнителей", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4", "Прочие непроизводственные", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_COST_MATERIAL:
                    break;
                case FmFinPlanLineType.FMFPL_COST_WORK_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2.1", "- Затраты ЦКБМ", HrmStructItemType.HRM_STRUCT_KB);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2.2", "- Затраты КБ \"Орион\"", HrmStructItemType.HRM_STRUCT_ORION);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2.3", "- Затраты ОЗМ", HrmStructItemType.HRM_STRUCT_OZM);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2.4", "- Затраты по договорам подряда", HrmStructItemType.HRM_STRUCT_CONTRACT);
                    break;
                case FmFinPlanLineType.FMFPL_COST_WORK_ITEM_TOTAL:
                    if (DepStruct == HrmStructItemType.HRM_STRUCT_KB || DepStruct == HrmStructItemType.HRM_STRUCT_OZM) {
                        sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            LineCode + ".1", "-- ФОТ", DepStruct);
                        sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_SOCIAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            LineCode + ".2", "-- Страховые взносы", DepStruct);
                        sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_ADDITION, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            LineCode + ".3", "-- Накладные расходы", DepStruct);
                    }
                    else {
                        sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_FOT, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            LineCode + ".1", "-- ФОТ", DepStruct);
                        sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_TIME, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            LineCode + ".2", "--- Трудоемкость в н/ч", DepStruct);
                        sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_SOCIAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            LineCode + ".3", "-- Страховые взносы", DepStruct);
                        sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_ADDITION, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            LineCode + ".4", "-- Накладные расходы", DepStruct);
                    }
                    break;
                case FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_FOT, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        LineCode + ".1", "--- Заработная плата", DepStruct);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_CF, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        LineCode + ".2", "--- Отчисления в ЦФ", DepStruct);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_TIME, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        LineCode + ".3", "--- Трудоемкость в н/ч", DepStruct);
                    break;
                case FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_FOT:
                case FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_CF:
                case FmFinPlanLineType.FMFPL_COST_WORK_ITEM_FOT_TIME:
                case FmFinPlanLineType.FMFPL_COST_WORK_ITEM_SOCIAL:
                case FmFinPlanLineType.FMFPL_COST_WORK_ITEM_ADDITION:
                    break;
                case FmFinPlanLineType.FMFPL_COST_PARTY_TOTAL:
                case FmFinPlanLineType.FMFPL_COST_PARTY_NPO:
                case FmFinPlanLineType.FMFPL_COST_PARTY_TRW:
                case FmFinPlanLineType.FMFPL_COST_PARTY_OTHER:
                    break;
                case FmFinPlanLineType.FMFPL_COST_OTHER_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.1", "- Затраты на командировки", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.2", "- Оформление лицензий", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.3", "- Оформление паспорт сделки", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.4", "- Затраты по транспортировке продукции", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.5", "- Таможенное оформление груза", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.6", "- Страхование груза", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.7", "- Услуги ВП МО РФ по контролю качества и приемки продукции (военно-техническое сопровождение)", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.8", "- Отчисления за использование прав РФ на результаты интеллектуальной деятельности (ФАПРИД)", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_COST_OTHER, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.4.9", "- Прочие", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_COST_OTHER:
                case FmFinPlanLineType.FMFPL_COST_TOTAL_CONS:
                    break;
                case FmFinPlanLineType.FMFPL_CASH_TOP:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1", "Выручка", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2", "Расходы по контракту, в том числе:", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_CASH_IN_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_VAL_CASH, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.1", "Поступления в валюте контракта (с НДС), в том числе:", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_VAL_SALE, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.2", "Выручка в валюте с НДС", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_RUB_CASH, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.3", "Поступления в рублях по контракту (с НДС), в том числе:", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_RUB_SALE, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.4", "Выручка в рублях с НДС", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_CASH_IN_VAL_CASH:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_VAL_CASH_PRE, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.1а", "Авансы", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line.PayType = fmPRPayType.PREPAYMENT;
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_VAL_CASH_POST, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.1р", "Выполненные работы", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line.PayType = fmPRPayType.POSTPAYMENT;
                    break;
                case FmFinPlanLineType.FMFPL_CASH_IN_VAL_CASH_PRE:
                case FmFinPlanLineType.FMFPL_CASH_IN_VAL_CASH_POST:
                case FmFinPlanLineType.FMFPL_CASH_IN_VAL_SALE:
                    break;
                case FmFinPlanLineType.FMFPL_CASH_IN_RUB_CASH:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_RUB_CASH_PRE, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.3а", "Авансы", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line.PayType = fmPRPayType.PREPAYMENT;
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_IN_RUB_CASH_POST, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "1.3р", "Выполненные работы", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line.PayType = fmPRPayType.POSTPAYMENT;
                    break;
                case FmFinPlanLineType.FMFPL_CASH_IN_RUB_CASH_PRE:
                case FmFinPlanLineType.FMFPL_CASH_IN_RUB_CASH_POST:
                case FmFinPlanLineType.FMFPL_CASH_IN_RUB_SALE:
                    break;
                case FmFinPlanLineType.FMFPL_CASH_OUT_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1", "Собственные работы, в том числе:", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_PARTY_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2", "Оплата работ соисполнителей (с НДС),в том числе:", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_MATERIAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1.1", "Оплата ТМЦ", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_WORK_ITEM_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1.2", "Структуры", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.6", "Прочие прямые оплаты (с НДС),в том числе:", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_MATERIAL:
                case FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM:
                case FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_WORK_ITEM:
                    break;
                case FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_WORK_ITEM_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_WORK_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1.2", "Расходы ЦКБМ", HrmStructItemType.HRM_STRUCT_KB);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_WORK_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1.3", "Расходы ОЗМ", HrmStructItemType.HRM_STRUCT_OZM);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_WORK_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1.4", "Расходы КБ \"Орион\"", HrmStructItemType.HRM_STRUCT_ORION);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_WORK_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.1.5", "Расходы по договорам подряда", HrmStructItemType.HRM_STRUCT_CONTRACT);
                    break;
                case FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.6.1", "Командировки,", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.6.2", "Лицензии,", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.6.3", "Паспорт сделки,", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.6.4", "Транспортировка,", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.6.5", "Таможенное оформление груза,", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.6.6", "Страхование груза,", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.7.7", "Оплата услуг ВП МО РФ по контролю качества и приемки продукции (военно-техническое сопровождение),", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.8.8", "Отчисления за использование прав РФ на результаты интеллектуальной деятельности (ФАПРИД),", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_INTERNAL_OTHER_ITEM, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.9.9", "Прочие", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    break;
                case FmFinPlanLineType.FMFPL_CASH_OUT_PARTY_TOTAL:
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_PARTY_PRE_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2а", "Авансы", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line.PayType = fmPRPayType.PREPAYMENT;
                    sub_line = new FmFinPlanDocLine(this.Session, FmFinPlanLineType.FMFPL_CASH_OUT_PARTY_POST_TOTAL, this, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                        "2.2р", "Расчет", HrmStructItemType.HRM_STRUCT_UNKNOW);
                    sub_line.PayType = fmPRPayType.POSTPAYMENT;
                    break;
                case FmFinPlanLineType.FMFPL_CASH_OUT_PARTY_PRE_TOTAL:
                case FmFinPlanLineType.FMFPL_CASH_OUT_PARTY_POST_TOTAL:
                    break;
            }
        }

    }

}
