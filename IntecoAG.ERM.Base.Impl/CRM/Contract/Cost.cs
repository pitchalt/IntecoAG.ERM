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
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс crmStageCost, связывает обсновные обязательства с модельями цен (много ко многим)
    /// </summary>
    [Persistent("crmCostValue")]
    public partial class crmCostValue : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmCostValue(Session session) : base(session) { }
        public crmCostValue(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА
        protected Boolean IsUpdate;
        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        ///  
        /// </summary>
        private crmCostCol _CostCol;
        [Association("crmCostCol-CostItems")]
        [Browsable(false)]
        public crmCostCol CostCol {
            get { return _CostCol; }
            set { SetPropertyValue<crmCostCol>("CostCol", ref _CostCol, value); }
        }
        /// <summary>
        ///  - Основные Обязательства
        /// </summary>
        //private crmObligation _Obligation;
        //[Association("crmObligation-crmCost")]
        //[Browsable(false)]
        //public crmObligation Obligation {
        //    get { return _Obligation; }
        //    set { SetPropertyValue<crmObligation>("Obligation", ref _Obligation, value); }
        //}
        /// <summary>
        ///  - Основные Обязательства
        /// </summary>
        //private crmObligationUnit _ObligationUnit;
        //[Association("crmObligationUnit-crmCost")]
        //[Browsable(false)]
        //public crmObligationUnit ObligationUnit {
        //    get { return _ObligationUnit; }
        //    set { SetPropertyValue<crmObligationUnit>("ObligationUnit", ref _ObligationUnit, value); }
        //}

        ///// <summary>
        ///// Этап
        ///// </summary>
        //private crmStage _Stage;
        //[Association("crmStage-crmCost")]
        //[Browsable(false)]
        //public crmStage Stage {
        //    get { return _Stage; }
        //    set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        //}

        /// <summary>
        /// CostModel
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set { SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value); }
        }
        //
        private csNDSRate _NDSRate;
        public csNDSRate NDSRate {
            get { return _NDSRate; }
            set { SetPropertyValue<csNDSRate>("NDSRate", ref _NDSRate, value); }
        }
//        public virtual csValuta PriceValuta {
//            get { return null; }
//        }

        protected decimal _SummCost;
        public decimal SummCost {
            get { return _SummCost; }
            set {
                if (!IsLoading) {
                    if (this.CostCol != null)
                        this.CostCol.UpdateCostBefore(this);
                }
                SetPropertyValue<decimal>("SummCost", ref _SummCost, value);
                if (!IsLoading) {
                    if (this.NDSRate != null)
                        if (this.NDSRate.Denominator != 0)
                            this.SummNDS = this._SummCost * this.NDSRate.Numerator / this.NDSRate.Denominator;
                        else
                            this.SummNDS = 0;
                    if (this.CostCol != null)
                        this.CostCol.UpdateCostAfter(this);
                    OnChanged("SummFull");
                }
                //OnChanged("SummFull");
                //if (!IsLoading) {
                //    if (!IsUpdate && Obligation != null) 
                //        Obligation.UpdateCost(this, true);
                //}
            }
        }

        protected decimal _SummNDS;
        public decimal SummNDS {
            get { return _SummNDS; }
            set {
                if (!IsLoading) {
                    if (this.CostCol != null)
                        this.CostCol.UpdateCostBefore(this);
                }
                SetPropertyValue<decimal>("SummNDS", ref _SummNDS, value);
                if (!IsLoading) {
                    OnChanged("SummFull");
                    if (this.CostCol != null)
                        this.CostCol.UpdateCostAfter(this);
                }
            }
        }
        [Persistent("SummFull")]
        protected decimal _SummFull;
        [PersistentAlias("_SummFull")]
        public decimal SummFull {
            get { return SummNDS + SummCost; }
        }
        /// <summary>
        /// csValuta
        /// </summary>
        private csValuta _Valuta;
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<csValuta>("Valuta", ref _Valuta, value); }
        }

        #endregion


        #region МЕТОДЫ
        public void UpdateCost(crmCostValue sp, Boolean mode) {
            this.IsUpdate = true;
            if (mode) {
                this.SummCost = this.SummCost + sp.SummCost;
                this.SummNDS = this.SummNDS + sp.SummNDS;
            }
            else {
                this.SummCost = this.SummCost - sp.SummCost;
                this.SummNDS = this.SummNDS - sp.SummNDS;
            }
            this.IsUpdate = false;
        }
        //
        public crmCostValue Copy() {
            crmCostValue sp = new crmCostValue(this.Session);
            sp._CostModel = this.CostModel;
            sp._NDSRate = this.NDSRate;
            sp._Valuta = this.Valuta;
//            sp._Stage = this.Stage;
//            sp._Obligation = this.Obligation;
//            sp._ObligationUnit = this.ObligationUnit;
            sp._SummCost = this.SummCost;
            sp._SummFull = this.SummFull;
            sp._SummNDS = this.SummNDS;
            sp._Valuta = this.Valuta;
            return sp;
        }

        #endregion

    }

}