namespace IntecoAG.ERM.Module {
    partial class MiniNavigationController {
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
            this.MiniNavigationAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // MiniNavigationAction
            // 
            this.MiniNavigationAction.Caption = "Mini Navigation Action";
            this.MiniNavigationAction.Category = "View";
            this.MiniNavigationAction.ConfirmationMessage = null;
            this.MiniNavigationAction.Id = "MiniNavigationAction";
            this.MiniNavigationAction.ImageName = "starthere_16x16.png";
            this.MiniNavigationAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.MiniNavigationAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MiniNavigationAction.Shortcut = null;
            this.MiniNavigationAction.Tag = null;
            this.MiniNavigationAction.TargetObjectsCriteria = null;
            this.MiniNavigationAction.TargetViewId = null;
            this.MiniNavigationAction.ToolTip = null;
            this.MiniNavigationAction.TypeOfView = null;
            this.MiniNavigationAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MiniNavigationAction_Execute);
            // 
            // MiniNavigationController
            // 
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction MiniNavigationAction;
    }
}
