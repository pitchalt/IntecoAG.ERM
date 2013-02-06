namespace IntecoAG.ERM.CS.Security
{
    partial class csCSecurityUserListController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.UpdateListAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // UpdateListAction
            // 
            this.UpdateListAction.Caption = "Sync With AD";
            this.UpdateListAction.Category = "RecordEdit";
            this.UpdateListAction.ConfirmationMessage = null;
            this.UpdateListAction.Id = "csCSecurityUserListController_UpdateListAction";
            this.UpdateListAction.ImageName = null;
            this.UpdateListAction.Shortcut = null;
            this.UpdateListAction.Tag = null;
            this.UpdateListAction.TargetObjectsCriteria = null;
            this.UpdateListAction.TargetObjectType = typeof(IntecoAG.ERM.CS.Security.csCSecurityUser);
            this.UpdateListAction.TargetViewId = null;
            this.UpdateListAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.UpdateListAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.UpdateListAction.ToolTip = "Sync User With AD Group";
            this.UpdateListAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.UpdateListAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateListAction_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction UpdateListAction;
    }
}
