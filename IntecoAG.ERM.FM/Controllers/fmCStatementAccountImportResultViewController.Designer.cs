namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCStatementAccountImportResultViewController {
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
            this.ProcessAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ProcessAction
            // 
            this.ProcessAction.Caption = "Process";
            this.ProcessAction.ConfirmationMessage = null;
            this.ProcessAction.Id = "fmCStatementAccountImportResultViewController_ProcessAction";
            this.ProcessAction.ImageName = null;
            this.ProcessAction.Shortcut = null;
            this.ProcessAction.Tag = null;
            this.ProcessAction.TargetObjectsCriteria = null;
            this.ProcessAction.TargetViewId = null;
            this.ProcessAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ProcessAction.ToolTip = null;
            this.ProcessAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ProcessAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProcessAction_Execute);
            // 
            // fmCStatementAccountImportResultViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.StatementAccount.fmCSAImportResult);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ProcessAction;
    }
}
