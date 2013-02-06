namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCDocRCBManualMappingViewController {
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
            this.ManualMapDocuments = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ManualMapDocuments
            // 
            this.ManualMapDocuments.Caption = "Manual Map Documents";
            this.ManualMapDocuments.Category = "PaymentDocumentsMapping";
            this.ManualMapDocuments.ConfirmationMessage = null;
            this.ManualMapDocuments.Id = "ManualMapDocuments";
            this.ManualMapDocuments.ImageName = null;
            this.ManualMapDocuments.Shortcut = null;
            this.ManualMapDocuments.Tag = null;
            this.ManualMapDocuments.TargetObjectsCriteria = null;
            this.ManualMapDocuments.TargetViewId = null;
            this.ManualMapDocuments.ToolTip = null;
            this.ManualMapDocuments.TypeOfView = null;
            this.ManualMapDocuments.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MapDocuments_Execute);
            // 
            // fmCDocRCBManualMappingViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.PaymentRequest.fmCPRRepaymentJurnal);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ManualMapDocuments;
    }
}
