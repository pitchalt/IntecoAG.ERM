namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCPRUnknownPaymentTaskViewController {
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
            this.BindRequestAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BindRequestAction
            // 
            this.BindRequestAction.Caption = "BindRequest";
            this.BindRequestAction.Category = "View";
            this.BindRequestAction.ConfirmationMessage = null;
            this.BindRequestAction.Id = "fmCPRUnknownPaymentTaskViewController_BindRequestAction";
            this.BindRequestAction.ImageName = null;
            this.BindRequestAction.Shortcut = null;
            this.BindRequestAction.Tag = null;
            this.BindRequestAction.TargetObjectsCriteria = null;
            this.BindRequestAction.TargetViewId = null;
            this.BindRequestAction.ToolTip = null;
            this.BindRequestAction.TypeOfView = null;
            this.BindRequestAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BindRequestAction_Execute);
            // 
            // fmCPRUnknownPaymentTaskViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.PaymentRequest.fmCPRRepaymentTask);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BindRequestAction;
    }
}
