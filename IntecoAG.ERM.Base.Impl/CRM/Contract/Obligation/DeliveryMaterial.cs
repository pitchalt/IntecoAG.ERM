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
    /// Класс DeliveryItem, представляющий план работ по Договору
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


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// 
        /// </summary>
        public override csNomenclature Nomenclature {
            get { return this.Material; }
        }
        /// <summary>
        /// Номентклатура
        /// </summary>
        private csMaterial _Material;
        public csMaterial Material {
            get { return _Material; }
            set { 
                SetPropertyValue<csMaterial>("Material", ref _Material, value);
                if (!IsLoading) {
                    if (String.IsNullOrEmpty(this.NomenclatureName))
                        this.NomenclatureName = this.Material.NameShort;
                    UpdateTrwNomenclature();
                }
            }
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