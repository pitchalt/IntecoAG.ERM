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

namespace IntecoAG.ERM.CS.Nomenclature
{
    /// <summary>
    /// Класс, отражающий сущность валюты
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csValuta")]
    //[DefaultProperty("CodeRuAlfa3")] // Это поле на форме отсутствует и поэтому остаётся пустым
    [DefaultProperty("Code")]
    public class csValuta : csFinancial
    {
        public csValuta(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        String _CodeAlfa3;
        [Size(3)]
        String CodeAlfa3 {
            get { return _CodeAlfa3; }
            set { SetPropertyValue<String>("CodeAlfa3", ref _CodeAlfa3, value );}
        }
        //
        String _CodeRuAlfa3;
        [Size(3)]
        String CodeRuAlfa3
        {
            get { return _CodeRuAlfa3; }
            set { SetPropertyValue<String>("CodeRuAlfa3", ref _CodeRuAlfa3, value); }
        }
        //
        String _CodeNumeric;
        [Size(3)]
        String CodeNumeric
        {
            get { return _CodeNumeric; }
            set { SetPropertyValue<String>("CodeNumeric", ref _CodeNumeric, value); }
        }
        //
        String _NameEnFull;
        [Size(3)]
        String NameEnFull
        {
            get { return _NameEnFull; }
            set { SetPropertyValue<String>("NameEnFull", ref _NameEnFull, value); }
        }
        //
        String _NameRuFull;
        [Size(3)]
        String NameRuFull
        {
            get { return _NameRuFull; }
            set { SetPropertyValue<String>("NameRuFull", ref _NameRuFull, value); }
        }
        #endregion

    }

}