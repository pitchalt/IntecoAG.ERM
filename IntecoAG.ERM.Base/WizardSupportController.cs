using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM;

namespace IntecoAG.ERM.Module {
    public partial class WizardSupportController : ViewController {
        static String WizardSupportControllerPair = "WizardSupportControllerPair";

        public WizardSupportController() {
            InitializeComponent();
            RegisterActions(components);
        }
        //
        protected override void OnActivated() {
            base.OnActivated();
            if (View == null) return;

            if (View.CurrentObject is IWizardSupport) {
                this.Active[WizardSupportControllerPair] = true;
            }
            else
                this.Active[WizardSupportControllerPair] = false;
        }
        //
        private void WizardSupportCompleteAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IXPObject goobj;
            using (IObjectSpace os = this.ObjectSpace.CreateNestedObjectSpace()) {
                IWizardSupport wsobj = (IWizardSupport)(os.GetObject(View.CurrentObject));
                goobj = wsobj.Complete();
                os.CommitChanges();
            }
            this.ObjectSpace.CommitChanges();
            if (goobj == null) return;

            IObjectSpace os2 = Application.CreateObjectSpace();
            object obj = os2.GetObject(goobj);

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os2, obj);
            e.ShowViewParameters.TargetWindow = TargetWindow.Current;
        }
    }
}
