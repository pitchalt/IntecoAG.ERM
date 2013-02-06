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

namespace IntecoAG.ERM.CS.Measurement
{
    /// <summary>
    /// Класс, отражающий сущность Единицы измерения
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Code")]
    [Persistent("csUnit")]
    public class csUnit : BaseObject
    {
        public csUnit(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Code - описание
        /// </summary>
        private string _Code;
        [Size(5)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        /// <summary>
        /// Code - описание
        /// </summary>
        private string _Name;
        [VisibleInLookupListView(true)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Description;
        [VisibleInListView(false)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}