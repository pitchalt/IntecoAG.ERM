namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCDocRCBCreatePaymentDocsViewController {
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
            this.CreatePaymentDocuments = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreatePaymentDocuments
            // 
            this.CreatePaymentDocuments.Caption = "Create Payment Documents";
            this.CreatePaymentDocuments.Category = "View";
            this.CreatePaymentDocuments.ConfirmationMessage = null;
            this.CreatePaymentDocuments.Id = "CreatePaymentDocuments";
            this.CreatePaymentDocuments.ImageName = null;
            this.CreatePaymentDocuments.Shortcut = null;
            this.CreatePaymentDocuments.Tag = null;
            this.CreatePaymentDocuments.TargetObjectsCriteria = null;
            this.CreatePaymentDocuments.TargetViewId = null;
            this.CreatePaymentDocuments.ToolTip = null;
            this.CreatePaymentDocuments.TypeOfView = null;
            this.CreatePaymentDocuments.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreatePaymentDocuments_Execute);
            // 
            // fmCDocRCBCreatePaymentDocsViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.StatementAccount.fmCSAStatementAccount);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreatePaymentDocuments;
    }
}
