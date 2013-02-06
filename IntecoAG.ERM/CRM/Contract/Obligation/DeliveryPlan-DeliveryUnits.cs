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
//using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmDeliveryPlan
    {
        [Association("crmDeliveryPlan-DeliveryUnits"), Aggregated]
        public XPCollection<crmDeliveryUnit> DeliveryUnits {
            get {
                return GetCollection<crmDeliveryUnit>("DeliveryUnits");
            }
        }
    }
}

namespace IntecoAG.ERM.CRM.Contract.Obligation {

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmDeliveryUnit
    {
        private crmDeliveryPlan _DeliveryPlan;
        [Association("crmDeliveryPlan-DeliveryUnits")]
        public crmDeliveryPlan DeliveryPlan {
            get { return _DeliveryPlan; }
            set {
                if (!IsLoading) {
                    if (value != null) {
                        this.DealVersion = value.DealVersion;
                        this.Sender = value.Sender;
                        this.Receiver = value.Receiver;
                        this.CostModel = value.CostModel;
                        this.NDSRate = value.NDSRate;
                        this.CostItem = value.CostItem;
                        this.Valuta = value.Valuta;
                        this.Order = value.Order;
                        this.CurrentCost.UpCol = value.CurrentCost;
                    }
                    else {
                        if (this.CurrentCost != null) {
                            if (this.CurrentCost.UpCol != null) {
                                this.CurrentCost.UpCol.DownCols.Remove(this.CurrentCost);
                            }
                        }
                    }
                }
                SetPropertyValue<crmDeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value);
            }
        }
    }

}
