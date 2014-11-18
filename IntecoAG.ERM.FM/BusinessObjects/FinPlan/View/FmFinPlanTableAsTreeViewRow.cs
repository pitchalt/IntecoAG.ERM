using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.FinPlan.View {

    [NonPersistent]
    public class FmFinPlanTableAsTreeViewRow : FmFinPlanTableAsTreeViewRowBase {
        public FmFinPlanTableAsTreeViewRow(Session session) : base(session) { }

        protected FmFinPlanTableAsTreeViewRowBase _UpRowBase;
        public FmFinPlanTableAsTreeViewRowBase UpRowBase {
            get { return _UpRowBase; }
            set { SetPropertyValue<FmFinPlanTableAsTreeViewRowBase>("UpRowBase", ref _UpRowBase, value); }
        }

        public override String CodeFull {
            get { return UpRowBase.CodeFull + "." + Code; }
        }

        public override ITreeNode Parent {
            get { return UpRowBase; }
        }

        public override ITableView TableView {
            get { return TableViewRow.Table; }
        }

        protected FmFinPlanTableAsTreeView _TableAsTreeView;
        public override FmFinPlanTableAsTreeView TableAsTreeView {
            get { return _TableAsTreeView; }
            set { SetPropertyValue<FmFinPlanTableAsTreeView>("TableAsTreeView", ref _TableAsTreeView, value); }
        }

        protected ITableViewRow _TableViewRow;
        public override ITableViewRow TableViewRow {
            get { return _TableViewRow; }
            set { SetPropertyValue<ITableViewRow>("TableViewRow", ref _TableViewRow, value); }
        }
    }

}
