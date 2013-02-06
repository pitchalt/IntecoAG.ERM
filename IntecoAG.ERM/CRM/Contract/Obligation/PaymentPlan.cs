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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Contract.Deal;
//using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CS.Finance;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Класс crmPaymentPlan, представляющий план работ по Договору
    /// </summary>
    [Persistent("crmPaymentPlan")]
    public partial class crmPaymentPlan : crmObligationPlan   //VersionRecord   //BasePlan, IVersionSupport
    {
        public crmPaymentPlan(Session session) : base(session) { }  
        public crmPaymentPlan(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        public override crmContractParty Sender {
            set {
                crmContractParty oldValue = base.Sender;
                base.Sender = value;
                if (!IsLoading && value != null) {
                    foreach (crmPaymentUnit pu in this.PaymentUnits) {
                        if (pu.Sender == oldValue | pu.Sender == null) pu.Sender = value;
                    }
                }
            }
        }

        // Получатель оплаты, он же Испольнитель/Поставщиу
        public override crmContractParty Receiver {
            set {
                crmContractParty oldValue = base.Receiver;
                base.Receiver = value;
                if (!IsLoading && value != null) {
                    foreach (crmPaymentUnit pu in this.PaymentUnits) {
                        if (pu.Receiver == oldValue | pu.Receiver == null) pu.Receiver = value;
                    }
                }
            }
        }


        //private csNDSRate _NDSRate;
        // Ставка НДС (VAT Rate)
        //[RuleRequiredField("crmDealWithoutStageVersion.NDSRate.Required", "Save")]
        //[RuleRequiredField("crmDealWithoutStageVersion.Number.Required.Immediate", "Immediate")]
        public override csNDSRate NDSRate {
            get { return base.NDSRate; }
            set {
                csNDSRate oldValue = base.NDSRate;
                base.NDSRate = value;
                if (!this.IsLoading && value != null) {
                    foreach (crmPaymentUnit item in this.PaymentUnits) {
                        if (item.NDSRate == oldValue | item.NDSRate == null) item.NDSRate = value;
                    }
                }
            }
        }
        /// <summary>
        /// CostItem
        /// </summary>
        public override fmCostItem CostItem {
            get { return base.CostItem; }
            set {
                fmCostItem oldValue = base.CostItem;
                base.CostItem = value;
                if (!IsLoading && value != null) {
                    foreach (crmPaymentUnit du in this.PaymentUnits) {
                        //du.Order = value;
                        if (du.CostItem == oldValue | du.CostItem == null) du.CostItem = value;
                    }
                }
            }
        }

        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        public override fmOrder Order {
            get { return base.Order; }
            set {
                fmOrder oldValue = base.Order;
                base.Order = value;
                if (!IsLoading && value != null) {
                    foreach (crmPaymentUnit du in this.PaymentUnits) {
                        //du.Order = value;
                        if (du.Order == oldValue | du.Order == null) du.Order = value;
                    }
                }
            }
        }

        /// <summary>
        /// csValuta
        /// </summary>
        //private csValuta _Valuta;
        public override csValuta Valuta {
            get { return base.Valuta; }
            set {
                csValuta oldValue = base.Valuta;
                base.Valuta = value;
                if (!IsLoading && value != null) {
                    foreach (crmPaymentUnit item in PaymentUnits) {
                        if (item.Valuta == oldValue | item.Valuta == null) item.Valuta = value;
                    }
                }
            }
        }
        /// <summary>
        /// csValuta
        /// </summary>
        private csValuta _PaymentValuta;
        public csValuta PaymentValuta {
            get { return this._PaymentValuta; }
            set {
                csValuta old = this._PaymentValuta;
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
                if (!IsLoading && value != null) {
                    foreach (crmPaymentUnit item in PaymentUnits) {
                        if (item.PaymentValuta == null || item.Valuta == old) 
                            item.PaymentValuta = value;
                    }
                }
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        //private crmCostModel _CostModel;
        public override crmCostModel CostModel {
            get { return base.CostModel; }
            set {
                crmCostModel oldValue = base.CostModel;
                base.CostModel = value;
                if (!IsLoading && value != null) {
                    foreach (crmPaymentUnit item in PaymentUnits) {
                        if (item.CostModel == oldValue | item.CostModel == null) item.CostModel = value;
                    }
                }
            }
        }
        // Версия сделки.
//        private crmDealVersion _DealVersionLink;
        //[Delayed]
        //public crmDealVersion DealVersionLink {
        //    get { return GetDelayedPropertyValue<crmDealVersion>("DealVersionLink"); }
        //    set { SetDelayedPropertyValue<crmDealVersion>("DealVersionLink", value); }
        //}

        #endregion


        #region МЕТОДЫ

        public crmPaymentUnit PaymentUnitCreate() {
            crmPaymentUnit du = new crmPaymentUnit(this.Session, this.VersionState);
            this.PaymentUnits.Add(du);
            return du;
        }

        public crmPaymentCasheLess PaymentCasheLessCreate() {
            crmPaymentCasheLess du = new crmPaymentCasheLess(this.Session, this.VersionState);
            this.PaymentUnits.Add(du);
            return du;
        }

        #endregion

    }

}