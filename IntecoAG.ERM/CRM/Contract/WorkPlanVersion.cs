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
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;

using System.Windows.Forms;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс WorkPlanVersion, представляющий объект Договора
    /// </summary>
//    [DefaultClassOptions]
    [Appearance("WorkPlanVersionApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6 OR not isnull(ComplexContractVersion)", TargetItems = "WorkPlanVersionApprove", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "Any")]
    [Persistent("crmWorkPlanVersion")]
    public partial class WorkPlanVersion : VersionRecord   // BaseObject, IVersionSupport
    {
        public WorkPlanVersion(Session session) : base(session) { }
        public WorkPlanVersion(Session session, VersionStates state) : base(session, state) { }

        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    this.VersionAfterConstruction();
        //}

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.ContractDocument = new ContractDocument(this.Session);
            this.Customer = new crmContractParty(this.Session, this.VersionState);
            this.Supplier = new crmContractParty(this.Session, this.VersionState);
            this.StageStructure = new crmStageStructure(this.Session, this.VersionState);
            this.DeliveryPlan = new DeliveryPlan(this.Session, this.VersionState);
            this.DeliveryPlan.Customer = this.Customer;
            this.DeliveryPlan.Supplier = this.Supplier;
            this.DeliveryPlan.StageStructure = this.StageStructure;
            this.PaymentPlan = new PaymentPlan(this.Session, this.VersionState);
            this.PaymentPlan.Customer = this.Customer;
            this.PaymentPlan.Supplier = this.Supplier;
            this.PaymentPlan.StageStructure = this.StageStructure;
            this.StageStructure.DeliveryPlan = this.DeliveryPlan;
            this.StageStructure.PaymentPlan = this.PaymentPlan;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        //public override VersionStates VersionState {
        //    get { return _VersionState; }
        //    set { SetPropertyValue("VersionState", ref _VersionState, value); }

        //}
        /// <summary>
        /// ContractCategory
        /// </summary>
        private crmContractCategory _ContractCategory;
        [RuleRequiredField("WorkPlanVersion.ContractCategory.Required", "Save")]
        public crmContractCategory ContractCategory {
            get { return _ContractCategory; }
            set { SetPropertyValue<crmContractCategory>("ContractCategory", ref _ContractCategory, value); }
        }
        /// <summary>
        /// ContractDocument
        /// </summary>
        private ContractDocument _ContractDocument;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [Aggregated]
        public ContractDocument ContractDocument {
            get { return _ContractDocument; }
            set { SetPropertyValue<ContractDocument>("ContractDocument", ref _ContractDocument, value); }
        }

        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Description;
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        /// <summary>
        /// StageStructure
        /// </summary>
        private crmStageStructure _StageStructure;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmStageStructure StageStructure {
            get { return _StageStructure; }
            set { SetPropertyValue<crmStageStructure>("StageStructure", ref _StageStructure, value); }
        }

        /// <summary>
        /// PaymentPlan
        /// </summary>
        private PaymentPlan _PaymentPlan;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public PaymentPlan PaymentPlan {
            get { return _PaymentPlan; }
            set { SetPropertyValue<PaymentPlan>("PaymentPlan", ref _PaymentPlan, value); }
        }

        /// <summary>
        /// DeliveryPlan
        /// </summary>
        private DeliveryPlan _DeliveryPlan;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public DeliveryPlan DeliveryPlan {
            get { return _DeliveryPlan; }
            set { SetPropertyValue<DeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value); }
        }


        // Заказчик
        private crmContractParty _Customer;
        //[Aggregated]
        [DataSourceProperty("ComplexContractVersion.ContractPartys")]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmContractParty Customer {
            get { return _Customer; }
            //set { SetPropertyValue<ContractParty>("Customer", ref _Customer, value); }
            set {
                //if (!IsLoading) {
                //    //if (_WorkPartySource != _WorkPartyTarget) this.RemoveParty(_WorkPartySource);
                //    this.AddParty(value);
                //}
                //if (!IsLoading) {
                //    if (_Customer != null) _Customer.Delete();
                //}
                SetPropertyValue<crmContractParty>("Customer", ref _Customer, value);
            }
        }
        // Исполнитель
        private crmContractParty _Supplier;
        //[Aggregated]
        [DataSourceProperty("ComplexContractVersion.ContractPartys")]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmContractParty Supplier {
            get { return _Supplier; }
            //set { SetPropertyValue<ContractParty>("Supplier", ref _Supplier, value); }
            set {
                //if (!IsLoading) {
                //    //if (_WorkPartySource != _WorkPartyTarget) this.RemoveParty(_WorkPartySource);
                //    this.AddParty(value);
                //}
                //if (!IsLoading) {
                //    if (_Supplier != null) _Supplier.Delete();
                //}
                SetPropertyValue<crmContractParty>("Supplier", ref _Supplier, value);
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
                    if (DeliveryPlan != null)
                        DeliveryPlan.fmOrder = value;
                    if (PaymentPlan != null)
                        PaymentPlan.fmOrder = value;
                }
            }
        }


