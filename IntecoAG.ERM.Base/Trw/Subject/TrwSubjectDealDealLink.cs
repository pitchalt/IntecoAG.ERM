using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CRM.Contract.Deal;

namespace IntecoAG.ERM.Trw.Subject {

    public class TrwSubjectDealDealLink : XPObject {

        private TrwSubjectDealBase _TrwSubjectDeal;
        [Association("TrwSubjectDealBase-TrwSubjectDealDealLink")]
        public TrwSubjectDealBase TrwSubjectDeal {
            get { return _TrwSubjectDeal; }
            set { SetPropertyValue<TrwSubjectDealBase>("TrwSubjectDeal", ref _TrwSubjectDeal, value); }
        }

        private crmContractDeal _CrmContractDeal;
        public crmContractDeal CrmContractDeal {
            get { return _CrmContractDeal; }
            set { SetPropertyValue<crmContractDeal>("CrmContractDeal", ref _CrmContractDeal, value); }
        }

        public TrwSubjectDealDealLink(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}
