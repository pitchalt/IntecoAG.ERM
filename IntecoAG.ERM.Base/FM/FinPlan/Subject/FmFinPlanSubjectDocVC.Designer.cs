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
            this.ImportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportAction
            // 
            this.ImportAction.Caption = "FmFinPlanSubjectDocVC_ImportAction";
            this.ImportAction.ConfirmationMessage = null;
            this.ImportAction.Id = "FmFinPlanSubjectDocVC_ImportAction";
            this.ImportAction.ImageName = null;
            this.ImportAction.Shortcut = null;
            this.ImportAction.Tag = null;
            this.ImportAction.TargetObjectsCriteria = null;
            this.ImportAction.TargetViewId = null;
            this.ImportAction.ToolTip = null;
            this.ImportAction.TypeOfView = null;
            this.ImportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportAction_Execute);
            // 
            // FmFinPlanSubjectDocVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.FinPlan.Subject.FmFinPlanSubjectDoc);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportAction;
    }
}
