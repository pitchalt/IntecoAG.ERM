namespace IntecoAG.ERM.FM.Controllers {
    partial class fmFinIndexStructureViewController {
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
            this.UpdateFinStructureAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // UpdateFinStructureAction
            // 
            this.UpdateFinStructureAction.Caption = "fmFinIndexStructureViewController_UpdateFinStructureAction";
            this.UpdateFinStructureAction.ConfirmationMessage = null;
            this.UpdateFinStructureAction.Id = "fmFinIndexStructureViewController_UpdateFinStructureAction";
            this.UpdateFinStructureAction.ImageName = null;
            this.UpdateFinStructureAction.Shortcut = null;
            this.UpdateFinStructureAction.Tag = null;
            this.UpdateFinStructureAction.TargetObjectsCriteria = null;
            this.UpdateFinStructureAction.TargetViewId = null;
            this.UpdateFinStructureAction.ToolTip = null;
            this.UpdateFinStructureAction.TypeOfView = null;
            this.UpdateFinStructureAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateFinStructureAction_Execute);
            // 
            // fmFinIndexStructureViewController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction UpdateFinStructureAction;
    }
}
