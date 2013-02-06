namespace IntecoAG.ERM.HRM {
    partial class DepartmentSyncController  {
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
            this.SyncDepartmentListAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SyncDepartmentListAction
            // 
            this.SyncDepartmentListAction.Caption = "Sync IBSDepartment List Sync Action";
            this.SyncDepartmentListAction.ConfirmationMessage = null;
            this.SyncDepartmentListAction.Id = "SyncIBSDepartmentListSyncAction";
            this.SyncDepartmentListAction.ImageName = null;
            this.SyncDepartmentListAction.Shortcut = null;
            this.SyncDepartmentListAction.Tag = null;
            this.SyncDepartmentListAction.TargetObjectsCriteria = null;
            this.SyncDepartmentListAction.TargetViewId = null;
            this.SyncDepartmentListAction.ToolTip = null;
            this.SyncDepartmentListAction.TypeOfView = null;
            this.SyncDepartmentListAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SyncDepartmentListAction_Execute);
            // 
            // DepartmentSyncController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.HRM.Organization.hrmDepartment);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SyncDepartmentListAction;
    }
}
