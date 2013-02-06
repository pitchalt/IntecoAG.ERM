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

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс PaymentOfBill, представляющий документ оплаты векселем
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmPaymentOfBill")]
    public partial class crmPaymentMoney : crmPaymentItem
    {
        public crmPaymentMoney(Session session) : base(session) { }
        public crmPaymentMoney(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        public override CS.Nomenclature.Nomenclature Nomenclature {
            get { return this.AccountValuta; }
        }
        ///// <summary>
        ///// Номентклатура
        ///// </summary>
        //private CS.Nomenclature.Valuta _PayValuta;
        //public CS.Nomenclature.Valuta PayValuta {
        //    get { return _PayValuta; }
        //    set { SetPropertyValue("Valuta", ref _PayValuta, value); }
        //}

        public override void UpdateCost(crmCost sp, Boolean mode) {
            if (PaymentUnit != null)
                PaymentUnit.UpdateCost(sp, mode);
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