using System;
using System.IO;
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

    //    [VisibleInReports]
    [NavigationItem("AVT")]
    [Persistent("fmAVTBookBuhStruct")]
    public class fmCAVTBookBuhStruct : csCCodedComponent, csIImportSupport {

        public fmCAVTBookBuhStruct(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        [Aggregated]
        [Association("fmAVTBookBuhStruct-InInvoiceStructRecords")]
        public XPCollection<fmCAVTBookBuhStructRecord> InInvoiceRecords {
            get { return GetCollection<fmCAVTBookBuhStructRecord>("InInvoiceRecords"); }            
        }
        [Aggregated]
        [Association("fmAVTBookBuhStruct-OutInvoiceStructRecords")]
        public XPCollection<fmCAVTBookBuhStructRecord> OutInvoiceRecords {
            get { return GetCollection<fmCAVTBookBuhStructRecord>("OutInvoiceRecords"); }
        }

        public void Import(IObjectSpace os, string file_name) {
            using (Stream stream = new FileStream(file_name, FileMode.Open)) {
                fmCAVTBookBuhStructLogic.Import(this, os, stream);
            }
        }
    }

}