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
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using System.Windows.Forms;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс SimpleContractVersion, представляющий объект Договора
    /// </summary>
    
    // Упразднение кнопки  везде для любого такого объекта, в котором поле DoHide = true
    [Appearance("SimpleContractVersionApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6", TargetItems = "SimpleContractVersionApprove", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "Any")]
    //[DefaultClassOptions]
    [Persistent("crmSimpleContractVersion")]
    public partial class SimpleContractVersion : ContractVersion
    {
        public SimpleContractVersion(Session session) : base(session) { }
        public SimpleContractVersion(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.DeliveryPlan = new DeliveryPlan(this.Session, this.VersionState);
            this.PaymentPlan = new PaymentPlan(this.Session, this.VersionState);
            this.Customer = new crmContractParty(this.Session, this.VersionState);
            this.Supplier = new crmContractParty(this.Session, this.VersionState);
        }                  

        #region ПОЛЯ КЛАССА
        
        #endregion


        #region СВОЙСТВА КЛАССА

        // Заказчик
        private crmContractParty _Customer;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmContractParty Customer {
            get { return _Customer; }
            set { 
                SetPropertyValue<crmContractParty>("Customer", ref _Customer, value);
                if (!IsLoading) {
                    if (this.DeliveryPlan != null) this.DeliveryPlan.Customer = this.Customer;
                    if (this.PaymentPlan != null) this.PaymentPlan.Customer = this.Customer;
                }
            }
        }

        // Исполнитель
        private crmContractParty _Supplier;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmContractParty Supplier {
            get { return _Supplier; }
            set { 
                SetPropertyValue<crmContractParty>("Supplier", ref _Supplier, value);
                if (!IsLoading) {
                    if (this.DeliveryPlan != null) this.PaymentPlan.Supplier = this.Supplier;
                    if (this.PaymentPlan != null) this.DeliveryPlan.Supplier = this.Supplier;
                }
            }
        }

        // План поставок
        private DeliveryPlan _DeliveryPlan;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public DeliveryPlan DeliveryPlan {
            get { return _DeliveryPlan; }
            set { SetPropertyValue<DeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value); }
        }

        // План расчётов платежей)
        private PaymentPlan _PaymentPlan;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public PaymentPlan PaymentPlan {
            get { return _PaymentPlan; }
            set { SetPropertyValue<PaymentPlan>("PaymentPlan", ref _PaymentPlan, value); }
        }
        //
        public override crmContractCategory Category {
            get { return base.Category; }
            set {
                base.Category = value;
                if (!IsLoading) {
                    if (this.SimpleContract != null) {
                        if (this.SimpleContract.Category == null)
                            this.SimpleContract.Category = value;
                    }
                }
            }
        }

        public override string Number {
            get { return base.Number; }
            set {
                base.Number = value;
                if (!IsLoading) {
                    if (this.SimpleContract != null) {
                        if (this.SimpleContract.Contract.ContractDocument != null)
                            this.SimpleContract.Contract.ContractDocument.Number = value;
                    }
                }
            }
        }

        public override DateTime Date {
            get { return base.Date; }
            set {
                base.Date = value;
                if (!IsLoading) {
                    if (!IsLoading) {
                        if (this.SimpleContract != null) {
                            if (this.SimpleContract.Contract.ContractDocument != null)
                                this.SimpleContract.Contract.ContractDocument.Date = value;
                        }
                    }
                }
            }
        }

        public override crmContractDocumentCategory DocumentCategory {
            get { return base.DocumentCategory; }
            set {
                base.DocumentCategory = value;
                if (!IsLoading) {
                    if (!IsLoading) {
                        if (this.SimpleContract != null) {
                            if (this.SimpleContract.Contract.ContractDocument != null)
                                this.SimpleContract.Contract.ContractDocument.DocumentCategory = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        private fmOrder _fmOrder;
        public fmOrder fmOrder {
            get { return _fmOrder; }
            set {
                SetPropertyValue<fmOrder>("fmOrder", ref _fmOrder, value);
                if (!IsLoading) {
                    if (this.DeliveryPlan != null) DeliveryPlan.fmOrder = value;
                    if (this.PaymentPlan != null) PaymentPlan.fmOrder = value;
                }
            }
        }
        /// <summary>
        /// Valuta
        /// </summary>
        private Valuta _Valuta;
        public Valuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<Valuta>("Valuta", ref _Valuta, value);
                if (!IsLoading) {
                    if (this.DeliveryPlan != null) this.DeliveryPlan.Valuta = value;
                    if (this.PaymentPlan != null) this.PaymentPlan.Valuta = value;
                }
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set {
                SetPropertyValue<crmCostModel>("Costmodel", ref _CostModel, value);
                if (!IsLoading) {
                    if (this.DeliveryPlan != null) this.DeliveryPlan.CostModel = value;
                    if (this.PaymentPlan != null) this.PaymentPlan.CostModel = value;
                }
            }
        }

        public VersionStates VersionStatus {
            get { return VersionState; }
        }

        string _Comment;
        public string Comment {
            get { return _Comment; }
            set { SetPropertyValue<string>("Comment", ref _Comment, value); }
        }
        
        /// <summary>
        /// ContractDocument для версии
        /// </summary>
        private ContractDocument _VersionContractDocument;
        [DataSourceProperty("SimpleContract.Contract.ContractDocuments")]
        public ContractDocument VersionContractDocument {
            get { return _VersionContractDocument; }
            set { SetPropertyValue<ContractDocument>("VersionContractDocument", ref _VersionContractDocument, value); }
        }

        public override object MainObject {
            get { return this.SimpleContract; }
        }

        #endregion


        #region МЕТОДЫ

        public void ApproveVersion() {
            this.SimpleContract.ApproveVersion(this);
        }


        protected override void TriggerObjectChanged(ObjectChangeEventArgs args) {
            if (this.VersionContractDocument == null) {
                base.TriggerObjectChanged(args);
                return;
            }
            //if (args.NewValue != null & (args.PropertyName == "Number" | args.PropertyName == "Date" | args.PropertyName == "DocumentCategory" | args.PropertyName == "VersionContractDocument")) {
            if (args.PropertyName == "Number" | args.PropertyName == "Date" | args.PropertyName == "DocumentCategory" | args.PropertyName == "VersionContractDocument") {
                if (this.VersionContractDocument == null) {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Перед редактированием полей 'Номер', 'Дата', 'Категория документа' необходимо указать документ!");
                } else {
                    this.VersionContractDocument.Number = this.Number;
                    this.VersionContractDocument.Date = this.Date;
                    this.VersionContractDocument.DocumentCategory = this.DocumentCategory;
                }
            }
            base.TriggerObjectChanged(args);
        }
        #endregion

    }

}