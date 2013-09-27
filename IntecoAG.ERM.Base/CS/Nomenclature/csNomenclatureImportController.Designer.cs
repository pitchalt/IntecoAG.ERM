namespace IntecoAG.ERM.CS.Nomenclature {
    partial class csNomenclatureImportController {
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
            this.ImportMaterialAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportMaterialAction
            // 
            this.ImportMaterialAction.Caption = "Import";
            this.ImportMaterialAction.ConfirmationMessage = null;
            this.ImportMaterialAction.Id = "csNomenclatureImportController_ImportMaterialAction";
            this.ImportMaterialAction.ImageName = null;
            this.ImportMaterialAction.Shortcut = null;
            this.ImportMaterialAction.Tag = null;
            this.ImportMaterialAction.TargetObjectsCriteria = null;
            this.ImportMaterialAction.TargetObjectType = typeof(IntecoAG.ERM.CS.Nomenclature.csMaterial);
            this.ImportMaterialAction.TargetViewId = null;
            this.ImportMaterialAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ImportMaterialAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportMaterialAction.ToolTip = null;
            this.ImportMaterialAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportMaterialAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportMaterialAction_Execute);
            // 
            // csNomenclatureImportController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.CS.Nomenclature.csNomenclature);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportMaterialAction;
    }
}
