using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.FinPlan.View;

namespace IntecoAG.ERM.FM.FinPlan {

    [Persistent]
    public class FmFinPlanDocColumn : XPObject, ITableViewColumn {
        public FmFinPlanDocColumn(Session session) : base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public ITableView Table {
            get { throw new NotImplementedException(); }
        }

        public ITableViewColumn UpColumn {
            get { throw new NotImplementedException(); }
        }

        public IList<ITableViewColumn> Columns {
            get { throw new NotImplementedException(); }
        }
    }

}
