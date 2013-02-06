using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Docs;
//
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmCDocRCBImportViewController : ViewController {

        public fmCDocRCBImportViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ProcessAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || View.CurrentObject == null || View.CurrentObject as fmCSATaskImporter == null) return;

            fmCSATaskImporter taskImporter = View.CurrentObject as fmCSATaskImporter;
            fmCSAImportResult importResult = fmCSAStatementAccountImportLogic.ImportProcess(this.ObjectSpace, taskImporter);

            if (importResult != null) {
                string DetailViewId = Frame.Application.FindDetailViewId(importResult.GetType());

                IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
                BaseObject passedObj = objectSpace.GetObject<BaseObject>(importResult);

                TargetWindow openMode = TargetWindow.NewWindow;
                DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, passedObj);
                ShowViewParameters svp = new ShowViewParameters() { CreatedView = dv, TargetWindow = openMode, Context = TemplateContext.View, CreateAllControllers = true };
                e.ShowViewParameters.Assign(svp);
            } else {
                throw new Exception("Statement of accounts has not been created");
            }
        }
    }
}
