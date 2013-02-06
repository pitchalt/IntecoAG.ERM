namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCPRRepaymentTaskViewController {
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
            this.UnknownAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CloseUnknownAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.NewRequestAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // UnknownAction
            // 
            this.UnknownAction.Caption = "Unknown";
            this.UnknownAction.Category = "View";
            this.UnknownAction.ConfirmationMessage = null;
            this.UnknownAction.Id = "fmCPRRepaymentTaskViewController_UnknownAction";
            this.UnknownAction.ImageName = null;
            this.UnknownAction.Shortcut = null;
            this.UnknownAction.Tag = null;
            this.UnknownAction.TargetObjectsCriteria = null;
            this.UnknownAction.TargetViewId = null;
            this.UnknownAction.ToolTip = null;
            this.UnknownAction.TypeOfView = null;
            this.UnknownAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UnknownAction_Execute);
            // 
            // CloseUnknownAction
            // 
            this.CloseUnknownAction.Caption = "Close Unknown";
            this.CloseUnknownAction.Category = "View";
            this.CloseUnknownAction.ConfirmationMessage = null;
            this.CloseUnknownAction.Id = "fmCPRRepaymentTaskViewController_CloseUnknownAction";
            this.CloseUnknownAction.ImageName = null;
            this.CloseUnknownAction.Shortcut = null;
            this.CloseUnknownAction.Tag = null;
            this.CloseUnknownAction.TargetObjectsCriteria = "[State] = ##Enum#IntecoAG.ERM.FM.PaymentRequest.RepaymentTaskStates,UNKNOWN#";
            this.CloseUnknownAction.TargetViewId = null;
            this.CloseUnknownAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.CloseUnknownAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CloseUnknownAction.ToolTip = null;
            this.CloseUnknownAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CloseUnknownAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CloseUnknownAction_Execute);
            // 
            // NewRequestAction
            // 
            this.NewRequestAction.Caption = "New Request";
            this.NewRequestAction.Category = "View";
            this.NewRequestAction.ConfirmationMessage = null;
            this.NewRequestAction.Id = "fmCPRRepaymentTaskViewController_NewRequest";
            this.NewRequestAction.ImageName = null;
            this.NewRequestAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.NewRequestAction.Shortcut = null;
            this.NewRequestAction.ShowItemsOnClick = true;
            this.NewRequestAction.Tag = null;
            this.NewRequestAction.TargetObjectsCriteria = null;
            this.NewRequestAction.TargetViewId = null;
            this.NewRequestAction.ToolTip = null;
            this.NewRequestAction.TypeOfView = null;
            this.NewRequestAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.NewRequestAction_Execute);
            // 
            // fmCPRRepaymentTaskViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.PaymentRequest.fmCPRRepaymentTask);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction UnknownAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CloseUnknownAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction NewRequestAction;
    }
}
