using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.XAFExt.CDS;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.CRM.Contract.Deal {

    [NonPersistent]
    public class crmDealObligation: LinqQuery<crmDealObligation, crmDeliveryItem>   { 
//        public crmDealRegistrationStatistics() {}
        public crmDealObligation(Session ses): base(ses) { }

        public crmDealObligation(Session ses, hrmStaff staff, Int32 count)
            : base(ses) {
            this._Staff = staff;
            this._ContractCount = count;
        }

        private hrmStaff _Staff;
        private Int32 _ContractCount;

        public hrmStaff Staff {
            get { return _Staff; }
        }

        public Int32 ContractCount {
            get { return _ContractCount; }
        }

        public override IQueryable<crmDealRegistrationStatistics> GetQuery() {
//            XPQuery<crmContractDeal> deals = new XPQuery<crmContractDeal>(ses);
            var query = from deal in Provider
                        where deal.DateRegistration >= new DateTime(2012, 01, 01)
                        group deal by deal.UserRegistrator into gdeal
                        orderby gdeal.Count()
                        select new crmDealRegistrationStatistics(this.session,  gdeal.Key, gdeal.Count());
            return query;
        }
    }
}
