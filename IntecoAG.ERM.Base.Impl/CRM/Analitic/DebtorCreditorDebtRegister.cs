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
    /// Класс crmDebtorCreditorDebtRegister
    /// </summary>
    //[DefaultClassOptions]
    [VisibleInReports]
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

        //private decimal _BalanceCost;
        /// <summary>
        /// BalanceCost
        /// Стоимость в валюте договора
        /// </summary>
        public decimal BalanceCost {
            get { return DebitCost - CreditCost; }
            //set { SetPropertyValue<decimal>("BalanceCost", ref _BalanceCost, value); }
        }

        //private csValuta _BalanceValuta;
        /// <summary>
        /// BalanceValuta
        /// Валюта договора
        /// </summary>
        public csValuta BalanceValuta {
            get { return DebitValuta; }
            //set {
            //    SetPropertyValue<csValuta>("BalanceValuta", ref _BalanceValuta, value);
            //}
        }

        //private decimal _BalanceCostInRUR;
        /// <summary>
        /// BalanceCostInRUR
        /// Стоимость в рублях
        /// </summary>
        public decimal BalanceCostInRUR {
            get { return DebitCostInRUR - CreditCostInRUR; }
            //set { SetPropertyValue<decimal>("BalanceCostInRUR", ref _BalanceCostInRUR, value); }
        }

        #endregion

        #endregion


        #region МЕТОДЫ

        #endregion

    }
}