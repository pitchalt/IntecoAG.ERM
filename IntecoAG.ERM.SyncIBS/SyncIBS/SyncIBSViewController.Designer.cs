namespace IntecoAG.ERM.SyncIBS {
    partial class SyncIBSViewController {
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
            this.SyncIBSAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SyncIBSAction
            // 
            this.SyncIBSAction.Caption = "SyncIBS";
            this.SyncIBSAction.Category = "Edit";
            this.SyncIBSAction.ConfirmationMessage = null;
            this.SyncIBSAction.Id = "fmFJSaleDocViewController_SyncIBSAction";
            this.SyncIBSAction.ImageName = null;
            this.SyncIBSAction.Shortcut = null;
            this.SyncIBSAction.Tag = null;
            this.SyncIBSAction.TargetObjectsCriteria = null;
            this.SyncIBSAction.TargetObjectType = typeof(IntecoAG.ERM.FM.FinJurnal.fmCFJSaleDoc);
            this.SyncIBSAction.TargetViewId = null;
            this.SyncIBSAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.SyncIBSAction.ToolTip = null;
            this.SyncIBSAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.SyncIBSAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SyncIBSAction_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SyncIBSAction;
    }
}
