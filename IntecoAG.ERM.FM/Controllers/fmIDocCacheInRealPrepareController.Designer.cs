namespace IntecoAG.ERM.FM.Controllers {
    partial class fmIDocCacheInRealPrepareController {
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
            this.ApproveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ApproveAction
            // 
            this.ApproveAction.Caption = "Approve";
            this.ApproveAction.Category = "RecordEdit";
            this.ApproveAction.ConfirmationMessage = null;
            this.ApproveAction.Id = "fmIDocCacheInRealPrepareController_ApproveAction";
            this.ApproveAction.ImageName = null;
            this.ApproveAction.Shortcut = null;
            this.ApproveAction.Tag = null;
            this.ApproveAction.TargetObjectsCriteria = null;
            this.ApproveAction.TargetObjectType = typeof(IntecoAG.ERM.FM.Docs.Cache.fmIDocCacheInRealPrepare);
            this.ApproveAction.TargetViewId = null;
            this.ApproveAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ApproveAction.ToolTip = null;
            this.ApproveAction.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ApproveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAction_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAction;
    }
}
