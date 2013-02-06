namespace IntecoAG.ERM.CRM.Party {
    partial class PartySyncController {
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
            this.PartySyncAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PartySyncAction
            // 
            this.PartySyncAction.Caption = "Party Sync Action";
            this.PartySyncAction.Category = "RecordEdit";
            this.PartySyncAction.ConfirmationMessage = "Is you want sync";
            this.PartySyncAction.Id = "PartySyncAction";
            this.PartySyncAction.ImageName = null;
            this.PartySyncAction.Shortcut = null;
            this.PartySyncAction.Tag = null;
            this.PartySyncAction.TargetObjectsCriteria = null;
            this.PartySyncAction.TargetViewId = null;
            this.PartySyncAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.PartySyncAction.ToolTip = null;
            this.PartySyncAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.PartySyncAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PartySyncAction_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PartySyncAction;
    }
}
