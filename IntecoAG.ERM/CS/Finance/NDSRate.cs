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
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Validation;


namespace IntecoAG.ERM.CS.Finance
{
    /// <summary>
    /// Класс csNDSRate, представляющий справочник ставок НДС
    /// </summary>
    [DefaultProperty("Code")]
    //[DefaultClassOptions]
    [Persistent("csNDSRate")]
    public partial class csNDSRate : BaseObject
    {
        public csNDSRate(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private decimal _RateOfNDS;
        // Ставка НДС (VAT Rate)
        [RuleRequiredField("csNDSRate.RateOfNDS.Required", "Save")]
        public virtual decimal RateOfNDS {
            get { return _RateOfNDS; }
            set {
                SetPropertyValue<decimal>("RateOfNDS", ref _RateOfNDS, value);
            }
        }

        private int _Numerator;
        // Числитель простой дроби
        [RuleRequiredField("csNDSRate.Numerator.Required", "Save")]
        public virtual int Numerator {
            get { return _Numerator; }
            set {
                SetPropertyValue<int>("Numerator", ref _Numerator, value);
            }
        }

        private int _Denominator;
        // Знаменатель простой дроби
        [RuleRequiredField("csNDSRate.Denominator.Required", "Save")]
        public virtual int Denominator {
            get { return _Denominator; }
            set {
                SetPropertyValue<int>("Denominator", ref _Denominator, value);
            }
        }

        private string _Code;
        [RuleRequiredField("csNDSRate.Code.Required", "Save")]
        [Size(30)]
        [VisibleInListView(false)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        private string _Name;
        [RuleRequiredField("csNDSRate.Name.Required", "Save")]
        [Size(50)]
        [VisibleInListView(false)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        #endregion


        #region МЕТОДЫ

        public static decimal getNDS(decimal summ, csNDSRate rate) {
            
            if (rate.Denominator > 0  && rate.Numerator > 0) {
                return (decimal)((summ * rate.Numerator) / rate.Denominator);
            }

            if (rate.RateOfNDS >= 0) {
                return summ * rate.RateOfNDS;
            }

            return 0;
        }

        #endregion

    }

}