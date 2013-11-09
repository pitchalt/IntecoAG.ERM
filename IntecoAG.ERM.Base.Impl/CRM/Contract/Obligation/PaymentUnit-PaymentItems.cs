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

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.CRM.Contract.Obligation
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

        public crmPaymentItem PaymentItemsCreateMoney() {
            crmPaymentMoney pay_item = new crmPaymentMoney(this.Session, this.VersionState);
            PaymentItems.Add(pay_item);
            return pay_item;
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
                        this.CostItem = value.CostItem;
                        this.Valuta = value.Valuta;
                        this.AccountValuta = value.Valuta;
//                        this.ObligationValuta = value.Valuta;
                        this.Order = value.Order;
                        this.CurrentCost.UpCol = value.CurrentCost;
                    }
                }
                SetPropertyValue<crmPaymentUnit>("PaymentUnit", ref _PaymentUnit, value);
            }
        }
    }

}
