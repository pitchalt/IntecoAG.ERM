namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCDocRCBImportResultViewController {
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
            this.AutoBinding = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ProcessAction
            // 
            this.ProcessAction.Caption = "Process";
            this.ProcessAction.Category = "View";
            this.ProcessAction.ConfirmationMessage = null;
            this.ProcessAction.Id = "fmCDocRCBImportResultViewController_Process";
            this.ProcessAction.ImageName = null;
            this.ProcessAction.Shortcut = null;
            this.ProcessAction.Tag = null;
            this.ProcessAction.TargetObjectsCriteria = null;
            this.ProcessAction.TargetViewId = null;
            this.ProcessAction.ToolTip = null;
            this.ProcessAction.TypeOfView = null;
            this.ProcessAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProcessAction_Execute);
            // 
            // AutoBinding
            // 
            this.AutoBinding.Caption = "Auto Binding";
            this.AutoBinding.Category = "View";
            this.AutoBinding.ConfirmationMessage = null;
            this.AutoBinding.Id = "fmCDocRCBImportResultViewController_AutoBinding";
            this.AutoBinding.ImageName = null;
            this.AutoBinding.Shortcut = null;
            this.AutoBinding.Tag = null;
            this.AutoBinding.TargetObjectsCriteria = null;
            this.AutoBinding.TargetViewId = null;
            this.AutoBinding.ToolTip = null;
            this.AutoBinding.TypeOfView = null;
            this.AutoBinding.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AutoBinding_Execute);
            // 
            // fmCDocRCBImportResultViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.StatementAccount.fmCSAImportResult);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        public DevExpress.ExpressApp.Actions.SimpleAction ProcessAction;
        public DevExpress.ExpressApp.Actions.SimpleAction AutoBinding;

    }
}
