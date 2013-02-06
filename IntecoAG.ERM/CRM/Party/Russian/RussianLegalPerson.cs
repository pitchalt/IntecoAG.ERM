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
    /// Класс RussianLegalPerson, представляющий (не ИЧП) предприятие как участника (сторону) Договора
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmRussianLegalPerson")]
    public partial class RussianLegalPerson : LegalPerson
    {
        public RussianLegalPerson(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// INN
        /// </summary>
        private string _INN;
        public string INN {
            get { return _INN; }
            set { SetPropertyValue("INN", ref _INN, value); }
        }

        /// <summary>
        /// ОГРН юридического лица
        /// </summary>
        private string _OGRNLP;
        public string OGRNLP {
            get { return _OGRNLP; }
            set { SetPropertyValue("OGRNLP", ref _OGRNLP, value); }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }

}