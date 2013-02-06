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
using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmDeliveryUnit
    {
        [Association("crmDeliveryUnit-crmDeliveryItem"), Aggregated]
        public XPCollection<crmDeliveryItem> DeliveryItems {
            get {
                return GetCollection<crmDeliveryItem>("DeliveryItems");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmDeliveryItem
    {
        private crmDeliveryUnit _DeliveryUnit;
        [Association("crmDeliveryUnit-crmDeliveryItem")]
        public crmDeliveryUnit DeliveryUnit {
            get { return _DeliveryUnit; }
            set {
                if (!IsLoading) {
                    if (value != null) {
                        this.CostModel = value.CostModel;
                        this.NDSRate = value.NDSRate;
                        this.CostItem = value.CostItem;
                        this.Valuta = value.Valuta;
                        this.Order = value.Order;
                        this.CurrentCost.UpCol = value.CurrentCost;
                    }
                }
                SetPropertyValue<crmDeliveryUnit>("DeliveryUnit", ref _DeliveryUnit, value); 
            }
        }
    }

}
