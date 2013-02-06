namespace IntecoAG.ERM.FM.Controllers {
    partial class fmCDocRCBPaymentOrdeEditorViewController {
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
            this.PayerEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReceiverEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PaymentFieldEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PayerEdit
            // 
            this.PayerEdit.Caption = "Payer Edit";
            this.PayerEdit.Category = "CashlessPaymentOrderPayerEditor";
            this.PayerEdit.ConfirmationMessage = null;
            this.PayerEdit.Id = "PayerEdit";
            this.PayerEdit.ImageName = null;
            this.PayerEdit.Shortcut = null;
            this.PayerEdit.Tag = null;
            this.PayerEdit.TargetObjectsCriteria = null;
            this.PayerEdit.TargetViewId = null;
            this.PayerEdit.ToolTip = null;
            this.PayerEdit.TypeOfView = null;
            //this.PayerEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PayerEditor_Execute);
            // 
            // ReceiverEdit
            // 
            this.ReceiverEdit.Caption = "Receiver Edit";
            this.ReceiverEdit.Category = "CashlessPaymentOrderReceiverEditor";
            this.ReceiverEdit.ConfirmationMessage = null;
            this.ReceiverEdit.Id = "ReceiverEdit";
            this.ReceiverEdit.ImageName = null;
            this.ReceiverEdit.Shortcut = null;
            this.ReceiverEdit.Tag = null;
            this.ReceiverEdit.TargetObjectsCriteria = null;
            this.ReceiverEdit.TargetViewId = null;
            this.ReceiverEdit.ToolTip = null;
            this.ReceiverEdit.TypeOfView = null;
            //this.ReceiverEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReceiverEditor_Execute);
            // 
            // PaymentFieldEdit
            // 
            this.PaymentFieldEdit.Caption = "Payment Field Edit";
            this.PaymentFieldEdit.Category = "DocRCBPaymentActions";
            this.PaymentFieldEdit.ConfirmationMessage = null;
            this.PaymentFieldEdit.Id = "PaymentFieldEdit";
            this.PaymentFieldEdit.Shortcut = null;
            this.PaymentFieldEdit.Tag = null;
            this.PaymentFieldEdit.TargetObjectsCriteria = null;
            this.PaymentFieldEdit.TargetViewId = null;
            this.PaymentFieldEdit.ToolTip = null;
            this.PaymentFieldEdit.TypeOfView = null;
            this.PaymentFieldEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PaymentFieldEdit_Execute);
            // 
            // fmCDocRCBPaymentOrdeEditorViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.Docs.fmCDocRCB);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PayerEdit;
        private DevExpress.ExpressApp.Actions.SimpleAction ReceiverEdit;
        private DevExpress.ExpressApp.Actions.SimpleAction PaymentFieldEdit;
    }
}
