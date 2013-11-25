namespace IntecoAG.ERM.Trw.Subject {
    partial class TrwSubjectVC {
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
            this.ImportSaleDeals_Action = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            // 
            // ImportSaleDeals_Action
            // 
            this.ImportSaleDeals_Action.Caption = "ImportSaleDeals";
            this.ImportSaleDeals_Action.Category = "RecordEdit";
            this.ImportSaleDeals_Action.ConfirmationMessage = null;
            this.ImportSaleDeals_Action.Id = "TrwSubjectVC_ImportSaleDeals_Action";
            this.ImportSaleDeals_Action.ImageName = null;
            this.ImportSaleDeals_Action.ShortCaption = null;
            this.ImportSaleDeals_Action.Shortcut = null;
            this.ImportSaleDeals_Action.Tag = null;
            this.ImportSaleDeals_Action.TargetObjectsCriteria = null;
            this.ImportSaleDeals_Action.TargetViewId = null;
            this.ImportSaleDeals_Action.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ImportSaleDeals_Action.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportSaleDeals_Action.ToolTip = null;
            this.ImportSaleDeals_Action.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportSaleDeals_Action.Execute += new DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventHandler(this.ImportSaleDeals_Action_Execute);
            // 
            // TrwSubjectVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.Trw.Subject.TrwSubject);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.ParametrizedAction ImportSaleDeals_Action;
    }
}
