#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.
using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

// === IntecoAG namespaces ===
using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.GFM
{

    /// <summary>
    /// Класс AbstractSubject представляет базовую функциональность
    /// </summary>
    //[NonPersistent]
    [Persistent("gfmAnalitic")]
    [DefaultProperty("Code")]
    [FriendlyKeyProperty("Code")]
    public class gfmCAnalytic : csCCodedComponent, gfmIAnalytic //, ITreeNode //IHDenormalization<fmAbstractSubject>
    {
        public static DateTime DateUnlimitedValue = new DateTime(2999,12,31);
        public static DateTime DateMaxValue = new DateTime(2199, 12, 31);
        public static DateTime DateMinValue = new DateTime(1950, 1, 1);

        public gfmCAnalytic(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(gfmCAnalytic);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

//        private string _Code;
        private String _NameFull;
        private DateTime _DateBegin;
        private DateTime _DateEnd;
        private Boolean  _IsPeriodUnlimited;

        [Persistent("IsClosed")]
        private Boolean _IsClosed;

        #endregion


        #region СВОЙСТВА КЛАССА

        public override String Name {
            set {
                String old = base.Name;
                base.Name = value;
                if (!IsLoading && (String.IsNullOrEmpty(NameFull) || NameFull == old))
                    NameFull = value;
            }
        }
        //
        [Size(240)]
        public virtual String NameFull {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }
        //
        [RuleRequiredField]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set {
                if (!IsLoading && (value < DateMinValue || value > DateMaxValue))
                    throw new ArgumentOutOfRangeException("DateBegin");
                SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value);
                if (!IsLoading && DateEnd < DateBegin)
                    DateEnd = DateBegin;
            }
        }
        //
        [RuleRequiredField]
        [Appearance("", AppearanceItemType.ViewItem, "IsPeriodUnlimited", Enabled=false)]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                if (!IsLoading) {
                    if (IsPeriodUnlimited)
                        throw new InvalidOperationException("PeriodUnlimited");
                    if (value < DateMinValue || value > DateMaxValue)
                        throw new ArgumentOutOfRangeException("DateEnd");
                }
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
                if (!IsLoading && DateBegin > DateEnd )
                    DateBegin = DateEnd;
            }
        }
        //
        [PersistentAlias("_IsClosed")]
        public Boolean IsClosed {
            get { return _IsClosed; }
            set {
                SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value);
            }
        }
        //
        public Boolean IsPeriodUnlimited {
            get { return _IsPeriodUnlimited; }
            set {
                SetPropertyValue<Boolean>("IsPeriodUnlimited", ref _IsPeriodUnlimited, value);
                if (!IsLoading && IsPeriodUnlimited) {
                    DateTime old_date_end = _DateEnd;
                    _DateEnd = DateUnlimitedValue;
                    OnChanged("DateEnd", old_date_end, DateUnlimitedValue);
                }
            }
        }

//        [Action(PredefinedCategory.Edit, Caption = "ReOpen", ToolTip = "ReOpen order")]
//        public void ReOpen() {
//            IsClosed = false;
//
//        }
//
        #endregion

    }
}