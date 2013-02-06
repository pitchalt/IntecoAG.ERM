namespace IntecoAG.ERM.Module {
    partial class VersionSupportViewController {
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
            this.VersionApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreateNewVersionAction
            // 
            this.CreateNewVersionAction.Caption = "Create Version";
            this.CreateNewVersionAction.Category = "VersionActions";
            this.CreateNewVersionAction.ConfirmationMessage = null;
            this.CreateNewVersionAction.Id = "NewVersionAction";
            this.CreateNewVersionAction.ImageName = "folder_txt_16x16.png";
            this.CreateNewVersionAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.CreateNewVersionAction.Shortcut = null;
            this.CreateNewVersionAction.Tag = null;
            this.CreateNewVersionAction.TargetViewId = "";
            this.CreateNewVersionAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.CreateNewVersionAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreateNewVersionAction.ToolTip = "Create new version";
            this.CreateNewVersionAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // VersionApprove
            // 
            this.VersionApprove.Caption = "Approve";
            this.VersionApprove.Category = "VersionActions";
            this.VersionApprove.ConfirmationMessage = "Утвердить документ?";
            this.VersionApprove.Id = "VersionApprove";
            this.VersionApprove.ImageName = "auction_hammer_16x16.png";
            this.VersionApprove.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.VersionApprove.Shortcut = null;
            this.VersionApprove.Tag = null;
            this.VersionApprove.TargetViewId = "";
            this.VersionApprove.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.VersionApprove.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.VersionApprove.ToolTip = "Approve the version";
            this.VersionApprove.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // VersionSupportViewController
            // 
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateNewVersionAction;
        private DevExpress.ExpressApp.Actions.SimpleAction VersionApprove;
    }
}
