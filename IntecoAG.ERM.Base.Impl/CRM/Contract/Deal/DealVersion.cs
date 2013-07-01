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
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Класс crmDealVersion
    /// </summary>
    [MiniNavigation("ContractDeal.Current", "Действующая редакция", TargetWindow.Current, 1)]
    [MiniNavigation("ContractDeal.Contract", "Договор", TargetWindow.Default, 2)]
    [Persistent("crmDealVersion")]
    public partial class crmDealVersion : VersionRecord    // BaseObject, ICategorizedItem
    {
        public crmDealVersion(Session ses) : base(ses) { }
        public crmDealVersion(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            //this.crmContract = new crmContract(this.Session);
            //DealState = DealStates.DEAL_PROJECT;
            
        }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.DealCode = "ВЕД1";
            this.Customer = new crmContractParty(this.Session, this.VersionState);
            this.Supplier = new crmContractParty(this.Session, this.VersionState);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        // Список всех версий. Получаем из головного объекта примерно таким обращением: .Current.Versions
        public XPCollection<crmDealVersion> DealVersions {
            get { return this.ContractDeal.DealVersions; }
        }

        public XPCollection<fmCSubject> Subjects {
            get { return this.ContractDeal.Subjects; }
        }

        [Aggregated]
        [Association("crmDealVersion-DealNomenclature", typeof(crmDealNomenclature))]
        public XPCollection<crmDealNomenclature> DealNomenclatures {
            get {
                return GetCollection<crmDealNomenclature>("DealNomenclatures");
            }
        }

        //private DealStates _DealState;
        //[RuleRequiredField("crmDealVersion.DealState.Required", "Save")]
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("ContractDeal.State")]
        public virtual DealStates DealState {
            get { return this.ContractDeal.State; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IList<crmContractParty> Partys {
            get {
                List<crmContractParty> lr = new List<crmContractParty>();
                lr.Add(this.Customer);
                lr.Add(this.Supplier);
                return lr;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public VersionStates ShowVersionState {
            get { return this.VersionState; }
        }

        /// <summary>
        /// StageStructure
        /// </summary>
        private crmStageStructure _StageStructure;
        [Browsable(false)]
        [Aggregated]
        public virtual crmStageStructure StageStructure {
            get { return _StageStructure; }
            set { SetPropertyValue<crmStageStructure>("StageStructure", ref _StageStructure, value); }
        }
        
        private string _DescriptionShort;
        public virtual string DescriptionShort {
            get { return _DescriptionShort; }
            set { SetPropertyValue<string>("DescriptionShort", ref _DescriptionShort, value); }
        }

        private string _DescriptionLong;
        [Size(SizeAttribute.Unlimited)]
        public virtual string DescriptionLong {
            get { return _DescriptionLong; }
            set { SetPropertyValue<string>("DescriptionLong", ref _DescriptionLong, value); }
        }

        private DateTime _DateBegin;
        //[VisibleInListView(false)]
        public virtual DateTime DateBegin {
            get { return _DateBegin; }
            set {
                SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value);
                if (!IsLoading) {
                    if (this.DateEnd < value) {
                        this.DateEnd = value;
                    }
                }
            }
        }

        private DateTime _DateEnd;
        public virtual DateTime DateEnd {
            get { return _DateEnd; }
            set {
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
                if (!IsLoading) {
                    if (this.DateBegin > value) {
                        this.DateBegin = value;
                    }
                    if (this.DateFinish < value) {
                        this.DateFinish = value;
                    }
                }
            }
        }

        private DateTime _DateFinish;
        public virtual DateTime DateFinish {
            get { return _DateFinish; }
            set { 
                SetPropertyValue<DateTime>("DateFinish", ref _DateFinish, value);
                if (this.DateEnd > value) {
                    this.DateEnd = value;
                }
            }
        }

        private string _DealCode;
        // Кодовое обозначение Простого договора. Кодом обычно является номер договорного документа (ContractDocument.Number).
        [Size(30)]
        //[RuleRequiredField("crmDealWithoutStageVersion.DealCode.Required", "Save")]
        //[RuleRequiredField("crmDealWithoutStageVersion.Number.Required.Immediate", "Immediate")]
        public virtual string DealCode {
            get { return _DealCode; }
            set {
                SetPropertyValue<string>("DealCode", ref _DealCode, value);
            }
        }

        private decimal _Price;
        public virtual decimal Price {
            get { return _Price; }
            set { SetPropertyValue<decimal>("Price", ref _Price, value); }
        }

        /// <summary>
        /// csValuta
        /// </summary>
        private csValuta _Valuta;
        public virtual csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        /// <summary>
        /// csValuta
        /// </summary>
        private csValuta _PaymentValuta;
        public virtual csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set {
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
            }
        }

        // Ставка НДС (VAT Rate)
        private csNDSRate _NDSRate;
        public virtual csNDSRate NDSRate {
            get { return _NDSRate; }
            set { SetPropertyValue<csNDSRate>("NDSRate", ref _NDSRate, value);  }
        }


        /// <summary>
        /// FinancialStructure
        /// </summary>
        private crmFinancialStructure _FinancialStructure;
        [Aggregated]
        public crmFinancialStructure FinancialStructure {
            get { return _FinancialStructure; }
            set { SetPropertyValue<crmFinancialStructure>("FinancialStructure", ref _FinancialStructure, value); }
        }

        [RuleRequiredField(TargetCriteria = "ContractDeal.State != 'DEAL_CLOSED' && ContractDeal.State != 'DEAL_DELETED'")]
        [PersistentAlias("ContractDeal.TRVType")]
        public crmContractDealTRVType TRVType {
            get { return this.ContractDeal.TRVType; }
            set {
                crmContractDealTRVType old = this.ContractDeal.TRVType;
                this.ContractDeal.TRVType = value;
                if (old != value)
                    OnChanged("TRVType", old, value);
            }
        }

        [RuleRequiredField(TargetCriteria = "ContractDeal.State != 'DEAL_CLOSED' && ContractDeal.State != 'DEAL_DELETED'")]
        [PersistentAlias("ContractDeal.TRVContractor")]
        public crmContractDealTRVContractor TRVContractor {
            get { return this.ContractDeal.TRVContractor; }
            set {
                crmContractDealTRVContractor old = this.ContractDeal.TRVContractor;
                this.ContractDeal.TRVContractor = value;
                if (old != value)
                    OnChanged("TRVContractor", old, value);
            }
        }

        /// <summary>
        /// Registrator
        /// </summary>
        [PersistentAlias("ContractDeal.DepartmentRegistrator")]
        public hrmDepartment Registrator {
            get { return this.ContractDeal.DepartmentRegistrator; }
            set {
                this.ContractDeal.DepartmentRegistrator = value;
                OnChanged("Registrator");
            }
        }

        /// <summary>
        /// Curator
        /// </summary>
        [PersistentAlias("ContractDeal.CuratorRegistrator")]
        public hrmDepartment Curator {
            get { return this.ContractDeal.CuratorDepartment; }
            set {
                this.ContractDeal.CuratorDepartment = value;
                OnChanged("Curator");
            }
        }

        /// <summary>
        /// ContractDocument 
        /// </summary>
        private crmContractDocument _ContractDocument;
        //[ExpandObjectMembers(ExpandObjectMembers.Always)]
        [DataSourceProperty("ContractDocuments")]
        public crmContractDocument ContractDocument {
            get { return _ContractDocument; }
            set {
                if (!IsLoading) {
                    if (value.Contract == null) {
                        value.Contract = this.ContractDeal.Contract;
                    }
                }
                SetPropertyValue<crmContractDocument>("ContractDocument", ref _ContractDocument, value);
            }
        }
        /// <summary>
        /// ContractDocument 
        /// </summary>
        private crmContractDocument _DealDocument;
        //[ExpandObjectMembers(ExpandObjectMembers.Always)]
        [DataSourceProperty("ContractDocuments")]
        public crmContractDocument DealDocument {
            get {
                if (this._DealDocument != null)
                    return _DealDocument;
                else
                    return (this.ContractDeal != null) ? this.ContractDeal.ContractDocument : null;
            }
            set {
                if (!IsLoading) {
                    if (value.Contract == null) {
                        value.Contract = this.ContractDeal.Contract;
                    }
                    if (value == this.ContractDeal.ContractDocument)
                        value = null;
                }
                SetPropertyValue<crmContractDocument>(" DealDocument", ref _DealDocument, value);
            }
        }

        public XPCollection<crmContractDocument> ContractDocuments {
            //        public IList<crmContractDocument> ContractDocuments {
            get {
                return this.ContractDeal.ContractDocuments;
            }
        }


        [PersistentAlias("ContractDeal.Category")]
        public crmContractCategory Category {
            get { return this.ContractDeal.Category; }
            set { this.ContractDeal.Category = value; }
        }


        private crmContractParty _Customer;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public virtual crmContractParty Customer {
            get { return _Customer; }
            set {
                SetPropertyValue<crmContractParty>("Customer", ref _Customer, value);
            }
        }

        private crmContractParty _Supplier;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public virtual crmContractParty Supplier {
            get { return _Supplier; }
            set { SetPropertyValue<crmContractParty>("Supplier", ref _Supplier, value); }
        }



        /// <summary>
        /// CostItem
        /// </summary>
        private fmCostItem _CostItem;
        public virtual fmCostItem CostItem {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }

        /// <summary>
        /// CostModel
        /// </summary>
        private crmCostModel _CostModel;
        public virtual crmCostModel CostModel {
            get { return _CostModel; }
            set { SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value); }
        }

        /// <summary>
        /// Order
        /// </summary>
        private fmCOrder _Order;
        public virtual fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }


        #endregion


        #region МЕТОДЫ

        // Методы создания главных объектов для финансового Stage и для ObligationUnit (DeliveryUnit и PaymentUnit)''

        public crmStageMain CreateStageMain(crmStage stage) {
            if (stage.StageMain == null) {
                crmStageMain StageMain = new crmStageMain(this.Session);
                StageMain.Current = stage;
                StageMain.ContractDeal = ContractDeal;
                stage.StageMain = StageMain;
            }
            return stage.StageMain;
        }

        public IList<crmObligationUnitMain> CreateFinanceDeliveryList(crmDeliveryPlan deliveryPlan) {
            IList<crmObligationUnitMain> res = new List<crmObligationUnitMain>();
            foreach (crmObligationUnit deliveryUnit in deliveryPlan.DeliveryUnits) {
                if (deliveryUnit.ObligationUnitMain == null) {
                    crmObligationUnitMain ObligationUnitMain = new crmObligationUnitMain(this.Session);
                    ObligationUnitMain.Current = deliveryUnit;
                    ObligationUnitMain.ContractDeal = ContractDeal;
                    deliveryUnit.ObligationUnitMain = ObligationUnitMain;
                }
                if (!res.Contains(deliveryUnit.ObligationUnitMain)) res.Add(deliveryUnit.ObligationUnitMain);
            }
            return res;
        }

        public IList<crmObligationUnitMain> CreateFinancePaymentList(crmPaymentPlan paymentPlan) {
            IList<crmObligationUnitMain> res = new List<crmObligationUnitMain>();
            foreach (crmObligationUnit paymentUnit in paymentPlan.PaymentUnits) {
                if (paymentUnit.ObligationUnitMain == null) {
                    crmObligationUnitMain ObligationUnitMain = new crmObligationUnitMain(this.Session);
                    ObligationUnitMain.Current = paymentUnit;
                    ObligationUnitMain.ContractDeal = ContractDeal;
                    paymentUnit.ObligationUnitMain = ObligationUnitMain;
                }
                if (!res.Contains(paymentUnit.ObligationUnitMain)) res.Add(paymentUnit.ObligationUnitMain);
            }
            return res;
        }

        #endregion



        /*
        ITreeNode ICategorizedItem.Category {
            get {
                return Category;
            }
            set {
                Category = (crmContractCategory) value;
            }
        }
        */
    }

}