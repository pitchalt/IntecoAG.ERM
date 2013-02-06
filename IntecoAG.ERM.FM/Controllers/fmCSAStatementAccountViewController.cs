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

    /// <summary>
    /// Методы автоматической првязки счетов к Заявкам об оплате
    /// </summary>
    public partial class fmCSARepaymentStatementAccountViewController : ViewController {

        public fmCSARepaymentStatementAccountViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        //private fmCSAStatementAccount current = null;

        protected override void OnActivated() {
            base.OnActivated();

            // Кнопку Open надо удалить, но пока пусть будет скрытой
            Open.Active["Open_Show"] = false;

            //current = View.CurrentObject as fmCSAStatementAccount;

            if (Frame as NestedFrame != null) {
                ListViewProcessCurrentObjectController processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
                if (processCurrentObjectController != null) {
                    processCurrentObjectController.CustomProcessSelectedItem += processCurrentObjectController_CustomProcessSelectedItem;
                }
            }
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (Frame as NestedFrame != null) {
                ListViewProcessCurrentObjectController processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
                if (processCurrentObjectController != null) {
                    processCurrentObjectController.CustomProcessSelectedItem -= processCurrentObjectController_CustomProcessSelectedItem;
                }
            }
        }

        private void processCurrentObjectController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {

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
            e.Handled = true;
        }

        private void Open_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCSAStatementAccount current = e.CurrentObject as fmCSAStatementAccount;
            if (current == null) return;

            // Show object
            string DetailViewId = Frame.Application.FindDetailViewId(current.GetType());

            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            BaseObject passedObj = objectSpace.GetObject<BaseObject>(current);

            TargetWindow openMode = TargetWindow.NewWindow;
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, passedObj);
            ShowViewParameters svp = new ShowViewParameters() { CreatedView = dv, TargetWindow = openMode, Context = TemplateContext.View, CreateAllControllers = true };
            e.ShowViewParameters.Assign(svp);
        }

        private void AutoBinding_Execute(object sender, SimpleActionExecuteEventArgs e) {

            //fmCSAStatementAccount current = e.CurrentObject as fmCSAStatementAccount;
            //if (current == null) return;
            //using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
            //    current = os.GetObject<fmCSAStatementAccount>(current);
            //    current.AutoBinding(null);
            //    os.CommitChanges();
            //}

            foreach (fmCSAStatementAccount sa in e.SelectedObjects) {
                using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                    fmCSAStatementAccount nestedSA = os.GetObject<fmCSAStatementAccount>(sa);
                    nestedSA.AutoBinding(null);
                    os.CommitChanges();
                }
            }
            ObjectSpace.CommitChanges();

            DevExpress.XtraEditors.XtraMessageBox.Show("Автоматическая привязка Заявок и платёжных документов произведена успешно");
        }

    }
}
