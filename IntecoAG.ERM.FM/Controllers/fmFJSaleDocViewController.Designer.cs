namespace IntecoAG.ERM.FM.Controllers {
    partial class fmFJSaleDocViewController {
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
            this.ImportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportAction
            // 
            this.ImportAction.Caption = "Import";
            this.ImportAction.Category = "Edit";
            this.ImportAction.ConfirmationMessage = null;
            this.ImportAction.Id = "fmFJSaleDocViewController_ImportAction";
            this.ImportAction.ImageName = null;
            this.ImportAction.Shortcut = null;
            this.ImportAction.Tag = null;
            this.ImportAction.TargetObjectsCriteria = null;
            this.ImportAction.TargetViewId = null;
            this.ImportAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportAction.ToolTip = null;
            this.ImportAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportAction_Execute);
            // 
            // ApproveAction
            // 
            this.ApproveAction.Caption = "Approve";
            this.ApproveAction.Category = "Edit";
            this.ApproveAction.ConfirmationMessage = null;
            this.ApproveAction.Id = "fmFJSaleDocViewController_ApproveAction";
            this.ApproveAction.ImageName = null;
            this.ApproveAction.Shortcut = null;
            this.ApproveAction.Tag = null;
            this.ApproveAction.TargetObjectsCriteria = null;
            this.ApproveAction.TargetViewId = null;
            this.ApproveAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ApproveAction.ToolTip = null;
            this.ApproveAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ApproveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAction_Execute);
            // 
            // fmFJSaleDocViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.FinJurnal.fmCFJSaleDoc);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAction;
    }
}
