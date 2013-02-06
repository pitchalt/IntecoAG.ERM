namespace IntecoAG.ERM.Module {
    partial class ShowRootObjectController {
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
            this.GotoMainAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GotoMainAction
            // 
            this.GotoMainAction.Caption = "Main object";
            this.GotoMainAction.Category = "View";
            this.GotoMainAction.ConfirmationMessage = null;
            this.GotoMainAction.Id = "GotoMainAction";
            this.GotoMainAction.ImageName = null;
            this.GotoMainAction.Shortcut = null;
            this.GotoMainAction.Tag = null;
            this.GotoMainAction.TargetObjectsCriteria = null;
            this.GotoMainAction.TargetViewId = "";
            this.GotoMainAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.GotoMainAction.ToolTip = "Go to main object";
            this.GotoMainAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.GotoMainAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GotoMainAction_Execute);
            // 
            // ShowRootObjectController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction GotoMainAction;
    }
}
