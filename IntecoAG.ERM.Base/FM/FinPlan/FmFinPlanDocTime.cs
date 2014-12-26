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

namespace IntecoAG.ERM.FM.FinPlan {

    public enum FmFinPlanTimeType {
        FMFPT_UNKNOW = 0,
        FMFPT_SALDO = 1,
        FMFPT_TOTAL = 2,
        FMFPT_YEAR = 3,
        FMFPT_QUARTER = 4,
        FMFPT_MONTH = 5
    }

    [Appearance("", AppearanceItemType.ViewItem, "IsError", TargetItems = "*", BackColor = "Red")]
    [Persistent("FmFinPlanDocTime")]
    public class FmFinPlanDocTime : XPObject, ITreeNode {
        public FmFinPlanDocTime(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private FmFinPlanDocLine _DocLine;
        [Association("FmFinPlanDocLine-FmFinPlanDocTime")]
        public FmFinPlanDocLine DocLine {
            get { return _DocLine; }
            set { SetPropertyValue<FmFinPlanDocLine>("DocLine", ref _DocLine, value); }
        }

        [Persistent("IsExpanded")]
        private Boolean _IsExpanded;
        [PersistentAlias("_IsExpanded")]
        public Boolean IsExpanded {
            get { return _IsExpanded; }
        }
        public void IsExpandedSet(Boolean value) {
            SetPropertyValue<Boolean>("IsExpanded", ref _IsExpanded, value);
        }

        private FmFinPlanDocTime _TopTime;
        [Association("FmFinPlanDocTime-SubTimes")]
        public FmFinPlanDocTime TopTime {
            get { return _TopTime; }
            set { 
                SetPropertyValue<FmFinPlanDocTime>("TopTime", ref _TopTime, value);
                if (!IsLoading && value != null) {
                    DocLine = value.DocLine;
                    value.IsExpandedSet(true);
                    YearSet(value.Year);
                    QuarterSet(value.Quarter);
                    MonthSet(value.Month);
                    DateSet(value.Date);
                    switch (value.TimeType) {
                        case FmFinPlanTimeType.FMFPT_UNKNOW:
                            break;
                        case FmFinPlanTimeType.FMFPT_SALDO:
                            break;
                        case FmFinPlanTimeType.FMFPT_TOTAL:
                            TimeTypeSet(FmFinPlanTimeType.FMFPT_YEAR);
                            break;
                        case FmFinPlanTimeType.FMFPT_YEAR:
                            TimeTypeSet(FmFinPlanTimeType.FMFPT_QUARTER);
                            break;
                        case FmFinPlanTimeType.FMFPT_QUARTER:
                            TimeTypeSet(FmFinPlanTimeType.FMFPT_MONTH);
                            break;
                        case FmFinPlanTimeType.FMFPT_MONTH:
                            break;
                    }
                }
            }
        }

        [Association("FmFinPlanDocTime-SubTimes"), Aggregated]
        public XPCollection<FmFinPlanDocTime> SubTimes {
            get { return GetCollection<FmFinPlanDocTime>("SubTimes"); } 
        }

        [Persistent("TimeType")]
        private FmFinPlanTimeType _TimeType;
        [PersistentAlias("_TimeType")]
        public FmFinPlanTimeType TimeType {
            get { return _TimeType; }
        }
        public void TimeTypeSet(FmFinPlanTimeType value) {
            SetPropertyValue<FmFinPlanTimeType>("TimeType", ref _TimeType, value);
        }

        [Persistent("Date")]
        private DateTime _Date;
        [PersistentAlias("_Date")]
        public DateTime Date {
            get { return _Date; }
        }
        public void DateSet(DateTime value) {
            SetPropertyValue<DateTime>("Date", ref _Date, value);
        }

        [Persistent("Year")]
        private Int16 _Year;
        [PersistentAlias("_Year")]
        public Int16 Year {
            get { return _Year; }
        }
        public void YearSet(Int16 value) {
            SetPropertyValue<Int16>("Year", ref _Year, value);
        }

        [Persistent("Quarter")]
        private Int16 _Quarter;
        [PersistentAlias("_Quarter")]
        public Int16 Quarter {
            get { return _Quarter; }
        }
        public void QuarterSet(Int16 value) {
            SetPropertyValue<Int16>("Quarter", ref _Quarter, value);
        }

        [Persistent("Month")]
        private Int16 _Month;
        [PersistentAlias("_Month")]
        public Int16 Month {
            get { return _Month; }
        }
        public void MonthSet(Int16 value) {
            SetPropertyValue<Int16>("month", ref _Month, value);
        }

        private Decimal _ValueManual;
        public Decimal ValueManual {
            get { return _ValueManual; }
            set { 
                SetPropertyValue<Decimal>("ValueManual", ref _ValueManual, value); 
                if (!IsLoading) {
                    if (!IsExpanded) {
                        ValueAutomaticSet(value);
                    }
                }
            }
        }

        public void ValueAutomaticUpdate() {
            if (DocLine != null && DocLine.TotalType != FmFinPlanTotalType.FMFPT_NOTOTAL) { 
                Decimal value = 0;
                foreach (var sub_time in SubTimes) {
                    value += sub_time.ValueAutomatic;
                }
                ValueAutomaticSet(value);
            }
        }

        [Persistent("ValueAutomatic")]
        private Decimal _ValueAutomatic;
        [PersistentAlias("_ValueAutomatic")]
        public Decimal ValueAutomatic {
            get { return _ValueAutomatic; }
        }
        public void ValueAutomaticSet(Decimal value) {
            SetPropertyValue<Decimal>("ValueAutomatic", ref _ValueAutomatic, value);
            if (TopTime != null)
                TopTime.ValueAutomaticUpdate();
        }

        [VisibleInListView(false)]
        public Boolean IsError {
            get {
                if (ValueManual != 0 && ValueManual != ValueAutomatic)
                    return true;
                else
                    return false;
            }
        }

        public override String ToString() {
            switch (TimeType) {
                case FmFinPlanTimeType.FMFPT_SALDO:
                    return Year.ToString() + "." + Month.ToString() + ".Saldo";
                case FmFinPlanTimeType.FMFPT_TOTAL:
                    return "Total";
                case FmFinPlanTimeType.FMFPT_YEAR:
                    return Year.ToString();
                case FmFinPlanTimeType.FMFPT_QUARTER:
                    return Year.ToString() + "." + Quarter.ToString();
                case FmFinPlanTimeType.FMFPT_MONTH:
                    return Year.ToString() + "." + Month.ToString();
                default:
                    return "Unknow";
            }
        }

        [Browsable(false)]
        public IBindingList Children {
            get { return SubTimes; }
        }

        public String Name {
            get { return ToString(); }
        }

        [Browsable(false)]
        public ITreeNode Parent {
            get { return TopTime; }
        }
    }

}
