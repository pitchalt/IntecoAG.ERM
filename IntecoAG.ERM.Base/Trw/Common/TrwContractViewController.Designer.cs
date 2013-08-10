namespace IntecoAG.ERM.Module.Trw.Common {
    partial class TrwContractViewController {
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
            this.SendToTrwAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SendToTrwAction
            // 
            this.SendToTrwAction.Caption = "f54f6964-164f-49cf-acc1-d209eb1c899d";
            this.SendToTrwAction.ConfirmationMessage = null;
            this.SendToTrwAction.Id = "f54f6964-164f-49cf-acc1-d209eb1c899d";
            this.SendToTrwAction.ImageName = null;
            this.SendToTrwAction.Shortcut = null;
            this.SendToTrwAction.Tag = null;
            this.SendToTrwAction.TargetObjectsCriteria = null;
            this.SendToTrwAction.TargetViewId = null;
            this.SendToTrwAction.ToolTip = null;
            this.SendToTrwAction.TypeOfView = null;
            this.SendToTrwAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SendToTrwAction_Execute);
            // 
            // TrwContractViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.CRM.Contract.Deal.crmContractDeal);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SendToTrwAction;
    }
}
