using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.CS.Security {
    public partial class csCSecurityActionExecuteViewController : ViewController {
        public csCSecurityActionExecuteViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            //ExportController controller = Frame.GetController<ExportController>();
            //controller.ExportAction.Executing += ExportAction_Executing;
            //controller.Active.SetItemValue("Security", SecuritySystem.IsGranted(new ExportPermissionRequest()));
        }

        void ExportAction_Executing(object sender, System.ComponentModel.CancelEventArgs e) {
            //SecuritySystem.Demand(new ExportPermissionRequest());
        }

    }
}
