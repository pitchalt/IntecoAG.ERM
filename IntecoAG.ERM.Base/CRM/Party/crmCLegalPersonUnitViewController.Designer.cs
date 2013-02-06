namespace IntecoAG.ERM.CRM.Party {
    partial class crmCLegalPersonUnitViewController
    {
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
            this.SetChecked = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetChecked
            // 
            this.SetChecked.Caption = "Set checked";
            this.SetChecked.Category = "View";
            this.SetChecked.ConfirmationMessage = null;
            this.SetChecked.Id = "crmCLegalPersonUnitViewController_SetChecked";
            this.SetChecked.ImageName = null;
            this.SetChecked.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SetChecked.Shortcut = null;
            this.SetChecked.Tag = null;
            this.SetChecked.TargetObjectsCriteria = null;
            this.SetChecked.TargetObjectType = typeof(IntecoAG.ERM.CRM.Party.crmCLegalPersonUnit);
            this.SetChecked.TargetViewId = null;
            this.SetChecked.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.SetChecked.ToolTip = "Set checked";
            this.SetChecked.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.SetChecked.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SetChecked_Execute);
            // 
            // crmCLegalPersonUnitViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.CRM.Party.crmCLegalPersonUnit);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SetChecked;

    }
}
