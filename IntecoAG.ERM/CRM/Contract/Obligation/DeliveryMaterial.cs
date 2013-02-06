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

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
//using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Obligation
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
        /// <summary>
        /// 
        /// </summary>
        public override CS.Nomenclature.csNomenclature Nomenclature {
            get { return this.Material; }
        }
        /// <summary>
        /// �������������
        /// </summary>
        private CS.Nomenclature.csMaterial _Material;
        public CS.Nomenclature.csMaterial Material {
            get { return _Material; }
            set { 
                SetPropertyValue<CS.Nomenclature.csMaterial>("Material", ref _Material, value);
                if (!IsLoading) {
                    this.NomenclatureName = this.Material.NameShort;
                }
            }
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