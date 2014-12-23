namespace IntecoAG.ERM.FM.FinPlan.Subject {
    partial class FmFinPlanSubjectDocVC {
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
            this.TransactAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ConfirmAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TransactAction
            // 
            this.TransactAction.Caption = "Transact";
            this.TransactAction.Category = "RecordEdit";
            this.TransactAction.ConfirmationMessage = null;
            this.TransactAction.Id = "FmFinPlanSubjectDocVC_TransactAction";
            this.TransactAction.ImageName = null;
            this.TransactAction.Shortcut = null;
            this.TransactAction.Tag = null;
            this.TransactAction.TargetObjectsCriteria = null;
            this.TransactAction.TargetViewId = null;
            this.TransactAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TransactAction.ToolTip = null;
            this.TransactAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.TransactAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TransactAction_Execute);
            // 
            // ConfirmAction
            // 
            this.ConfirmAction.Caption = "Confirm";
            this.ConfirmAction.Category = "RecordEdit";
            this.ConfirmAction.ConfirmationMessage = null;
            this.ConfirmAction.Id = "FmFinPlanSubjectDocVC_ConfirmAction";
            this.ConfirmAction.ImageName = null;
            this.ConfirmAction.Shortcut = null;
            this.ConfirmAction.Tag = null;
            this.ConfirmAction.TargetObjectsCriteria = null;
            this.ConfirmAction.TargetViewId = null;
            this.ConfirmAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ConfirmAction.ToolTip = null;
            this.ConfirmAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ConfirmAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ConfirmAction_Execute);
            // 
            // FmFinPlanSubjectDocVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.FinPlan.Subject.FmFinPlanSubjectDoc);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction TransactAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ConfirmAction;

    }
}
