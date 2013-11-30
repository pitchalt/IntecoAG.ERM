namespace IntecoAG.ERM.Trw.Budget {
    partial class TrwBudgetVC {
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
            this.CalculateAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CalculateAction
            // 
            this.CalculateAction.Caption = "Calculate";
            this.CalculateAction.Category = "Edit";
            this.CalculateAction.ConfirmationMessage = null;
            this.CalculateAction.Id = "TrwBudgetVC_CalculateAction";
            this.CalculateAction.ImageName = null;
            this.CalculateAction.Shortcut = null;
            this.CalculateAction.Tag = null;
            this.CalculateAction.TargetObjectsCriteria = null;
            this.CalculateAction.TargetViewId = null;
            this.CalculateAction.ToolTip = null;
            this.CalculateAction.TypeOfView = null;
            this.CalculateAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CalculateAction_Execute);
            // 
            // TrwBudgetVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.Trw.Budget.TrwBudgetBase);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CalculateAction;
    }
}
