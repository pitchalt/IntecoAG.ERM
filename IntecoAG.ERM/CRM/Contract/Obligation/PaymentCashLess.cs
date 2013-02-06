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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CS.Finance;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{

    /// <summary>
    /// Класс PaymentUnit, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmPaymentCasheLess : crmPaymentUnit   //BaseObject, IVersionSupport
    {
        public crmPaymentCasheLess(Session session) : base(session) { }
        public crmPaymentCasheLess(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.Payment = new crmPaymentMoney(this.Session, this.VersionState);
            this.PaymentItems.Add(this.Payment);
        }

        private crmPaymentMoney _Payment;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmPaymentMoney Payment {
            get { return _Payment; }
            set { SetPropertyValue("Payment", ref _Payment, value); }
        }

        //public Decimal ObligationSumma;
        /// <summary>
        /// 
        /// </summary>
        public override decimal SummFull {
            get { return base.SummFull; }
            set {
                if (this.Payment != null) {
                    if (this.NDSRate != null) {
                        this.Payment.CurrentCost.SummNDS = value * this.NDSRate.Numerator / (100 + this.NDSRate.Numerator);
                        this.Payment.CurrentCost.SummCost = value - this.Payment.CurrentCost.SummNDS;
                    }
                    else {
                        this.Payment.CurrentCost.SummCost = value;
                        this.Payment.CurrentCost.SummNDS = 0;
                    }
                } 
            }
        }
    }

}