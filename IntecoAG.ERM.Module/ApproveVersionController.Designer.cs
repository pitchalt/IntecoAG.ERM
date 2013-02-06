namespace IntecoAG.ERM.Module {
    partial class ApproveVersionController {
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
            this.ComplexContractVersionApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.WorkPlanVersionApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
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
            this.SimpleContractVersionApprove.TargetObjectType = typeof(IntecoAG.ERM.CRM.Contract.SimpleContractVersion);
            this.SimpleContractVersionApprove.TargetViewId = "SimpleContractVersion_DetailView";
            this.SimpleContractVersionApprove.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.SimpleContractVersionApprove.ToolTip = "Approve the version of simple contract";
            this.SimpleContractVersionApprove.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.SimpleContractVersionApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SimpleContractApproveVersion_Execute);
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
            this.ComplexContractVersionApprove.TargetObjectType = typeof(IntecoAG.ERM.CRM.Contract.ComplexContractVersion);
            this.ComplexContractVersionApprove.TargetViewId = "ComplexContractVersion_DetailView";
            this.ComplexContractVersionApprove.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ComplexContractVersionApprove.ToolTip = "Approve the version of Complex contract";
            this.ComplexContractVersionApprove.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ComplexContractVersionApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ComplexContractApproveVersion_Execute);
            // 
            // WorkPlanVersionApprove
            // 
            this.WorkPlanVersionApprove.Caption = "Approve";
            this.WorkPlanVersionApprove.Category = "View";
            this.WorkPlanVersionApprove.ConfirmationMessage = "Утвердить документ?";
            this.WorkPlanVersionApprove.Id = "WorkPlanVersionApprove";
            this.WorkPlanVersionApprove.ImageName = null;
            this.WorkPlanVersionApprove.Shortcut = null;
            this.WorkPlanVersionApprove.Tag = null;
            this.WorkPlanVersionApprove.TargetObjectsCriteria = null;
            this.WorkPlanVersionApprove.TargetObjectType = typeof(IntecoAG.ERM.CRM.Contract.WorkPlanVersion);
            this.WorkPlanVersionApprove.TargetViewId = "WorkPlanVersion_DetailView";
            this.WorkPlanVersionApprove.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.WorkPlanVersionApprove.ToolTip = "Approve the version of work plan";
            this.WorkPlanVersionApprove.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.WorkPlanVersionApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WorkPlanApproveVersion_Execute);
            // 
            // ApproveVersionController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SimpleContractVersionApprove;
        private DevExpress.ExpressApp.Actions.SimpleAction ComplexContractVersionApprove;
        private DevExpress.ExpressApp.Actions.SimpleAction WorkPlanVersionApprove;
    }
}
