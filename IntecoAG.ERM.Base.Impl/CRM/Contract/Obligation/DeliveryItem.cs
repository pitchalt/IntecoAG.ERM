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
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Validation;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.CRM.Contract.Obligation {
    public enum CostCalculateMethod {
        NOT_SELECT = 0,
        NO_COST = 1,
        HAND_MAKE = 2,
        CALC_PRICE = 3,
        CALC_COST = 4,
        CALC_COUNT = 5
    }
    //
    public enum NDSCalculateMethod {
        NOT_SELECT = 0,
        NO_NDS = 1,
        HAND_MAKE = 2,
        FROM_PRICE = 3,
        FROM_COST = 4,
        FROM_FULL = 5
    }

    public enum FullCalculateMethod {
        NOT_SELECT = 0,
        NO_COST = 1,
        HAND_MAKE = 2,
        CALC_FULL = 3,
        CALC_NDS = 4,
        CALC_COST = 5
    }
    /// <summary>
    /// Класс DeliveryItem, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    //[Persistent("crmDeliveryItem")]
    [Appearance("crmDeliveryItem.IsNoCount", AppearanceItemType.ViewItem, "IsNoCount",
        TargetItems = "CountValue,CountUnit,Price", Enabled = false)]
    [Appearance("crmDeliveryItem.IsNoCost", AppearanceItemType.ViewItem, "IsNoCost",
        TargetItems = "CostCalculateMethod,NDSCalculateMethod,FullCalculateMethod", Enabled = false)]
    //
    [Appearance("crmDeliveryItem.CostMethodNoCost", AppearanceItemType.ViewItem, "CostCalculateMethod == 'NO_COST'",
        TargetItems = "Price,SummCost", Enabled = false)]
    [Appearance("crmDeliveryItem.CostMethodCalcCount", AppearanceItemType.ViewItem, "CostCalculateMethod == 'CALC_COUNT'",
        TargetItems = "CountValue", Enabled = false)]
    [Appearance("crmDeliveryItem.CostMethodCalcPrice", AppearanceItemType.ViewItem, "CostCalculateMethod == 'CALC_PRICE'",
        TargetItems = "Price", Enabled = false)]
    [Appearance("crmDeliveryItem.CostMethodCalcCost", AppearanceItemType.ViewItem, "CostCalculateMethod == 'CALC_COST'",
        TargetItems = "SummCost", Enabled = false)]
    [Appearance("crmDeliveryItem.NDSMethodAuto", AppearanceItemType.ViewItem, "NDSCalculateMethod != 'HAND_MAKE' && NDSCalculateMethod != 'NOT_SELECT' ",
        TargetItems = "SummNDS", Enabled = false)]
    [Appearance("crmDeliveryItem.FullMethodCalcFull", AppearanceItemType.ViewItem, "FullCalculateMethod == 'CALC_FULL'",
        TargetItems = "SummFull", Enabled = false)]
    [Appearance("crmDeliveryItem.FullMethodCalcCost", AppearanceItemType.ViewItem, "FullCalculateMethod == 'CALC_COST'",
        TargetItems = "SummCost", Enabled = false)]
    [Appearance("crmDeliveryItem.FullMethodCalcNDS", AppearanceItemType.ViewItem, "FullCalculateMethod == 'CALC_NDS'",
        TargetItems = "SummNDS", Enabled = false)]
    [RuleCriteria("crmDeliveryItem.FullCheck", DefaultContexts.Save, "SummCost + SummNDS == SummFull", "SummFull must be equal SummCost + SummNDS ")]
    //[RuleCriteria("crmDeliveryItem.NDSRateRequired2", DefaultContexts.Save, "NDSRate == 'Null'", "NDSRate Required", TargetCriteria="!IsNoCost")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [VisibleInReports]
    public abstract partial class crmDeliveryItem : crmObligationTransfer {
        public crmDeliveryItem(Session session) : base(session) { }
        public crmDeliveryItem(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА
        //
        protected NDSCalculateMethod _NDSCalculateMethod;
        protected CostCalculateMethod _CostCalculateMethod;
        protected FullCalculateMethod _FullCalculateMethod;
        //
        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// 
        /// </summary>
        [ImmediatePostData]
        public CostCalculateMethod CostCalculateMethod {
            get { return _CostCalculateMethod; }
            set {
                SetPropertyValue<CostCalculateMethod>("CostCalculateMethod", ref _CostCalculateMethod, value);
                if (!IsLoading) {
                    if (this.CostCalculateMethod == CostCalculateMethod.CALC_COST) {
                        if (this.FullCalculateMethod == FullCalculateMethod.CALC_COST)
                            this.FullCalculateMethod = FullCalculateMethod.CALC_FULL;
                    }
                    UpdateAll();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ImmediatePostData]
        public NDSCalculateMethod NDSCalculateMethod {
            get { return _NDSCalculateMethod; }
            set {
                SetPropertyValue<NDSCalculateMethod>("NDSCalculateMethod", ref _NDSCalculateMethod, value);
                if (!IsLoading) {
                    if (this.NDSCalculateMethod == NDSCalculateMethod.FROM_COST ||
                        this.NDSCalculateMethod == NDSCalculateMethod.FROM_PRICE)
                        if (this.FullCalculateMethod == FullCalculateMethod.CALC_NDS) {
                            this.FullCalculateMethod = FullCalculateMethod.CALC_FULL;
                        }
                    UpdateAll();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ImmediatePostData]
        public FullCalculateMethod FullCalculateMethod {
            get { return _FullCalculateMethod; }
            set {
                SetPropertyValue<FullCalculateMethod>("FullCalculateMethod", ref _FullCalculateMethod, value);
                if (!IsLoading) {
                    if (this.FullCalculateMethod == FullCalculateMethod.CALC_COST) {
                        if (this.CostCalculateMethod == CostCalculateMethod.CALC_COST)
                            this.CostCalculateMethod = CostCalculateMethod.CALC_PRICE;
                    }
                    if (this.FullCalculateMethod == FullCalculateMethod.CALC_NDS) {
                        if (this.NDSCalculateMethod == NDSCalculateMethod.FROM_COST ||
                            this.NDSCalculateMethod == NDSCalculateMethod.FROM_PRICE)
                            this.NDSCalculateMethod = NDSCalculateMethod.FROM_FULL;
                    }
                    UpdateAll();
                }
            }
        }
        //
        [Browsable(false)]
        public Boolean IsFirstUpdate;
        //
        void UpdateAll() {
            if (!IsFirstUpdate) {
                IsFirstUpdate = true;
                using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                    crmDeliveryItem item = uow.GetNestedObject<crmDeliveryItem>(this);
                    item.UpdateCost();
                    item.UpdateNDS();
                    item.UpdateFull();
                    uow.CommitChanges();
                }
                IsFirstUpdate = false;
            }
            else {
                UpdateCost();
                UpdateNDS();
                UpdateFull();
            }
        }
        //
        void UpdateCost() {
            switch (this.CostCalculateMethod) {
                case CostCalculateMethod.NOT_SELECT:
                    if (this.Price != 0 && this.SummCost != 0 && this.CountValue == 0) {
                        this.CostCalculateMethod = CostCalculateMethod.CALC_COUNT;
                        break;
                    }
                    if (this.Price != 0 && this.SummCost == 0 && this.CountValue != 0) {
                        this.CostCalculateMethod = CostCalculateMethod.CALC_COST;
                        break;
                    }
                    if (this.Price == 0 && this.SummCost != 0 && this.CountValue != 0) {
                        this.CostCalculateMethod = CostCalculateMethod.CALC_PRICE;
                        break;
                    }
                    break;
                case CostCalculateMethod.NO_COST:
                    if (this.Price != 0) this.Price = 0;
                    if (this.SummCost != 0) this.SummCost = 0;
                    break;
                case CostCalculateMethod.HAND_MAKE:
                    break;
                case CostCalculateMethod.CALC_COST:
                    Decimal cost = this.Price * this.CountValue;
                    if (cost != this.SummCost)
                        this.SummCost = cost;
                    break;
                case CostCalculateMethod.CALC_PRICE:
                    if (this.CountValue != 0) {
                        Decimal price = this.SummCost / this.CountValue;
                        if (price != this.Price)
                            this.Price = price;
                    }
                    break;
                case CostCalculateMethod.CALC_COUNT:
                    if (this.Price != 0) {
                        Decimal count = this.SummCost / this.Price;
                        if (count != this.CountValue)
                            this.CountValue = count;
                    }
                    break;
            }
        }
        //
        void UpdateNDS() {
            Decimal nds;
            switch (this.NDSCalculateMethod) {

                case NDSCalculateMethod.NOT_SELECT:
                    if (this.NDSRate != null) {
                        if (this.NDSRate.Numerator == 0) {
                            this.NDSCalculateMethod = NDSCalculateMethod.NO_NDS;
                            break;
                        }
                        if (this.SummCost != 0 && this.SummNDS == 0) {
                            this.NDSCalculateMethod = NDSCalculateMethod.FROM_COST;
                            break;
                        }
                        if (this.SummFull != 0 && this.SummNDS == 0) {
                            this.NDSCalculateMethod = NDSCalculateMethod.FROM_FULL;
                            break;
                        }
                    }
                    break;
                case NDSCalculateMethod.NO_NDS:
                    if (this.SummNDS != 0)
                        this.SummNDS = 0;
                    break;
                case NDSCalculateMethod.HAND_MAKE:
                    break;
                case NDSCalculateMethod.FROM_COST:
                    if (this.NDSRate != null && this.NDSRate.Denominator != 0) {
                        nds = this.SummCost * this.NDSRate.Numerator / this.NDSRate.Denominator;
                        if (this.SummNDS != nds)
                            this.SummNDS = nds;
                    }
                    break;
                case NDSCalculateMethod.FROM_PRICE:
                    if (this.NDSRate != null && this.NDSRate.Denominator != 0) {
                        nds = (this.Price * this.NDSRate.Numerator / this.NDSRate.Denominator) * this.CountValue;
                        if (this.SummNDS != nds)
                            this.SummNDS = nds;
                    }
                    break;
                case NDSCalculateMethod.FROM_FULL:
                    if (this.FullCalculateMethod == FullCalculateMethod.CALC_NDS) {
                        break;
                    }
                    else {
                        if (this.NDSRate != null && this.NDSRate.Numerator + this.NDSRate.Denominator != 0) {
                            nds = this.SummFull * this.NDSRate.Numerator / (this.NDSRate.Numerator + this.NDSRate.Denominator);
                            if (this.SummNDS != nds)
                                this.SummNDS = nds;
                        }
                    }
                    break;
            }
        }
        //
        void UpdateFull() {
            switch (this.FullCalculateMethod) {
                case FullCalculateMethod.NOT_SELECT:
                    if (this.SummFull == 0 && this.SummCost != 0 && (this.SummNDS != 0 || this.NDSCalculateMethod == NDSCalculateMethod.NO_NDS)) {
                        this.FullCalculateMethod = FullCalculateMethod.CALC_FULL;
                        break;
                    }
                    if (this.SummFull != 0 && this.SummCost == 0 && (this.SummNDS != 0 || this.NDSCalculateMethod == NDSCalculateMethod.NO_NDS)) {
                        this.FullCalculateMethod = FullCalculateMethod.CALC_COST;
                        break;
                    }
                    if (this.SummFull != 0 && this.SummCost != 0 && this.SummNDS == 0) {
                        this.FullCalculateMethod = FullCalculateMethod.CALC_NDS;
                        break;
                    }
                    break;
                case FullCalculateMethod.NO_COST:
                    if (this.SummFull != 0)
                        this.SummFull = 0;
                    break;
                case FullCalculateMethod.HAND_MAKE:
                    break;
                case FullCalculateMethod.CALC_NDS:
                    Decimal nds = this.SummFull - this.SummCost;
                    if (this.SummNDS != nds)
                        this.SummNDS = nds;
                    break;
                case FullCalculateMethod.CALC_COST:
                    Decimal cost = this.SummFull - this.SummNDS;
                    if (this.SummCost != cost)
                        this.SummCost = cost;
                    break;
                case FullCalculateMethod.CALC_FULL:
                    Decimal full = this.SummNDS + this.SummCost;
                    if (this.SummFull != full)
                        this.SummFull = full;
                    break;
            }
        }
        //
        private Boolean _IsNoCount;
        /// <summary>
        /// 
        /// </summary>
        [ImmediatePostData]
        public Boolean IsNoCount {
            get { return _IsNoCount; }
            set {
                SetPropertyValue("IsNoCount", ref _IsNoCount, value);
                if (!IsLoading) {
                    if (this.IsNoCount) {
                        this.CountValue = 0;
                        this.CountUnit = null;
                    }
                }
            }
        }
        //
        private Boolean _IsNoCost;
        /// <summary>
        /// 
        /// </summary>
        [ImmediatePostData]
        public Boolean IsNoCost {
            get { return _IsNoCost; }
            set {
                Boolean old = this._IsNoCost;
                if (old == value) return;
                SetPropertyValue("IsNoCost", ref _IsNoCost, value);
                if (!IsLoading) {
                    if (this._IsNoCost) {
                        this.FullCalculateMethod = FullCalculateMethod.NO_COST;
                        this.NDSCalculateMethod = NDSCalculateMethod.NO_NDS;
                        this.CostCalculateMethod = CostCalculateMethod.NO_COST;
                    }
                    else {
                        this.FullCalculateMethod = FullCalculateMethod.NOT_SELECT;
                        this.NDSCalculateMethod = NDSCalculateMethod.NOT_SELECT;
                        this.CostCalculateMethod = CostCalculateMethod.NOT_SELECT;
                    }
                }
            }
        }
        //
        private decimal _CountValue;
        /// <summary>
        /// CountCode
        /// </summary>
        [RuleRequiredField("crmDeliveryItem.CountValue", DefaultContexts.Save, TargetCriteria = "!IsNoCount")]
        public decimal CountValue {
            get { return _CountValue; }
            set {
                SetPropertyValue("CountValue", ref _CountValue, value);
                if (!IsLoading) {
                    this.UpdateAll();
                }
            }
        }
        //
        private CS.Measurement.csUnit _CountUnit;
        /// <summary>
        /// CountUnit
        /// </summary>
        [RuleRequiredField("crmDeliveryItem.CountUnit", DefaultContexts.Save, TargetCriteria = "!IsNoCount")]
        public CS.Measurement.csUnit CountUnit {
            get { return _CountUnit; }
            set { SetPropertyValue("CountUnit", ref _CountUnit, value); }
        }

        protected decimal _Price;
        /// <summary>
        /// 
        /// </summary>
        public decimal Price {
            get { return _Price; }
            set {
                SetPropertyValue<decimal>("Price", ref _Price, value);
                if (!IsLoading) {
                    UpdateAll();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [NonPersistent]
        public override decimal SummCost {
            set {
                if (!IsLoading) {
                }
                base.SummCost = value;
                if (!IsLoading) {
                    UpdateAll();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [NonPersistent]
        public override Decimal SummNDS {
            set {
                Decimal old = base.SummNDS;
                base.SummNDS = value;
                if (!IsLoading) {
                    UpdateAll();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override Decimal SummFull {
            set {
                Decimal old = base.SummFull;
                base.SummFull = value;
                if (!IsLoading) {
                    UpdateAll();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ImmediatePostData]
        [RuleRequiredField(null, DefaultContexts.Save, TargetCriteria = "!IsNoCost")]
        public override csNDSRate NDSRate {
            get { return base.NDSRate; }
            set {
                base.NDSRate = value;
                if (!IsLoading) {
                    this.NDSCalculateMethod = NDSCalculateMethod.NOT_SELECT;
                }
            }
        }
        //
        public override crmStage Stage {
            get {
                if (this.DeliveryUnit != null) {
                    return this.DeliveryUnit.Stage;
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// NomenclatureName
        /// </summary>
        private string _NomenclatureName;
        public string NomenclatureName {
            get { return _NomenclatureName; }
            set { SetPropertyValue<string>("NomenclatureName", ref _NomenclatureName, value); }
        }

        public override fmCOrder Order {
            get {
                return base.Order;
            }
            set {
                base.Order = value;
                if (!IsLoading) {
                    UpdateTrwNomenclature();
                }
            }
        }
        private crmDealNomenclature _DealNomenclature;
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public virtual crmDealNomenclature DealNomenclature {
            get { return _DealNomenclature; }
            set {
                SetPropertyValue<crmDealNomenclature>("DealNomenclature", ref _DealNomenclature, value);
            }
        }
        //
        [Persistent("TrwSaleNomenclature")]
        private TrwSaleNomenclature _TrwSaleNomenclature;
        [PersistentAlias("_TrwSaleNomenclature")]
        public TrwSaleNomenclature TrwSaleNomenclature {
            get { return _TrwSaleNomenclature; }
        }

        #endregion


        #region МЕТОДЫ

        public void UpdateTrwNomenclature() {
            if (Nomenclature == null || Order == null) return;
            TrwSaleNomenclature old = _TrwSaleNomenclature;
            if (old == null || old.Order != Order || old.Nomenclature != Nomenclature) {
                IObjectSpace os = ObjectSpace.FindObjectSpaceByObject(this);
                _TrwSaleNomenclature = os.GetObjects<TrwSaleNomenclature>(
                        new OperandProperty("Order") == Order &
                        new OperandProperty("Nomenclature") == Nomenclature
                    ).FirstOrDefault();
                if (_TrwSaleNomenclature == null) {
                    _TrwSaleNomenclature = os.CreateObject<TrwSaleNomenclature>();
                    _TrwSaleNomenclature.Order = Order;
                    _TrwSaleNomenclature.Nomenclature = Nomenclature;
                }
                OnChanged("TrwSaleNomenclature", old, _TrwSaleNomenclature);
            }
        }

        #endregion

        public override IList<fmCOrder> OrderSource {
            get {
                IList<fmCOrder> orders = new List<fmCOrder>();
                foreach (fmCSubject subject in DeliveryUnit.DeliveryPlan.DealVersion.ContractDeal.Subjects) {
                    foreach (fmCOrder order in subject.Orders)
                        orders.Add(order);
                }
                return orders;
            }
        }


    }

}