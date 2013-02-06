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
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS.Finance;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    public enum DeliveryUnitType {
        LIST_ITEMS = 0,
        SINGLE_MATERIAL = 1,
        SINGLE_SERVICE = 2
    }

    /// <summary>
    /// Класс DeliveryUnit, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDeliveryUnit : crmObligationUnit //BaseObject, IVersionSupport
    {
        public crmDeliveryUnit(Session session) : base(session) { }
        public crmDeliveryUnit(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.Code = "Поставка";
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        //
        crmDeliveryItem _DeliveryItem;
        /// <summary>
        /// 
        /// </summary>
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmDeliveryItem DeliveryItem {
            get { return this._DeliveryItem; }
            set { SetPropertyValue<crmDeliveryItem>("DeliveryItem", ref _DeliveryItem, value); }
        }
        //
        DeliveryUnitType _DeliveryUnitType;
        /// <summary>
        /// 
        /// </summary>
        public DeliveryUnitType DeliveryUnitType {
            get { return this._DeliveryUnitType; }
            set {
                DeliveryUnitType old = this.DeliveryUnitType;
                if (old == value) return;
                SetPropertyValue<DeliveryUnitType>("DeliveryUnitType", ref _DeliveryUnitType, value);
                if (!IsLoading) {
                    DeliveryUnitTypeSet(value);
                }
            }
        }
        //
        public void DeliveryUnitTypeSet(DeliveryUnitType unit_type) {
            DeliveryUnitType old = this._DeliveryUnitType;
            this.DeliveryItem = null;
            switch (unit_type) {
                case DeliveryUnitType.SINGLE_MATERIAL:
                    DeliveryItemsClear();
                    this.DeliveryItem = new crmDeliveryMaterial(this.Session, this.VersionState);
                    this.DeliveryItems.Add(this.DeliveryItem);
                    break;
                case DeliveryUnitType.SINGLE_SERVICE:
                    DeliveryItemsClear();
                    this.DeliveryItem = new crmDeliveryService(this.Session, this.VersionState);
                    this.DeliveryItems.Add(this.DeliveryItem);
                    break;
                case DeliveryUnitType.LIST_ITEMS:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeliveryItemsClear() {
            List<crmDeliveryItem> dil = new List<crmDeliveryItem>();
            foreach (crmDeliveryItem di in this.DeliveryItems)
                dil.Add(di);
            foreach (crmDeliveryItem di in dil) {
                if (di != this.DeliveryItem) {
                    this.DeliveryItems.Remove(di);
                    di.Delete();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override crmContractParty Debitor {
            get { return this.DeliveryPlan.Debitor; }
        }

        // Получатель оплаты, он же Испольнитель/Поставщиу
        // private crmContractParty _Creditor;
        public override crmContractParty Creditor {
            get { return this.DeliveryPlan.Creditor; }
        }


        public override fmOrder Order {
            get { return base.Order; }
            set {
                fmOrder oldValue = base.Order;
                if (!IsLoading) {
                    foreach (crmDeliveryItem di in this.DeliveryItems) {
                        //di.Order = value;
                        if (di.Order == oldValue | di.Order == null) di.Order = value;
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
                if (!this.IsLoading) {
                    foreach (crmDeliveryItem item in this.DeliveryItems) {
                        if (item.Valuta == oldValue | item.Valuta == null) item.Valuta = value;
                    }
                }
                base.Valuta = value;
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
                    foreach (crmDeliveryItem item in this.DeliveryItems) {
                        if (item.CostModel == oldValue | item.CostModel == null) item.CostModel = value;
                    }
                }
                base.CostModel = value;
            }
        }

        //private crmFinancialDealVersion _FinancialDeal;
        //[Aggregated]
        //public crmFinancialDealVersion FinancialDeal {
        //    get { return _FinancialDeal; }
        //    set { SetPropertyValue<crmFinancialDealVersion>("FinancialDeal", ref _FinancialDeal, value); }
        //}

        //public override crmStage Stage {
        //    get { return DeliveryStage; }
        //}

        //private crmStage _DeliveryStage;
        ////[Association("crmStage-DeliveryUnits")]
        //[DataSourceProperty("DealVersion.StageStructure.Stages")]
        public override crmStage Stage {
            get { return  this.DeliveryPlan.Stage; }
        }

        public override fmCostItem CostItem {
            get { return base.CostItem; }
            set {
                fmCostItem oldValue = base.CostItem;
                if (!this.IsLoading) {
                    foreach (crmDeliveryItem item in this.DeliveryItems) {
                        if (item.CostItem == oldValue | item.CostItem == null) item.CostItem = value;
                    }
                }
                base.CostItem = value;
            }
        }

        //private csNDSRate _NDSRate;
        // Ставка НДС (VAT Rate)
        // [Size(30)]
        //[RuleRequiredField("crmDealWithoutStageVersion.NDSRate.Required", "Save")]
        //[RuleRequiredField("crmDealWithoutStageVersion.Number.Required.Immediate", "Immediate")]
        public override csNDSRate NDSRate {
            get { return base.NDSRate; }
            set {
                csNDSRate oldValue = base.NDSRate;
                base.NDSRate = value;
                if (!this.IsLoading && value != null) {
                    foreach (crmDeliveryItem item in this.DeliveryItems) {
                        if (item.NDSRate == oldValue | item.NDSRate == null) item.NDSRate = value;
                    }
                }
                //SetPropertyValue<csNDSRate>("NDSRate", ref _NDSRate, value);
            }
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