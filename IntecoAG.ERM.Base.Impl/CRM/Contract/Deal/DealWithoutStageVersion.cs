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
using System.Drawing;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    public enum PaymentMethod {
        PREPAYMENT = 1,
        ADVANCE = 2,
        SETTLEMENT = 3,
        SCHEDULE = 4
    }

    /// <summary>
    /// Класс crmDealWithoutStageVersion
    /// </summary>
    [Appearance("crmDealWithoutStageVersion.ApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6", TargetItems = "VersionApprove", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    // Управление отображением расчетов
    [Appearance("crmDealWithoutStageVersion.PaymentUnitsHidden", 
        AppearanceItemType = "LayoutItem", Criteria = "PaymentMethod != 4", 
        TargetItems = "PaymentUnits", Visibility = ViewItemVisibility.Hide, Enabled= false)]
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
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDealWithoutStageVersion : crmDealVersion, IVersionSupport, IVersionBusinessLogicSupport
    {
        public crmDealWithoutStageVersion(Session ses) : base(ses) { }
        public crmDealWithoutStageVersion(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            //
            this.DeliveryPlan = new crmDeliveryPlan(this.Session, this.VersionState);
            this.DeliveryPlan.DealVersion = this;
            this.DeliveryPlan.Creditor = this.Customer;
            this.DeliveryPlan.Debitor = this.Supplier;
            //
            this.PaymentPlan = new crmPaymentPlan(this.Session, this.VersionState);
            this.PaymentPlan.DealVersion = this;
            this.PaymentPlan.Debitor = this.Customer;
            this.PaymentPlan.Creditor = this.Supplier;
            //
            this.DeliveryMethod = DeliveryMethod.UNIT_AT_THE_END;
            this.PaymentMethod = PaymentMethod.ADVANCE;
            //
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        
        #region crmDealVersion

        public override String DescriptionShort {
            set {
                String old = base.DescriptionShort;
                base.DescriptionShort = value;
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

        public override String DescriptionLong {
            set {
                String old = base.DescriptionLong;
                base.DescriptionLong = value;
                if (!IsLoading) {
                    if (this.DeliveryItem != null) {
                        if (this.DeliveryItem.Description == old ||
                            String.IsNullOrEmpty(this.DeliveryItem.Description) ) {
                            this.DeliveryItem.Description = value;
                        }
                    }
                }
            }
        }

        public override DateTime DateBegin {
            set {
                DateTime old = base.DateBegin;
                base.DateBegin = value;
                if (!IsLoading) {
                    this.DeliveryPlan.DateBegin = value;
                    if (AdvanceDate == DateTime.MinValue ||
                        AdvanceDate == old) {
                        AdvanceDate = value;
                    }
                }
            }
        }

        public override DateTime DateEnd {
            set {
                DateTime old = base.DateEnd;
                base.DateEnd = value;
                if (!IsLoading) {
                    this.DeliveryPlan.DateEnd = value;
                    if (DeliveryDate == DateTime.MinValue ||
                        DeliveryDate == old) {
                        DeliveryDate = value;
                    }
                }
            }
        }

        public override DateTime DateFinish {
            set {
                DateTime old = base.DateFinish;
                base.DateFinish = value;
                if (!IsLoading) {
                    if (SettlementDate == DateTime.MinValue || 
                        SettlementDate == old) {
                        SettlementDate = value;
                    }
                }
            }
        }
        //
        public override csValuta Valuta {
            set {
                base.Valuta = value;
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmDealWithoutStageVersion deal_version = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                        if (deal_version.DeliveryPlan != null)
                            deal_version.DeliveryPlan.Valuta = value == null ? null : uow.GetNestedObject<csValuta>(value);
                        if (deal_version.PaymentPlan != null)
                            deal_version.PaymentPlan.Valuta = value == null ? null : uow.GetNestedObject<csValuta>(value);
                        uow.CommitChanges();
                    }
                }
            }
        }


        /// <summary>
        /// CostModel
        /// </summary>
        public override crmCostModel CostModel {
            set {
                base.CostModel = value;
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmDealWithoutStageVersion deal_version = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                        if (deal_version.DeliveryPlan != null)
                            deal_version.DeliveryPlan.CostModel = value == null ? null : uow.GetNestedObject<crmCostModel>(value);
                        if (deal_version.PaymentPlan != null)
                            deal_version.PaymentPlan.CostModel = value == null ? null : uow.GetNestedObject<crmCostModel>(value);
                        uow.CommitChanges();
                    }
                }
            }
        }
        /// <summary>
        /// CostItem
        /// </summary>
        public override fmCostItem CostItem {
            set {
                base.CostItem = value;
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmDealWithoutStageVersion deal_version = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                        if (deal_version.DeliveryPlan != null)
                            deal_version.DeliveryPlan.CostItem = value == null ? null : uow.GetNestedObject<fmCostItem>(value);
                        if (deal_version.PaymentPlan != null)
                            deal_version.PaymentPlan.CostItem = value == null ? null : uow.GetNestedObject<fmCostItem>(value);
                        uow.CommitChanges();
                    }
                }
            }
        }
        /// <summary>
        /// Order
        /// </summary>
        public override fmCOrder Order {
            set {
                base.Order = value;
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmDealWithoutStageVersion deal_version = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                        if (deal_version.DeliveryPlan != null)
                            deal_version.DeliveryPlan.Order = value == null ? null : uow.GetNestedObject<fmCOrder>(value);
                        if (deal_version.PaymentPlan != null)
                            deal_version.PaymentPlan.Order = value == null ? null : uow.GetNestedObject<fmCOrder>(value);
                        uow.CommitChanges();
                    }
                }
            }
        }
        // Ставка НДС (VAT Rate)
        public override csNDSRate NDSRate {
            set {
                base.NDSRate = value;
                if (!IsLoading) {
                    using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                        crmDealWithoutStageVersion deal_version = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                        if (deal_version.DeliveryPlan != null)
                            deal_version.DeliveryPlan.NDSRate = value == null ? null : uow.GetNestedObject<csNDSRate>(value);
                        if (deal_version.PaymentPlan != null)
                            deal_version.PaymentPlan.NDSRate = value == null ? null : uow.GetNestedObject<csNDSRate>(value);
                        uow.CommitChanges();
                    }
                }
            }
        }
        //
        #endregion

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
                }
                else
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
                }
                else
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

        #region Payment

        private PaymentMethod _PaymentMethod;
        // Выбор способа расчёта
        [RuleRequiredField("crmDealWithoutStageVersion.PaymentMethod.Required", "Save")]
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
                        crmDealWithoutStageVersion deal = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                        deal.PaymentMethodSet();
                        uow.CommitChanges();
                    }
                    OnChanged("PaymentUnits");
                }
            }
        }
        //
        void PaymentMethodSet() {
            switch (this.PaymentMethod) {
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
            if (_PaymentMethod != PaymentMethod.SCHEDULE) {
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
        /// <summary>
        /// crmPaymentPlan
        /// </summary>
        //private crmPaymentPlan _PaymentPlan;
        [Delayed]
        [Aggregated]
        [Browsable(false)]
        public crmPaymentPlan PaymentPlan {
            //get { return _PaymentPlan; }
            //set { SetPropertyValue<crmPaymentPlan>("PaymentPlan", ref _PaymentPlan, value); }
            get { return GetDelayedPropertyValue<crmPaymentPlan>("PaymentPlan"); }
            set { SetDelayedPropertyValue<crmPaymentPlan>("PaymentPlan", value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [NonPersistent]
        public XPCollection<crmPaymentUnit> PaymentUnits {
            get {
                if (this.PaymentPlan != null)
                    return this.PaymentPlan.PaymentUnits;
                else
                    return null;
            }
        }
        [Action(PredefinedCategory.Tools, Caption = "Update payments")]
        public void UpdatePaymentsAction() {
            using (NestedUnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                crmDealWithoutStageVersion deal = uow.GetNestedObject<crmDealWithoutStageVersion>(this);
                deal.UpdatePayments();
                uow.CommitChanges();
            }
        }
        //
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

        // Процент аванса
        protected Decimal _AdvancePercentage;
        [RuleRequiredField("crmDealWithoutStageVersion.AdvancePercentage.Required", "Save")]
        [Appearance("crmDealWithoutStageVersion.AdvancePercentage.Caption.Bold", AppearanceItemType = "LayoutItem", FontColor = "Red", FontStyle = FontStyle.Bold)]
        [Size(4)]
        public Decimal AdvancePercentage {
            get { return _AdvancePercentage; }
            set {
                SetPropertyValue<Decimal>("AdvancePercentage", ref _AdvancePercentage, value);
                if (!IsLoading) {
                    UpdatePaymentsAction();
                }
            }
        }

        // Сумма
        protected decimal _AdvanceSumm;
        [RuleRequiredField("crmDealWithoutStageVersion.AdvanceSumm.Required", "Save")]
        [Appearance("crmDealWithoutStageVersion.AdvanceSumm.Caption.Bold", AppearanceItemType = "LayoutItem", FontColor = "Red", FontStyle = FontStyle.Bold)]
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
        #endregion
        //
        [PersistentAlias("DeliveryPlan.CurrentCost")]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public crmCostCol CurrentCost {
            get {
                return DeliveryPlan.CurrentCost;
            }
        }
        //
        [PersistentAlias("PaymentPlan.CurrentCost")]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public crmCostCol CurrentPayment {
            get {
                return PaymentPlan.CurrentCost;
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
        /// <summary>
        /// FinancialDeal
        /// </summary>
        private crmFinancialDealVersion _FinancialDeal;
        [Browsable(false)]
        public crmFinancialDealVersion FinancialDeal {
            get { return _FinancialDeal; }
            set { SetPropertyValue<crmFinancialDealVersion>("FinancialDeal", ref _FinancialDeal, value); }
        }
        /// <summary>
        /// Получение головного объекта
        /// </summary>
        public override object MainObject {
            get { return this.ContractDeal as crmDealWithoutStage; }
        }

        #endregion


        #region МЕТОДЫ

        public void ApproveVersion() {
            if (this.ContractDeal as crmDealWithoutStage != null) (this.ContractDeal as crmDealWithoutStage).ApproveVersion(this);
        }

        #endregion

        #region IVersionBusinessLogicSupport

        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(((crmDealWithoutStage)(this.MainObject)).Current), vHelper);
        }

        public void Approve(IVersionSupport obj) {
        }

        #endregion

    }
}