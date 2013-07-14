using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Trw.Party {
    [Persistent("TrwPartyMarket")]
    public class TrwPartyMarket : csCCodedComponent {
        public TrwPartyMarket(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
