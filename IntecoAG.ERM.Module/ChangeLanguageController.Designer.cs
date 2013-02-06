namespace IntecoAG.ERM.Module {
    partial class ChangeLanguageController {
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
            this.ChooseLanguage = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ChooseFormattingCulture = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ChooseLanguage
            // 
            this.ChooseLanguage.Caption = "Choose Language";
            this.ChooseLanguage.Category = "Tools";
            this.ChooseLanguage.ConfirmationMessage = null;
            this.ChooseLanguage.Id = "ChooseLanguage";
            this.ChooseLanguage.ImageName = null;
            this.ChooseLanguage.Shortcut = null;
            this.ChooseLanguage.Tag = null;
            this.ChooseLanguage.TargetObjectsCriteria = null;
            this.ChooseLanguage.TargetViewId = null;
            this.ChooseLanguage.ToolTip = null;
            this.ChooseLanguage.TypeOfView = null;
            // 
            // ChooseFormattingCulture
            // 
            this.ChooseFormattingCulture.Caption = " Choose Formatting Culture";
            this.ChooseFormattingCulture.ConfirmationMessage = null;
            this.ChooseFormattingCulture.Id = "ChooseFormattingCulture";
            this.ChooseFormattingCulture.ImageName = null;
            this.ChooseFormattingCulture.Shortcut = null;
            this.ChooseFormattingCulture.Tag = null;
            this.ChooseFormattingCulture.TargetObjectsCriteria = null;
            this.ChooseFormattingCulture.TargetViewId = null;
            this.ChooseFormattingCulture.ToolTip = null;
            this.ChooseFormattingCulture.TypeOfView = null;
            // 
            // ChangeLanguageController
            // 
            this.TargetWindowType = DevExpress.ExpressApp.WindowType.Main;

        }

        #endregion

        public DevExpress.ExpressApp.Actions.SingleChoiceAction ChooseLanguage;
        public DevExpress.ExpressApp.Actions.SingleChoiceAction ChooseFormattingCulture;

    }
}
