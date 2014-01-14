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
            this.ImportSaleDealsAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ImportBayDealsAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ImportSaleDealsAction
            // 
            this.ImportSaleDealsAction.AcceptButtonCaption = null;
            this.ImportSaleDealsAction.CancelButtonCaption = null;
            this.ImportSaleDealsAction.Caption = "ImportSaleDeals";
            this.ImportSaleDealsAction.Category = "Tools";
            this.ImportSaleDealsAction.ConfirmationMessage = null;
            this.ImportSaleDealsAction.Id = "TrwSubjectVC_ImportSaleDeals_Action";
            this.ImportSaleDealsAction.ImageName = null;
            this.ImportSaleDealsAction.Shortcut = null;
            this.ImportSaleDealsAction.Tag = null;
            this.ImportSaleDealsAction.TargetObjectsCriteria = "Status == \'TRW_SUBJECT_CONF_SUBJECT_LIST\'";
            this.ImportSaleDealsAction.TargetViewId = null;
            this.ImportSaleDealsAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ImportSaleDealsAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportSaleDealsAction.ToolTip = null;
            this.ImportSaleDealsAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportSaleDealsAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportSaleDealsAction_CustomizePopupWindowParams);
            this.ImportSaleDealsAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportSaleDealsAction_Execute);
            // 
            // ImportBayDealsAction
            // 
            this.ImportBayDealsAction.AcceptButtonCaption = null;
            this.ImportBayDealsAction.CancelButtonCaption = null;
            this.ImportBayDealsAction.Caption = "ImportBayDeals";
            this.ImportBayDealsAction.Category = "Tools";
            this.ImportBayDealsAction.ConfirmationMessage = null;
            this.ImportBayDealsAction.Id = "TrwSubjectVC_ImportBayDeals_Action";
            this.ImportBayDealsAction.ImageName = null;
            this.ImportBayDealsAction.Shortcut = null;
            this.ImportBayDealsAction.Tag = null;
            this.ImportBayDealsAction.TargetObjectsCriteria = "Status == \'TRW_SUBJECT_CONF_SUBJECT_LIST\'";
            this.ImportBayDealsAction.TargetViewId = null;
            this.ImportBayDealsAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ImportBayDealsAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportBayDealsAction.ToolTip = null;
            this.ImportBayDealsAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportBayDealsAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportBayDealsAction_CustomizePopupWindowParams);
            this.ImportBayDealsAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportBayDealsAction_Execute);
            // 
            // TrwSubjectVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.Trw.Subject.TrwSubject);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportSaleDealsAction;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportBayDealsAction;
    }
}
