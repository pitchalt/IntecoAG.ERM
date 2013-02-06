namespace IntecoAG.ERM.Module {
    partial class ListViewFilterPanelController {
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
            this.ListViewApplyFilter = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ListViewClearFilter = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FilteringCriterionListAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ListViewApplyFilter
            // 
            this.ListViewApplyFilter.Caption = "Apply Filter";
            this.ListViewApplyFilter.Category = "FullTextSearch";
            this.ListViewApplyFilter.ConfirmationMessage = null;
            this.ListViewApplyFilter.Id = "ListViewApplyFilter";
            this.ListViewApplyFilter.ImageName = "dialog-apply_16x16.png";
            this.ListViewApplyFilter.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ListViewApplyFilter.Shortcut = null;
            this.ListViewApplyFilter.Tag = null;
            this.ListViewApplyFilter.TargetObjectsCriteria = null;
            this.ListViewApplyFilter.TargetViewId = null;
            this.ListViewApplyFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ListViewApplyFilter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ListViewApplyFilter.ToolTip = "Apply Filter";
            this.ListViewApplyFilter.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ListViewApplyFilter.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ListViewApplyFilter_Execute);
            // 
            // ListViewClearFilter
            // 
            this.ListViewClearFilter.Caption = "Clear Filter";
            this.ListViewClearFilter.Category = "FullTextSearch";
            this.ListViewClearFilter.ConfirmationMessage = null;
            this.ListViewClearFilter.Id = "ListViewClearFilter";
            this.ListViewClearFilter.ImageName = "edit-clear_16x16.png";
            this.ListViewClearFilter.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ListViewClearFilter.Shortcut = null;
            this.ListViewClearFilter.Tag = null;
            this.ListViewClearFilter.TargetObjectsCriteria = null;
            this.ListViewClearFilter.TargetViewId = null;
            this.ListViewClearFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.ListViewClearFilter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ListViewClearFilter.ToolTip = "Clear Filter";
            this.ListViewClearFilter.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ListViewClearFilter.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ListViewClearFilter_Execute);
            // 
            // FilteringCriterionListAction
            // 
            this.FilteringCriterionListAction.Caption = "Filtering Criterion";
            this.FilteringCriterionListAction.Category = "FullTextSearch";
            this.FilteringCriterionListAction.ConfirmationMessage = null;
            this.FilteringCriterionListAction.DefaultItemMode = DevExpress.ExpressApp.Actions.DefaultItemMode.LastExecutedItem;
            this.FilteringCriterionListAction.Id = "FilteringCriterionListAction";
            this.FilteringCriterionListAction.ImageMode = DevExpress.ExpressApp.Actions.ImageMode.UseItemImage;
            this.FilteringCriterionListAction.ImageName = "kappfinder_16x16.png";
            this.FilteringCriterionListAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.FilteringCriterionListAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.FilteringCriterionListAction.Shortcut = null;
            this.FilteringCriterionListAction.Tag = null;
            this.FilteringCriterionListAction.TargetObjectsCriteria = null;
            this.FilteringCriterionListAction.TargetViewId = null;
            this.FilteringCriterionListAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.FilteringCriterionListAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.FilteringCriterionListAction.ToolTip = "Поиск";
            this.FilteringCriterionListAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.FilteringCriterionListAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionListAction_Execute);
            // 
            // ListViewFilterPanelController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.ListViewFilterPanelController_Activated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ListViewApplyFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction ListViewClearFilter;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction FilteringCriterionListAction;
    }
}
