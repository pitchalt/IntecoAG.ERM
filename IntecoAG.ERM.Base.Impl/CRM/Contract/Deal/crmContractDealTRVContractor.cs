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

    [Persistent("crmContractDealTRVContractor")]
    public class crmContractDealTRVContractor : csCComponent {
        public crmContractDealTRVContractor(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private String _Code;
        private String _Name;

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

    }

}
