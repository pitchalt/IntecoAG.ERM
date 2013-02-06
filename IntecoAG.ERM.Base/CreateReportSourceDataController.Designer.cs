namespace IntecoAG.ERM.Module
{
    partial class CreateReportSourceDataController
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
            this.FillSourceData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FillSourceData
            // 
            this.FillSourceData.Caption = "Заполнить источник данных";
            this.FillSourceData.Category = "View";
            this.FillSourceData.ConfirmationMessage = null;
            this.FillSourceData.Id = "FillSourceData";
            this.FillSourceData.ImageName = null;
            this.FillSourceData.Shortcut = null;
            this.FillSourceData.Tag = null;
            this.FillSourceData.TargetObjectsCriteria = null;
            this.FillSourceData.TargetObjectType = typeof(DevExpress.ExpressApp.Reports.ReportData);
            this.FillSourceData.TargetViewId = null;
            this.FillSourceData.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.FillSourceData.ToolTip = null;
            this.FillSourceData.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.FillSourceData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FillSourceData_Execute);
            // 
            // CreateReportSourceDataController
            // 
            this.TargetObjectType = typeof(DevExpress.ExpressApp.Reports.ReportData);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FillSourceData;
    }
}
