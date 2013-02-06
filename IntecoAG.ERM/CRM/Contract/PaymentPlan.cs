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
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс PaymentPlan, представляющий план работ по Договору
    /// </summary>
    [Persistent("crmPaymentPlan")]
    public partial class PaymentPlan : VersionRecord   //BasePlan, IVersionSupport
    {
        public PaymentPlan(Session session) : base(session) { }  
        public PaymentPlan(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmContractParty _Customer;
        public crmContractParty Customer {
            get { return _Customer; }
            set { 
                SetPropertyValue<crmContractParty>("Customer", ref _Customer, value);
                if (!IsLoading) {
                    foreach (crmPaymentUnit du in PaymentUnits)
                        du.Debitor = value;
                }
            }
        }

        private crmContractParty _Supplier;
        public crmContractParty Supplier {
            get { return _Supplier; }
            set { 
                SetPropertyValue<crmContractParty>("Supplier", ref _Supplier, value);
                if (!IsLoading) {
                    foreach (crmPaymentUnit du in PaymentUnits)
                        du.Creditor = value;
                }
            }
        }
        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        private fmOrder _fmOrder;
        public fmOrder fmOrder {
            get { return _fmOrder; }
            set {
                SetPropertyValue<fmOrder>("fmOrder", ref _fmOrder, value);
                if (!IsLoading) {
//                    foreach (crmDeliveryUnit du in this.DeliveryUnits)
//                        du.fmOrder = value;
                }
            }
        }

        /// <summary>
        /// Valuta
        /// </summary>
        private Valuta _Valuta;
        public Valuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<Valuta>("Valuta", ref _Valuta, value);
                if (!IsLoading) {
                    foreach (crmPaymentUnit du in PaymentUnits)
                        du.Valuta = value;
                }
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set {
                SetPropertyValue<crmCostModel>("Costmodel", ref _CostModel, value);
                if (!IsLoading) {
                    foreach (crmPaymentUnit du in PaymentUnits)
                        du.CostModel = value;
                }
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        private crmStageStructure _StageStructure;
        [Browsable(false)]
        public crmStageStructure StageStructure {
            get { return _StageStructure; }
            set { SetPropertyValue<crmStageStructure>("StageStructure", ref _StageStructure, value); }
        }

        [Browsable(false)]
        public XPCollection<crmStage> Stages {
            get {
                if (StageStructure != null)
                    return this.StageStructure.Stages;
                else
                    return null;
            }
        }
        #endregion


        #region МЕТОДЫ
        public crmPaymentUnit PaymentUnitCreate() {
            crmPaymentUnit du = new crmPaymentUnit(this.Session, this.VersionState);
            this.PaymentUnits.Add(du);
            return du;
        }
        #endregion


        #region МЕТОДЫ

        ///// <summary>
        ///// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion

    }

}