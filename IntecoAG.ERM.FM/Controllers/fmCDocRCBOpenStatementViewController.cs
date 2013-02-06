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
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Docs;
//
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmCDocRCBOpenStatementViewController : ViewController {

        public fmCDocRCBOpenStatementViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ListViewProcessCurrentObjectController processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (processCurrentObjectController != null)
            {
                processCurrentObjectController.CustomProcessSelectedItem += processCurrentObjectController_CustomProcessSelectedItem;
            }
        }
        private void processCurrentObjectController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            e.Handled = true;


            fmCSAStatementAccount current = View.CurrentObject as fmCSAStatementAccount;
            if (current == null) return;

            // Show object
            string DetailViewId = Frame.Application.FindDetailViewId(current.GetType());

            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            BaseObject passedObj = objectSpace.GetObject<BaseObject>(current);

            TargetWindow openMode = TargetWindow.NewWindow;
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, passedObj);
            ShowViewParameters svp = new ShowViewParameters() { CreatedView = dv, TargetWindow = openMode, Context = TemplateContext.View, CreateAllControllers = true };
            e.InnerArgs.ShowViewParameters.Assign(svp);
        }

    }
}
