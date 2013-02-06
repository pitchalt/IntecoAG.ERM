namespace IntecoAG.ERM.Module {
    partial class WizardSupportController {
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
            this.WizardSupportCompleteAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // WizardSupportCompleteAction
            // 
            this.WizardSupportCompleteAction.Caption = "Wizard Support Complete Action";
            this.WizardSupportCompleteAction.Category = "Wizard";
            this.WizardSupportCompleteAction.ConfirmationMessage = null;
            this.WizardSupportCompleteAction.Id = "WizardSupportCompleteAction";
            this.WizardSupportCompleteAction.ImageName = null;
            this.WizardSupportCompleteAction.Shortcut = null;
            this.WizardSupportCompleteAction.Tag = null;
            this.WizardSupportCompleteAction.TargetObjectsCriteria = null;
            this.WizardSupportCompleteAction.TargetViewId = null;
            this.WizardSupportCompleteAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.WizardSupportCompleteAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.WizardSupportCompleteAction.ToolTip = null;
            this.WizardSupportCompleteAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.WizardSupportCompleteAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WizardSupportCompleteAction_Execute);

        }

        #endregion

        public DevExpress.ExpressApp.Actions.SimpleAction WizardSupportCompleteAction;

    }
}
