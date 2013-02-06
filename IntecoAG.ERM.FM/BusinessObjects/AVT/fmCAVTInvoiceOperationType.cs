using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.AVT {
    [NavigationItem("AVT")]
    [Persistent("fmAVTInvoiceOperationType ")]
    public class fmCAVTInvoiceOperationType : csCCodedComponent {
        public fmCAVTInvoiceOperationType(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
