namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCPRPaymentRequestMemorandumViewController
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
            this.CreateTemplate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveMemorandum = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreateTemplate
            // 
            this.CreateTemplate.Caption = "CreateTemplate";
            this.CreateTemplate.Category = "BusinessLogic";
            this.CreateTemplate.ConfirmationMessage = null;
            this.CreateTemplate.Id = "fmCPRPaymentRequestMemorandumViewController_CreateTemplate";
            this.CreateTemplate.ImageName = null;
            this.CreateTemplate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.CreateTemplate.Shortcut = null;
            this.CreateTemplate.Tag = null;
            this.CreateTemplate.TargetObjectsCriteria = null;
            this.CreateTemplate.TargetViewId = null;
            this.CreateTemplate.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreateTemplate.ToolTip = "Create Template";
            this.CreateTemplate.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CreateTemplate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateTemplate_Execute);
            // 
            // ApproveMemorandum
            // 
            this.ApproveMemorandum.Caption = "Approve Memorandum";
            this.ApproveMemorandum.Category = "BusinessLogic";
            this.ApproveMemorandum.ConfirmationMessage = null;
            this.ApproveMemorandum.Id = "fmCPRPaymentRequestMemorandumViewController_ApproveMemorandum";
            this.ApproveMemorandum.ImageName = null;
            this.ApproveMemorandum.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ApproveMemorandum.Shortcut = null;
            this.ApproveMemorandum.Tag = null;
            this.ApproveMemorandum.TargetObjectsCriteria = null;
            this.ApproveMemorandum.TargetViewId = null;
            this.ApproveMemorandum.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ApproveMemorandum.ToolTip = "Approve Memorandum";
            this.ApproveMemorandum.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ApproveMemorandum.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveMemorandum_Execute);
            // 
            // fmCPRPaymentRequestMemorandumViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.PaymentRequest.fmPaymentRequestMemorandum);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateTemplate;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveMemorandum;

    }
}
