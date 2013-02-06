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

using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс Obligation, представляющий обязательства по Договора
    /// </summary>
    [Persistent("crmObligation")]
    public abstract partial class crmObligation : VersionRecord   //BaseObject
    {
        public crmObligation(Session session) : base(session) { }
        public crmObligation(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        private fmOrder _fmOrder;
        public fmOrder fmOrder {
            get { return _fmOrder; }
            set { SetPropertyValue<fmOrder>("fmOrder", ref _fmOrder, value); }
        }

        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set {
                if (!IsLoading) {
                //    foreach (crmStage ss in SubStages)
                //        ss.CurrentCostModel = value;
                    UpdateCurrentCost(value, Valuta);
                }
                SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value);
            }
        }
        private crmCost _CurrentCost;
//        [Browsable(false)]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public crmCost CurrentCost {
            get { return _CurrentCost; }
            set { SetPropertyValue<crmCost>("CurrentCost", ref _CurrentCost, value); }
        }
//
        private Valuta _Valuta;
        public Valuta Valuta {
            get { return _Valuta; }
            set {
                if (!IsLoading) {
                    UpdateCurrentCost(CostModel, value);
                }
                SetPropertyValue<Valuta>("Valuta", ref _Valuta, value); 
            }
        }
//
        [Association("crmObligation-crmCost"), Aggregated]
        public XPCollection<crmCost> ObligationCosts {
            get { return GetCollection<crmCost>("ObligationCosts"); }
        }
        #endregion


        #region МЕТОДЫ КЛАССА
        void UpdateCurrentCost(crmCostModel cm, Valuta vl ) {
            UpdateCost(CurrentCost, false);
            crmCost old_cost = CurrentCost;
            CurrentCost = null;
            if (cm == null || vl == null) return;
            foreach (crmCost sp in ObligationCosts) {
                if (sp.CostModel == cm && sp.Valuta == vl) {
                    CurrentCost = sp;
                    break;
                }
            }
            if (CurrentCost == null) {
                if (old_cost == null) {
                    CurrentCost = new crmCost(this.Session);
                } 
                else {
                    if (old_cost.Valuta != vl )
                        CurrentCost = new crmCost(this.Session);
                    else 
                        CurrentCost = old_cost.Copy();
                }
                CurrentCost.CostModel = cm;
                CurrentCost.Valuta = vl;
                ObligationCosts.Add(CurrentCost);
            }
            UpdateCost(CurrentCost, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract void  UpdateCost(crmCost sp, Boolean mode);

        #endregion

    }

}