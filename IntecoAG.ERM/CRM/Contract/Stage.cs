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
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;


//*****************************************************************************************************//
// Из документации.
//*****************************************************************************************************//
// Stage – этап работ 
// - DateBegin, DateEnd – атрибуты задающие дату начала и окончания этапа посредством 
// CS.CRM.crmContract..Time этот атрибут отличается от DateStart и DateStop, которые задают 
// абсолютные даты эти атрибуты должны автоматически заполняться через ввод DateBegin и DateEnd
//
//*****************************************************************************************************//

namespace IntecoAG.ERM.CRM.Contract
{
    //public enum DisplaySummMode {
    //    DISPLAY_FULL = 1,
    //    DISPLAY_COST = 2
    //}

    public enum StageType {
        AGREGATE = 1,
        FINANCE = 2,
        TECHNICAL = 3
    }

    /// <summary>
    /// Класс для поддержки структуры этапов проекта
    /// </summary>
    //[DefaultClassOptions]
    [Appearance("crmStage.PaymentUnitsHidden", AppearanceItemType = "LayoutItem", Criteria = "PaymentMethod != 4", TargetItems = "PaymentUnits", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    // Управление отображением поставок
    [Appearance("crmStage.DeliveryDateHide",
        AppearanceItemType = "ViewItem", Criteria = "DeliveryMethod != 'UNIT_AT_THE_END' && DeliveryMethod != 'SINGLE_SERVICE_AT_THE_END' && DeliveryMethod != 'SINGLE_MATERIAL_AT_THE_END'",
        TargetItems = "DeliveryDate", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    //[Appearance("crmStage.DeliveryItemAtTheEndHide",
    //    AppearanceItemType = "LayoutItem", Criteria = "DeliveryMethod != 'SINGLE_SERVICE_AT_THE_END' && DeliveryMethod != 'SINGLE_MATERIAL_AT_THE_END' ",
    //    TargetItems = "DeliveryItem,DeliveryItemDescription", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("crmStage.DeliveryItemAtTheEndHide",
        AppearanceItemType = "ViewItem", Criteria = "DeliveryMethod != 'SINGLE_SERVICE_AT_THE_END' && DeliveryMethod != 'SINGLE_MATERIAL_AT_THE_END' ",
        TargetItems = "DeliveryItem", Visibility = ViewItemVisibility.Hide, Enabled = false)]    
    [Appearance("crmStage.DeliveryUnitAtTheEndHide",
        AppearanceItemType = "ViewItem", Criteria = "DeliveryMethod != 'UNIT_AT_THE_END'",
        TargetItems = "DeliveryItems", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("crmStage.DeliveryItemsSheduleHide",
        AppearanceItemType = "ViewItem", Criteria = "DeliveryMethod != 'ITEMS_SHEDULE'",
        TargetItems = "ItemShedules", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("crmStage.DeliveryUnitsSheduleHide",
        AppearanceItemType = "ViewItem", Criteria = "DeliveryMethod != 'UNITS_SHEDULE'",
        TargetItems = "DeliveryUnits", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    //
    [Appearance("crmStage.PaymentsHidden", AppearanceItemType = "LayoutItem", Criteria = "StageType == 0 || StageType == 1 || StageType == 3", TargetItems = "Payments", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    [Appearance("crmStage.ObligationsHidden", AppearanceItemType = "LayoutItem", Criteria = "StageType == 0 || StageType == 1", TargetItems = "Obligations", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    //
    [DefaultProperty("Code")]
    [Persistent("crmStage")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public partial class crmStage : VersionRecord, ITreeNode// CS.Work.csWork
    {
        public crmStage(Session session) : base(session) { }
        public crmStage(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            //
            this.DeliveryPlan = new crmDeliveryPlan(this.Session, this.VersionState);
            //this.DeliveryPlan.DealVersion = this.DealVersion; 
            this.DeliveryPlan.Stage = this;
            //
            this.PaymentPlan = new crmPaymentPlan(this.Session, this.VersionState);
            //this.PaymentPlan.DealVersion = this.DealVersion;
            this.PaymentPlan.Stage = this;
        }

        #region СВОЙСТВА КЛАССА

        [Browsable(false)]
        [NonPersistent]
        public crmContractParty Customer {
            get { return this.StageStructure.Customer;  }
            set {
                this.DeliveryPlan.Creditor = this.Customer;
                this.PaymentPlan.Debitor   = this.Customer;
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public crmDealVersion DealVersion {
            get { return this.StageStructure.DealVersion; }
            set {
                this.DeliveryPlan.DealVersion = this.DealVersion;
                this.PaymentPlan.DealVersion = this.DealVersion;
            }
        }


        [Browsable(false)]
        [NonPersistent]
        public crmContractParty Supplier {
            get { return this.StageStructure.Supplier; }
            set {
                this.DeliveryPlan.Debitor = this.Supplier;
                this.PaymentPlan.Creditor = this.Supplier;
            }
        }
        /// <summary>
        /// Дата начала события
        /// </summary>
        private DateTime _DateBegin;
        [RuleRequiredField("crmStage.DateBegin.Required", "Save")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set {
                DateTime old = this._DateBegin;
                if (!IsLoading) {
                    if (TopStage != null) {
                        if (TopStage.DateBegin > value)
                            value = TopStage.DateBegin;
                        if (TopStage.DateEnd < value)
                            value = TopStage.DateEnd;
                    }
                }
                SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value);
                if (!IsLoading) {
                    this.DeliveryPlan.DateBegin = value;
                    if (this.DateEnd < value) {
                        this.DateEnd = value;
                    }
                    if (this.AdvanceDate < value || this.AdvanceDate == old) {
                        this.AdvanceDate = value;
                    }
                    foreach (crmStage stage in this.SubStages)
                        if (stage.DateBegin == old)
                            stage.DateBegin = value;
                }
            }
        }
        /// <summary>
        /// Дата конца события
        /// </summary>
        private DateTime _DateEnd;
        [RuleRequiredField("crmStage.DateEnd.Required", "Save")]
        //[RuleRequiredField("crmStage.DateEnd.Required.Immediate", "Immediate")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                DateTime old = this._DateEnd;
                if (!IsLoading) {
                    if (TopStage != null) {
                        if (TopStage.DateBegin > value)
                            value = TopStage.DateBegin;
                        if (TopStage.DateEnd < value)
                            value = TopStage.DateEnd;
                    }
                }
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
                if (!IsLoading) {
                    this.DeliveryPlan.DateEnd = value;
                    if (this.DateBegin > value) {
                        this.DateBegin = value;
                    }
                    if (this.DateFinish < value) {
                        this.DateFinish = value;
                    }
                    if (this.DeliveryDate > value || this.DeliveryDate < this.DateBegin || this.DeliveryDate == old) {
                        this.DeliveryDate = value;
                    }
                    foreach (crmStage stage in this.SubStages)
                        if (stage.DateEnd == old)
                            stage.DateEnd = value;
                }
            }
        }
        /// <summary>
        /// Дата конца события
        /// </summary>
        private DateTime _DateFinish;
        [RuleRequiredField("crmStage.DateFinish.Required", "Save")]
        //[RuleRequiredField("crmStage.DateEnd.Required.Immediate", "Immediate")]
        public DateTime DateFinish {
            get { return _DateFinish; }
            set {
                DateTime old = this._DateFinish;
                if (!IsLoading) {
                    if (TopStage != null) {
                        if (TopStage.DateBegin > value)
                            value = TopStage.DateBegin;
                        if (TopStage.DateFinish < value)
                            value = TopStage.DateEnd;
                    }
                }
                SetPropertyValue<DateTime>("DateFinish", ref _DateFinish, value);
                if (!IsLoading) {
                    if (this.DateEnd > value) {
                        this.DateEnd = value;
                    }
                    if (this.SettlementDate > value || this.SettlementDate < this.DateEnd || this.SettlementDate == old) {
                        this.SettlementDate = value;
                    }
                    foreach (crmStage stage in this.SubStages)
                        if (stage.DateFinish == old)
                            stage.DateFinish = value;
                }
            }
        }

        /// <summary>
        /// Code
        /// </summary>
        private string _Code;
        [Size(10)]
        [RuleRequiredField("crmStage.Code.Required", "Save")]
        //[RuleRequiredField("crmStage.Code.Required.Immediate", "Immediate")]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        /// <summary>
        /// Code
        /// </summary>
        public string FullCode {
            get {
                if (TopStage == null)
                    return Code;
                else
                    return String.Concat(TopStage.FullCode, ".", Code);
            }
        }

        /// <summary>
        /// Description - описание
        /// </summary>
        private string _DescriptionShort;
        [Size(70)]
        public string DescriptionShort {
            get { return _DescriptionShort; }
            set { 
                String old = this._DescriptionShort;
                SetPropertyValue<string>("DescriptionShort", ref _DescriptionShort, value);
                if (!IsLoading) {
                    if (this.DeliveryItem != null) {
                        if (this.DeliveryItem.NomenclatureName == old ||
                            String.IsNullOrEmpty(this.DeliveryItem.NomenclatureName)) {
                            this.DeliveryItem.NomenclatureName = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Description - описание
        /// </summary>
        private string _DescriptionLong;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false)]
        public string DescriptionLong {
            get { return _DescriptionLong; }
            set { 
                String old = this._DescriptionLong;
                SetPropertyValue<string>("DescriptionLong", ref _DescriptionLong, value);
                if (!IsLoading) {
                    if (this.DeliveryItem != null) {
                        if (this.DeliveryItem.Description == old ||
                            String.IsNullOrEmpty(this.DeliveryItem.Description)) {
                            this.DeliveryItem.Description = value;
                        }
                    }
                }
            }
        }
        //
        crmStage _TopStage;
        [Browsable(false)]
        [Association("crmStage-SubStages")]
        public virtual crmStage TopStage {
            get { return _TopStage; }
            set {
                SetPropertyValue<crmStage>("TopStage", ref _TopStage, value);
                if (!IsLoading) {
                    //                    this.TopWork = value;
                    if (value != null) {
                        this.StageStructure = value.StageStructure;
                        this.Customer = value.Customer;
                        this.Supplier = value.Supplier;
                        this.DealVersion = value.DealVersion;
                        this.CurrentCost.UpCol = value.CurrentCost;
                        this.CurrentPayment.UpCol = value.CurrentPayment;
                        if (value.StageType == CRM.Contract.StageType.FINANCE)
                            this.StageType = CRM.Contract.StageType.TECHNICAL;
                        else
                            this.StageType = value.StageType;
                        // this.DisplaySummMode = value.DisplaySummMode;
                        this.DateBegin = value.DateBegin;
                        this.DateEnd = value.DateEnd;
                        this.DateFinish = value.DateFinish;
                        this.CostModel = value.CostModel;
                        this.Valuta = value.Valuta;
                        this.PaymentValuta = value.PaymentValuta;
                        this.NDSRate = value.NDSRate;
                        this.Order = value.Order;
                        this.CostItem = value.CostItem;
                    }
                }
            }
        }

        [Association("crmStage-SubStages", typeof(crmStage))]
        public virtual XPCollection<crmStage> SubStages {
            get { return GetCollection<crmStage>("SubStages"); }
        }


        #region Delivery
        //
        private DeliveryMethod _DeliveryMethod;
        // Вид поставки	Поставка в конце/Поставки по графику.
        // Способ задания основных обязательств Поставка в конце, означает, что в договоре присутствует 
        // только одна поставка и производиться она в конце срока исполнения обязательств.	
        // Если задано  «Поставка в конце», то заполняется вкладка Обязательства, если задано  «Поставки по графику», 
        // то заполняется вкладка «График поставки».
        [ImmediatePostData]
        public virtual DeliveryMethod DeliveryMethod {
            get { return _DeliveryMethod; }
            set {
                DeliveryMethod old = this._DeliveryMethod;
                if (old == value) return;
                SetPropertyValue<DeliveryMethod>("DeliveryType", ref _DeliveryMethod, value);
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        //                        crmDealWithoutStageVersion deal_version = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                        //                        deal_version.DeliveryPlan.DeliveryMethodSet(value);
                        //if (deal_version.DeliveryItem != null) {
                        //    if (String.IsNullOrEmpty(deal_version.DeliveryItem.NomenclatureName))
                        //        deal_version.DeliveryItem.NomenclatureName = this.DescriptionShort;
                        //    if (String.IsNullOrEmpty(deal_version.DeliveryItem.Description))
                        //        deal_version.DeliveryItem.Description = this.DescriptionLong;
                        //}
                        crmDeliveryPlan plan = uow.GetNestedObject<crmDeliveryPlan>(this.DeliveryPlan);
                        plan.DeliveryMethodSet(value);
                        uow.CommitChanges();
                    }
                    if (this.DeliveryItem != null) {
                        if (String.IsNullOrEmpty(this.DeliveryItem.NomenclatureName))
                            this.DeliveryItem.NomenclatureName = this.DescriptionShort;
                        if (String.IsNullOrEmpty(this.DeliveryItem.Description))
                            this.DeliveryItem.Description = this.DescriptionLong;
                    }
                    OnChanged("DeliveryItem");
                    OnChanged("DeliveryItems");
                    OnChanged("DeliveryUnits");
                }
            }
        }
        //
        /// <summary>
        /// crmDeliveryPlan
        /// </summary>
        //private crmDeliveryPlan _DeliveryPlan;
        [Delayed]
        [Aggregated]
        [Browsable(false)]
        public crmDeliveryPlan DeliveryPlan {
            //get { return _DeliveryPlan; }
            //set { SetPropertyValue<crmDeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value); }
            get { return GetDelayedPropertyValue<crmDeliveryPlan>("DeliveryPlan"); }
            set { SetDelayedPropertyValue<crmDeliveryPlan>("DeliveryPlan", value); }
        }
        //
        [Aggregated]
        [NonPersistent]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public crmDeliveryItem DeliveryItem {
            get {
                if (this.DeliveryPlan != null && this.DeliveryPlan.DeliveryUnit != null)
                    return this.DeliveryPlan.DeliveryUnit.DeliveryItem;
                else
                    return null;
            }
        }
        //
        [Aggregated]
        [NonPersistent]
        public XPCollection<crmDeliveryItem> DeliveryItems {
            get {
                if (this.DeliveryPlan != null && this.DeliveryPlan.DeliveryUnit != null)
                    return this.DeliveryPlan.DeliveryUnit.DeliveryItems;
                else
                    return null;
            }
        }
        //
        [Aggregated]
        [NonPersistent]
        public XPCollection<crmDeliveryUnit> DeliveryUnits {
            get {
                if (this.DeliveryMethod != DeliveryMethod.NO_DELIVERY) {
                    if (this.DeliveryPlan != null)
                        return this.DeliveryPlan.DeliveryUnits;
                    else
                        return null;
                } else
                    return null;
            }
        }
        //
        [Aggregated]
        [NonPersistent]
        public XPCollection<crmDeliveryPlanItemShedule> ItemShedules {
            get {
                if (this.DeliveryPlan != null) {
                    return this.DeliveryPlan.ItemShedules;
                } else
                    return null;
            }
        }
        // Дата планируемой поставки
        [NonPersistent]
        public DateTime DeliveryDate {
            get {
                if (this.DeliveryPlan.DeliveryUnit != null)
                    return this.DeliveryPlan.DeliveryUnit.DatePlane;
                else
                    return DateTime.MinValue;
            }
            set {
                if (this.DeliveryPlan.DeliveryUnit != null)
                    this.DeliveryPlan.DeliveryUnit.DatePlane = value;
                OnChanged("DeliveryDate");
            }
        }
        //
        //
        #endregion

        [VisibleInListView(false)]
        [Aggregated]
        public crmPaymentPlan PaymentPlan;

        /// <summary>
        /// Payment::crmPaymentUnit 
        /// </summary>
        private crmPaymentCasheLess _Settlement;
        [Browsable(false)]
        public crmPaymentCasheLess Settlement {
            get { return _Settlement; }
            set { SetPropertyValue<crmPaymentCasheLess>("Settlement", ref _Settlement, value); }
        }

        /// <summary>
        /// Advance::crmPaymentUnit 
        /// </summary>
        private crmPaymentCasheLess _Advance;
        [Browsable(false)]
        public crmPaymentCasheLess Advance {
            get { return _Advance; }
            set { SetPropertyValue<crmPaymentCasheLess>("Advance", ref _Advance, value); }
        }

        private PaymentMethod _PaymentMethod;
        // Выбор способа расчёта
        [RuleRequiredField("crmStage.PaymentMethod.Required", "Save")]
        [ImmediatePostData]
        public virtual PaymentMethod PaymentMethod {
            get { return _PaymentMethod; }
            set {
                if (!IsLoading)
                    if (_PaymentMethod == value)
                        return;
                SetPropertyValue<PaymentMethod>("PaymentMethod ", ref _PaymentMethod, value);
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmStage stage = uow.GetNestedObject<crmStage>(this);
                        stage.PaymentMethodSet(value);
                        uow.CommitChanges();
                    }
                    OnChanged("PaymentUnits");
                }
            }
        }

        void PaymentMethodSet(PaymentMethod payment_method) {
            this._PaymentMethod = payment_method;
            switch (payment_method) {
                case PaymentMethod.SCHEDULE:
                    this.Advance = null;
                    this.Settlement = null;
                    break;
                case PaymentMethod.PREPAYMENT:
                    if (this.Advance == null) {
                        this.Advance = this.PaymentPlan.PaymentCasheLessCreate();
                        this.Advance.Code = "Предоплата";
                        this.Advance.DatePlane = this.DateBegin;
                    }
                    this.Settlement = null;
                    break;
                case PaymentMethod.ADVANCE:
                    if (this.Advance == null) {
                        this.Advance = this.PaymentPlan.PaymentCasheLessCreate();
                        this.Advance.Code = "Аванс";
                        this.Advance.DatePlane = this.DateBegin;
                    }
                    if (this.Settlement == null) {
                        this.Settlement = this.PaymentPlan.PaymentCasheLessCreate();
                        this.Settlement.Code = "Расчет";
                        this.Settlement.DatePlane = this.DateFinish;
                    }
                    break;
                case PaymentMethod.SETTLEMENT:
                    if (this.Settlement == null) {
                        this.Settlement = this.PaymentPlan.PaymentCasheLessCreate();
                        this.Settlement.Code = "Расчет";
                        this.Settlement.DatePlane = this.DateFinish;
                    }
                    this.Advance = null;
                    break;
                default:
                    break;
            }
            if (this._PaymentMethod != PaymentMethod.SCHEDULE) {
                List<crmPaymentUnit> pl = new List<crmPaymentUnit>();
                foreach (crmPaymentUnit pu in this.PaymentUnits) {
                    pl.Add(pu);
                }
                foreach (crmPaymentUnit pu in pl) {
                    if (pu != this.Advance && pu != this.Settlement) {
                        this.PaymentPlan.PaymentUnits.Remove(pu);
                        pu.Delete();
                    }
                }
            }
            UpdatePayments();
        }

        [Action(PredefinedCategory.Tools, Caption = "Update payments")]
        public void UpdatePaymentsAction() {
            using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                crmStage stage = uow.GetNestedObject<crmStage>(this);
                stage.UpdatePayments();
                uow.CommitChanges();
            }
        }
        public void UpdatePayments() {
            switch (this.PaymentMethod) {
                case PaymentMethod.ADVANCE:
                    this.Advance.SummFull = this.DeliveryPlan.SummFull * this.AdvancePercentage / 100;
                    this.Settlement.SummFull = this.DeliveryPlan.SummFull - this.Advance.SummFull;
                    break;
                case PaymentMethod.PREPAYMENT:
                    this.Advance.SummFull = this.DeliveryPlan.SummFull;
                    break;
                case PaymentMethod.SETTLEMENT:
                    this.Settlement.SummFull = this.DeliveryPlan.SummFull;
                    break;
                case PaymentMethod.SCHEDULE:
                    break;
            }
        }
        // Процент аванса
        protected Decimal _AdvancePercentage;
//        [RuleRequiredField("crmDealWithoutStageVersion.AdvancePercentage.Required", "Save")]
//        [Appearance("crmDealWithoutStageVersion.AdvancePercentage.Caption.Bold", AppearanceItemType = "LayoutItem", FontColor = "Red", FontStyle = FontStyle.Bold)]
        [Size(4)]
        public Decimal AdvancePercentage {
            get { return _AdvancePercentage; }
            set {
                SetPropertyValue<Decimal>("AdvancePercentage", ref _AdvancePercentage, value);
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmStage stage = uow.GetNestedObject<crmStage>(this);
                        stage.UpdatePayments();
                        uow.CommitChanges();
                    }
                }
            }
        }

        // Сумма
        protected decimal _AdvanceSumm;
        //[RuleRequiredField("crmDealWithoutStageVersion.AdvanceSumm.Required", "Save")]
        //[Appearance("crmDealWithoutStageVersion.AdvanceSumm.Caption.Bold", AppearanceItemType = "LayoutItem", FontColor = "Red", FontStyle = FontStyle.Bold)]
        public decimal AdvanceSumm {
            get { return _AdvanceSumm; }
            set {
                SetPropertyValue<decimal>("AdvanceSumm", ref _AdvanceSumm, value);
            }
        }

        //private DateTime _AdvanceDate;
        // Дата планируемого аванса (предоплаты)
        [NonPersistent]
        public DateTime? AdvanceDate {
            get {
                if (this.Advance != null)
                    return this.Advance.DatePlane;
                else
                    return null;
            }
            set {
                if (this.Advance != null)
                    if (value != null)
                        this.Advance.DatePlane = (DateTime)value;
                OnChanged("AdvanceDate");
            }
        }

        // private DateTime _SettlementDate;
        // Дата планируемого окончательного расчёта
        public DateTime? SettlementDate {
            get {
                if (this.Settlement != null)
                    return this.Settlement.DatePlane;
                else
                    return null;
            }
            set {
                if (this.Settlement != null)
                    if (value != null)
                        this.Settlement.DatePlane = (DateTime)value;
                OnChanged("SettlementDate");
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set {
                crmCostModel old = this._CostModel;
                SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value);
                if (!IsLoading) {
                    this.DeliveryPlan.CostModel = value;
                    this.PaymentPlan.CostModel = value;
                    foreach (crmStage stage in SubStages)
                        if (stage.CostModel == null || stage.CostModel == old)
                            stage.CostModel = value;
                }
            }
        }
        //
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        [NonPersistent]
        public crmCostCol CurrentCost{
            get { return DeliveryPlan.CurrentCost; }
        }
        //
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        [NonPersistent]
        public crmCostCol CurrentPayment {
            get { return PaymentPlan.CurrentCost; }
        }
        

        /// <summary>
        /// ObligationTransfer Обязательство на данном этапе
        /// </summary>
        //[Association("crmStage-PaymentUnits", typeof(crmPaymentUnit))]
        [NonPersistent]
        [Aggregated]
        public XPCollection<crmPaymentUnit> PaymentUnits {
            get { return PaymentPlan.PaymentUnits; }
        }

        private StageType _StageType;
        [ImmediatePostData]
        public StageType StageType {
            get { return _StageType; }
            set {
                if (!IsLoading) {
                    if (this.TopStage != null) {
                        if (this.TopStage.StageType == StageType.AGREGATE) {
                            if (value != StageType.AGREGATE && value != StageType.FINANCE) {
                                value = StageType.AGREGATE;
                            }
                        }
                        if (this.TopStage.StageType == StageType.FINANCE || this.TopStage.StageType == StageType.TECHNICAL) {
                            value = StageType.TECHNICAL;
                        }
                    }
                }
                SetPropertyValue<StageType>("StageType", ref _StageType, value);
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmStage stage = uow.GetNestedObject(this);
                        stage.StageTypeSet(value);
                        uow.CommitChanges();
                    }
                }
            }
        }

        public void StageTypeSet(StageType stage_type) {
            this._StageType = stage_type;
            if (this.DeliveryMethod == 0) {
                if (stage_type == StageType.FINANCE || stage_type == StageType.TECHNICAL) {
                    this.DeliveryMethod = DeliveryMethod.SINGLE_SERVICE_AT_THE_END;
                }
                else {
                    this.DeliveryMethod = DeliveryMethod.NO_DELIVERY;
                }
            }
            if (this.PaymentMethod == 0) {
                if (stage_type == StageType.FINANCE) {
                    this.PaymentMethodSet(PaymentMethod.ADVANCE);
                }
                else {
                    this.PaymentMethodSet(PaymentMethod.SCHEDULE);
                }
            }
            if (stage_type == StageType.AGREGATE) {
                List<crmDeliveryUnit> dul = new List<crmDeliveryUnit>();
                foreach (crmDeliveryUnit du in this.DeliveryPlan.DeliveryUnits)
                    dul.Add(du);
                foreach (crmDeliveryUnit du in dul) {
                    this.DeliveryPlan.DeliveryUnits.Remove(du);
                    du.Delete();
                }
            }
            if (stage_type == StageType.AGREGATE || stage_type == StageType.TECHNICAL) {
                //    //
                List<crmPaymentUnit> pul = new List<crmPaymentUnit>();
                foreach (crmPaymentUnit pu in this.PaymentPlan.PaymentUnits)
                    pul.Add(pu);
                foreach (crmPaymentUnit pu in pul) {
                    this.PaymentPlan.PaymentUnits.Remove(pu);
                    pu.Delete();
                }
            }
            if (this.StageType == StageType.FINANCE || this.StageType == StageType.TECHNICAL) {
                foreach (crmStage st in this.SubStages) {
                    st.StageTypeSet(StageType.TECHNICAL);
                }
            }
        }

        /// <summary>
        /// csValuta
        /// </summary>
        private csValuta _Valuta;
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                csValuta old = this._Valuta;
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
                if (!IsLoading) {
                    this.DeliveryPlan.Valuta = value;
                    this.PaymentPlan.Valuta = value;
                    foreach (crmStage stage in SubStages)
                        if (stage.Valuta == null || stage.Valuta == old)
                            stage.Valuta = value;
                }
            }
        }
        /// <summary>
        /// csValuta
        /// </summary>
        private csValuta _PaymentValuta;
        public virtual csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set {
                csValuta old = this._PaymentValuta;
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
                if (!IsLoading) {
                    this.PaymentPlan.PaymentValuta = value;
                    foreach (crmStage stage in SubStages)
                        if (stage.PaymentValuta == null || stage.PaymentValuta == old)
                            stage.PaymentValuta = value;
                }
            }
        }
        // Ставка НДС (VAT Rate)
        private csNDSRate _NDSRate;
        public virtual csNDSRate NDSRate {
            get { return _NDSRate; }
            set {
                csNDSRate old = this._NDSRate;
                SetPropertyValue<csNDSRate>("NDSRate", ref _NDSRate, value);
                if (!IsLoading) {
                    this.DeliveryPlan.NDSRate = value;
                    this.PaymentPlan.NDSRate = value;
                    foreach (crmStage stage in SubStages)
                        if (stage.NDSRate == null || stage.NDSRate == old)
                            stage.NDSRate = value;
                }
            }
        }

        /// <summary>
        /// CostItem
        /// </summary>
        private fmCostItem _CostItem;
        public virtual fmCostItem CostItem {
            get { return _CostItem; }
            set {
                fmCostItem old = this._CostItem;
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
                if (!IsLoading) {
                    this.DeliveryPlan.CostItem = value;
                    this.PaymentPlan.CostItem = value;
                    foreach (crmStage stage in SubStages)
                        if (stage.CostItem == null || stage.CostItem == old)
                            stage.CostItem = value;
                }
            }
        }

        /// <summary>
        /// Order
        /// </summary>
        private fmOrder _Order;
        public virtual fmOrder Order {
            get { return _Order; }
            set {
                fmOrder old = this._Order;
                SetPropertyValue<fmOrder>("Order", ref _Order, value);
                if (!IsLoading) {
                    this.DeliveryPlan.Order = value;
                    this.PaymentPlan.Order = value;
                    foreach (crmStage stage in SubStages)
                        if (stage.Order == null || stage.Order == old)
                            stage.Order = value;
                }
            }
        }
        //
        protected override void OnLoaded() {
            base.OnLoaded();
            CurrentCost.Changed += OnCurrentCostChanged;
            CurrentPayment.Changed += OnCurrentPaymentChanged;
        }
        //
        public void OnCurrentCostChanged(object sender, ObjectChangeEventArgs e) {
            OnChanged("CurrentCost");
        }
        //
        public void OnCurrentPaymentChanged(object sender, ObjectChangeEventArgs e) {
            OnChanged("CurrentPayment");
        }

        #endregion
        

        #region МЕТОДЫ
        #region ITreeNode Members

        IBindingList ITreeNode.Children {
            get { return SubStages; }
        }

        string ITreeNode.Name {
            get { return Code; }
        }

        ITreeNode ITreeNode.Parent {
            get { return TopStage; }
        }

        #endregion

        protected void UpdateStageCostInt(crmCostValue sp, Boolean mode) {
            //if (CurrentStageCost != null)
            //    CurrentStageCost.UpdatePrice(sp, mode);
            //UpdateStageCost(sp, mode);
        }

        //public void UpdateCost(crmCostItem sp, Boolean mode) {
        //    crmCostItem cost = this.CostGet(sp.CostModel, sp.Valuta);
        //    if (cost != null)
        //        cost.UpdateCost(sp, mode);
        //    if (TopStage != null)
        //        TopStage.UpdateCost(sp, mode);
        //}


        //public crmStageMain GetStageMain() {
        //    if (this.StageMain == null) {
        //        this.StageMain = new crmStageMain(this.Session);
        //        this.StageMain.Current = this;
        //        this.StageMain.ContractDeal = this.DealVersion.ContractDeal;
        //    }
        //    return this.StageMain;
        //}

        #endregion

    }

}