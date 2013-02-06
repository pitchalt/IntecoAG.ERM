using System;
using System.Collections.Generic;
using System.Data;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Reports;

namespace IntecoAG.ERM.Module {

    public class XafExtReport : XafReport {
        public XafExtReport() { }

        protected override void InitializeDataSource() {
            //base.InitializeDataSource();
        }

        protected override void RefreshDataSourceForPrint() {
            //base.RefreshDataSourceForPrint();
        }
    }
}
