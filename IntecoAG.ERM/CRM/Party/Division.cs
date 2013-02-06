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
using System.ComponentModel;

namespace IntecoAG.ERM.CRM.Party {
    /// <summary>
    /// Класс Person, представляющий участника (как сторону) Договора
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmDivision")]
    public partial class Division : Party 
    {
        public Division(Session ses) : base(ses) { }

        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// KPP
        /// </summary>
        private string _KPP;
        public string KPP {
            get { return _KPP; }
            set { SetPropertyValue("KPP", ref _KPP, value); }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Name;
        }

        #endregion

    }

}