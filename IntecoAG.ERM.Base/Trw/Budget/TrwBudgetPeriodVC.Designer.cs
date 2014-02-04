namespace IntecoAG.ERM.Trw.Budget {
    partial class TrwBudgetPeriodVC {
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
            this.ImportInContractBsrAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportInContractBsrAction
            // 
            this.ImportInContractBsrAction.Caption = "TrwBudgetPeriodVC_ImportInContractBsrAction";
            this.ImportInContractBsrAction.Category = "Tools";
            this.ImportInContractBsrAction.ConfirmationMessage = null;
            this.ImportInContractBsrAction.Id = "TrwBudgetPeriodVC_ImportInContractBsrAction";
            this.ImportInContractBsrAction.ImageName = null;
            this.ImportInContractBsrAction.Shortcut = null;
            this.ImportInContractBsrAction.Tag = null;
            this.ImportInContractBsrAction.TargetObjectsCriteria = null;
            this.ImportInContractBsrAction.TargetViewId = null;
            this.ImportInContractBsrAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportInContractBsrAction.ToolTip = null;
            this.ImportInContractBsrAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportInContractBsrAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportInContractBsrAction_Execute);
            // 
            // TrwBudgetPeriodVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.Trw.Budget.TrwBudgetPeriod);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportInContractBsrAction;
    }
}
