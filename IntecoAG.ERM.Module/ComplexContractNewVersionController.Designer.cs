namespace IntecoAG.ERM.Module {
    partial class ComplexContractNewVersionController {
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
            this.ComplexContractNewVersion = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ComplexContractNewVersion
            // 
            this.ComplexContractNewVersion.Caption = "New Version";
            this.ComplexContractNewVersion.Category = "View";
            this.ComplexContractNewVersion.ConfirmationMessage = null;
            this.ComplexContractNewVersion.Id = "ComplexContractNewVersion";
            this.ComplexContractNewVersion.ImageName = null;
            this.ComplexContractNewVersion.Shortcut = null;
            this.ComplexContractNewVersion.Tag = null;
            this.ComplexContractNewVersion.TargetObjectsCriteria = null;
            this.ComplexContractNewVersion.TargetViewId = null;
            this.ComplexContractNewVersion.ToolTip = "Create new version of complex contract";
            this.ComplexContractNewVersion.TypeOfView = null;
            this.ComplexContractNewVersion.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SimpleContractNewVersion_Execute);
            // 
            // ComplexContractNewVersionController
            // 
            this.TargetViewId = "ComplexContract_DetailView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ComplexContractNewVersion;
    }
}
