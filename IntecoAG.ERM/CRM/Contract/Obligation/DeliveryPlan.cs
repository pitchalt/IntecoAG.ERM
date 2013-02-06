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

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Contract.Deal;
//using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CS.Finance;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    public enum DeliveryMethod {
        NO_DELIVERY = 0,
        SINGLE_MATERIAL_AT_THE_END = 1,
        SINGLE_SERVICE_AT_THE_END = 2,
        UNIT_AT_THE_END = 3,
        ITEMS_SHEDULE = 4,
        UNITS_SHEDULE = 5
    }
    //
    [Persistent]
    public class crmDeliveryPlanItemShedule : VersionRecord {
        public crmDeliveryPlanItemShedule(Session session) : base(session) { }
        public crmDeliveryPlanItemShedule(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.DealNomenclature = new crmDealNomenclature(this.Session, this.VersionState);
            UpdateUnitShedules();
        }

        private crmDeliveryPlan _DeliveryPlan;

        [Browsable(false)]
        [Association("crmDeliveryPlan-ItemShedules")]
        public crmDeliveryPlan DeliveryPlan {
            get { return this._DeliveryPlan; }
            set { 
                SetPropertyValue<crmDeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value);
                if (!IsLoading) {
                    if (value != null) {
                        this.DeliveryPlan.DealVersion.DealNomenclatures.Add(this.DealNomenclature);
                    }
                    UpdateUnitShedules();
                }
            }
        }

        crmDealNomenclature _DealNomenclature;

        //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        [DataSourceProperty("DeliveryPlan.DealVersion.DealNomenclatures")]
        [Browsable(false)]
        public crmDealNomenclature DealNomenclature {
            get { return this._DealNomenclature; }
            set { SetPropertyValue<crmDealNomenclature>("DealNomenclature", ref _DealNomenclature, value); }
        }
        //
        private Decimal _Price;
        /// <summary>
        /// 
        /// </summary>
        public Decimal Price {
            get { return this._Price; }
            set {
                SetPropertyValue<Decimal>("Price", ref _Price, value);
                if (!IsLoading) {
                    foreach (crmDeliveryPlanUnitShedule dus in this.DeliveryPlanUnitShedules) {
                        if (dus.DeliveryItem != null) {
                            dus.DeliveryItem.Price = value;
                        }
                    }
                    UpdateCachedValues();
                }
            }
        }
        //
        private Decimal _SummCost;
        /// <summary>
        /// 
        /// </summary>
        public Decimal SummCost {
            get { return this._SummCost; }
            set { SetPropertyValue("SummCost", ref _SummCost, value); }
        }
        //
        private Decimal _SummFull;
        /// <summary>
        /// 
        /// </summary>
        public Decimal SummFull {
            get { return this._SummFull; }
            set { SetPropertyValue("SummFull", ref _SummFull, value); }
        }
        //
        private Decimal _CountValue;
        /// <summary>
        /// 
        /// </summary>
        public Decimal CountValue {
            get {
                return _CountValue;
            }
            set {
                SetPropertyValue<Decimal>("CountValue", ref _CountValue, value);
            }
        }
        //
        public void UpdateCachedValues() {
            decimal tempCount = 0m;
            decimal tempSummCost = 0m;
            decimal tempSummFull = 0m;
            foreach (crmDeliveryPlanUnitShedule detail in this.DeliveryPlanUnitShedules) {
                tempCount += detail.CountValue;
                if (detail.DeliveryItem != null) {
                    tempSummCost += detail.DeliveryItem.SummCost;
                    tempSummFull += detail.DeliveryItem.SummFull;
                }
            }
            this.CountValue = tempCount;
            this.SummCost = tempSummCost;
            this.SummFull = tempSummFull;
        }
        /// <summary>
        /// 
        /// </summary>
        [RuleRequiredField]
        [PersistentAlias("DealNomenclature.CountUnit")]
        public CS.Measurement.csUnit CountUnit {
            get {
                if (this.DealNomenclature != null)
                    return this.DealNomenclature.CountUnit;
                else
                    return null;
            }
            set {
                CS.Measurement.csUnit old = this.CountUnit;
                this.DealNomenclature.CountUnit = value;
                foreach (crmDeliveryPlanUnitShedule dus in this.DeliveryPlanUnitShedules) {
                    dus.CountUnit = value;
                }
                if (old != value)
                    OnChanged("CountUnit", old, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("DealNomenclature.Nomenclature")]
        [VisibleInListView(false)]
        public csNomenclature Nomenclature {
            get { return this.DealNomenclature.Nomenclature; }
            set {
                csNomenclature old = this.Nomenclature;
                if (old == value) return;
                this.DealNomenclature.Nomenclature = value;
                OnChanged("Nomenclature", old, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("DealNomenclature.NomenclatureName")]
        [RuleRequiredField]
        public String NomenclatureName {
            get { return this.DealNomenclature.NomenclatureName; }
            set {
                String old = this.NomenclatureName;
                if (old == value) return;
                this.DealNomenclature.NomenclatureName = value;
                foreach (crmDeliveryPlanUnitShedule dus in this.DeliveryPlanUnitShedules) {
                    if (dus.DeliveryItem != null) {
                        dus.DeliveryItem.NomenclatureName = value;
                    }
                }
                OnChanged("NomenclatureName", old, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [Association("crmDeliveryPlanItemShedule-DeliveryPlanUnitShedule", typeof(crmDeliveryPlanUnitShedule))]
        public XPCollection<crmDeliveryPlanUnitShedule> DeliveryPlanUnitShedules {
            get {

                XPCollection<crmDeliveryPlanUnitShedule> col = GetCollection<crmDeliveryPlanUnitShedule>("DeliveryPlanUnitShedules");
                return col;
            }
        }
        //
        protected override void OnLoaded() {
            base.OnLoaded();
            UpdateCachedValues();
            UpdateUnitShedules();
        }
        //
        void UpdateUnitShedules() {
            if (this.DeliveryPlan != null) {
                foreach (crmDeliveryUnit du in this.DeliveryPlan.DeliveryUnits) {
                    bool IsFound = false;
                    foreach (crmDeliveryPlanUnitShedule us in this.DeliveryPlanUnitShedules) {
                        if (us.DeliveryUnit == du) {
                            IsFound = true;
                            break;
                        }
                    }
                    if (!IsFound) {
                        crmDeliveryPlanUnitShedule item = new crmDeliveryPlanUnitShedule(this.Session, this.VersionState);
                        item.DeliveryUnit = du;
                        this.DeliveryPlanUnitShedules.Add(item);
                    }
                }
            }
        }

    }
    //
    [Persistent]
    public class crmDeliveryPlanUnitShedule : VersionRecord {
        public crmDeliveryPlanUnitShedule(Session session) : base(session) { }
        public crmDeliveryPlanUnitShedule(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        [Browsable(false)]
        crmDeliveryPlan DeliveryPlan {
            get {
                if (this.DeliveryPlanItemShedule != null)
                    return this.DeliveryPlanItemShedule.DeliveryPlan;
                else
                    return null;
            }
        }
        //
        crmDeliveryPlanItemShedule _DeliveryPlanItemShedule;
        //
        [Association("crmDeliveryPlanItemShedule-DeliveryPlanUnitShedule")]
        public crmDeliveryPlanItemShedule DeliveryPlanItemShedule {
            get { return this._DeliveryPlanItemShedule; }
            set { SetPropertyValue<crmDeliveryPlanItemShedule>("DeliveryPlanItemShedule", ref _DeliveryPlanItemShedule, value); }
        }
        //
        crmDeliveryUnit _DeliveryUnit;
        //
        [Browsable(false)]
        public crmDeliveryUnit DeliveryUnit {
            get { return this._DeliveryUnit; }
            set { SetPropertyValue<crmDeliveryUnit>("DeliveryUnit", ref _DeliveryUnit, value); }
        }
        //
        crmDeliveryItem _DeliveryItem;
        //
        [Browsable(false)]
        public crmDeliveryItem DeliveryItem {
            get { return this._DeliveryItem; }
            set { SetPropertyValue<crmDeliveryItem>("DeliveryItem", ref _DeliveryItem, value); }
        }
        //
        //Decimal _CountValue;
        //
        [PersistentAlias("DeliveryItem.CountValue")]
        public Decimal CountValue {
            get {
                if (this.DeliveryItem != null)
                    return this.DeliveryItem.CountValue;
                else
                    return 0;
            }
            set {
                Decimal old = this.CountValue;
                if (this.DeliveryItem != null)
                    this.DeliveryItem.CountValue = value;
                else {
                    if (value != 0 && old == 0) {
                        if (this.DatePlane == DateTime.MinValue) {
                            this.DatePlane = DeliveryPlan.DateEnd;
                            if (this.DatePlane == DateTime.MinValue) {
                                this.DatePlane = DateTime.Now;
                            }
                        }
                        this.DeliveryItem = new crmDeliveryMaterial(this.Session, this.VersionState);
                        this.DeliveryUnit.DeliveryItems.Add(this.DeliveryItem);
                        if (this.DeliveryPlanItemShedule != null) {
                            this.DeliveryItem.CountUnit = this.DeliveryPlanItemShedule.CountUnit;
                            this.DeliveryItem.Price = this.DeliveryPlanItemShedule.Price;
                            this.DeliveryItem.NomenclatureName = this.DeliveryPlanItemShedule.NomenclatureName;
                        }
                        this.DeliveryItem.CountValue = value;
                    }
                    if (value == 0 && old != 0) {
                        if (this.DeliveryItem != null) {
                            this.DeliveryUnit.DeliveryItems.Remove(this.DeliveryItem);
                            this.DeliveryItem.Delete();
                            this.DeliveryItem = null;
                        }
                    }

                }
                this.UpdateCachedValues();
                if (old != value)
                    OnChanged("CountValue", old, value);
            }
        }
        //
        [Browsable(false)]
        [PersistentAlias("DeliveryItem.CountUnit")]
        public CS.Measurement.csUnit CountUnit {
            get {
                if (this.DeliveryItem != null)
                    return this.DeliveryItem.CountUnit;
                else
                    return null;
            }
            set { 
                CS.Measurement.csUnit old = this.CountUnit;
                if (this.DeliveryItem != null) {
                    this.DeliveryItem.CountUnit = value;
                    if (old != value)
                        OnChanged("CountUnit", old, value);
                }
            }
        }
        //
        protected void UpdateCachedValues() {
            if (this.DeliveryPlanItemShedule != null)
                DeliveryPlanItemShedule.UpdateCachedValues();
            OnChanged("SummCost");
            OnChanged("SummFull");
        }
        //
        [PersistentAlias("DeliveryItem.SummCost")]
        public Decimal SummCost {
            get {
                if (this.DeliveryItem != null)
                    return this.DeliveryItem.SummCost;
                else
                    return 0;
            }
        }
        //
        [PersistentAlias("DeliveryItem.SummFull")]
        public Decimal SummFull {
            get {
                if (this.DeliveryItem != null)
                    return this.DeliveryItem.SummFull;
                else
                    return 0;
            }
        }
        //
        [PersistentAlias("DeliveryUnit.DatePlane")]
        public DateTime DatePlane {
            get {
                if (this.DeliveryUnit != null)
                    return this.DeliveryUnit.DatePlane;
                else
                    return DateTime.MinValue;
            }
            set {
                DateTime old = this.DatePlane;
                if (this.DeliveryUnit == null) {
                    foreach (crmDeliveryUnit du in this.DeliveryPlan.DeliveryUnits) {
                        if (du.DatePlane == value) {
                            this.DeliveryUnit = du;
                            break;
                        }
                    }
                }
                if (this.DeliveryUnit == null) {
                    this.DeliveryUnit = new crmDeliveryUnit(this.Session, this.VersionState);
                    this.DeliveryPlan.DeliveryUnits.Add(this.DeliveryUnit);
                }
                //!Паша нужно переделать, что бы при смене даты сливались и разливались поставки
                this.DeliveryUnit.DatePlane = value;
                OnChanged("DatePlan", old, value);
            }
        }
        //
        //private crmDeliveryPlanItemShedule _crmDeliveryPlanItemDeleted;
        //
        protected override void OnDeleting() {
            base.OnDeleting();
            if (this.DeliveryItem != null) {
                this.DeliveryItem.Delete();
                this.DeliveryItem = null;
            }
            this.UpdateCachedValues();

            //// Запомним корневой элемент перед удалением 
            //_crmDeliveryPlanItemDeleted = this.DeliveryPlanItemShedule;
        }
        //
        protected override void OnLoaded() {
            base.OnLoaded();
            if (this.DeliveryItem != null && this.DeliveryItem.IsDeleted) {
                this.CountValue = 0;
                this.DeliveryItem = null;
            }
            if (this.DeliveryUnit != null && this.DeliveryUnit.IsDeleted) {
                this.DeliveryUnit = null;
            }
        }

        ////
        //protected override void OnDeleted() {
        //    base.OnDeleted();
        //    if (_crmDeliveryPlanItemDeleted != null) {
        //        // Обновим запомненный корневой элемент после удаления
        //        _crmDeliveryPlanItemDeleted.UpdateCachedValues();
        //        _crmDeliveryPlanItemDeleted = null;
        //    }

        //}
    }
    /// <summary>
    /// Класс crmDeliveryPlan, представляющий план работ по Договору
    /// </summary>
    [Persistent("crmDeliveryPlan")]
    public partial class crmDeliveryPlan : crmObligationPlan   //VersionRecord   //BasePlan, IVersionSupport
    {
        public crmDeliveryPlan(Session session) : base(session) { }
        public crmDeliveryPlan(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }
        
        #region ПОЛЯ КЛАССА

        private DeliveryMethod _DeliveryMethod;

        #endregion


        #region СВОЙСТВА КЛАССА

        #region ObligationPlan
        /// <summary>
        /// Дата начала события
        /// </summary>
        public override DateTime DateBegin {
            get { return base.DateBegin; }
            set {
                DateTime old = base.DateBegin;
                base.DateBegin = value;
                if (!IsLoading) {
                    if (this.DateEnd < value) {
                        this.DateEnd = value;
                    }
                    foreach (crmDeliveryUnit du in this.DeliveryUnits) {
                        if (du.DateBegin == old)
                            du.DateBegin = value;
                    }
                }
            }
        }
        /// <summary>
        /// Дата конца события
        /// </summary>
        public override DateTime DateEnd {
            get { return base.DateEnd; }
            set {
                DateTime old = base.DateEnd;
                base.DateEnd = value;
                if (!IsLoading) {
                    if (this.DateBegin > value) {
                        this.DateBegin = value;
                    }
                    foreach (crmDeliveryUnit du in this.DeliveryUnits) {
                        if (du.DatePlane == old)
                            du.DatePlane = value;
                        if (du.DateEnd == old)
                            du.DateEnd = value;
                    }
                }
            }
        }
        // Плательщик, он же Заказчик/Полкупатель
        public override crmContractParty Sender {
            set {
                crmContractParty oldValue = base.Sender;
                base.Sender = value;
                if (!IsLoading && value != null) {
                    foreach (crmDeliveryUnit du in this.DeliveryUnits) {
                        if (du.Sender == oldValue | du.Sender == null) du.Sender = value;
                    }
                }
            }
        }

        // Получатель оплаты, он же Испольнитель/Поставщиу
        public override crmContractParty Receiver {
            set {
                crmContractParty oldValue = base.Receiver;
                base.Receiver = value;
                if (!IsLoading && value != null) {
                    foreach (crmDeliveryUnit du in this.DeliveryUnits) {
                        if (du.Receiver == oldValue | du.Receiver == null) du.Receiver = value;
                    }
                }
            }
        }

        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        public override fmOrder Order {
            //            get { return base.Order; }
            set {
                fmOrder oldValue = base.Order;
                base.Order = value;
                if (!IsLoading && value != null) {
                    foreach (crmDeliveryUnit du in this.DeliveryUnits) {
                        //du.Order = value;
                        if (du.Order == oldValue | du.Order == null) du.Order = value;
                    }
                }
            }
        }

        /// <summary>
        /// CostItem
        /// </summary>
        public override fmCostItem CostItem {
            //            get { return base.CostItem; }
            set {
                fmCostItem oldValue = base.CostItem;
                if (!IsLoading && value != null) {
                    foreach (crmDeliveryUnit du in this.DeliveryUnits) {
                        //du.Order = value;
                        if (du.CostItem == oldValue | du.CostItem == null) du.CostItem = value;
                    }
                }
            }
        }

        //private csNDSRate _NDSRate;
        // Ставка НДС (VAT Rate)
        public override csNDSRate NDSRate {
            get { return base.NDSRate; }
            //set {
            //    SetPropertyValue<csNDSRate>("NDSRate", ref _NDSRate, value);
            //}
            set {
                csNDSRate oldValue = base.NDSRate;
                base.NDSRate = value;
                if (!this.IsLoading && value != null) {
                    foreach (crmDeliveryUnit item in this.DeliveryUnits) {
                        if (item.NDSRate == oldValue | item.NDSRate == null) item.NDSRate = value;
                    }
                }

            }
        }

        /// <summary>
        /// csValuta
        /// </summary>
        //private csValuta _Valuta;
        public override csValuta Valuta {
            get { return base.Valuta; }
            set {
                csValuta oldValue = base.Valuta;
                base.Valuta = value;
                if (!IsLoading && value != null) {
                    foreach (crmDeliveryUnit item in DeliveryUnits) {
                        //item.Valuta = value;
                        if (item.Valuta == oldValue | item.Valuta == null) item.Valuta = value;
                    }
                }
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        //private crmCostModel _CostModel;
        public override crmCostModel CostModel {
            get { return base.CostModel; }
            set {
                crmCostModel oldValue = base.CostModel;
                base.CostModel = value;
                if (!IsLoading && value != null) {
                    foreach (crmDeliveryUnit item in DeliveryUnits) {
                        //item.CostModel = value;
                        if (item.CostModel == oldValue | item.CostModel == null) item.CostModel = value;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public DeliveryMethod DeliveryMethod {
            get { return this._DeliveryMethod; }
            set {
                DeliveryMethod old = this._DeliveryMethod;
                if (old == value) return;
                if (!IsLoading) {
                    DeliveryMethodSet(value);
                } else {
                    this._DeliveryMethod = value;
                }
                OnChanged("DeliveryMethod", old, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeliveryMethodSet(DeliveryMethod method) { 
            DeliveryMethod old = this._DeliveryMethod;
            //if (old == method) return;
            //
            this._DeliveryMethod = method;
            this.DeliveryUnit = null;
            switch (method) {
                case DeliveryMethod.NO_DELIVERY:
                    ItemShedulesClear();
                    DeliveryUnitsClear();
                    break;
                case DeliveryMethod.SINGLE_MATERIAL_AT_THE_END:
                    ItemShedulesClear();
                    DeliveryUnitsClear();
                    this.DeliveryUnit = this.DeliveryUnitCreate();
                    this.DeliveryUnit.DeliveryUnitTypeSet(DeliveryUnitType.SINGLE_MATERIAL);
                    break;
                case DeliveryMethod.SINGLE_SERVICE_AT_THE_END:
                    ItemShedulesClear();
                    DeliveryUnitsClear();
                    this.DeliveryUnit = this.DeliveryUnitCreate();
                    this.DeliveryUnit.DeliveryUnitTypeSet(DeliveryUnitType.SINGLE_SERVICE);
                    break;
                case DeliveryMethod.UNIT_AT_THE_END:
                    ItemShedulesClear();
                    DeliveryUnitsClear();
                    this.DeliveryUnit = this.DeliveryUnitCreate();
                    this.DeliveryUnit.DeliveryUnitTypeSet(DeliveryUnitType.LIST_ITEMS);
                    break;
                case DeliveryMethod.ITEMS_SHEDULE:
                    DeliveryUnitsClear();
//                    ItemShedulesClear();
                    break;
                case DeliveryMethod.UNITS_SHEDULE:
                    ItemShedulesClear();
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeliveryUnitsClear() { 
            List<crmDeliveryUnit> dul = new List<crmDeliveryUnit>();
            foreach (crmDeliveryUnit du in this.DeliveryUnits)
                dul.Add(du);
            foreach (crmDeliveryUnit du in dul) {
                if (du != this.DeliveryUnit) {
                    this.DeliveryUnits.Remove(du);
                    du.Delete();
                }
            }
        }
        //
        crmDeliveryUnit _DeliveryUnit;
        /// <summary>
        /// 
        /// </summary>
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmDeliveryUnit DeliveryUnit {
            get { return this._DeliveryUnit; }
            set { SetPropertyValue<crmDeliveryUnit>("DeliveryUnit", ref _DeliveryUnit, value); }
        }
//        //
//        private BindingList<ItemShedule> _ItemShedules = null;
        ///
        [Aggregated]
        [Association("crmDeliveryPlan-ItemShedules", typeof(crmDeliveryPlanItemShedule))]
        public XPCollection<crmDeliveryPlanItemShedule> ItemShedules {
            get {
//                if (this.DeliveryMethod == DeliveryMethod.ITEMS_SHEDULE) {
                    return this.GetCollection<crmDeliveryPlanItemShedule>("ItemShedules");
//                }
//                else
//                    return null;
            }
        }

        protected void ItemShedulesClear() {
            if (this.ItemShedules != null) {
                List<crmDeliveryPlanItemShedule> isl = new List<crmDeliveryPlanItemShedule>();
                foreach (crmDeliveryPlanItemShedule isi in this.ItemShedules)
                    isl.Add(isi);
                foreach (crmDeliveryPlanItemShedule isi in isl) {
                    this.ItemShedules.Remove(isi);
                    isi.Delete();
                }
            }
        }

        //protected void ItemShedulesFill() {
        //    Dictionary<crmDealNomenclature, ItemShedule> dic = new Dictionary<crmDealNomenclature, ItemShedule>();
        //    foreach (crmDeliveryUnit du in this.DeliveryUnits) {
        //        foreach (crmDeliveryItem di in du.DeliveryItems) {
        //            if (di.DealNomenclature != null) {
        //                ItemShedule item;
        //                if (!dic.ContainsKey(di.DealNomenclature)) {
        //                    item = new ItemShedule(this.Session);
        //                    item.DeliveryPlan = this;
        //                    item.DealNomenclature = di.DealNomenclature;
        //                    dic[di.DealNomenclature] = item;
        //                    this.ItemShedules.Add(item);
        //                }
        //                else
        //                    item = dic[di.DealNomenclature];
        //                item.Count += di.CountValue;
        //            }
        //        }
        //    }
        //    ItemShedule item2 = new ItemShedule(this.Session);
        //    item2.Count = 10;
        //    this.ItemShedules.Add(item2);
        //}

        #endregion


        #region МЕТОДЫ

        //protected override void OnLoaded() {
        //    base.OnLoaded();
        //    if (this.DeliveryMethod == DeliveryMethod.ITEMS_SHEDULE) {
        //        ItemShedulesFill();                
        //    }
        //}

        public crmDeliveryUnit DeliveryUnitCreate() {
            crmDeliveryUnit du = new crmDeliveryUnit(this.Session, this.VersionState);
            this.DeliveryUnits.Add(du);
            du.Code = "Поставка";
            du.DatePlane = this.DateEnd;
            return du;
        }
        #endregion

    }

}