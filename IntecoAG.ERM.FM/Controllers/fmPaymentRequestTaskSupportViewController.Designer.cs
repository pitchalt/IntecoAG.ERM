namespace IntecoAG.ERM.FM.Controllers {
    partial class fmPaymentRequestTaskSupportViewController {
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
            this.ApproveAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            this.DeclineAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            this.SuspendAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            // 
            // ApproveAction
            // 
            this.ApproveAction.Caption = "Approve";
            this.ApproveAction.Category = "View";
            this.ApproveAction.ConfirmationMessage = null;
            this.ApproveAction.Id = "fmCPRPaymentRequestTaskController_ApproveAction";
            this.ApproveAction.ImageName = null;
            this.ApproveAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ApproveAction.Shortcut = null;
            this.ApproveAction.Tag = null;
            this.ApproveAction.TargetObjectsCriteria = null;
            this.ApproveAction.TargetViewId = null;
            this.ApproveAction.ToolTip = "Approve Request";
            this.ApproveAction.TypeOfView = null;
            this.ApproveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveRequest_Execute);
            // 
            // DeclineAction
            // 
            this.DeclineAction.Caption = "Decline";
            this.DeclineAction.Category = "View";
            this.DeclineAction.ConfirmationMessage = null;
            this.DeclineAction.Id = "fmCPRPaymentRequestTaskController_DeclineAction";
            this.DeclineAction.ImageName = null;
            this.DeclineAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.DeclineAction.Shortcut = null;
            this.DeclineAction.Tag = null;
            this.DeclineAction.TargetObjectsCriteria = null;
            this.DeclineAction.TargetViewId = null;
            this.DeclineAction.ToolTip = "Decline Request";
            this.DeclineAction.TypeOfView = null;
            this.DeclineAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeclineRequest_Execute);
            // 
            // SuspendAction
            // 
            this.SuspendAction.Caption = "Suspend";
            this.SuspendAction.Category = "View";
            this.SuspendAction.ConfirmationMessage = null;
            this.SuspendAction.Id = "fmCPRPaymentRequestTaskController_SuspendAction";
            this.SuspendAction.ImageName = null;
            this.SuspendAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SuspendAction.Shortcut = null;
            this.SuspendAction.Tag = null;
            this.SuspendAction.TargetObjectsCriteria = null;
            this.SuspendAction.TargetViewId = null;
            this.SuspendAction.ToolTip = "Suspend Request";
            this.SuspendAction.TypeOfView = null;
            this.SuspendAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SuspendAction_Execute);
            // 
            // fmPaymentRequestTaskSupportViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.PaymentRequest.fmCPRPaymentRequest);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DeclineAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SuspendAction;
    }
}
