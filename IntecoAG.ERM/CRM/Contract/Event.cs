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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс Event, представляющий событие
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmEvent")]
    public partial class crmEvent : CS.Work.Event
    {
        public crmEvent() : base() { }
        public crmEvent(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// IsExternal - признак внешнего управления событием
        /// </summary>
        private bool _IsExternal;
        public bool IsExternal {
            get { return _IsExternal; }
            set { if (_IsExternal != value) SetPropertyValue("IsExternal", ref _IsExternal, value); }
        }
/*
        /// <summary>
        /// DatePlan - дата, присущая событию
        /// </summary>
        private Time _DatePlan;
        public Time DatePlan {
            get { return _DatePlan; }
            set { if (_DatePlan != value) SetPropertyValue("DatePlan", ref _DatePlan, value); }
        }
*/
        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
            //Res = this.;
            return Res;
        }

        #endregion

    }

}