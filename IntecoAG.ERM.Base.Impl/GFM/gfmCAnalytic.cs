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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

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
        public gfmCAnalytic(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(gfmCAnalytic);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА
//        private string _Code;
        private string _NameFull;
        private DateTime _DateBegin;
        private DateTime _DateEnd;

        [Persistent("IsClosed")]
        private Boolean _IsClosed;
        #endregion


        #region СВОЙСТВА КЛАССА
        [Size(250)]
        public virtual String NameFull {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }
        //
        [RuleRequiredField]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
            }
        }
        [PersistentAlias("_IsClosed")]
        public Boolean IsClosed {
            get { return _IsClosed; }
            set {
                SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value);
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