using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    [NavigationItem("AVT")]
    [VisibleInReports]
    [Persistent("fmAVTBookBuhImport")]
//    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCAVTBookBuhImport :  csCCodedComponent {
//        fmCAVTBookBuh {

        public fmCAVTBookBuhImport(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private Boolean _IsNotUse;
        public Boolean IsNotUse {
            get { return _IsNotUse; }
            set { SetPropertyValue<Boolean>("IsNotUse", ref _IsNotUse, value); }
        }

        [Aggregated]
        [Association("fmAVTBookBuhImport-fmAVTBookBuhRecords")]
        public XPCollection<fmCAVTBookBuhRecord> BookBuhRecords {
            get { return GetCollection<fmCAVTBookBuhRecord>("BookBuhRecords"); }
        }
    }

}