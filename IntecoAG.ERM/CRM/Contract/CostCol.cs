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
    /// Класс связывает обсновные обязательства с модельями цен (много ко многим)
    /// </summary>
    [Persistent("crmCost")]
    public partial class crmCostCol : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmCostCol(Session session) : base(session) { }
        public crmCostCol(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА
        protected Boolean IsUpdate;
        //
        private crmCostValue _CurrentCost;
        private crmCostModel _CostModel;
        protected csValuta _Valuta;
        protected crmCostCol _UpCol;
        //
        protected decimal _SummCost;
        protected decimal _SummNDS;
        //
        [Persistent("SummFull")]
        protected decimal _SummFull;
        #endregion

        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// CostModel
        /// </summary>
        public crmCostModel CostModel {
            get { return _CostModel; }
            set { 
                SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value);
                if (!IsLoading) {
                    this.CurrentCost = this.GetCostValue(this.CostModel, this.Valuta);
                }
            }
        }
        /// <summary>
        /// csValuta
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set { 
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
                if (!IsLoading) {
                    this.CurrentCost = this.GetCostValue(this.CostModel, this.Valuta);
                }
            }
        }
        private csNDSRate _NDSRate;
        public csNDSRate NDSRate {
            get { return _NDSRate; }
            set { 
                SetPropertyValue<csNDSRate>("NDSRate", ref _NDSRate, value);
                if (!IsLoading) {
                    foreach (crmCostValue ndsrate in this.CostItems) {
                        ndsrate.NDSRate = value;
                    }
                }
            }
        }
        //
        /// <summary>
        /// CostModel
        /// </summary>
        [Browsable(false)]
        public crmCostValue CurrentCost {
            get { return _CurrentCost; }
            set { SetPropertyValue<crmCostValue>("CurrentCost", ref _CurrentCost, value); }
        }
        //
        [Association("crmCostCol-CostItems", typeof(crmCostValue)), Aggregated]
        public XPCollection<crmCostValue> CostItems {
            get { return this.GetCollection<crmCostValue>("CostItems"); }
        }
        //
        [Association("crmCostCol-CostCols")]
        public crmCostCol UpCol {
            get { return this._UpCol; }
            set {
                if (!IsLoading) {
                    if (UpCol != null) {
                        UpCol.SummCost = UpCol.SummCost - this.SummCost;
                        UpCol.SummNDS = UpCol.SummNDS - this.SummNDS;
                    }
                }
                SetPropertyValue<crmCostCol>("UpCol", ref _UpCol, value);
                if (!IsLoading) {
                    if (value != null) {
                        value.SummCost = value.SummCost + this.SummCost;
                        value.SummNDS = value.SummNDS + this.SummNDS;
                    }
                }
            }
        }
        //
        [Association("crmCostCol-CostCols", typeof(crmCostCol)), Aggregated]
        public XPCollection<crmCostCol> DownCols {
            get { return this.GetCollection<crmCostCol>("DownCols"); }
        }
        //
        public decimal SummCost {
            get { return _SummCost; }
            set {
                if (!IsLoading) {
                    if (UpCol != null)
                        UpCol.SummCost = UpCol.SummCost - this.SummCost;
                }
                SetPropertyValue<decimal>("SummCost", ref _SummCost, value);
                if (!IsLoading) {
                    if (!IsLoading) {
                        if (UpCol != null)
                            UpCol.SummCost = UpCol.SummCost + this.SummCost;
                    }
                    OnChanged("SummFull");
                }
            }
        }
        //
        public decimal SummNDS {
            get { return _SummNDS; }
            set {
                if (!IsLoading) {
                    if (UpCol != null)
                        UpCol.SummNDS = UpCol.SummNDS - this.SummNDS;
                }
                SetPropertyValue<decimal>("SummNDS", ref _SummNDS, value);
                if (!IsLoading) {
                    if (!IsLoading) {
                        if (UpCol != null)
                            UpCol.SummNDS = UpCol.SummNDS + this.SummNDS;
                    }
                    OnChanged("SummFull");
                }
            }
        }
        //
        [PersistentAlias("_SummFull")]
        public decimal SummFull {
            get { return SummNDS + SummCost; }
        }
        //

        #endregion


        #region МЕТОДЫ
        //public void UpdateCost(crmCostValue sp, Boolean mode) {
        //    this.IsUpdate = true;
        //    if (mode) {
        //        this.SummCost = this.SummCost + sp.SummCost;
        //        this.SummNDS = this.SummNDS + sp.SummNDS;
        //    }
        //    else {
        //        this.SummCost = this.SummCost - sp.SummCost;
        //        this.SummNDS = this.SummNDS - sp.SummNDS;
        //    }
        //    this.IsUpdate = false;
        //}

        public void UpdateCostBefore(crmCostValue cv) {
            if (this.CurrentCost == cv) {
                this.SummCost = this.SummCost - cv.SummCost;
                this.SummNDS = this.SummNDS - cv.SummNDS;
            }
            if (this.UpCol != null) {
                this.UpCol.UpdateCostBefore(cv);
            }
        }

        public void UpdateCostAfter(crmCostValue cv) {
            if (this.CurrentCost == cv) {
                this.SummCost = this.SummCost + cv.SummCost;
                this.SummNDS = this.SummNDS + cv.SummNDS;
            }
            if (this.UpCol != null) {
                this.UpCol.UpdateCostAfter(cv);
            }
        }

        public crmCostValue GetCostValue(crmCostModel cm, csValuta val) {
            crmCostValue cv = null;
            if (cm == null || val == null)
                return null;
            foreach (crmCostValue ccv in this.CostItems)
                if (ccv.Valuta == val && ccv.CostModel == cm) {
                    cv = ccv;
                }
            if (cv == null) {
                foreach (crmCostValue ccv in this.CostItems)
                    if (ccv.Valuta == val) {
                        cv = ccv.Copy();
                        cv.CostModel = cm;
                    }
                if (cv == null) {
                    cv = new crmCostValue(this.Session);
                    cv.CostCol = this;
                    cv.CostModel = cm;
                    cv.Valuta = val;
                }
            }
            return cv;
        }

        //public crmCostCol Copy() {
        //    crmCostCol sp = new crmCostCol(this.Session,this.VersionState);
        //    sp._CostModel = this.CostModel;
        //    sp._SummCost = this.SummCost;
        //    sp._SummFull = this.SummFull;
        //    sp._SummNDS = this.SummNDS;
        //    sp._Valuta = this.Valuta;
        //    return sp;
        //}

        #endregion

    }

}