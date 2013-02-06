namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCRHPayedReqReportParamsViewController
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
            this.NextAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // NextAction
            // 
            this.NextAction.Caption = "Next";
            this.NextAction.Category = "fmCRHPayedReqReportParams";
            this.NextAction.ConfirmationMessage = null;
            this.NextAction.Id = "fmCRHUPayedReqReportParamsViewController_NextAction";
            this.NextAction.ImageName = null;
            this.NextAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.NextAction.Shortcut = null;
            this.NextAction.Tag = null;
            this.NextAction.TargetObjectsCriteria = null;
            this.NextAction.TargetViewId = null;
            this.NextAction.ToolTip = "Next";
            this.NextAction.TypeOfView = null;
            this.NextAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.NextRequest_Execute);
            // 
            // fmCRHPayedReqReportParamsViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.ReportHelper.fmCRHPayedRequestReportParameters);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction NextAction;
    }
}
