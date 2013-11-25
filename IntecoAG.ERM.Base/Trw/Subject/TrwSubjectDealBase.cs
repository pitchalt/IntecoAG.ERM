using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract.Deal;
//
namespace IntecoAG.ERM.Trw.Subject {

    [Persistent("TrwSubjectDeal")]
    public abstract class TrwSubjectDealBase : XPObject {

        private crmContractDeal _Deal;
        [DataSourceProperty("DealSource")]
        public crmContractDeal Deal {
            get { return _Deal; }
            set { SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value); }
        }

        [Browsable(false)]
        public abstract XPCollection<crmContractDeal> DealSource { get; }

        public TrwSubjectDealBase(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
