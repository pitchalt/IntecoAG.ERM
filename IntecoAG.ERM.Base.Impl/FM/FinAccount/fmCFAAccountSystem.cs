using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.FinAccount {

    [Persistent("fmFAAccountSystem")]
    public class fmCFAAccountSystem : csCCodedComponent {
        public fmCFAAccountSystem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        [Association("fmCFAAccountSystem-fmCFAAccount")]
        [Aggregated]
        public XPCollection<fmCFAAccount> Accounts {
            get {
                return GetCollection<fmCFAAccount>("Accounts");
            }
        }
    }
}
