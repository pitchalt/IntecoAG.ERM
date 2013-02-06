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
        public XPCollection<crmPaymentUnit> PaymentUnits {
            get {
                return GetCollection<crmPaymentUnit>("PaymentUnits");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmPaymentUnit
    {
        private PaymentPlan _PaymentPlan;
        [Association("crmPaymentPlan-PaymentUnits")]
        public PaymentPlan PaymentPlan {
            get { return _PaymentPlan; }
            set { 
                SetPropertyValue<PaymentPlan>("PaymentPlan", ref _PaymentPlan, value);
                if (!IsLoading) {
                    if (value != null) {
                        this.Debitor = value.Customer;
                        this.Creditor = value.Supplier;
                        this.CostModel = value.CostModel;
                        this.Valuta = value.Valuta;
                        this.fmOrder = value.fmOrder;
                    }
                }
            }
        }
    }

}
