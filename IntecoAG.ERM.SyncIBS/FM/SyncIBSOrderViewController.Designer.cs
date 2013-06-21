namespace IntecoAG.ERM.SyncIBS.FM {
    partial class SyncIBSOrderViewController {
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
            this.SyncOrderListAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SyncOrderListAction
            // 
            this.SyncOrderListAction.Caption = null;
            this.SyncOrderListAction.ConfirmationMessage = null;
            this.SyncOrderListAction.Id = "d67412b9-482f-4bb9-9687-77f16483e22a";
            this.SyncOrderListAction.ImageName = null;
            this.SyncOrderListAction.Shortcut = null;
            this.SyncOrderListAction.Tag = null;
            this.SyncOrderListAction.TargetObjectsCriteria = null;
            this.SyncOrderListAction.TargetViewId = null;
            this.SyncOrderListAction.ToolTip = null;
            this.SyncOrderListAction.TypeOfView = null;
            this.SyncOrderListAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SyncOrderListAction_Execute);
            // 
            // SyncIBSOrderViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.Order.fmCOrderExt);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SyncOrderListAction;
    }
}
