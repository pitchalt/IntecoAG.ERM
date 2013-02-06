namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCSAStatementAccountDocViewController {
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
            this.AutoBinding = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ManualBinding
            // 
            this.ManualBinding.Caption = "Manual Binding";
            this.ManualBinding.Category = "View";
            this.ManualBinding.ConfirmationMessage = null;
            this.ManualBinding.Id = "fmCSAStatementAccountDocViewController_ManualBinding";
            this.ManualBinding.ImageName = null;
            this.ManualBinding.Shortcut = null;
            this.ManualBinding.Tag = null;
            this.ManualBinding.TargetObjectsCriteria = null;
            this.ManualBinding.TargetViewId = null;
            this.ManualBinding.ToolTip = null;
            this.ManualBinding.TypeOfView = null;
            this.ManualBinding.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ManualBinding_Execute);
            // 
            // AutoBinding
            // 
            this.AutoBinding.Caption = "Auto Binding";
            this.AutoBinding.Category = "View";
            this.AutoBinding.ConfirmationMessage = null;
            this.AutoBinding.Id = "fmCSAStatementAccountDocViewController_AutoBinding";
            this.AutoBinding.ImageName = null;
            this.AutoBinding.Shortcut = null;
            this.AutoBinding.Tag = null;
            this.AutoBinding.TargetObjectsCriteria = null;
            this.AutoBinding.TargetViewId = null;
            this.AutoBinding.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.AutoBinding.ToolTip = null;
            this.AutoBinding.TypeOfView = null;
            this.AutoBinding.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AutoBinding_Execute);
            // 
            // fmCSAStatementAccountDocViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.StatementAccount.fmCSAStatementAccountDoc);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ManualBinding;
        private DevExpress.ExpressApp.Actions.SimpleAction AutoBinding;
    }
}
