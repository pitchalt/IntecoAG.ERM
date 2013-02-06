namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCDocRCBImportViewController {
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
            this.ProcessAction.Category = "View";
            this.ProcessAction.ConfirmationMessage = null;
            this.ProcessAction.Id = "fmCDocRCBImportViewController_Process";
            this.ProcessAction.ImageName = null;
            this.ProcessAction.Shortcut = null;
            this.ProcessAction.Tag = null;
            this.ProcessAction.TargetObjectsCriteria = null;
            this.ProcessAction.TargetViewId = null;
            this.ProcessAction.ToolTip = null;
            this.ProcessAction.TypeOfView = null;
            this.ProcessAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProcessAction_Execute);
            // 
            // fmCDocRCBImportViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.StatementAccount.fmCSATaskImporter);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ProcessAction;
    }
}
