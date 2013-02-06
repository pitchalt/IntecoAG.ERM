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
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM.Controllers {

    /// <summary>
    /// По текущему документу выписки открывается платёжный документ
    /// </summary>
    public partial class fmCSAStatementAccountDocViewController : ObjectViewController {

        public fmCSAStatementAccountDocViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        //private fmCSAStatementAccountDoc current = null;

        protected override void OnActivated() {
            base.OnActivated();
            //current = View.CurrentObject as fmCSAStatementAccountDoc;
        }

        /// <summary>
        /// Создать и открыть объект fmCPRRepaymentTask в модальном окне
        /// </summary>
        private void ManualBinding_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCSAStatementAccountDoc current = e.CurrentObject as fmCSAStatementAccountDoc;
            if (current == null) return;

            //fmCPRRepaymentTask RT = ObjectSpace.CreateObject<fmCPRRepaymentTask>();
            //IObjectSpace objectSpace = ObjectSpace.CreateNestedObjectSpace();   // Frame.Application.CreateObjectSpace();
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            // Create object
            //fmCPRRepaymentTask nestedRT = objectSpace.GetObject<fmCPRRepaymentTask>(RT);
            fmCPRRepaymentTask nestedRT = objectSpace.CreateObject<fmCPRRepaymentTask>();
            if (current.StatementAccountIn != null) {
                nestedRT.BankAccount = objectSpace.GetObject<crmBankAccount>(current.StatementAccountIn.BankAccount);
            } else if (current.StatementAccountOut.BankAccount != null) {
                nestedRT.BankAccount = objectSpace.GetObject<crmBankAccount>(current.StatementAccountOut.BankAccount);
            }
            nestedRT.PaymentDocument = objectSpace.GetObject<fmCDocRCB>(current.PaymentDocument);
            nestedRT.FillRepaymentTaskLines();
            nestedRT.FillRequestList();

            // Show object
            string DetailViewId = Frame.Application.FindDetailViewId(nestedRT.GetType());

            //BaseObject passedObj = nestedRT;   // objectSpace.GetObject<BaseObject>(rt);

            TargetWindow openMode = TargetWindow.NewModalWindow;
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, nestedRT);
            ShowViewParameters svp = new ShowViewParameters() { CreatedView = dv, TargetWindow = openMode, Context = TemplateContext.View, CreateAllControllers = true };
            e.ShowViewParameters.Assign(svp);
        }

        private void AutoBinding_Execute(object sender, SimpleActionExecuteEventArgs e) {
            foreach (fmCSAStatementAccountDoc doc in e.SelectedObjects) {
                //current = View.CurrentObject as fmCSAStatementAccountDoc;
                if (doc == null) continue;
                using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                    fmCSAStatementAccountDoc current = os.GetObject<fmCSAStatementAccountDoc>(doc);
                    doc.PaymentDocument.AutoBinding(current, null);
                    os.CommitChanges();
                }
                ObjectSpace.CommitChanges();
            }
            //DevExpress.XtraEditors.XtraMessageBox.Show("Автоматическая привязка Заявок произведена успешно");
        }

    }
}
