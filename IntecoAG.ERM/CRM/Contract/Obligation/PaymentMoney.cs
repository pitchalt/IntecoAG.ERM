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
    /// ����� PaymentOfBill, �������������� �������� ������ ��������
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


        #region ���� ������

        #endregion


        #region �������� ������

        // SHU!!! 2011-08-24 ���������� �� �����. ������������, ��� ����� ��������.
        //private CS.Nomenclature.csNomenclature _Nomenclature;
        //public override CS.Nomenclature.csNomenclature Nomenclature {
        //    get { return this.AccountValuta; }
            //set { this.AccountValuta = value; }
            //set { SetPropertyValue<CS.Nomenclature.csNomenclature>("Nomenclature", ref _Nomenclature, value); }
        //}
        ///// <summary>
        ///// �������������
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


        #region ������

        ///// <summary>
        ///// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
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