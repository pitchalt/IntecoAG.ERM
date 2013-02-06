namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCPRPaymentRequestListItemViewController {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.ManualBinding = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RemoveRequest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ManualBinding
            // 
            this.ManualBinding.Caption = "Manual Binding";
            this.ManualBinding.Category = "View";
            this.ManualBinding.ConfirmationMessage = null;
            this.ManualBinding.Id = "fmCPRPaymentRequestListItemViewController_ManualBinding";
            this.ManualBinding.ImageName = null;
            this.ManualBinding.Shortcut = null;
            this.ManualBinding.Tag = null;
            this.ManualBinding.TargetObjectsCriteria = null;
            this.ManualBinding.TargetViewId = null;
            this.ManualBinding.ToolTip = null;
            this.ManualBinding.TypeOfView = null;
            this.ManualBinding.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ManualBinding_Execute);
            // 
            // RemoveRequest
            // 
            this.RemoveRequest.Caption = "Remove Request";
            this.RemoveRequest.Category = "View";
            this.RemoveRequest.ConfirmationMessage = null;
            this.RemoveRequest.Id = "fmCPRPaymentRequestListItemViewController_RemoveRequest";
            this.RemoveRequest.ImageName = null;
            this.RemoveRequest.Shortcut = null;
            this.RemoveRequest.Tag = null;
            this.RemoveRequest.TargetObjectsCriteria = null;
            this.RemoveRequest.TargetViewId = null;
            this.RemoveRequest.ToolTip = null;
            this.RemoveRequest.TypeOfView = null;
            this.RemoveRequest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveRequest_Execute);
            // 
            // fmCPRPaymentRequestListItemViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.PaymentRequest.fmCPRRepaymentTask.PaymentRequestListItem);
            this.TargetViewId = "fmCPRRepaymentTask_AllRequests_ListView";
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ManualBinding;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveRequest;
    }
}
