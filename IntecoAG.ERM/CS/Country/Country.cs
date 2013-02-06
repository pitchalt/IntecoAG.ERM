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
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace IntecoAG.ERM.CS.Country
{
    /// <summary>
    /// Класс Country, представляющий стороны Договора
    /// </summary>
    [Persistent("csCountry")]
    [DefaultProperty("NameRuShortLow")]
    [FriendlyKeyProperty("CodeRuAlfa3")]
    public partial class csCountry : BaseObject
    {
        public csCountry(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            CodeAlfa2 = String.Empty;
            CodeAlfa3 = String.Empty;
            CodeNumeric = String.Empty;
            CodeRuAlfa3 = String.Empty;
//            NameEnShortUp = String.Empty;
            NameEnShortLow = String.Empty;
            NameEnFull = String.Empty;
//            NameRuShortUp = String.Empty;
            NameRuShortLow = String.Empty;
            NameRuFull = String.Empty;
        }


        #region ПОЛЯ КЛАССА

        private string _CodeAlfa2;
        private string _CodeAlfa3;
        private string _CodeNumeric;
        private string _CodeRuAlfa3;
        //        private string _NameEnShortUp;
        private string _NameEnShortLow;
        private string _NameEnFull;
        //        private string _NameRuShortUp;
        private string _NameRuShortLow;
        private string _NameRuFull;
        private string _Comment;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Description - описание
        /// </summary>
//        [Size(30)]
        public string NameEnShortUp {
            get { return NameEnShortLow.ToUpper(); }
//            set { SetPropertyValue("NameEnShortUp", ref _NameEnShortUp, value.ToUpper()); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(30)]
        public string NameEnShortLow {
            get { return _NameEnShortLow; }
            set { SetPropertyValue("NameEnShortLow", ref _NameEnShortLow, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(70)]
        public string NameEnFull {
            get { return _NameEnFull; }
            set { SetPropertyValue("NameEnFull", ref _NameEnFull, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
//        [Size(30)]
        public string NameRuShortUp {
            get { return NameRuShortLow.ToUpper(); }
//            set { SetPropertyValue("NameRuShortUp", ref _NameRuShortUp, value.ToUpper()); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(30)]
//        [VisibleInLookupListView(true)]
        public string NameRuShortLow {
            get { return _NameRuShortLow; }
            set { SetPropertyValue("NameRuShortLow", ref _NameRuShortLow, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(70)]
        public string NameRuFull {
            get { return _NameRuFull; }
            set { SetPropertyValue("NameRuFull", ref _NameRuFull, value.ToUpper()); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(2)]
        [RuleRequiredField]
        public string CodeAlfa2 {
            get { return _CodeAlfa2; }
            set { SetPropertyValue("CodeAlfa2", ref _CodeAlfa2, value.ToUpper()); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(3)]
        [RuleRequiredField]
        public string CodeAlfa3 {
            get { return _CodeAlfa3; }
            set { SetPropertyValue("CodeAlfa3", ref _CodeAlfa3, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(3)]
        [RuleRequiredField]
        public string CodeRuAlfa3 {
            get { return _CodeRuAlfa3; }
            set { SetPropertyValue("CodeRuAlfa3", ref _CodeRuAlfa3, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(3)]
        public string CodeNumeric {
            get { return _CodeNumeric; }
            set { SetPropertyValue("CodeNumeric", ref _CodeNumeric, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(70)]
        public string Comment {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }
        #endregion


        #region МЕТОДЫ

        ///// <summary>
        ///// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{

        //    return string.Concat(CodeAlfa2, " - ", NameRuShortLow);
        //}

        #endregion

    }

}