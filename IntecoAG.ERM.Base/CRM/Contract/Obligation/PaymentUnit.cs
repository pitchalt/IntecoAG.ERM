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
using System.ComponentModel;
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
    public partial class crmPaymentUnit : crmObligationUnit   //BaseObject, IVersionSupport
    {
        public crmPaymentUnit(Session session) : base(session) { }
        public crmPaymentUnit(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.Code = "Оплата";
        }

        #region ПОЛЯ КЛАССА

        #endregion

        #region СВОЙСТВА КЛАССА

        public override crmContractParty Debitor {
            get { return this.PaymentPlan.Debitor; }
        }

        // Получатель оплаты, он же Испольнитель/Поставщиу
        // private crmContractParty _Creditor;
        public override crmContractParty Creditor {
            get { return this.PaymentPlan.Creditor; }
        }

        public override fmCOrder Order {
            get { return base.Order; }
            set {
                fmCOrder oldValue = base.Order;
                if (!IsLoading) {
                    foreach (crmPaymentItem pi in this.PaymentItems) {
                        //pi.Order = value;
                        if (pi.Order == oldValue | pi.Order == null) pi.Order = value;
                    }
                }
                base.Order = value;
            }
        }

        /// <summary>
        /// csValuta
        /// </summary>
        public override csValuta Valuta {
            get { return base.Valuta; }
            set {
                csValuta oldValue = base.Valuta;
                base.Valuta = value;
                if (!this.IsLoading) {
                    foreach (crmPaymentItem item in this.PaymentItems) {
                        if (item.Valuta == oldValue || item.Valuta == null) 
                            item.Valuta = value;
                    }
                }
            }
        }
        //
        private csValuta _PaymentValuta;
        /// <summary>
        /// 
        /// </summary>
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set {
                csValuta oldValue = base.Valuta;
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
                if (!this.IsLoading) {
                    foreach (crmPaymentItem item in this.PaymentItems) {
                        if (item.AccountValuta == oldValue || item.AccountValuta == null) 
                            item.AccountValuta = value;
                    }
                }
            }
        }

        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        public override crmCostModel CostModel {
            get { return base.CostModel; }
            set {
                crmCostModel oldValue = base.CostModel;
                if (!this.IsLoading) {
                    foreach (crmPaymentItem item in this.PaymentItems) {
                        if (item.CostModel == oldValue | item.CostModel == null) item.CostModel = value;
                    }
                }
                base.CostModel = value;
            }
        }

        //public override crmStage Stage {
        //    get { return PaymentStage; }
        //}

        //private crmStage _PaymentStage;
        //[Association("crmStage-PaymentUnits")]
        //[DataSourceProperty("DealVersion.StageStructure.Stages")]
        //public crmStage PaymentStage {
        //    get { return this._PaymentStage; }
        //    set { SetPropertyValue<crmStage>("Stage", ref _PaymentStage, value); }
        //}

        public override fmCostItem CostItem {
            get { return base.CostItem; }
            set {
                fmCostItem oldValue = base.CostItem;
                if (!this.IsLoading) {
                    foreach (crmPaymentItem item in this.PaymentItems) {
                        if (item.CostItem == oldValue | item.CostItem == null) item.CostItem = value;
                    }
                }
                base.CostItem = value;
            }
        }

        //private csNDSRate _NDSRate;
        // Ставка НДС (VAT Rate)
        //[Size(30)]
        //[RuleRequiredField("crmDealWithoutStageVersion.NDSRate.Required", "Save")]
        //[RuleRequiredField("crmDealWithoutStageVersion.Number.Required.Immediate", "Immediate")]
        public override csNDSRate NDSRate {
            get { return base.NDSRate; }
            set {
                csNDSRate oldValue = base.NDSRate;
                base.NDSRate = value;
                if (!this.IsLoading && value != null) {
                    foreach (crmPaymentItem item in this.PaymentItems) {
                        if (item.NDSRate == oldValue | item.NDSRate == null) item.NDSRate = value;
                    }
                }
                //SetPropertyValue<csNDSRate>("NDSRate", ref _NDSRate, value);
            }
        }

        public override crmStage Stage {
            get {
                return this.PaymentPlan.Stage;
            }
        }

        #endregion

        #region МЕТОДЫ

        //public crmObligationUnitMain GetObligationUnitMain() {
        //    if (this.ObligationUnitMain == null) {
        //        this.ObligationUnitMain = new crmObligationUnitMain(this.Session);
        //        this.ObligationUnitMain.Current = this;
        //    }
        //    return this.ObligationUnitMain;
        //}

        #endregion

    }

}