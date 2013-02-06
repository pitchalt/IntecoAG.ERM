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
    public partial class crmPaymentPlan
    {
        [Association("crmPaymentPlan-PaymentUnits"), Aggregated]
        public XPCollection<crmPaymentUnit> PaymentUnits {
            get {
                return GetCollection<crmPaymentUnit>("PaymentUnits");
            }
        }
    }
}

namespace IntecoAG.ERM.CRM.Contract.Obligation {

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmPaymentUnit
    {
        private crmPaymentPlan _PaymentPlan;
        [Association("crmPaymentPlan-PaymentUnits")]
        public crmPaymentPlan PaymentPlan {
            get { return _PaymentPlan; }
            set { 
                SetPropertyValue<crmPaymentPlan>("PaymentPlan", ref _PaymentPlan, value);
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
                }
            }
        }
    }

}
