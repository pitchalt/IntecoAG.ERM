namespace IntecoAG.ERM.Module {
    partial class NewVersionController {
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
            this.CreateNewVersionAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreateNewVersionAction
            // 
            this.CreateNewVersionAction.Caption = "Create Version";
            this.CreateNewVersionAction.Category = "View";
            this.CreateNewVersionAction.ConfirmationMessage = null;
            this.CreateNewVersionAction.Id = "NewVersionAction";
            this.CreateNewVersionAction.ImageName = null;
            this.CreateNewVersionAction.Shortcut = null;
            this.CreateNewVersionAction.Tag = null;
            this.CreateNewVersionAction.TargetObjectsCriteria = "This is IntecoAG.ERM.CS.INewVersionSupport";
            this.CreateNewVersionAction.TargetViewId = "WorkPlan_DetailView";
            this.CreateNewVersionAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreateNewVersionAction.ToolTip = "Create new version";
            this.CreateNewVersionAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CreateNewVersionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateNewVersionAction_Execute);
            // 
            // NewVersionController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateNewVersionAction;
    }
}
