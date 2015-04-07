namespace IntecoAG.ERM.FM.Tax.RuVat {
    partial class TestDataGenerator {
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
            this.GenerateTestDataAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GenerateTestDataAction
            // 
            this.GenerateTestDataAction.Caption = "Генерировать тестовые  данные";
            this.GenerateTestDataAction.Category = "Tools";
            this.GenerateTestDataAction.ConfirmationMessage = null;
            this.GenerateTestDataAction.Id = "TestDataGenerator_GenerateTestDataAction";
            this.GenerateTestDataAction.ToolTip = null;
            this.GenerateTestDataAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GenerateTestDataAction_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction GenerateTestDataAction;
    }
}
