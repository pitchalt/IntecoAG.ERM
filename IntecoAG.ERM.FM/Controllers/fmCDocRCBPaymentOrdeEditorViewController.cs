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
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
//
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmCDocRCBPaymentOrdeEditorViewController : ViewController {

        private string AllowEditPropertyReason = "AllowEditProperty";
        DetailViewController dvController = null;

        public fmCDocRCBPaymentOrdeEditorViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            // Анализ состояния объекта и определение его редактируемости
            if (View == null || View.CurrentObject == null) return;
            View.AllowEdit[AllowEditPropertyReason] = ((fmCDocRCB)View.CurrentObject).GetAllowEdit();
        }

        protected override void OnDeactivated() {
            if (View.AllowEdit.Contains(AllowEditPropertyReason)) 
                View.AllowEdit.RemoveItem(AllowEditPropertyReason);
            if (dvController != null) {
                dvController.SaveAction.Execute -= new SimpleActionExecuteEventHandler(SaveAction_Execute);
            }
            base.OnDeactivated();
        }

        private void PaymentFieldEdit_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || View.CurrentObject == null) return;
            ((fmCDocRCB)View.CurrentObject).SetAllowEdit();

            if (((fmCDocRCB)View.CurrentObject).GetAllowEdit()) {
                View.AllowEdit[AllowEditPropertyReason] = true;
            }

            // http://community.devexpress.com/forums/t/96112.aspx
        }

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
            dvController = Frame.GetController<DetailViewController>();
            if (dvController == null) return;
            
            dvController.SaveAction.Execute += new SimpleActionExecuteEventHandler(SaveAction_Execute);
        }

        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            OnSave(e.CurrentObject);
        }

        private void OnSave(object currentObject) {
            if (currentObject != null && View.CurrentObject as fmCDocRCB != null) {
                View.AllowEdit[AllowEditPropertyReason] = ((fmCDocRCB)View.CurrentObject).GetAllowEdit();
            }
        }

    }
}
