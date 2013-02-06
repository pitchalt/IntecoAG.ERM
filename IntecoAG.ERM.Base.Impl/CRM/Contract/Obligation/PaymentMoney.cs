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
//using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Класс PaymentOfBill, представляющий документ оплаты векселем
    /// </summary>
    //[DefaultClassOptions]
//    [Persistent("crmPaymentOfBill")]
    [MapInheritance(MapInheritanceType.ParentTable)]
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

        // SHU!!! 2011-08-24 Нестыковка по типам. Пересмотреть, что здесь делается.
        //private CS.Nomenclature.csNomenclature _Nomenclature;
        //public override CS.Nomenclature.csNomenclature Nomenclature {
        //    get { return this.AccountValuta; }
            //set { this.AccountValuta = value; }
            //set { SetPropertyValue<CS.Nomenclature.csNomenclature>("Nomenclature", ref _Nomenclature, value); }
        //}
        ///// <summary>
        ///// Номентклатура
        ///// </summary>
        //private CS.csNomenclature.csValuta _PayValuta;
        //public CS.csNomenclature.csValuta PayValuta {
        //    get { return _PayValuta; }
        //    set { SetPropertyValue("csValuta", ref _PayValuta, value); }
        //}

        //public override void UpdateCost(crmCost sp, Boolean mode) {
        //    if (PaymentUnit != null)
        //        PaymentUnit.UpdateCost(sp, mode);
        //}

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