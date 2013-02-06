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
        public XPCollection<crmDeliveryUnit> DeliveryUnits {
            get {
                return GetCollection<crmDeliveryUnit>("DeliveryUnits");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmDeliveryUnit
    {
        private DeliveryPlan _DeliveryPlan;
        [Association("crmDeliveryPlan-DeliveryUnits")]
        public DeliveryPlan DeliveryPlan {
            get { return _DeliveryPlan; }
            set {
                if (!IsLoading) {
                    if (value != null) {
                        this.Debitor = value.Supplier;
                        this.Creditor = value.Customer;
                        this.CostModel = value.CostModel;
                        this.Valuta = value.Valuta;
                        this.fmOrder = value.fmOrder;
                    }
                }
                SetPropertyValue<DeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value);
            }
        }
    }

}
