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
    /// Класс DeliveryPlan, представляющий план работ по Договору
    /// </summary>
    [Persistent("crmDeliveryPlan")]
    public partial class DeliveryPlan : VersionRecord   //BasePlan, IVersionSupport
    {
        public DeliveryPlan(Session session) : base(session) { }
        public DeliveryPlan(Session session, VersionStates state) : base(session, state) { }

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
                    foreach (crmDeliveryUnit du in DeliveryUnits)
                        du.Creditor = value;
                }
            }
        }

        private crmContractParty _Supplier;
        public crmContractParty Supplier {
            get { return _Supplier; }
            set { 
                SetPropertyValue<crmContractParty>("Supplier", ref _Supplier, value);
                if (!IsLoading) {
                    foreach (crmDeliveryUnit du in DeliveryUnits)
                        du.Debitor = value;
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
                    foreach (crmDeliveryUnit du in this.DeliveryUnits)
                        du.fmOrder = value;
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
                    foreach (crmDeliveryUnit du in DeliveryUnits)
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
                    foreach (crmDeliveryUnit du in DeliveryUnits)
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
        public crmDeliveryUnit DeliveryUnitCreate() {
            crmDeliveryUnit du = new crmDeliveryUnit(this.Session, this.VersionState);
            this.DeliveryUnits.Add(du);
            return du;
        }
        #endregion

    }

}