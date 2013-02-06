using DevExpress.ExpressApp.Win.Templates;
namespace DevExpress.ExpressApp.Win.Templates {
    partial class IagErmDetailForm {
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
                this.mainBarManager = null;
                this.barDockControlBottom = null;
                this.barDockControlLeft = null;
                this.barDockControlRight = null;
                this.barDockControlTop = null;
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IagErmDetailForm));
            this.mainBarManager = new DevExpress.ExpressApp.Win.Templates.Controls.XafBarManager(this.components);
            this._mainMenuBar = new DevExpress.ExpressApp.Win.Templates.Controls.XafBar();
            this.barSubItemFile = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cObjectsCreation = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cFile = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cClose = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cSave = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cExport = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cPrint = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.barSubItemEdit = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cUndoRedo = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cEdit = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cRecordEdit = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cWorkflow = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cOpenObject = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.barSubItemView = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cRecordsNavigation = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cView = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cReports = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cSearch = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cFullTextSearch = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cFilters = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.barSubItemTools = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cTools = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.cOptions = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.cDiagnostic = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.barSubItemHelp = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cAbout = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.standardToolBar = new DevExpress.ExpressApp.Win.Templates.Controls.XafBar();
            this._statusBar = new DevExpress.ExpressApp.Win.Templates.Controls.XafBar();
            this.cBusinessLogic = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.cMenu = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.viewSitePanel = new DevExpress.XtraEditors.PanelControl();
            this.formStateModelSynchronizerComponent = new DevExpress.ExpressApp.Win.Core.FormStateModelSynchronizer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSitePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // actionsContainersManager
            // 
            this.actionsContainersManager.ActionContainerComponents.Add(this.cObjectsCreation);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cFile);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cClose);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cSave);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cExport);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cPrint);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cUndoRedo);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cEdit);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cRecordEdit);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cWorkflow);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cOpenObject);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cRecordsNavigation);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cView);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cReports);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cSearch);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cFullTextSearch);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cFilters);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cTools);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cOptions);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cDiagnostic);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cAbout);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cMenu);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cBusinessLogic);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cObjectsCreation);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cSave);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cEdit);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cOpenObject);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cUndoRedo);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cReports);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cClose);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cRecordEdit);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cView);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cPrint);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cExport);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cMenu);
            this.actionsContainersManager.DefaultContainer = this.cView;
            // 
            // modelSynchronizationManager
            // 
            this.modelSynchronizationManager.ModelSynchronizableComponents.Add(this.formStateModelSynchronizerComponent);
            this.modelSynchronizationManager.ModelSynchronizableComponents.Add(this.mainBarManager);
            // 
            // viewSiteManager
            // 
            this.viewSiteManager.ViewSiteControl = this.viewSitePanel;
            // 
            // mainBarManager
            // 
            this.mainBarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this._mainMenuBar,
            this.standardToolBar,
            this._statusBar});
            this.mainBarManager.DockControls.Add(this.barDockControlTop);
            this.mainBarManager.DockControls.Add(this.barDockControlBottom);
            this.mainBarManager.DockControls.Add(this.barDockControlLeft);
            this.mainBarManager.DockControls.Add(this.barDockControlRight);
            this.mainBarManager.Form = this;
            this.mainBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItemFile,
            this.barSubItemEdit,
            this.barSubItemView,
            this.barSubItemTools,
            this.barSubItemHelp,
            this.cFile,
            this.cObjectsCreation,
            this.cClose,
            this.cSave,
            this.cEdit,
            this.cOpenObject,
            this.cUndoRedo,
            this.cReports,
            this.cPrint,
            this.cExport,
            this.cMenu,
            this.cRecordEdit,
            this.cWorkflow,
            this.cRecordsNavigation,
            this.cSearch,
            this.cFullTextSearch,
            this.cFilters,
            this.cView,
            this.cTools,
            this.cOptions,
            this.cDiagnostic,
            this.cAbout,
            this.cBusinessLogic});
            this.mainBarManager.MainMenu = this._mainMenuBar;
            this.mainBarManager.MaxItemId = 24;
            this.mainBarManager.StatusBar = this._statusBar;
            // 
            // _mainMenuBar
            // 
            this._mainMenuBar.BarName = "Main Menu";
            this._mainMenuBar.DockCol = 0;
            this._mainMenuBar.DockRow = 0;
            this._mainMenuBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this._mainMenuBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemFile),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemEdit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemView),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemTools),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemHelp)});
            this._mainMenuBar.OptionsBar.MultiLine = true;
            this._mainMenuBar.OptionsBar.UseWholeRow = true;
            this._mainMenuBar.TargetPageCategoryColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this._mainMenuBar, "_mainMenuBar");
            // 
            // barSubItemFile
            // 
            resources.ApplyResources(this.barSubItemFile, "barSubItemFile");
            this.barSubItemFile.Id = 0;
            this.barSubItemFile.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cObjectsCreation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cFile, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cClose, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cExport, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cPrint, true)});
            this.barSubItemFile.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.barSubItemFile.Name = "barSubItemFile";
            this.barSubItemFile.VisibleInRibbon = false;
            // 
            // cObjectsCreation
            // 
            this.cObjectsCreation.ApplicationMenuIndex = 1;
            this.cObjectsCreation.ApplicationMenuItem = true;
            resources.ApplyResources(this.cObjectsCreation, "cObjectsCreation");
            this.cObjectsCreation.ContainerId = "ObjectsCreation";
            this.cObjectsCreation.Id = 17;
            this.cObjectsCreation.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cObjectsCreation.Name = "cObjectsCreation";
            this.cObjectsCreation.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cFile
            // 
            this.cFile.ApplicationMenuIndex = 2;
            this.cFile.ApplicationMenuItem = true;
            resources.ApplyResources(this.cFile, "cFile");
            this.cFile.ContainerId = "File";
            this.cFile.Id = 5;
            this.cFile.MergeOrder = 2;
            this.cFile.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cFile.Name = "cFile";
            this.cFile.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cClose
            // 
            this.cClose.ApplicationMenuIndex = 2;
            this.cClose.ApplicationMenuItem = true;
            resources.ApplyResources(this.cClose, "cClose");
            this.cClose.ContainerId = "Close";
            this.cClose.Id = 18;
            this.cClose.MergeOrder = 2;
            this.cClose.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cClose.Name = "cClose";
            this.cClose.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cSave
            // 
            this.cSave.ApplicationMenuIndex = 7;
            this.cSave.ApplicationMenuItem = true;
            resources.ApplyResources(this.cSave, "cSave");
            this.cSave.ContainerId = "Save";
            this.cSave.Id = 17;
            this.cSave.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cSave.Name = "cSave";
            this.cSave.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cExport
            // 
            this.cExport.ApplicationMenuIndex = 10;
            this.cExport.ApplicationMenuItem = true;
            resources.ApplyResources(this.cExport, "cExport");
            this.cExport.ContainerId = "Export";
            this.cExport.Id = 7;
            this.cExport.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cExport.Name = "cExport";
            this.cExport.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cPrint
            // 
            this.cPrint.ApplicationMenuIndex = 11;
            this.cPrint.ApplicationMenuItem = true;
            resources.ApplyResources(this.cPrint, "cPrint");
            this.cPrint.ContainerId = "Print";
            this.cPrint.Id = 6;
            this.cPrint.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cPrint.Name = "cPrint";
            this.cPrint.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // barSubItemEdit
            // 
            resources.ApplyResources(this.barSubItemEdit, "barSubItemEdit");
            this.barSubItemEdit.Id = 1;
            this.barSubItemEdit.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cUndoRedo, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cEdit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cRecordEdit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cWorkflow, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cOpenObject, true)});
            this.barSubItemEdit.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.barSubItemEdit.Name = "barSubItemEdit";
            this.barSubItemEdit.VisibleInRibbon = false;
            // 
            // cUndoRedo
            // 
            resources.ApplyResources(this.cUndoRedo, "cUndoRedo");
            this.cUndoRedo.ContainerId = "UndoRedo";
            this.cUndoRedo.Id = 19;
            this.cUndoRedo.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cUndoRedo.Name = "cUndoRedo";
            this.cUndoRedo.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cEdit
            // 
            resources.ApplyResources(this.cEdit, "cEdit");
            this.cEdit.ContainerId = "Edit";
            this.cEdit.Id = 18;
            this.cEdit.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cEdit.Name = "cEdit";
            this.cEdit.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cRecordEdit
            // 
            resources.ApplyResources(this.cRecordEdit, "cRecordEdit");
            this.cRecordEdit.ContainerId = "RecordEdit";
            this.cRecordEdit.Id = 9;
            this.cRecordEdit.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cRecordEdit.Name = "cRecordEdit";
            this.cRecordEdit.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cWorkflow
            // 
            resources.ApplyResources(this.cWorkflow, "cWorkflow");
            this.cWorkflow.ContainerId = "Workflow";
            this.cWorkflow.Id = 9;
            this.cWorkflow.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cWorkflow.Name = "cWorkflow";
            this.cWorkflow.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cOpenObject
            // 
            resources.ApplyResources(this.cOpenObject, "cOpenObject");
            this.cOpenObject.ContainerId = "OpenObject";
            this.cOpenObject.Id = 20;
            this.cOpenObject.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cOpenObject.Name = "cOpenObject";
            this.cOpenObject.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // barSubItemView
            // 
            resources.ApplyResources(this.barSubItemView, "barSubItemView");
            this.barSubItemView.Id = 2;
            this.barSubItemView.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cRecordsNavigation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cView, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cReports, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cSearch, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.cFullTextSearch, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.cFilters, true)});
            this.barSubItemView.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.barSubItemView.Name = "barSubItemView";
            // 
            // cRecordsNavigation
            // 
            resources.ApplyResources(this.cRecordsNavigation, "cRecordsNavigation");
            this.cRecordsNavigation.ContainerId = "RecordsNavigation";
            this.cRecordsNavigation.Id = 10;
            this.cRecordsNavigation.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cRecordsNavigation.Name = "cRecordsNavigation";
            this.cRecordsNavigation.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cView
            // 
            resources.ApplyResources(this.cView, "cView");
            this.cView.ContainerId = "View";
            this.cView.Id = 12;
            this.cView.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cView.Name = "cView";
            this.cView.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cReports
            // 
            resources.ApplyResources(this.cReports, "cReports");
            this.cReports.ContainerId = "Reports";
            this.cReports.Id = 20;
            this.cReports.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cReports.Name = "cReports";
            this.cReports.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cSearch
            // 
            resources.ApplyResources(this.cSearch, "cSearch");
            this.cSearch.ContainerId = "Search";
            this.cSearch.Id = 11;
            this.cSearch.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cSearch.Name = "cSearch";
            this.cSearch.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cFullTextSearch
            // 
            this.cFullTextSearch.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            resources.ApplyResources(this.cFullTextSearch, "cFullTextSearch");
            this.cFullTextSearch.ContainerId = "FullTextSearch";
            this.cFullTextSearch.Id = 12;
            this.cFullTextSearch.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cFullTextSearch.Name = "cFullTextSearch";
            this.cFullTextSearch.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cFilters
            // 
            resources.ApplyResources(this.cFilters, "cFilters");
            this.cFilters.ContainerId = "Filters";
            this.cFilters.Id = 26;
            this.cFilters.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cFilters.Name = "cFilters";
            this.cFilters.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // barSubItemTools
            // 
            resources.ApplyResources(this.barSubItemTools, "barSubItemTools");
            this.barSubItemTools.Id = 3;
            this.barSubItemTools.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cTools, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cOptions, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cDiagnostic, true)});
            this.barSubItemTools.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.barSubItemTools.Name = "barSubItemTools";
            // 
            // cTools
            // 
            resources.ApplyResources(this.cTools, "cTools");
            this.cTools.ContainerId = "Tools";
            this.cTools.Id = 13;
            this.cTools.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cTools.Name = "cTools";
            this.cTools.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cOptions
            // 
            resources.ApplyResources(this.cOptions, "cOptions");
            this.cOptions.ContainerId = "Options";
            this.cOptions.Id = 14;
            this.cOptions.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cOptions.Name = "cOptions";
            this.cOptions.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cDiagnostic
            // 
            resources.ApplyResources(this.cDiagnostic, "cDiagnostic");
            this.cDiagnostic.ContainerId = "Diagnostic";
            this.cDiagnostic.Id = 16;
            this.cDiagnostic.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cDiagnostic.Name = "cDiagnostic";
            this.cDiagnostic.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // barSubItemHelp
            // 
            resources.ApplyResources(this.barSubItemHelp, "barSubItemHelp");
            this.barSubItemHelp.Id = 4;
            this.barSubItemHelp.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cAbout, true)});
            this.barSubItemHelp.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.barSubItemHelp.Name = "barSubItemHelp";
            this.barSubItemHelp.VisibleInRibbon = false;
            // 
            // cAbout
            // 
            resources.ApplyResources(this.cAbout, "cAbout");
            this.cAbout.ContainerId = "About";
            this.cAbout.Id = 15;
            this.cAbout.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cAbout.Name = "cAbout";
            this.cAbout.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // standardToolBar
            // 
            this.standardToolBar.BarName = "Main Toolbar";
            this.standardToolBar.DockCol = 0;
            this.standardToolBar.DockRow = 1;
            this.standardToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.standardToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cObjectsCreation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cEdit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cUndoRedo, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cRecordEdit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cOpenObject),
            new DevExpress.XtraBars.LinkPersistInfo(this.cWorkflow, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cBusinessLogic),
            new DevExpress.XtraBars.LinkPersistInfo(this.cView, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cReports),
            new DevExpress.XtraBars.LinkPersistInfo(this.cRecordsNavigation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cClose, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cFilters, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cSearch, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cFullTextSearch)});
            this.standardToolBar.OptionsBar.UseWholeRow = true;
            this.standardToolBar.TargetPageCategoryColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.standardToolBar, "standardToolBar");
            // 
            // _statusBar
            // 
            this._statusBar.BarName = "StatusBar";
            this._statusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this._statusBar.DockCol = 0;
            this._statusBar.DockRow = 0;
            this._statusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this._statusBar.OptionsBar.AllowQuickCustomization = false;
            this._statusBar.OptionsBar.DisableClose = true;
            this._statusBar.OptionsBar.DisableCustomization = true;
            this._statusBar.OptionsBar.DrawDragBorder = false;
            this._statusBar.OptionsBar.DrawSizeGrip = true;
            this._statusBar.OptionsBar.UseWholeRow = true;
            this._statusBar.TargetPageCategoryColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this._statusBar, "_statusBar");
            // 
            // cBusinessLogic
            // 
            resources.ApplyResources(this.cBusinessLogic, "cBusinessLogic");
            this.cBusinessLogic.ContainerId = "BusinessLogic";
            this.cBusinessLogic.Id = 23;
            this.cBusinessLogic.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cBusinessLogic.Name = "cBusinessLogic";
            this.cBusinessLogic.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            resources.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            resources.ApplyResources(this.barDockControlRight, "barDockControlRight");
            // 
            // cMenu
            // 
            resources.ApplyResources(this.cMenu, "cMenu");
            this.cMenu.ContainerId = "Menu";
            this.cMenu.Id = 8;
            this.cMenu.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cMenu.Name = "cMenu";
            this.cMenu.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // viewSitePanel
            // 
            this.viewSitePanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            resources.ApplyResources(this.viewSitePanel, "viewSitePanel");
            this.viewSitePanel.Name = "viewSitePanel";
            // 
            // formStateModelSynchronizerComponent
            // 
            this.formStateModelSynchronizerComponent.Form = this;
            this.formStateModelSynchronizerComponent.Model = null;
            // 
            // IagErmDetailForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BarManager = this.mainBarManager;
            this.Controls.Add(this.viewSitePanel);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "IagErmDetailForm";
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSitePanel)).EndInit();
            this.ResumeLayout(false);

        }


        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.ExpressApp.Win.Templates.Controls.XafBar _mainMenuBar;
        private DevExpress.ExpressApp.Win.Templates.Controls.XafBar standardToolBar;
        private DevExpress.ExpressApp.Win.Templates.Controls.XafBar _statusBar;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cFile;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cObjectsCreation;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cClose;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cSave;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cEdit;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cOpenObject;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cUndoRedo;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cReports;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cPrint;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cExport;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cMenu;
        private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemFile;
        private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemEdit;
        private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemView;
        private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemTools;
        private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemHelp;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cRecordEdit;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cWorkflow;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cRecordsNavigation;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cSearch;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cFullTextSearch;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cFilters;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cView;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cTools;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cOptions;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cAbout;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cDiagnostic;
        #endregion
        protected DevExpress.ExpressApp.Win.Templates.Controls.XafBarManager mainBarManager;
        protected DevExpress.XtraEditors.PanelControl viewSitePanel;
        protected DevExpress.ExpressApp.Win.Core.FormStateModelSynchronizer formStateModelSynchronizerComponent;
        private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cBusinessLogic;
    }
}
