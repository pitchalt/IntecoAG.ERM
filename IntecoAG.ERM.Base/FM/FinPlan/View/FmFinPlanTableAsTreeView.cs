using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.FinPlan.View {

    [Persistent("FmFinPlanTableAsTreeView")]
    public abstract class FmFinPlanTableAsTreeView : FmFinPlanTableAsTreeViewRowBase {
        public FmFinPlanTableAsTreeView(Session session) : base(session) { }

        public override FmFinPlanTableAsTreeView TableAsTreeView {
            get { return this; }
            set { }
        }

        public override string CodeFull {
            get { return Code; }
        }

        public override ITreeNode Parent {
            get { return null; }
        }

        public override ITableViewRow TableViewRow {
            get { return TableView; }
            set { }
        }
    }

}
