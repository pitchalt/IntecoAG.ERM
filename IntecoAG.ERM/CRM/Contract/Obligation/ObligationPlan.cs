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
using System.ComponentModel;
using System.Linq;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
//using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Класс crmObligationPlan, представляющий план обязательств по Договору
    /// </summary>
    [Persistent("crmObligationPlan")]
    public abstract partial class crmObligationPlan : VersionRecord   //BasePlan, IVersionSupport
    {
        public crmObligationPlan(Session session) : base(session) { }
        public crmObligationPlan(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.CurrentCost = new crmCostCol(this.Session);
        }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.CurrentCost.VersionState = this.VersionState;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private XPDelayedProperty _DealVersion = new XPDelayedProperty();
//        [Association("crmDealVersion-DeliveryPlans")]
        [Browsable(false)]
        [Delayed("_DealVersion", true)]
        public crmDealVersion DealVersion {
            get { return (crmDealVersion)_DealVersion.Value; }
            set { _DealVersion.Value = value; }
        }
        //
        private DateTime _DateBegin;
        /// <summary>
        /// DateStart
        /// <summary>
        public virtual DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }
        //
        private DateTime _DateEnd;
        /// <summary>
        /// DateStop
        /// </summary>
        public virtual DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        }
        //
        //private crmContractParty _Debitor;
        [Delayed]
        public virtual crmContractParty Debitor {
            //get { return _Debitor; }
            //set { SetPropertyValue<crmContractParty>("Debitor", ref _Debitor, value); }
            get { return GetDelayedPropertyValue<crmContractParty>("Debitor"); }
            set {
                crmContractParty old = this.Debitor;
                SetDelayedPropertyValue<crmContractParty>("Debitor", value);
                if (!IsLoading) {
                    if (this.Sender == null || this.Sender == old)
                        this.Sender = this.Debitor;
                }
            }
        }
        //
        //private crmContractParty _Creditor;
        [Delayed]
        public virtual crmContractParty Creditor {
            //get { return _Creditor; }
            //set { SetPropertyValue<crmContractParty>("Creditor", ref _Creditor, value); }
            get { return GetDelayedPropertyValue<crmContractParty>("Creditor"); }
            set {
                crmContractParty old = this.Creditor;
                SetDelayedPropertyValue<crmContractParty>("Creditor", value);
                if (!IsLoading) {
                    if (this.Receiver == null || this.Receiver == old)
                        this.Receiver = this.Creditor;
                }
            }
        }
        // Плательщик, он же Заказчик/Полкупатель
        //private crmPhysicalPerson _PersonSender;
        [Delayed]
        public virtual crmContractParty Sender {
            //get { return _PersonSender; }
            //set { SetPropertyValue<crmPhysicalPerson>("PersonSender", ref _PersonSender, value); }
            get { return GetDelayedPropertyValue<crmContractParty>("Sender"); }
            set { SetDelayedPropertyValue<crmContractParty>("Sender", value); }
        }

        // Получатель оплаты, он же Испольнитель/Поставщиу
        //private crmPhysicalPerson _PersonReceiver;
        [Delayed]
        public virtual crmContractParty Receiver {
            //get { return _PersonReceiver; }
            //set { SetPropertyValue<crmPhysicalPerson>("PersonReceiver", ref _PersonReceiver, value); }
            get { return GetDelayedPropertyValue<crmContractParty>("Receiver"); }
            set { SetDelayedPropertyValue<crmContractParty>("Receiver", value); }
        }
        //
        [NonPersistent]
        public virtual csNDSRate NDSRate{
            get { return this.CurrentCost.NDSRate; }
            set {
                this.CurrentCost.NDSRate = value;
                OnChanged("NDSRate");
            }
        }
        //
        [NonPersistent]
        public virtual csValuta Valuta {
            get { return this.CurrentCost.Valuta; }
            set {
                this.CurrentCost.Valuta = value;
                //SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value);
                OnChanged("Valuta");
            }
        }
        //
        [NonPersistent]
        public virtual crmCostModel CostModel {
            get { return this.CurrentCost.CostModel; }
            set {
                this.CurrentCost.CostModel = value;
                //SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value);
                OnChanged("CostModel");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [NonPersistent]
        public virtual decimal SummCost {
            get { return this.CurrentCost.SummCost; }
            //set {
            //    this.CurrentCost.SummCost = value;
            //    OnChanged("SummCost");
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        [NonPersistent]
        public virtual decimal SummNDS {
            get { return this.CurrentCost.SummNDS; }
            //set {
            //    this.CurrentCost.SummNDS = value;
            //    OnChanged("SummCost");
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        [NonPersistent]
        public virtual decimal SummFull {
            get { return this.CurrentCost.SummFull; }
        }
        /// <summary>
        /// 
        /// </summary>
        private crmCostCol _CurrentCost;
        [Aggregated]
        [Browsable(false)]
        //        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public crmCostCol CurrentCost {
            get { return _CurrentCost; }
            set { SetPropertyValue<crmCostCol>("CurrentCost", ref _CurrentCost, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        private fmCostItem _CostItem;
        public virtual fmCostItem CostItem {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }

        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        private fmOrder _Order;
        public virtual fmOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmOrder>("Order", ref _Order, value); }
        }

        #endregion

        private crmStage _Stage;
        public virtual crmStage Stage {
            get { return _Stage; }
            set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        }

        #region МЕТОДЫ
 
        #endregion

    }

}