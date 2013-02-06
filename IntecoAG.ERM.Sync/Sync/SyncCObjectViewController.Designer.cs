namespace IntecoAG.ERM.Sync {
    partial class SyncCObjectViewController {
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
            this.SyncAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SyncAction
            // 
            this.SyncAction.Caption = "Sync";
            this.SyncAction.Category = "Edit";
            this.SyncAction.ConfirmationMessage = null;
            this.SyncAction.Id = "SyncCObjectViewController_SyncAction";
            this.SyncAction.ImageName = "SyncAction_16x16.ico";
            this.SyncAction.Shortcut = null;
            this.SyncAction.Tag = null;
            this.SyncAction.TargetObjectsCriteria = null;
            this.SyncAction.TargetViewId = null;
            this.SyncAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.SyncAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.SyncAction.ToolTip = null;
            this.SyncAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.SyncAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SyncAction_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SyncAction;
    }
}
