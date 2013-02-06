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

using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// Класс crmPlaneFactRegister
    /// </summary>
    //[DefaultClassOptions]
    [VisibleInReports]
    [Persistent("crmPlaneFactRegister")]
    public class crmPlaneFactRegister : crmBaseDebitCreditRegister
    {
        public crmPlaneFactRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private decimal _Cost;
        /// <summary>
        /// Cost
        /// Стоимость в валюте договора
        /// </summary>
        public decimal Cost {
            get { return _Cost; }
            set { SetPropertyValue<decimal>("Cost", ref _Cost, value); }
        }

        private csValuta _Valuta;
        /// <summary>
        /// Valuta
        /// Валюта договора
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        private decimal _CostInRUR;
        /// <summary>
        /// CostInRUR
        /// Стоимость в рублях
        /// </summary>
        public decimal CostInRUR {
            get { return _CostInRUR; }
            set { SetPropertyValue<decimal>("CostInRUR", ref _CostInRUR, value); }
        }

        private decimal _Volume;
        /// <summary>
        /// Volume
        /// Объем в натуральном выражении
        /// </summary>
        public virtual decimal Volume {
            get { return _Volume; }
            set { SetPropertyValue<decimal>("Volume", ref _Volume, value); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }
}