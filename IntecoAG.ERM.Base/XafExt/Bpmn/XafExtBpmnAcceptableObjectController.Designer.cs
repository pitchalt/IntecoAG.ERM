namespace IntecoAG.XafExt.Bpmn {
    partial class XafExtBpmnAcceptableObjectController {
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
            this.AcceptAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RejectAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptAction
            // 
            this.AcceptAction.Caption = "Accept";
            this.AcceptAction.Category = "Save";
            this.AcceptAction.ConfirmationMessage = null;
            this.AcceptAction.Id = "XafExtBpmnAcceptableObjectController_AcceptAction";
            this.AcceptAction.ImageName = null;
            this.AcceptAction.Shortcut = null;
            this.AcceptAction.Tag = null;
            this.AcceptAction.TargetObjectsCriteria = "IsAcceptable";
            this.AcceptAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.AcceptAction.TargetObjectType = typeof(IntecoAG.XafExt.Bpmn.XafExtBpmnIAcceptableObject);
            this.AcceptAction.TargetViewId = null;
            this.AcceptAction.ToolTip = null;
            this.AcceptAction.TypeOfView = null;
            this.AcceptAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptAction_Execute);
            // 
            // RejectAction
            // 
            this.RejectAction.Caption = "Reject";
            this.RejectAction.Category = "Save";
            this.RejectAction.ConfirmationMessage = null;
            this.RejectAction.Id = "XafExtBpmnAcceptableObjectController_RejectAction";
            this.RejectAction.ImageName = null;
            this.RejectAction.Shortcut = null;
            this.RejectAction.Tag = null;
            this.RejectAction.TargetObjectsCriteria = "IsRejectable";
            this.RejectAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.RejectAction.TargetObjectType = typeof(IntecoAG.XafExt.Bpmn.XafExtBpmnIAcceptableObject);
            this.RejectAction.TargetViewId = null;
            this.RejectAction.ToolTip = null;
            this.RejectAction.TypeOfView = null;
            this.RejectAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RejectAction_Execute);
            // 
            // XafExtBpmnAcceptableObjectController
            // 
            this.TargetObjectType = typeof(IntecoAG.XafExt.Bpmn.XafExtBpmnIAcceptableObject);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptAction;
        private DevExpress.ExpressApp.Actions.SimpleAction RejectAction;
    }
}
