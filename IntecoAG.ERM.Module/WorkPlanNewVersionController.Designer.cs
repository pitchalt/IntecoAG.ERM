namespace IntecoAG.ERM.Module {
    partial class WorkPlanNewVersionController {
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
            this.WorkPlanVersionNewVersion = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // WorkPlanVersionNewVersion
            // 
            this.WorkPlanVersionNewVersion.Caption = "New Version";
            this.WorkPlanVersionNewVersion.Category = "View";
            this.WorkPlanVersionNewVersion.ConfirmationMessage = null;
            this.WorkPlanVersionNewVersion.Id = "WorkPlanVersionNewVersion";
            this.WorkPlanVersionNewVersion.ImageName = null;
            this.WorkPlanVersionNewVersion.Shortcut = null;
            this.WorkPlanVersionNewVersion.Tag = null;
            this.WorkPlanVersionNewVersion.TargetObjectsCriteria = null;
            this.WorkPlanVersionNewVersion.TargetViewId = null;
            this.WorkPlanVersionNewVersion.ToolTip = "Create new version of work plan";
            this.WorkPlanVersionNewVersion.TypeOfView = null;
            this.WorkPlanVersionNewVersion.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SimpleContractNewVersion_Execute);
            // 
            // WorkPlanNewVersionController
            // 
            this.TargetViewId = "WorkPlan_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction WorkPlanVersionNewVersion;
    }
}
