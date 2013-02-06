namespace IntecoAG.ERM.Module {
    partial class ComplexContractApproveVersionControlle {
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
            this.ComplexContractVersionApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ComplexContractVersionApprove
            // 
            this.ComplexContractVersionApprove.Caption = "Approve";
            this.ComplexContractVersionApprove.Category = "View";
            this.ComplexContractVersionApprove.ConfirmationMessage = "Утвердить документ?";
            this.ComplexContractVersionApprove.Id = "ComplexContractVersionApprove";
            this.ComplexContractVersionApprove.ImageName = null;
            this.ComplexContractVersionApprove.Shortcut = null;
            this.ComplexContractVersionApprove.Tag = null;
            this.ComplexContractVersionApprove.TargetObjectsCriteria = null;
            this.ComplexContractVersionApprove.TargetViewId = null;
            this.ComplexContractVersionApprove.ToolTip = "Approve the version of Complex contract";
            this.ComplexContractVersionApprove.TypeOfView = null;
            this.ComplexContractVersionApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ComplexContractNewVersion_Execute);
            // 
            // ComplexContractApproveVersionControlle
            // 
            this.TargetViewId = "ComplexContractVersion_DetailView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ComplexContractVersionApprove;
    }
}
