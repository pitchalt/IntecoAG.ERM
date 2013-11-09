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
    /// Класс DeliveryItem, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDeliveryService : crmDeliveryItem
    {
        public crmDeliveryService(Session session) : base(session) { }
        public crmDeliveryService(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        public override CS.Nomenclature.csNomenclature Nomenclature {
            get { return this.Service;  }
        }
        /// <summary>
        /// Номентклатура
        /// </summary>
        private CS.Nomenclature.csService _Service;
        public CS.Nomenclature.csService Service {
            get { return _Service; }
            set { 
                SetPropertyValue("csService", ref _Service, value);
                if (!IsLoading) {
                    if (String.IsNullOrEmpty(this.NomenclatureName))
                        this.NomenclatureName = this.Service.NameShort;
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