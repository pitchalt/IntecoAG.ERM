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
    public partial class crmPaymentUnit
    {
        [Association("crmPaymentUnit-PaymentItems"), Aggregated]
        public XPCollection<crmPaymentItem> PaymentItems {
            get {
                return GetCollection<crmPaymentItem>("PaymentItems");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmPaymentItem
    {
        private crmPaymentUnit _PaymentUnit;
        [Association("crmPaymentUnit-PaymentItems")]
        public crmPaymentUnit PaymentUnit {
            get { return _PaymentUnit; }
            set {
                if (!IsLoading) {
                    if (value != null) {
                        this.CostModel = value.CostModel;
                        this.Valuta = value.Valuta;
                        this.fmOrder = value.fmOrder;
                    }
                }
                SetPropertyValue<crmPaymentUnit>("PaymentUnit", ref _PaymentUnit, value);
            }
        }
    }

}
