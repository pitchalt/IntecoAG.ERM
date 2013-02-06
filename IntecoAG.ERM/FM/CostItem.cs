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

using IntecoAG.ERM.CRM.Party;
//using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM
{
    /// <summary>
    /// Класс crmCostItem, представляющий объект Статья (Статья Затрат (ДДС))
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Code")]
    [Persistent("fmCostItem")]
    public partial class fmCostItem : BaseObject
    {
        public fmCostItem(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private string _Code;
        [Size(10)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        private string _Name;
        [Size(70)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        private string _Description;
        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        #endregion


        #region МЕТОДЫ

        public override string ToString()
        {
            string Res = "";
            Res = Description;
            return Res;
        }

        #endregion

    }

}