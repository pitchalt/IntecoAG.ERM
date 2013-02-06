namespace IntecoAG.ERM.HRM {
    partial class StaffSyncController  {
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
            this.SyncAllAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SyncChangesAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SyncAllAction
            // 
            this.SyncAllAction.Caption = "Sync All";
            this.SyncAllAction.Category = "BusinessLogic";
            this.SyncAllAction.ConfirmationMessage = null;
            this.SyncAllAction.Id = "StaffSyncController_SyncAllAction";
            this.SyncAllAction.ImageName = null;
            this.SyncAllAction.Shortcut = null;
            this.SyncAllAction.Tag = null;
            this.SyncAllAction.TargetObjectsCriteria = null;
            this.SyncAllAction.TargetObjectType = typeof(IntecoAG.ERM.HRM.Organization.hrmStaff);
            this.SyncAllAction.TargetViewId = null;
            this.SyncAllAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.SyncAllAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SyncAllAction.ToolTip = null;
            this.SyncAllAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SyncAllAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SyncAllAction_Execute);
            // 
            // SyncChangesAction
            // 
            this.SyncChangesAction.Caption = "Sync Changes";
            this.SyncChangesAction.Category = "BusinessLogic";
            this.SyncChangesAction.ConfirmationMessage = null;
            this.SyncChangesAction.Id = "StaffSyncController_SyncChangesAction";
            this.SyncChangesAction.ImageName = null;
            this.SyncChangesAction.Shortcut = null;
            this.SyncChangesAction.Tag = null;
            this.SyncChangesAction.TargetObjectsCriteria = null;
            this.SyncChangesAction.TargetViewId = null;
            this.SyncChangesAction.ToolTip = null;
            this.SyncChangesAction.TypeOfView = null;
            this.SyncChangesAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SyncChangesAction_Execute);
            // 
            // StaffSyncController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.HRM.Organization.hrmStaff);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SyncAllAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SyncChangesAction;
    }
}
