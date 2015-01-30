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

    public enum fmCAVTBookBuhStructStatus {
        BUH_STRUCT_CREATED = 1,
        BUH_STRUCT_IMPORTED = 2,
        BUH_STRUCT_PROCESSED = 3
    }

    [VisibleInReports]
    [NavigationItem("AVT")]
    [Persistent("fmAVTBookBuhStruct")]
//    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCAVTBookBuhStruct : csCCodedComponent, csIImportSupport {
//        fmCAVTBookBuh, csIImportSupport {

        public fmCAVTBookBuhStruct(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            StatusSet(fmCAVTBookBuhStructStatus.BUH_STRUCT_CREATED);
        }

        [Persistent("Status")]
        private fmCAVTBookBuhStructStatus _Status;
        [PersistentAlias("_Status")]
        public fmCAVTBookBuhStructStatus Status {
            get { return _Status; }
//            set { SetPropertyValue<fmCAVTBookBuhStructStatus>("Status", ref _Status, value); }
        }
        public void StatusSet(fmCAVTBookBuhStructStatus value) { 
            SetPropertyValue<fmCAVTBookBuhStructStatus>("Status", ref _Status, value); 
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