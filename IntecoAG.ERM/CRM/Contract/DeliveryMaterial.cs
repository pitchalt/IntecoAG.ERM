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
    /// ����� DeliveryItem, �������������� ���� ����� �� ��������
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDeliveryMaterial : crmDeliveryItem
    {
        public crmDeliveryMaterial(Session session) : base(session) { }
        public crmDeliveryMaterial(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ���� ������

        #endregion


        #region �������� ������
        public override CS.Nomenclature.Nomenclature Nomenclature {
            get { return this.Material; }
        }
        /// <summary>
        /// �������������
        /// </summary>
        private CS.Nomenclature.Material _Material;
        public CS.Nomenclature.Material Material {
            get { return _Material; }
            set { SetPropertyValue<CS.Nomenclature.Material>("Material", ref _Material, value); }
        }

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