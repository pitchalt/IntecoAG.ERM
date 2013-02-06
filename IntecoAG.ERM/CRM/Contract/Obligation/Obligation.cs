
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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Класс Obligation, представляющий обязательства по Договора
    /// </summary>
    [Persistent("crmObligation")]
    public abstract partial class crmObligation : VersionRecord   //BaseObject
    {
        public crmObligation(Session session) : base(session) { }
        public crmObligation(Session session, VersionStates state) : base(session, state) { }

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
        //
        private String _Description;
        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        //[RuleRequiredField("crmObligation.Order.Required", DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false)]
        public virtual String Description {
            get { return _Description; }
            set { SetPropertyValue<String>("Description", ref _Description, value); }
        }
        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        private fmOrder _Order;
        //[RuleRequiredField("crmObligation.Order.Required", DefaultContexts.Save)]
        public virtual fmOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmOrder>("Order", ref _Order, value); }
        }
        /// <summary>
        /// CostItem
        /// </summary>
        private fmCostItem _CostItem;
        public fmCostItem CostItem {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }
        //
        //private crmStage _Stage;
        //public virtual crmStage Stage {
        //    get { return this.ObligationUnit.; }
        //    set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        //}
        public abstract crmStage Stage {
            get;
        }
        //
        //private csValuta _Valuta;
        [NonPersistent]
        public virtual csValuta Valuta {
            get { return this.CurrentCost.Valuta; }
            set {
                this.CurrentCost.Valuta = value;
                OnChanged("Valuta");
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        //private crmCostModel _CostModel;
        [NonPersistent]
        public virtual crmCostModel CostModel {
            get { return this.CurrentCost.CostModel; }
            set {
                this.CurrentCost.CostModel = value;
                OnChanged("CostModel");
            }
        }
        //
        [NonPersistent]
        public virtual csNDSRate NDSRate {
            get { return this.CurrentCost.NDSRate; }
            set {
                this.CurrentCost.NDSRate = value;
                OnChanged("NDSRate");
            }
        }
        //
        [NonPersistent]
        public virtual decimal SummCost {
            get { return this.CurrentCost.SummCost; }
            set {
                this.CurrentCost.SummCost = value;
                OnChanged("SummCost");
            }
        }
        //
        [NonPersistent]
        public virtual decimal SummNDS {
            get { return this.CurrentCost.SummNDS; }
            set {
                this.CurrentCost.SummNDS = value;
                OnChanged("SummNDS");
            }
        }
        //
        Decimal _SummFull;
        /// <summary>
        /// 
        /// </summary>
        public virtual Decimal SummFull {
            get { return _SummFull; }
            set { SetPropertyValue<Decimal>("SummFull", ref _SummFull, value); }
        }
        //
        private crmCostCol _CurrentCost;
        [Browsable(false)]
        [Aggregated]
        //[ExpandObjectMembers(ExpandObjectMembers.Never)]
        public crmCostCol CurrentCost {
            get { return _CurrentCost; }
            set { SetPropertyValue<crmCostCol>("CurrentCost", ref _CurrentCost, value); }
        }

        //private crmFinancialDealVersion _FinancialDeal;
        ////[Browsable(false)]
        ////[ExpandObjectMembers(ExpandObjectMembers.Always)]
        //public crmFinancialDealVersion FinancialDeal {
        //    get { return _FinancialDeal; }
        //    set { SetPropertyValue<crmFinancialDealVersion>("FinancialDeal", ref _FinancialDeal, value); }
        //}

//
        //[Association("crmObligation-crmCost"), Aggregated]
        //public XPCollection<crmCostItem> ObligationCosts {
        //    get { return GetCollection<crmCost>("ObligationCosts"); }
        //}
        #endregion


        #region МЕТОДЫ КЛАССА
        //void UpdateCurrentCost(crmCostModel cm, csValuta vl ) {
        //    UpdateCost(CurrentCost, false);
        //    crmCostItem old_cost = CurrentCost;
        //    CurrentCost = null;
        //    if (cm == null || vl == null) return;
        //    foreach (crmCostItem sp in ObligationCosts) {
        //        if (sp.CostModel == cm && sp.Valuta == vl) {
        //            CurrentCost = sp;
        //            break;
        //        }
        //    }
        //    if (CurrentCost == null) {
        //        if (old_cost == null) {
        //            CurrentCost = new crmCostItem(this.Session);
        //        } 
        //        else {
        //            if (old_cost.Valuta != vl )
        //                CurrentCost = new crmCostItem(this.Session);
        //            else 
        //                CurrentCost = old_cost.Copy();
        //        }
        //        CurrentCost.CostModel = cm;
        //        CurrentCost.Valuta = vl;
        //        ObligationCosts.Add(CurrentCost);
        //    }
        //    UpdateCost(CurrentCost, true);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public abstract void  UpdateCost(crmCostItem sp, Boolean mode);

        #endregion

    }

}