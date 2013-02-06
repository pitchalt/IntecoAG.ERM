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
    [Persistent("gfmAbstractAnalitic")]
    [DefaultProperty("Code")]
    [FriendlyKeyProperty("Code")]
    public class AbstractAnaliticClass : BaseObject, IAbstractAnalitic //, ITreeNode //IHDenormalization<fmAbstractSubject>
    {
        public AbstractAnaliticClass(Session ses) : base(ses) { }

        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА
        private string _Code;
        private string _Name;
        private DateTime _DateBegin;
        private DateTime _DateEnd;

        [Persistent("IsClosed")]
        private Boolean _IsClosed;
        #endregion


        #region СВОЙСТВА КЛАССА
        [RuleRequiredField]
        [Size(15)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        [Size(70)]
        [VisibleInLookupListView(true)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }
        [RuleRequiredField]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                //if (!IsLoading) {
                //    if (value < DateBegin) {
                //        throw new InvalidOperationException();
                //    }
                //}
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
                if (!IsLoading) {
                    IsClosedUpdate();
                }
            }
        }
        [PersistentAlias("_IsClosed")]
        public Boolean IsClosed {
            get { return _IsClosed; }
        }

        protected void IsClosedUpdate() {
            Boolean old = _IsClosed;
            _IsClosed = DateEnd != DateTime.MinValue;
            OnChanged("IsClosed", old, _IsClosed);
        }

        [Action(PredefinedCategory.Edit, Caption = "ReOpen", ToolTip = "ReOpen order")]
        public void ReOpen() {
            DateEnd = DateTime.MinValue;
            IsClosedUpdate();
        }
        #endregion

    }
}