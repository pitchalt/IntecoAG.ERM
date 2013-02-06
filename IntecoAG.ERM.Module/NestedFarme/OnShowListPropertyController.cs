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
    public partial class OnShowListPropertyController : ViewController<DetailView> {
        public OnShowListPropertyController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                lpe.ValueRead += new EventHandler(View_Changed);
            }
        }

        protected override void OnDeactivated() {
            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                lpe.ValueRead -= new EventHandler(View_Changed);
            }
            base.OnDeactivated();
        }

        void View_Changed(object sender, EventArgs e) {
            AssignMasterProperty();
        }

        private void AssignMasterProperty() {
            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                if (lpe.Frame == null) continue;
                foreach (Controller c in lpe.Frame.Controllers) {
                    if (c is IMasterDetailViewInfo) {
                        ((IMasterDetailViewInfo)c).AssignMasterDetailViewId(View.Id);
                        ((IMasterDetailViewInfo)c).AssignMasterDetailViewFrame(Frame);
                        //((IMasterDetailViewInfo)c).AssignMasterView(Frame.View);
                    }
                }
            }

        }

    }
}
