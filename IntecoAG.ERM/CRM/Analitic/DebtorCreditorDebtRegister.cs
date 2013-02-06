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

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// Класс crmDebtorCreditorDebtRegister
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmDebtorCreditorDebtRegister")]
    public class crmDebtorCreditorDebtRegister : crmBasePrimaryPartyContragentRegister
    {
        public crmDebtorCreditorDebtRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА


        #region Измерители дебет

        private decimal _DebitCost;
        /// <summary>
        /// DebitCost
        /// Стоимость в валюте договора
        /// </summary>
        public decimal DebitCost {
            get { return _DebitCost; }
            set { SetPropertyValue<decimal>("DebitCost", ref _DebitCost, value); }
        }

        private csValuta _DebitValuta;
        /// <summary>
        /// DebitValuta
        /// Валюта договора
        /// </summary>
        public csValuta DebitValuta {
            get { return _DebitValuta; }
            set {
                SetPropertyValue<csValuta>("DebitValuta", ref _DebitValuta, value);
            }
        }

        private decimal _DebitCostInRUR;
        /// <summary>
        /// DebitCostInRUR
        /// Стоимость в рублях
        /// </summary>
        public decimal DebitCostInRUR {
            get { return _DebitCostInRUR; }
            set { SetPropertyValue<decimal>("DebitCostInRUR", ref _DebitCostInRUR, value); }
        }

        #endregion


        #region Показатели кредит

        private decimal _CreditCost;
        /// <summary>
        /// CreditCost
        /// Стоимость в валюте договора
        /// </summary>
        public decimal CreditCost {
            get { return _CreditCost; }
            set { SetPropertyValue<decimal>("CreditCost", ref _CreditCost, value); }
        }

        private csValuta _CreditValuta;
        /// <summary>
        /// CreditValuta
        /// Валюта договора
        /// </summary>
        public csValuta CreditValuta {
            get { return _CreditValuta; }
            set {
                SetPropertyValue<csValuta>("CreditValuta", ref _CreditValuta, value);
            }
        }

        private decimal _CreditCostInRUR;
        /// <summary>
        /// CreditCostInRUR
        /// Стоимость в рублях
        /// </summary>
        public decimal CreditCostInRUR {
            get { return _CreditCostInRUR; }
            set { SetPropertyValue<decimal>("CreditCostInRUR", ref _CreditCostInRUR, value); }
        }

        #endregion


        #region Показатели баланса: Рассчитываются по формуле: объем измерителя дебет – объем измерителя кредит

        private decimal _BalanceCost;
        /// <summary>
        /// BalanceCost
        /// Стоимость в валюте договора
        /// </summary>
        public decimal BalanceCost {
            get { return _BalanceCost; }
            set { SetPropertyValue<decimal>("BalanceCost", ref _BalanceCost, value); }
        }

        private csValuta _BalanceValuta;
        /// <summary>
        /// BalanceValuta
        /// Валюта договора
        /// </summary>
        public csValuta BalanceValuta {
            get { return _BalanceValuta; }
            set {
                SetPropertyValue<csValuta>("BalanceValuta", ref _BalanceValuta, value);
            }
        }

        private decimal _BalanceCostInRUR;
        /// <summary>
        /// BalanceCostInRUR
        /// Стоимость в рублях
        /// </summary>
        public decimal BalanceCostInRUR {
            get { return _BalanceCostInRUR; }
            set { SetPropertyValue<decimal>("BalanceCostInRUR", ref _BalanceCostInRUR, value); }
        }

        #endregion

        #endregion


        #region МЕТОДЫ

        #endregion

    }
}