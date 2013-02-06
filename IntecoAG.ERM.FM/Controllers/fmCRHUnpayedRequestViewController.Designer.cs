namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCRHUnpayedRequestViewController
    {
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
            this.FullPrintAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ShortPrintAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FullPrintAction
            // 
            this.FullPrintAction.Caption = "Full Print";
            this.FullPrintAction.Category = "View";
            this.FullPrintAction.ConfirmationMessage = null;
            this.FullPrintAction.Id = "fmCRHUnpayedRequestViewController_FullPrintAction";
            this.FullPrintAction.ImageName = null;
            this.FullPrintAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.FullPrintAction.Shortcut = null;
            this.FullPrintAction.Tag = null;
            this.FullPrintAction.TargetObjectsCriteria = null;
            this.FullPrintAction.TargetViewId = null;
            this.FullPrintAction.ToolTip = "Full Print";
            this.FullPrintAction.TypeOfView = null;
            this.FullPrintAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FullPrintAction_Execute);
            // 
            // ShortPrintAction
            // 
            this.ShortPrintAction.Caption = "Short Print";
            this.ShortPrintAction.Category = "View";
            this.ShortPrintAction.ConfirmationMessage = null;
            this.ShortPrintAction.Id = "fmCRHUnpayedRequestViewController_ShortPrintAction";
            this.ShortPrintAction.ImageName = null;
            this.ShortPrintAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ShortPrintAction.Shortcut = null;
            this.ShortPrintAction.Tag = null;
            this.ShortPrintAction.TargetObjectsCriteria = null;
            this.ShortPrintAction.TargetViewId = null;
            this.ShortPrintAction.ToolTip = "Short Print";
            this.ShortPrintAction.TypeOfView = null;
            this.ShortPrintAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ShortPrintAction_Execute);
            // 
            // fmCRHUnpayedRequestViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.ReportHelper.fmCRHUnpayedRequest);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FullPrintAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ShortPrintAction;
    }
}
