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
    public partial class PaymentPlan
    {
        [Association("crmPaymentPlan-PaymentUnits"), Aggregated]
        public XPCollection<PaymentUnit> PaymentUnits {
            get {
                return GetCollection<PaymentUnit>("PaymentUnits");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class PaymentUnit
    {
        private PaymentPlan _PaymentPlan;
        [Association("crmPaymentPlan-PaymentUnits")]
        public PaymentPlan PaymentPlan {
            get { return _PaymentPlan; }
            set { SetPropertyValue<PaymentPlan>("PaymentPlan", ref _PaymentPlan, value); }
        }
    }

}
