namespace IntecoAG.ERM.FM.AVT {
    partial class fmCAVTBookVATViewController {
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
            this.ImportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportAction
            // 
            this.ImportAction.Caption = "Import";
            this.ImportAction.ConfirmationMessage = null;
            this.ImportAction.Id = "fmCAVTBookVATViewController_ImportAction";
            this.ImportAction.ImageName = null;
            this.ImportAction.Shortcut = null;
            this.ImportAction.Tag = null;
            this.ImportAction.TargetObjectsCriteria = null;
            this.ImportAction.TargetViewId = null;
            this.ImportAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ImportAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportAction.ToolTip = null;
            this.ImportAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportAction_Execute);
            // 
            // fmCAVTBookVATViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.AVT.fmCAVTBookVAT);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportAction;
    }
}
