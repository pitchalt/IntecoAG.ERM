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

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс Cost, связывает обсновные обязательства с модельями цен (много ко многим)
    /// </summary>
    [Persistent("crmCost")]
    public partial class CostDelivery : VersionRecord   //BaseObject, IVersionSupport
    {
        public CostDelivery(Session session) : base(session) { }
        public CostDelivery(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// DeliveryItem - Это Основные Обязательства
        /// </summary>
        private DeliveryItem _DeliveryItem;
        public DeliveryItem DeliveryItem {
            get { return _DeliveryItem; }
            set { SetPropertyValue<DeliveryItem>("DeliveryItem", ref _DeliveryItem, value); }
        }

        /// <summary>
        /// CostModel
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set { SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value); }
        }

        /// <summary>
        /// Price - цена
        /// </summary>
        private decimal _Price;
        public decimal Price {
            get { return _Price; }
            set { SetPropertyValue<decimal>("Price", ref _Price, value); }
        }

        /// <summary>
        /// Valuta
        /// </summary>
        private Valuta _Valuta;
        public Valuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<Valuta>("Valuta", ref _Valuta, value); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}