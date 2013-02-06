using System;
using System.ComponentModel;
using System.Drawing;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CRM.Contract.Forms {
    //public enum PartyRole { 
    //    CUSTOMER = 1,
    //    SUPPLIER = 2
    //}
    // Регистрация проекта договора

    //[NavigationItem(true, GroupName = "crmContract")]
    [NonPersistent]
    public class crmDealRegistrationForm : crmCommonRegistrationForm, IWizardSupport {

        public crmDealRegistrationForm(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        #region IWizardSupport Members

        BaseObject IWizardSupport.Complete() {
            crmContractDeal deal = this.RegisterDeal();
            this.Session.CommitTransaction();
            return deal.Current;
        }

        #endregion
    }

}
