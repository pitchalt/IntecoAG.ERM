namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCSARepaymentStatementAccountViewController {
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
            this.Open = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AutoBinding = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Open
            // 
            this.Open.Caption = "Open";
            this.Open.Category = "View";
            this.Open.ConfirmationMessage = null;
            this.Open.Id = "fmCSARepaymentStatementAccountViewController_Open";
            this.Open.ImageName = null;
            this.Open.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Open.Shortcut = null;
            this.Open.Tag = null;
            this.Open.TargetObjectsCriteria = null;
            this.Open.TargetViewId = null;
            this.Open.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.Open.ToolTip = null;
            this.Open.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Open.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Open_Execute);
            // 
            // AutoBinding
            // 
            this.AutoBinding.Caption = "Auto Binding";
            this.AutoBinding.Category = "View";
            this.AutoBinding.ConfirmationMessage = null;
            this.AutoBinding.Id = "fmCSARepaymentStatementAccountViewController_AutoBinding";
            this.AutoBinding.ImageName = null;
            this.AutoBinding.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.AutoBinding.Shortcut = null;
            this.AutoBinding.Tag = null;
            this.AutoBinding.TargetObjectsCriteria = null;
            this.AutoBinding.TargetViewId = null;
            this.AutoBinding.ToolTip = null;
            this.AutoBinding.TypeOfView = null;
            this.AutoBinding.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AutoBinding_Execute);
            // 
            // fmCSARepaymentStatementAccountViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.StatementAccount.fmCSAStatementAccount);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Open;
        private DevExpress.ExpressApp.Actions.SimpleAction AutoBinding;
    }
}
