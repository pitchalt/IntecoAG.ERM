namespace IntecoAG.ERM.Module {
    partial class LinkWorkPlanToComplexContractController {
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
            this.ListWorkPlanPopupWindowShowAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ListWorkPlanPopupWindowShowAction
            // 
            this.ListWorkPlanPopupWindowShowAction.AcceptButtonCaption = null;
            this.ListWorkPlanPopupWindowShowAction.CancelButtonCaption = null;
            this.ListWorkPlanPopupWindowShowAction.Caption = "Work Plan List";
            this.ListWorkPlanPopupWindowShowAction.Category = "View";
            this.ListWorkPlanPopupWindowShowAction.ConfirmationMessage = null;
            this.ListWorkPlanPopupWindowShowAction.Id = "ListWorkPlanPopupWindowShowAction";
            this.ListWorkPlanPopupWindowShowAction.ImageName = null;
            this.ListWorkPlanPopupWindowShowAction.Shortcut = null;
            this.ListWorkPlanPopupWindowShowAction.Tag = null;
            this.ListWorkPlanPopupWindowShowAction.TargetObjectsCriteria = null;
            this.ListWorkPlanPopupWindowShowAction.TargetViewId = "ComplexContractVersion_WorkPlanVersions_ListView";
            this.ListWorkPlanPopupWindowShowAction.ToolTip = null;
            this.ListWorkPlanPopupWindowShowAction.TypeOfView = null;
            this.ListWorkPlanPopupWindowShowAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ListWorkPlanPopupWindowShowAction_CustomizePopupWindowParams);
            this.ListWorkPlanPopupWindowShowAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ListWorkPlanPopupWindowShowAction_Execute);
            // 
            // LinkWorkPlanToComplexContractController
            // 
            this.TargetViewId = "ComplexContractVersion_WorkPlanVersions_ListView";
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ListWorkPlanPopupWindowShowAction;
    }
}
