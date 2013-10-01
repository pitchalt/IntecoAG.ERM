namespace IntecoAG.ERM.XAFExt {
    partial class XafExtAnalysisFilterPanelController {
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
            this.DetailViewAnalysisApplyFilter = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DetailViewAnalysisClearFilter = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AnalysisCriterionListAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ClearFilterFields = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DetailViewAnalysisApplyFilter
            // 
            this.DetailViewAnalysisApplyFilter.Caption = "Apply Filter";
            this.DetailViewAnalysisApplyFilter.Category = "RecordEdit";
            this.DetailViewAnalysisApplyFilter.ConfirmationMessage = null;
            this.DetailViewAnalysisApplyFilter.Id = "DetailViewAnalysisApplyFilter";
            this.DetailViewAnalysisApplyFilter.ImageName = "dialog-apply_16x16.png";
            this.DetailViewAnalysisApplyFilter.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.DetailViewAnalysisApplyFilter.Shortcut = null;
            this.DetailViewAnalysisApplyFilter.Tag = null;
            this.DetailViewAnalysisApplyFilter.TargetObjectsCriteria = null;
            this.DetailViewAnalysisApplyFilter.TargetObjectType = typeof(IntecoAG.ERM.XAFExt.XafExtAnalysis);
            this.DetailViewAnalysisApplyFilter.TargetViewId = null;
            this.DetailViewAnalysisApplyFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.DetailViewAnalysisApplyFilter.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.DetailViewAnalysisApplyFilter.ToolTip = "Apply Filter";
            this.DetailViewAnalysisApplyFilter.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.DetailViewAnalysisApplyFilter.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApplyAnalysisFilter_Execute);
            // 
            // DetailViewAnalysisClearFilter
            // 
            this.DetailViewAnalysisClearFilter.Caption = "Clear Filter";
            this.DetailViewAnalysisClearFilter.Category = "RecordEdit";
            this.DetailViewAnalysisClearFilter.ConfirmationMessage = null;
            this.DetailViewAnalysisClearFilter.Id = "DetailViewAnalysisClearFilter";
            this.DetailViewAnalysisClearFilter.ImageName = "edit-clear_16x16.png";
            this.DetailViewAnalysisClearFilter.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.DetailViewAnalysisClearFilter.Shortcut = null;
            this.DetailViewAnalysisClearFilter.Tag = null;
            this.DetailViewAnalysisClearFilter.TargetObjectsCriteria = null;
            this.DetailViewAnalysisClearFilter.TargetObjectType = typeof(IntecoAG.ERM.XAFExt.XafExtAnalysis);
            this.DetailViewAnalysisClearFilter.TargetViewId = null;
            this.DetailViewAnalysisClearFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.DetailViewAnalysisClearFilter.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.DetailViewAnalysisClearFilter.ToolTip = "Clear Filter";
            this.DetailViewAnalysisClearFilter.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.DetailViewAnalysisClearFilter.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClearAnalusisFilter_Execute);
            // 
            // AnalysisCriterionListAction
            // 
            this.AnalysisCriterionListAction.Caption = "Filtering Criterion";
            this.AnalysisCriterionListAction.Category = "RecordEdit";
            this.AnalysisCriterionListAction.ConfirmationMessage = null;
            this.AnalysisCriterionListAction.DefaultItemMode = DevExpress.ExpressApp.Actions.DefaultItemMode.LastExecutedItem;
            this.AnalysisCriterionListAction.Id = "AnalysisCriterionListAction";
            this.AnalysisCriterionListAction.ImageMode = DevExpress.ExpressApp.Actions.ImageMode.UseItemImage;
            this.AnalysisCriterionListAction.ImageName = "kappfinder_16x16.png";
            this.AnalysisCriterionListAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.AnalysisCriterionListAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.AnalysisCriterionListAction.Shortcut = null;
            this.AnalysisCriterionListAction.Tag = null;
            this.AnalysisCriterionListAction.TargetObjectsCriteria = null;
            this.AnalysisCriterionListAction.TargetObjectType = typeof(IntecoAG.ERM.XAFExt.XafExtAnalysis);
            this.AnalysisCriterionListAction.TargetViewId = null;
            this.AnalysisCriterionListAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.AnalysisCriterionListAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AnalysisCriterionListAction.ToolTip = "Поиск";
            this.AnalysisCriterionListAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AnalysisCriterionListAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionListAction_Execute);
            // 
            // ClearFilterFields
            // 
            this.ClearFilterFields.Caption = "Clear Filter Fields";
            this.ClearFilterFields.Category = "ClearFilterFields";
            this.ClearFilterFields.ConfirmationMessage = null;
            this.ClearFilterFields.Id = "ClearFilterFields";
            this.ClearFilterFields.ImageName = "edit-clear_16x16.png";
            this.ClearFilterFields.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ClearFilterFields.Shortcut = null;
            this.ClearFilterFields.Tag = null;
            this.ClearFilterFields.TargetObjectsCriteria = null;
            this.ClearFilterFields.TargetViewId = null;
            this.ClearFilterFields.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ClearFilterFields.ToolTip = "Clear Filter Fields";
            this.ClearFilterFields.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ClearFilterFields.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClearFilterFields_Execute);
            // 
            // XafExtAnalysisFilterPanelController
            // 
            this.TargetObjectType = typeof(IntecoAG.ERM.XAFExt.XafExtAnalysis);
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.Activated += new System.EventHandler(this.DetailViewAnalysisFilterPanelController_Activated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction DetailViewAnalysisApplyFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction DetailViewAnalysisClearFilter;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction AnalysisCriterionListAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ClearFilterFields;
    }
}
