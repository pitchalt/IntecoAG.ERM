namespace IntecoAG.ERM.Module
{
    partial class ShowReportController
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
            this.ShowReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ClearReportFilterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ShowReportAction
            // 
            this.ShowReportAction.Caption = "Show report";
            this.ShowReportAction.Category = "ShowReport";
            this.ShowReportAction.ConfirmationMessage = null;
            this.ShowReportAction.Id = "ShowReportAction";
            this.ShowReportAction.ImageName = null;
            this.ShowReportAction.Shortcut = null;
            this.ShowReportAction.Tag = null;
            this.ShowReportAction.TargetObjectsCriteria = null;
            this.ShowReportAction.TargetViewId = null;
            this.ShowReportAction.ToolTip = "Show report";
            this.ShowReportAction.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ShowReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ShowReportAction_Execute);
            // 
            // ClearReportFilterAction
            // 
            this.ClearReportFilterAction.Caption = "Clear filter";
            this.ClearReportFilterAction.Category = "ShowReport";
            this.ClearReportFilterAction.ConfirmationMessage = null;
            this.ClearReportFilterAction.Id = "ClearReportFilterAction";
            this.ClearReportFilterAction.ImageName = null;
            this.ClearReportFilterAction.Shortcut = null;
            this.ClearReportFilterAction.Tag = null;
            this.ClearReportFilterAction.TargetObjectsCriteria = null;
            this.ClearReportFilterAction.TargetViewId = null;
            this.ClearReportFilterAction.ToolTip = "Clear report filter";
            this.ClearReportFilterAction.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ClearReportFilterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClearReportFilterAction_Execute);
            // 
            // ShowReportController
            // 
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ShowReportAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ClearReportFilterAction;
    }
}
