using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;

namespace IntecoAG.ERM.Module {
    public partial class MasterDetailViewControllerBase : ViewController {
        public MasterDetailViewControllerBase() {
            InitializeComponent();
            RegisterActions(components);

            TargetViewType = ViewType.DetailView;
            TargetViewNesting = Nesting.Root;
        }

        protected override void OnActivated() {
            base.OnActivated();
            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                if (lpe.Frame == null) continue;
                foreach (Controller c in lpe.Frame.Controllers) {
                    if (c is IMasterDetailViewInfo) {
                        ((IMasterDetailViewInfo)c).AssignMasterDetailViewId(View.Id);
                        ((IMasterDetailViewInfo)c).AssignMasterDetailViewFrame(Frame);
                    }
                }
            }
        }

    }
}
