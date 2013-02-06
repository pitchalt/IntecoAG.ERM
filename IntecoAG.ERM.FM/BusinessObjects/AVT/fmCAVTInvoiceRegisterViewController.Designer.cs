namespace IntecoAG.ERM.FM.AVT {
    partial class fmCAVTInvoiceRegisterViewController {
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
            this.ImportAvansAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReNumberAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InvoiceImportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportAction
            // 
            this.ImportAction.Caption = "Import";
            this.ImportAction.ConfirmationMessage = null;
            this.ImportAction.Id = "fmCAVTInvoiceRegisterViewController_ImportAction";
            this.ImportAction.ImageName = null;
            this.ImportAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImportAction.Shortcut = null;
            this.ImportAction.Tag = null;
            this.ImportAction.TargetObjectsCriteria = null;
            this.ImportAction.TargetViewId = null;
            this.ImportAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ImportAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportAction.ToolTip = null;
            this.ImportAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportAction_Execute);
            // 
            // ImportAvansAction
            // 
            this.ImportAvansAction.Caption = "ImportAvansAction";
            this.ImportAvansAction.Category = "RecordEdit";
            this.ImportAvansAction.ConfirmationMessage = null;
            this.ImportAvansAction.Id = "fmCAVTInvoiceRegisterViewController_ImportAvansAction";
            this.ImportAvansAction.ImageName = null;
            this.ImportAvansAction.Shortcut = null;
            this.ImportAvansAction.Tag = null;
            this.ImportAvansAction.TargetObjectsCriteria = null;
            this.ImportAvansAction.TargetViewId = null;
            this.ImportAvansAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ImportAvansAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportAvansAction.ToolTip = null;
            this.ImportAvansAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportAvansAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportAvansAction_Execute);
            // 
            // ReNumberAction
            // 
            this.ReNumberAction.Caption = "ReNumber";
            this.ReNumberAction.Category = "RecordEdit";
            this.ReNumberAction.ConfirmationMessage = null;
            this.ReNumberAction.Id = "fmCAVTInvoiceRegisterViewController_ReNumber";
            this.ReNumberAction.ImageName = null;
            this.ReNumberAction.Shortcut = null;
            this.ReNumberAction.Tag = null;
            this.ReNumberAction.TargetObjectsCriteria = null;
            this.ReNumberAction.TargetViewId = null;
            this.ReNumberAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ReNumberAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ReNumberAction.ToolTip = null;
            this.ReNumberAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ReNumberAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReNumberAction_Execute);
            // 
            // InvoiceImportAction
            // 
            this.InvoiceImportAction.Caption = "InvoiceImport";
            this.InvoiceImportAction.Category = "RecordEdit";
            this.InvoiceImportAction.ConfirmationMessage = null;
            this.InvoiceImportAction.Id = "fmCAVTInvoiceRegisterController_InvoiceImportAction";
            this.InvoiceImportAction.ImageName = null;
            this.InvoiceImportAction.Shortcut = null;
            this.InvoiceImportAction.Tag = null;
            this.InvoiceImportAction.TargetObjectsCriteria = null;
            this.InvoiceImportAction.TargetViewId = null;
            this.InvoiceImportAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.InvoiceImportAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.InvoiceImportAction.ToolTip = null;
            this.InvoiceImportAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.InvoiceImportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.InvoiceImportAction_Execute);
            // 
            // fmCAVTInvoiceRegisterViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.AVT.fmCAVTInvoiceRegister);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ImportAvansAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ReNumberAction;
        private DevExpress.ExpressApp.Actions.SimpleAction InvoiceImportAction;
    }
}
