namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCPRPaymentRequestViewController {
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
            this.InBankAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InPaymentAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuspendAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DeclineAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // InBankAction
            // 
            this.InBankAction.Caption = "InBank";
            this.InBankAction.Category = "BusinessLogic";
            this.InBankAction.ConfirmationMessage = null;
            this.InBankAction.Id = "fmCPRPaymentRequestViewController_InBank";
            this.InBankAction.ImageName = null;
            this.InBankAction.Shortcut = null;
            this.InBankAction.Tag = null;
            this.InBankAction.TargetObjectsCriteria = null;
            this.InBankAction.TargetViewId = null;
            this.InBankAction.ToolTip = null;
            this.InBankAction.TypeOfView = null;
            this.InBankAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.InBankAction_Execute);
            // 
            // InPaymentAction
            // 
            this.InPaymentAction.Caption = "InPayment";
            this.InPaymentAction.Category = "BusinessLogic";
            this.InPaymentAction.ConfirmationMessage = null;
            this.InPaymentAction.Id = "fmCPRPaymentRequestViewController_InPayment";
            this.InPaymentAction.ImageName = null;
            this.InPaymentAction.Shortcut = null;
            this.InPaymentAction.Tag = null;
            this.InPaymentAction.TargetObjectsCriteria = null;
            this.InPaymentAction.TargetViewId = null;
            this.InPaymentAction.ToolTip = null;
            this.InPaymentAction.TypeOfView = null;
            this.InPaymentAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.InPaymentAction_Execute);
            // 
            // SuspendAction
            // 
            this.SuspendAction.Caption = "Suspend";
            this.SuspendAction.Category = "BusinessLogic";
            this.SuspendAction.ConfirmationMessage = null;
            this.SuspendAction.Id = "fmCPRPaymentRequestViewController_SuspendAction";
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
            // ApproveAction
            // 
            this.ApproveAction.Caption = "Approve";
            this.ApproveAction.Category = "BusinessLogic";
            this.ApproveAction.ConfirmationMessage = null;
            this.ApproveAction.Id = "fmCPRPaymentRequestViewController_ApproveAction";
            this.ApproveAction.ImageName = null;
            this.ApproveAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ApproveAction.Shortcut = null;
            this.ApproveAction.Tag = null;
            this.ApproveAction.TargetObjectsCriteria = null;
            this.ApproveAction.TargetViewId = null;
            this.ApproveAction.ToolTip = "Approve Request";
            this.ApproveAction.TypeOfView = null;
            this.ApproveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAction_Execute);
            // 
            // DeclineAction
            // 
            this.DeclineAction.Caption = "Decline";
            this.DeclineAction.Category = "BusinessLogic";
            this.DeclineAction.ConfirmationMessage = null;
            this.DeclineAction.Id = "fmCPRPaymentRequestViewController_DeclineAction";
            this.DeclineAction.ImageName = null;
            this.DeclineAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.DeclineAction.Shortcut = null;
            this.DeclineAction.Tag = null;
            this.DeclineAction.TargetObjectsCriteria = null;
            this.DeclineAction.TargetViewId = null;
            this.DeclineAction.ToolTip = "Decline Request";
            this.DeclineAction.TypeOfView = null;
            this.DeclineAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeclineAction_Execute);
            // 
            // fmCPRPaymentRequestViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.PaymentRequest.fmCPRPaymentRequest);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction InBankAction;
        private DevExpress.ExpressApp.Actions.SimpleAction InPaymentAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SuspendAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DeclineAction;
    }
}
