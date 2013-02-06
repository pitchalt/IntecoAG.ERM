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
using System.ComponentModel;

using DevExpress.Xpo;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmDealVersion
    {
        [Association("crmDealVersion-DeliveryPlans"), Aggregated]
        [Browsable(false)]
        public XPCollection<crmDeliveryPlan> DeliveryPlans {
            get {
                return GetCollection<crmDeliveryPlan>("DeliveryPlans");
            }
        }
    }
}

namespace IntecoAG.ERM.CRM.Contract.Obligation {

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmDeliveryPlan
    {
//        private crmDealVersion _DealVersion;
        private XPDelayedProperty _DealVersion = new XPDelayedProperty();
        [Association("crmDealVersion-DeliveryPlans")]
        [Browsable(false)]
        [Delayed("_DealVersion", true)]
        public crmDealVersion DealVersion {
            get { return (crmDealVersion) _DealVersion.Value; }
            set { _DealVersion.Value = value; }
                //SetDelayedPropertyValue<crmDealVersion>("DealVersion", value); }
                // В момент включения DeliveryPlan в коллекцию к DealVersion, брать нужные свойства
                //bool newLining = (DealVersion == null);

                // SetDelayedPropertyValue<crmDealVersion>("DealVersion", value);
                //if (!IsLoading && newLining && DealVersion != null) {
//                if (!IsLoading && value != null) {
//                    this.CostItem = value.CostItem;
//                    this.Order = value.Order;
//                    this.CostModel = value.CostModel;
//                    this.Valuta = value.Valuta;
//                    this.NDSRate = value.NDSRate;
////                    this.Debitor = value.Customer;
////                    this.Creditor = value.Supplier;
//                }
//            }
        }
    }

}
