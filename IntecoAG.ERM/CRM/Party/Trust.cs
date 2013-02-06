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

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Trust, представляющий участника (как сторону) Договора
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmTrust")]
    public class Trust : CS.Document.Document
    {
        public Trust() : base() { }
        public Trust(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Инициирующая персона
        /// </summary>
        private Person _PersonFrom;
        public Person PersonFrom {
            get { return _PersonFrom; }
            set { if (_PersonFrom != value) SetPropertyValue("PersonFrom", ref _PersonFrom, value); }
        }

        /// <summary>
        /// Зависимая персона
        /// </summary>
        private Person _PersonTo;
        public Person PersonTo {
            get { return _PersonTo; }
            set { if (_PersonTo != value) SetPropertyValue("PersonTo", ref _PersonTo, value); }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
            Res = PersonFrom.ToString() + " - " + PersonTo.ToString();
            return Res;
        }

        #endregion

    }

}