/*
        /// <summary>
        /// WorkPartySource
        /// </summary>
        private WorkParty _WorkPartySource;
        public WorkParty WorkPartySource {
            get { return _WorkPartySource; }
            set {
                //if (!IsLoading) {
                //    if (_WorkPartySource != _WorkPartyTarget) this.RemoveParty(_WorkPartySource);
                //    this.AddParty(value);
                //}
                SetPropertyValue<WorkParty>("WorkPartySource", ref _WorkPartySource, value);
            }
        }

        /// <summary>
        /// WorkPartyTarget
        /// </summary>
        private WorkParty _WorkPartyTarget;
        public WorkParty WorkPartyTarget {
            get { return _WorkPartyTarget; }
            set {
                //if (!IsLoading) {
                //    if (_WorkPartySource != _WorkPartyTarget) this.RemoveParty(_WorkPartyTarget);
                //    this.AddParty(value);
                //}
                SetPropertyValue<WorkParty>("WorkPartyTarget", ref _WorkPartyTarget, value);
            }
        }
*/

        /// <summary>
        /// Current - Ссылка на версию с признаком VersionState = CURRENT
        /// </summary>
//        protected WorkPlanVersion _Current;
//        [Browsable(false)]
//        public new WorkPlanVersion Current {
//            get { return _Current; }
//            set { SetPropertyValue<WorkPlanVersion>("Current", ref _Current, value); }
//        }
        private crmCostModel _CurrentCostModel;
        public crmCostModel CurrentCostModel {
            get { return _CurrentCostModel; }
            set {
                if (!IsLoading && StageStructure != null && StageStructure.FirstStage != null) {
                    StageStructure.FirstStage.CostModel = value;
                }
                SetPropertyValue<crmCostModel>("CurrentCostModel", ref _CurrentCostModel, value);
            }
        }

        private CS.Nomenclature.Valuta _Valuta;
        [Browsable(false)]
        public CS.Nomenclature.Valuta Valuta {
            get { return _Valuta; }
            set {
                if (!IsLoading && StageStructure != null && StageStructure.FirstStage != null)
                {
                    StageStructure.FirstStage.Valuta = value;
                }
                SetPropertyValue<CS.Nomenclature.Valuta>("Valuta", ref _Valuta, value);
            }
        }


        private DisplaySummMode _DisplaySummMode;
        //[ImmediatePostData]
        //[Appearance("DisplaySummMode.Caption.Italic", AppearanceItemType.LayoutItem, "FontStyle = 'Italic'", FontStyle = FontStyle.Italic)]
        //[Appearance("DisplaySummMode.Caption.Regular", AppearanceItemType.LayoutItem, "FontStyle = 'Regular'", FontStyle = FontStyle.Regular)]
        //[Appearance("DisplaySummMode.Caption.Strikeout", AppearanceItemType.LayoutItem, "FontStyle = 'Strikeout'", FontStyle = FontStyle.Strikeout)]
        //[Appearance("DisplaySummMode.Caption.Underline", AppearanceItemType.LayoutItem, "FontStyle = 'Underline'", FontStyle = FontStyle.Underline)]
        //[Appearance("DisplaySummMode.Caption.BackColor.Red", AppearanceItemType.LayoutItem, "Severity = 'Severe'", BackColor = "Red", FontColor = "Black", Priority = 1)]
        //[Appearance("DisplaySummMode.Caption.Blue", AppearanceItemType.LayoutItem, "Priority = 'Low'", FontColor = "Blue")]
        //[Appearance("DisplaySummMode.Caption.FontClor.Red", AppearanceItemType.LayoutItem, "not isnull(DisplaySummMode)", FontColor = "Red")]
        //[Appearance("DisplaySummMode.Caption.Bold", AppearanceItemType.LayoutItem, "true", FontStyle = FontStyle.Bold)]
        [VisibleInListView(false)]
        public DisplaySummMode DisplaySummMode {
            get { return _DisplaySummMode; }
            set {
                if (!IsLoading && StageStructure != null && StageStructure.FirstStage != null) {
                    StageStructure.DisplaySummMode = value;
                }
                SetPropertyValue<DisplaySummMode>("DisplaySummMode", ref _DisplaySummMode, value);
            }
        }


        public override object MainObject {
            get { return this.WorkPlan; }
        }

        #endregion

        
        #region МЕТОДЫ


        public void AddContractParty(crmContractParty party) {
            // Или определить этот метод в ComplexContractVersion
            if (this.ComplexContractVersion == null) return;
            XPCollection<crmContractParty> contractParties = this.ComplexContractVersion.ContractPartys;
            //foreach (WorkParty cp in this.WorkPlanVersionPartys) {
            //    if (contractParties.IndexOf(cp) == -1) contractParties.Add(cp);
            //}
            if (contractParties.IndexOf(party) == -1) contractParties.Add(party);
        }

        public void RemoveContractParty(crmContractParty party) {
        }


        public void AddParty(crmContractParty party) {
            //if (this.WorkPlanVersionPartys.IndexOf(workParty) == -1) this.WorkPlanVersionPartys.Add(workParty);
            AddContractParty(party);
        }

