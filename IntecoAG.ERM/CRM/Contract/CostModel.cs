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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс CostModel, представляющий объект Договора
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("crmCostModel")]
    public partial class crmCostModel : BaseObject
    {
        public crmCostModel(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Code
        /// </summary>
        private string _Code;
        [Size(10)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        /// <summary>
        /// Name - описание
        /// </summary>
        private string _Name;
        [Size(70)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Description;
        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        #endregion


        #region МЕТОДЫ


        #endregion

    }

}