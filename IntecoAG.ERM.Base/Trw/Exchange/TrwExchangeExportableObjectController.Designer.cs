namespace IntecoAG.ERM.Trw.Exchange {
    partial class TrwExchangeExportableObjectController<T> {
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
            this.SendAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SendAction
            // 
            this.SendAction.Caption = "Send";
            this.SendAction.ConfirmationMessage = null;
            this.SendAction.Id = "TrwExchangeExportabledObjectController_SendAction";
            this.SendAction.ImageName = null;
            this.SendAction.Shortcut = null;
            this.SendAction.Tag = null;
            this.SendAction.TargetObjectsCriteria = "TrwExportState == 'PREPARED'";
            this.SendAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.SendAction.TargetObjectType = typeof(IntecoAG.ERM.Trw.Exchange.TrwExchangeIExportableObject);
            this.SendAction.TargetViewId = null;
            this.SendAction.ToolTip = null;
            this.SendAction.TypeOfView = null;
            this.SendAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SendAction_Execute);

        }

        #endregion

        protected DevExpress.ExpressApp.Actions.SimpleAction SendAction;

    }
}
