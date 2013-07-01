using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract.Deal {

    public enum crmContractDealTRVSuperType {
        DEAL_CREDIT = 1,
        DEAL_INVEST = 2,
        DEAL_BAY = 3,
        DEAL_SALE = 4
    }

    [Persistent("crmContractDealTRVType")]
    public class crmContractDealTRVType : csCComponent {
        public crmContractDealTRVType(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private String _Code;
        private String _Name;
        private crmContractDealTRVSuperType _TRVSuperType;

        [Size(20)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        [Size(60)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        public crmContractDealTRVSuperType TRVSuperType {
            get { return _TRVSuperType; }
            set { SetPropertyValue<crmContractDealTRVSuperType>("TRVSuperType", ref _TRVSuperType, value); }
        }

    }

}
