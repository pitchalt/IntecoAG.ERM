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

    [VisibleInReports]
    [NavigationItem("AVT")]
    [Persistent("fmAVTBookBuhStruct")]
//    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCAVTBookBuhStruct : csCCodedComponent, csIImportSupport {
//        fmCAVTBookBuh, csIImportSupport {

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

        [Aggregated]
        [Association("fmAVTBookBuhStruct-fmAVTBookBuhRecords")]
        public XPCollection<fmCAVTBookBuhRecord> BookBuhRecords {
            get { return GetCollection<fmCAVTBookBuhRecord>("BookBuhRecords"); }
        }

        private crmCParty _Party;
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }

        public void Import(IObjectSpace os, string file_name) {
            using (Stream stream = new FileStream(file_name, FileMode.Open)) {
                fmCAVTBookBuhStructLogic.Import(this, os, stream);
            }
        }

        [Action()]
        public void Process() {
            IObjectSpace ObjectSpace = CommonMethods.FindObjectSpaceByObject(this);
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                fmCAVTBookBuhStruct book_struct = os.GetObject<fmCAVTBookBuhStruct>(this);
                fmCAVTBookBuhStructLogic logic = new fmCAVTBookBuhStructLogic(os);
                logic.Process(book_struct, os);
                os.CommitChanges();
            }
        }
    }

}