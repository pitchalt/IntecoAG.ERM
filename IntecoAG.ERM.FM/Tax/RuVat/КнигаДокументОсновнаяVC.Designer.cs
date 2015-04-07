namespace IntecoAG.ERM.FM.Tax.RuVat {
    partial class КнигаДокументОсновнаяVC {
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
            this.UnloadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // UnloadAction
            // 
            this.UnloadAction.Caption = "Выгрузить";
            this.UnloadAction.Category = "RecordEdit";
            this.UnloadAction.ConfirmationMessage = null;
            this.UnloadAction.Id = "КнигаДокументОсновная_UnloadAction";
            this.UnloadAction.TargetObjectType = typeof(IntecoAG.ERM.FM.Tax.RuVat.КнигаДокументОсновная);
            this.UnloadAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.UnloadAction.ToolTip = null;
            this.UnloadAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.UnloadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.КнигаДокументОсновная_Сериализовать_Execute);
            // 
            // КнигаДокументОсновнаяVC
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.FM.Tax.RuVat.КнигаДокументОсновная);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction UnloadAction;
    }
}
