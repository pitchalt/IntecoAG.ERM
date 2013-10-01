namespace IntecoAG.ERM.Module {
    partial class ImportReportsViewController {
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
            this.ImportReports = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportReports
            // 
            this.ImportReports.Caption = "Import Reports";
            this.ImportReports.Category = "View";
            this.ImportReports.ConfirmationMessage = null;
            this.ImportReports.Id = "ImportReports";
            this.ImportReports.ImageName = null;
            this.ImportReports.Shortcut = null;
            this.ImportReports.Tag = null;
            this.ImportReports.TargetObjectsCriteria = null;
            this.ImportReports.TargetViewId = null;
            this.ImportReports.ToolTip = null;
            this.ImportReports.TypeOfView = null;
            this.ImportReports.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportReports_Execute);
            // 
            // ImportReportsViewController
            // 
            this.TargetObjectType = typeof(DevExpress.ExpressApp.Reports.ReportData);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportReports;
    }
}
