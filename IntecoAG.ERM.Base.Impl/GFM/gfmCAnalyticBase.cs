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
    [NonPersistent]
//    [Appearance("", AppearanceItemType.Action, "", TargetItems="Delete", Enabled=false)]
    public abstract class gfmCAnalyticBase : csCComponent, gfmIAnalytic //, ITreeNode //IHDenormalization<fmAbstractSubject>
    {
        public gfmCAnalyticBase(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.Analytic = new gfmCAnalytic(this.Session);
            this.Analytic.ComponentType = this.ComponentType;
            this.Analytic.CID = this.CID;
        }

        protected override void OnDeleting() {
            if (this.IsSaving) {
                throw new InvalidOperationException("Delete is not allowed");
            }
        }

        #region ПОЛЯ КЛАССА
//        private string _Code;
//        private string _Name;
        [Browsable(false)]
        public gfmCAnalytic Analytic;

        #endregion


        #region СВОЙСТВА КЛАССА
        [PersistentAlias("Analytic.Code")]
        [Size(14)]
        [RuleRequiredField]
        public String Code {
            get { return Analytic.Code; }
            set {
                String old = Analytic.Code;
                if (old == value) return;
                Analytic.Code = value;
                OnChanged("Code", old, value);
            }
        }
        //
        //[VisibleInLookupListView(true)]
        [PersistentAlias("Analytic.Name")]
        [Size(60)]
        [RuleRequiredField]
        public virtual String Name {
            get { return Analytic.Name; }
            set {
                String old = Analytic.Name;
                if (old == value) return;
                Analytic.Name = value;
                OnChanged("Name", old, value);
            }
        }
        //[VisibleInLookupListView(true)]
        [PersistentAlias("Analytic.NameFull")]
        [VisibleInListView(false)]
        [Size(240)]
        public String NameFull {
            get { return Analytic.NameFull; }
            set {
                String old = Analytic.NameFull;
                if (old == value) return;
                Analytic.NameFull = value;
                OnChanged("NameFull", old, value);
            }
        }
        //
        [VisibleInListView(false)]
        [PersistentAlias("Analytic.Description")]
        [Size(SizeAttribute.Unlimited)]
        public String Description {
            get { return Analytic.Description; }
            set {
                String old = Analytic.Description;
                if (old == value) return;
                Analytic.Description = value;
                OnChanged("Description", old, value);
            }
        }
        //
        [PersistentAlias("Analytic.IsClosed")]
        public Boolean IsClosed {
            get { return Analytic.IsClosed; }
            set {
                Boolean old = Analytic.IsClosed;
                if (old == value) return;
                Analytic.IsClosed = value;
                OnChanged("IsClosed", old, value);
            }
        }
        //
        [ImmediatePostData]
        [PersistentAlias("Analytic.IsPeriodUnlimited")]
        public Boolean IsPeriodUnlimited {
            get { return Analytic.IsPeriodUnlimited; }
            set {
                Boolean old = Analytic.IsPeriodUnlimited;
                if (old == value) return;
                Analytic.IsPeriodUnlimited = value;
                OnChanged("IsPeriodUnlimited", old, value);
            }
        }
        //
        [PersistentAlias("Analytic.DateBegin")]
        [RuleRequiredField]
        public DateTime DateBegin {
            get { return Analytic.DateBegin; }
            set {
                DateTime old = Analytic.DateBegin;
                if (old == value) return;
                Analytic.DateBegin = value;
                OnChanged("DateBegin", old, value);
            }
        }
        //
        [PersistentAlias("Analytic.DateEnd")]
        [RuleRequiredField]
        [Appearance("", AppearanceItemType.ViewItem, "IsPeriodUnlimited", Enabled = false)]
        public DateTime DateEnd {
            get { return Analytic.DateEnd; }
            set {
                DateTime old = Analytic.DateEnd;
                if (old == value) return;
                Analytic.DateEnd = value;
                OnChanged("DateEnd", old, value);
            }
        }
        //

        #endregion

    }
}