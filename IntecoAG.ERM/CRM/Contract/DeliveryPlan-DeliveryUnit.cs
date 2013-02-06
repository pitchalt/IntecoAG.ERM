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
using DevExpress.Xpo;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class DeliveryPlan
    {
        [Association("crmDeliveryPlan-DeliveryUnits"), Aggregated]
        public XPCollection<DeliveryUnit> DeliveryUnits {
            get {
                return GetCollection<DeliveryUnit>("DeliveryUnits");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class DeliveryUnit
    {
        private DeliveryPlan _DeliveryPlan;
        [Association("crmDeliveryPlan-DeliveryUnits")]
        public DeliveryPlan DeliveryPlan {
            get { return _DeliveryPlan; }
            set { SetPropertyValue<DeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value); }
        }
    }

}
