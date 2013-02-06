using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.CRM.Contract {
    public enum PartyRole { 
        CUSTOMER = 1,
        SUPPLIER = 2
    }
    public enum ContractType {
        SIMPLE_CONTRACT = 1,
        COMPLEX_CONTRACT = 2,
        WORK_PLAN = 3
    }
    //[NavigationItem(true, GroupName = "Contract")]
    [NonPersistent]
    public class crmContractNewForm : BaseObject, IWizardSupport {
        public crmContractNewForm(Session session)
            : base(session) {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            if (crmUserParty.CurrentUserParty.Value != null) {
                this.OurParty = (crmPartyRu) crmUserParty.CurrentUserPartyGet(this.Session).Party;
            }
            this.Document = new ContractDocument(this.Session);
            // Place here your initialization code.
        }
        //
        protected ContractType _ContractType;
        [RuleRequiredField("crmContractNewForm.ContractType.Required", "Next")]
        public ContractType ContractType {
            get { return _ContractType; }
            set { SetPropertyValue<ContractType>("ContractType", ref _ContractType, value); }
        }
        protected DateTime _DateBegin;
        [RuleRequiredField("crmContractNewForm.DateBegin.Required", "Next")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }
        protected DateTime _DateEnd;
        [RuleRequiredField("crmContractNewForm.DateEnd.Required", "Next")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        }
        //
        protected crmContractCategory _Category;
        [RuleRequiredField("crmContractNewForm.Category.Required", "Next")]
        public crmContractCategory Category {
            get { return _Category; }
            set { SetPropertyValue<crmContractCategory>("Category", ref _Category, value); }
        }
        //
        private ContractDocument _Document;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public ContractDocument Document {
            get { return _Document; }
            set { SetPropertyValue<ContractDocument>("Document", ref _Document, value); }
        }
        //
        private crmPartyRu _OurParty;
        [RuleRequiredField("crmContractNewForm.OurParty.Required", "Next")]
        public crmPartyRu OurParty {
            get { return _OurParty; }
            set { SetPropertyValue<crmPartyRu>("OurParty", ref _OurParty, value); }
        }
        private PartyRole _OurRole;
        [RuleRequiredField("crmContractNewForm.OurRole.Required", "Next")]
        [ImmediatePostData]
        public PartyRole OurRole {
            get { return _OurRole; }
            set {
                _OurRole = value;
                OnChanged("OutRole");
                OnChanged("PartnerRole");
            }
        }
        private crmPartyRu _PartnerParty;
        [RuleRequiredField("crmContractNewForm.PartnerParty.Required", "Next")]
        public crmPartyRu PartnerParty {
            get { return _PartnerParty; }
            set { SetPropertyValue<crmPartyRu>("PartnerParty", ref _PartnerParty, value); }
        }
        public PartyRole PartnerRole {
            get {
                if (OurRole == PartyRole.CUSTOMER)
                    return PartyRole.SUPPLIER;
                else
                    return PartyRole.CUSTOMER;
            }
        }

        #region IWizardSupport Members

        BaseObject IWizardSupport.Complete() {
            IContractFactory fac;
            if (ContractType == IntecoAG.ERM.CRM.Contract.ContractType.WORK_PLAN) {
                WorkPlan wp = new WorkPlan(this.Session);
                fac = wp;
                fac.Create(this);
                return wp.Current;
            }
            if (ContractType == IntecoAG.ERM.CRM.Contract.ContractType.SIMPLE_CONTRACT) {
                SimpleContract sc = new SimpleContract(this.Session);
                fac = sc;
                fac.Create(this);
                return sc.Current;
            }
            if (ContractType == IntecoAG.ERM.CRM.Contract.ContractType.COMPLEX_CONTRACT) {
                ComplexContract cc = new ComplexContract(this.Session);
                fac = cc;
                fac.Create(this);
                return cc.Current;
            }

            return null;
        }

        #endregion
    }

}
