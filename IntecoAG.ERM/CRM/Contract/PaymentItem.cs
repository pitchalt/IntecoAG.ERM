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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс PaymentItem, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract partial class crmPaymentItem : crmObligationTransfer
    {
        public crmPaymentItem(Session session) : base(session) { }
        public crmPaymentItem(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Date
        /// <summary>
        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
        }

        // Суммы

        /// <summary>
        /// AccountSumma - сумма платежа
        /// </summary>
        private decimal _AccountSumma;
        public decimal AccountSumma {
            get { return _AccountSumma; }
            set { SetPropertyValue<decimal>("AccountSumma", ref _AccountSumma, value); }
        }

        /// <summary>
        /// AccountValuta - валюта платежа
        /// </summary>
        private Valuta _AccountValuta;
        public Valuta AccountValuta {
            get { return _AccountValuta; }
            set { SetPropertyValue<Valuta>("AccountValuta", ref _AccountValuta, value); }
        }

        #endregion


        #region МЕТОДЫ

        ///// <summary>
        ///// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion

    }

}