/*
        public void RemoveParty(ContractParty party) {
        }
*/

/*
        protected override void OnSaved() {
            // Обновление списка WorkPlanVersionPartys
            // WorkPartySource, WorkPartyTarget
            
            List<WorkParty> WorkPartyListForRemove = new List<WorkParty>();

            foreach (WorkParty cp in this.WorkPlanVersionPartys) {
                if (cp != this.WorkPartySource & cp != this.WorkPartyTarget) WorkPartyListForRemove.Add(cp);
            }
            foreach (WorkParty cp in WorkPartyListForRemove) this.WorkPlanVersionPartys.Remove(cp);

            if (this.WorkPlanVersionPartys.IndexOf(this.WorkPartySource) == -1) this.WorkPlanVersionPartys.Add(WorkPartySource);
            if (this.WorkPlanVersionPartys.IndexOf(this.WorkPartyTarget) == -1) this.WorkPlanVersionPartys.Add(WorkPartyTarget);

            // Обновление списка участников версии контрактаэ
            XPCollection<ContractParty> contractParties = this.ComplexContractVersion.ContractPartys;
            foreach (WorkParty cp in this.WorkPlanVersionPartys) {
                if (contractParties.IndexOf(cp) == -1) contractParties.Add(cp);
            }

            base.OnSaved();
        }
*/
        #endregion


        #region ОБРАБОТКА СИТУАЦИИ С ТЕКУЩЕЙ ЗАПИСЬЮ


        #endregion


        #region КНОПКИ

        public void ApproveVersion() {
            this.WorkPlan.ApproveVersion(this);
        }

        #endregion

    }

}