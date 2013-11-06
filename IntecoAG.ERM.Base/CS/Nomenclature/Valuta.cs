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

        String _CodeAlfa3;
        String _CodeRuAlfa3;
        String _CodeNumeric;
        String _NameEnFull;
        String _NameRuFull;
        Int16 _ConversionIndex;   // Покзатель пересчёта: за какое количество иностранных единиц валюты в количестве 10 в степени ConversionIndex положено рублей по курсу.

        // О коде валютных ценностей см. стандарт ISO 4217 на web-стр. 
        // http://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D0%B4%D1%8B_%D0%B8_%D0%BA%D0%BB%D0%B0%D1%81%D1%81%D0%B8%D1%84%D0%B8%D0%BA%D0%B0%D1%82%D0%BE%D1%80%D1%8B_%D0%B2%D0%B0%D0%BB%D1%8E%D1%82
        // На этой странице этот код в таблице размещён в столбце с названием "Number-3" и у рубля не один код (два - 810 (доденомиационный) и 643 (современный))
        String _CodeCurrencyValue;   // Код валютной ценности ЦБ, отражается в номерах счетов 6-8 знаками. 
        // Может быть это повторение одного из закрытых полей, например, CodeNumeric, но, ведь, где комментарий, Павел ???????

        // Некоторые коды:
        // Казахстан тенге  398
        // Россия  810 (до деноминации 1998 года) и 643 (после деноминации)
        // EUR   978
        // USD   840

        #endregion


        #region СВОЙСТВА КЛАССА
        [Size(3)]
        String CodeAlfa3 {
            get { return _CodeAlfa3; }
            set { SetPropertyValue<String>("CodeAlfa3", ref _CodeAlfa3, value );}
        }
        
        [Size(3)]
        String CodeRuAlfa3
        {
            get { return _CodeRuAlfa3; }
            set { SetPropertyValue<String>("CodeRuAlfa3", ref _CodeRuAlfa3, value); }
        }
        
        [Size(3)]
        String CodeNumeric
        {
            get { return _CodeNumeric; }
            set { SetPropertyValue<String>("CodeNumeric", ref _CodeNumeric, value); }
        }
        
        [Size(3)]
        String NameEnFull
        {
            get { return _NameEnFull; }
            set { SetPropertyValue<String>("NameEnFull", ref _NameEnFull, value); }
        }
        
        [Size(3)]
        String NameRuFull
        {
            get { return _NameRuFull; }
            set { SetPropertyValue<String>("NameRuFull", ref _NameRuFull, value); }
        }

        /// <summary>
        /// Покзатель пересчёта: за какое количество иностранных единиц валюты в количестве 10 в степени ConversionIndex положено рублей по курсу.
        /// </summary>
        [ImmediatePostData(true)]
        public Int16 ConversionIndex {
            get {
                return _ConversionIndex;
            }
            set {
                SetPropertyValue<Int16>("ConversionIndex", ref _ConversionIndex, value);
            }
        }

        /// <summary>
        /// ConversionIndex = Log10(ConversionCount)
        /// </summary>
        //[PersistentAlias("10^ConversionIndex")]
        //[ReadOnly(true)]
        public Decimal ConversionCount {
            get {
                return (Decimal)Math.Pow(10, ConversionIndex);
            }
        }

        /// <summary>
        /// Код валютной ценности ЦБ, отражается в номерах счетов 6-8 знаками
        /// </summary>
        [Size(3)]
        public String CodeCurrencyValue {
            get {
                return _CodeCurrencyValue;
            }
            set {
                SetPropertyValue<String>("CodeCurrencyValue", ref _CodeCurrencyValue, value);
            }
        }

        #endregion

    }

}