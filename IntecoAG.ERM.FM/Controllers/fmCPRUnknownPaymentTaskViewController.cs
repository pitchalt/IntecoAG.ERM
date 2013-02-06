using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
//
using IntecoAG.ERM.Module;
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
    /// Назначение заявки для неопознанного платежа
    /// </summary>
    public partial class fmCPRUnknownPaymentTaskViewController : ViewController //, IMasterDetailViewInfo
    {

        public fmCPRUnknownPaymentTaskViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private fmCPRUnknownPaymentTask current = null;
        private ListViewProcessCurrentObjectController processCurrentObjectController = null;
        private fmCSAStatementAccountDoc currentStatementDoc = null;

        private fmCPRPaymentRequest selectedPaymentRequest = null;

        protected override void OnActivated() {
            base.OnActivated();
            //current = View.CurrentObject as fmCPRUnknownPaymentTask;

            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);

            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                if (lpe.ListView != null && lpe.ListView.ObjectTypeInfo.Type == typeof(fmCPRPaymentRequest)) {
                    lpe.ListView.ControlsCreated += new EventHandler(ListView_ControlsCreated);
                }
            }
        }

        protected override void OnDeactivated() {
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);

            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                if (lpe.ListView != null && lpe.ListView.ObjectTypeInfo.Type == typeof(fmCPRPaymentRequest)) {
                    lpe.ListView.ControlsCreated -= new EventHandler(ListView_ControlsCreated);
                }
            }
            base.OnDeactivated();
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e) {
            current = View.CurrentObject as fmCPRUnknownPaymentTask;
        }

         private void ListView_ControlsCreated(object sender, EventArgs e) {
             GridControl gridControl = ((ListView)sender).Control as GridControl;
             XafGridView gridView = gridControl.DefaultView as XafGridView;
             gridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView_FocusedRowChanged);
        }

         private void gridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e) {
             XafGridView gridView = (XafGridView)sender;
             selectedPaymentRequest = gridView.GetRow(e.FocusedRowHandle) as fmCPRPaymentRequest;
         }

        //private void processCurrentObjectController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {
        //    if (processCurrentObjectController == null)
        //        return;
        //    //if (processCurrentObjectController.TypeOfView != typeof(fmCSAStatementAccountDoc))
        //    //    return;
        //    currentStatementDoc = processCurrentObjectController.View.CurrentObject as fmCSAStatementAccountDoc;
        //}

        /// <summary>
        /// Создать и открыть объект fmCPRRepaymentTask в модальном окне
        /// </summary>
        private void BindRequestAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCPRUnknownPaymentTask;
            if (current == null) return;

            if (selectedPaymentRequest == null)
                return;

            current.ManualBinding(selectedPaymentRequest);

            // Обновление списка (например, удаление использованной Заявки)
            //FillRequestList();
        }

        /*
        private void RequestListAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCPRUnknownPaymentTask;
            if (current == null) return;

            //if (e.SelectedChoiceActionItem != null) {
            //    // Привязать
            //    current.PaymentDocument.AutoBinding(current);

            //    // Обновление списка (например, удаление использованной Заявки)
            //    //FillRequestList();
            //}
        }

        private void fmCPRRepaymentTaskViewController_Activated(object sender, EventArgs e) {
            current = View.CurrentObject as fmCPRUnknownPaymentTask;
            if (current == null) return;
            FillRequestList();
        }

        private void FillRequestList() {
            RequestListAction.Items.Clear();
            current.ClearRequestCollection();
            foreach (fmCPRPaymentRequest pr in current.AllRequests) {
                ChoiceActionItem cItem = new ChoiceActionItem(pr.Oid.ToString(), pr.Number, pr);
                RequestListAction.Items.Add(cItem);
            }
        }
        */
    }

}
