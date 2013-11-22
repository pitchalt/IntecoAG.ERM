namespace IntecoAG.ERM.Trw.Party {
    partial class TrwPartyPartyVC {
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
            this.GetNumberAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GetNumberAction
            // 
            this.GetNumberAction.Caption = "GetNumber";
            this.GetNumberAction.ConfirmationMessage = null;
            this.GetNumberAction.Id = "TrwPartyPartyVC_GetNumberAction";
            this.GetNumberAction.ImageName = null;
            this.GetNumberAction.Shortcut = null;
            this.GetNumberAction.Tag = null;
            this.GetNumberAction.TargetObjectsCriteria = null;
            this.GetNumberAction.TargetViewId = null;
            this.GetNumberAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.GetNumberAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.GetNumberAction.ToolTip = null;
            this.GetNumberAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.GetNumberAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GetNumberAction_Execute);
            // 
            // TrwPartyPartyVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.Trw.Party.TrwPartyParty);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction GetNumberAction;
    }
}
