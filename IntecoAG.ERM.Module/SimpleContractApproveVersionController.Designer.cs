namespace IntecoAG.ERM.Module {
    partial class SimpleContractApproveVersionController {
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
            this.SimpleContractVersionApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SimpleContractVersionApprove
            // 
            this.SimpleContractVersionApprove.Caption = "Approve";
            this.SimpleContractVersionApprove.Category = "View";
            this.SimpleContractVersionApprove.ConfirmationMessage = "Утвердить документ?";
            this.SimpleContractVersionApprove.Id = "SimpleContractVersionApprove";
            this.SimpleContractVersionApprove.ImageName = null;
            this.SimpleContractVersionApprove.Shortcut = null;
            this.SimpleContractVersionApprove.Tag = null;
            this.SimpleContractVersionApprove.TargetObjectsCriteria = null;
            this.SimpleContractVersionApprove.TargetViewId = null;
            this.SimpleContractVersionApprove.ToolTip = "Approve the version of simple contract";
            this.SimpleContractVersionApprove.TypeOfView = null;
            this.SimpleContractVersionApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SimpleContractNewVersion_Execute);
            // 
            // SimpleContractApproveVersionController
            // 
            this.TargetViewId = "SimpleContractVersion_DetailView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SimpleContractVersionApprove;
    }
}
