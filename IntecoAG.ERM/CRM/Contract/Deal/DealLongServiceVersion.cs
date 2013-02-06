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
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ConditionalAppearance;
//using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Deal
{

    /// <summary>
    ///  Î‡ÒÒ crmDealLongServiceVersion
    /// </summary>
    // œÂÂ‡·ÓÚ‡Ú¸ EditorStateRuleAttribute œ‡‚ÂÎ!!!
    //[EditorStateRuleAttribute("crmDealLongServiceVersion.DealVersions.Hide", "DealVersions", EditorState.Hidden, "not VersionState = 2", ViewType.Any)]
    [Appearance("crmDealLongServiceVersion.ApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6", TargetItems = "VersionApprove", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Persistent("crmDealLongServiceVersion")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDealLongServiceVersion : crmDealVersion, IVersionSupport, IVersionBusinessLogicSupport
    {
        public crmDealLongServiceVersion(Session ses) : base(ses) { }
        public crmDealLongServiceVersion(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.ContractDocument = new crmContractDocument(this.Session);
            this.Customer = new crmContractParty(this.Session, this.VersionState);
            this.Supplier = new crmContractParty(this.Session, this.VersionState);
//            this.StageStructure = new crmStageStructure(this.Session, this.VersionState);
//            this.DeliveryPlan = new crmDeliveryPlan(this.Session, this.VersionState);
//            this.DeliveryPlan.Creditor = this.Customer;
//            this.DeliveryPlan.Debitor = this.Supplier;
//            this.DeliveryPlan.StageStructure = this.StageStructure;
//            this.PaymentPlan = new crmPaymentPlan(this.Session, this.VersionState);
//            this.PaymentPlan.Debitor = this.Customer;
//            this.PaymentPlan.Creditor = this.Supplier;
//            this.PaymentPlan.StageStructure = this.StageStructure;
//            this.StageStructure.DeliveryPlan = this.DeliveryPlan;
//            this.StageStructure.PaymentPlan = this.PaymentPlan;
        }


        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿

        /// <summary>
        /// StageStructure
        /// </summary>
//        private crmStageStructure _StageStructure;
//        [Aggregated]
//        public crmStageStructure StageStructure {
//            get { return _StageStructure; }
//            set { SetPropertyValue<crmStageStructure>("StageStructure", ref _StageStructure, value); }
//        }

        /*
        private string _Number;
        [Size(30)]
        [RuleRequiredField("crmDealLongServiceVersion.Number.Required", "Save")]
        //[RuleRequiredField("crmDealLongServiceVersion.Number.Required.Immediate", "Immediate")]
        public virtual string Number {
            get { return _Number; }
            set {
                SetPropertyValue("Number", ref _Number, value);
            }
        }

        private DateTime _Date;
        [RuleRequiredField("crmDealLongServiceVersion.Date.Required", "Save")]
        //[RuleRequiredField("crmDealLongServiceVersion.Date.Required.Immediate", "Immediate")]
        public virtual DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }


        private crmContractDocumentType _DocumentCategory;
        [RuleRequiredField("crmDealLongServiceVersion.DocumentCategory.Required", "Save")]
        //[RuleRequiredField("crmDealLongServiceVersion.DocumentCategory.Immediate", "Immediate")]
        public virtual crmContractDocumentType DocumentCategory {
            get { return _DocumentCategory; }
            set {
                SetPropertyValue<crmContractDocumentType>("DocumentCategory", ref _DocumentCategory, value);
            }
        }
        */

        public override object MainObject {
            get { return this.ContractDeal as crmDealWithoutStage; }
        }

        #endregion


        #region Ã≈“Œƒ€

        public void ApproveVersion() {
            if (this.ContractDeal as crmDealLongService != null) (this.ContractDeal as crmDealLongService).ApproveVersion(this);
        }

        #endregion


        //#region  ÕŒœ »

        //public void ApproveVersion() {
        //    ((crmDealLongService)this.ContractDeal).ApproveVersion(this);
        //    //this.DealLongService.ApproveVersion(this);
        //}

        //#endregion


        #region IVersionBusinessLogicSupport

        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(((crmDealLongService)(this.MainObject)).Current), vHelper);
        }

        public void Approve(IVersionSupport obj) {
        }

        #endregion

    }
}