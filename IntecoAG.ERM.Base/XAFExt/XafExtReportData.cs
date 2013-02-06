using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Reports;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
namespace IntecoAG.ERM.XAFExt {

    [Persistent("XafExtReportData")]
    public class XafExtReportData: ReportData {
        public XafExtReportData(Session session) : base(session) { }

        private String _FileName;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public String FileName {
            get { return _FileName; }
            set { SetPropertyValue<String>("FileName", ref _FileName, value); }
        }
    }
}
