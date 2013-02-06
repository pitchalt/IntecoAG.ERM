namespace IntecoAG.ERM.CS.Nomenclature {
    partial class csNMCourseEditorViewController {
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
            this.CourseDateAction = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            // 
            // CourseDateAction
            // 
            this.CourseDateAction.Caption = "Course Date";
            this.CourseDateAction.Category = "fmCPRCourseEditorViewController_CourceDate";
            this.CourseDateAction.ConfirmationMessage = null;
            this.CourseDateAction.Id = "fmCPRCourseEditorViewController_CourseDateAction";
            this.CourseDateAction.ImageName = null;
            this.CourseDateAction.ShortCaption = null;
            this.CourseDateAction.Shortcut = null;
            this.CourseDateAction.Tag = null;
            this.CourseDateAction.TargetObjectsCriteria = null;
            this.CourseDateAction.TargetViewId = null;
            this.CourseDateAction.ToolTip = null;
            this.CourseDateAction.TypeOfView = null;
            this.CourseDateAction.ValueType = typeof(System.DateTime);
            this.CourseDateAction.Execute += new DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventHandler(this.CourseDateAction_Execute);
            // 
            // fmCPRCourseEditorViewController
            // 
            this.TargetObjectType = typeof(csCNMCourseEditor);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.ParametrizedAction CourseDateAction;

    }
}
