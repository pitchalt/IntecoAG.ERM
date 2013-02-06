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
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс crmStageCost, связывает обсновные обязательства с модельями цен (много ко многим)
    /// </summary>
    [Persistent("crmCost")]
    public partial class crmCost : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmCost(Session session) : base(session) { }
        public crmCost(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА
        protected Boolean IsUpdate;
        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        ///  - Основные Обязательства
        /// </summary>
        private crmObligation _Obligation;
        [Association("crmObligation-crmCost")]
        [Browsable(false)]
        public crmObligation Obligation {
            get { return _Obligation; }
            set { SetPropertyValue<crmObligation>("Obligation", ref _Obligation, value); }
        }
        /// <summary>
        ///  - Основные Обязательства
        /// </summary>
        private crmObligationUnit _ObligationUnit;
        [Association("crmObligationUnit-crmCost")]
        [Browsable(false)]
        public crmObligationUnit ObligationUnit {
            get { return _ObligationUnit; }
            set { SetPropertyValue<crmObligationUnit>("ObligationUnit", ref _ObligationUnit, value); }
        }

        /// <summary>
        /// Этап
        /// </summary>
        private crmStage _Stage;
        [Association("crmStage-crmCost")]
        [Browsable(false)]
        public crmStage Stage {
            get { return _Stage; }
            set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        }

        /// <summary>
        /// CostModel
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set { SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value); }
        }

//        public virtual Valuta PriceValuta {
//            get { return null; }
//        }

        protected decimal _SummCost;
        public decimal SummCost {
            get { return _SummCost; }
            set {
                if (!IsLoading) {
                    if (!IsUpdate)
                        if (Obligation == null)
                            value = _SummCost;
                        else
                            Obligation.UpdateCost(this, false);
                }
                SetPropertyValue<decimal>("SummCost", ref _SummCost, value);
                OnChanged("SummFull");
                if (!IsLoading) {
                    if (!IsUpdate && Obligation != null) 
                        Obligation.UpdateCost(this, true);
                }
            }
        }

        protected decimal _SummNDS;
        public decimal SummNDS {
            get { return _SummNDS; }
            set {
                if (!IsLoading) {
                    if (!IsUpdate)
                        if (Obligation == null)
                            value = _SummCost;
                        else
                            Obligation.UpdateCost(this, false);
                }
                SetPropertyValue<decimal>("SummNDS", ref _SummNDS, value);
                OnChanged("SummFull");
                if (!IsLoading) {
                    if (!IsUpdate && Obligation != null)
                        Obligation.UpdateCost(this, true);
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
        /// Valuta
        /// </summary>
        private Valuta _Valuta;
        public Valuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<Valuta>("Valuta", ref _Valuta, value); }
        }

        #endregion


        #region МЕТОДЫ
        public void UpdateCost(crmCost sp, Boolean mode) {
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
        public crmCost Copy() {
            crmCost sp = new crmCost(this.Session);
            sp._CostModel = this.CostModel;
            sp._Stage = this.Stage;
            sp._Obligation = this.Obligation;
            sp._ObligationUnit = this.ObligationUnit;
            sp._SummCost = this.SummCost;
            sp._SummFull = this.SummFull;
            sp._SummNDS = this.SummNDS;
            sp._Valuta = this.Valuta;
            return sp;
        }

        #endregion

    }

}