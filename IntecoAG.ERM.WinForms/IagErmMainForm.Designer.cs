#region Copyright (c) 2000-2012 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{       eXpressApp Framework                                        }
{                                                                   }
{       Copyright (c) 2000-2012 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2012 Developer Express Inc.

using DevExpress.XtraBars;
using DevExpress.ExpressApp.Win.Templates;

namespace DevExpress.ExpressApp.Win.Templates {
	partial class IagErmMainForm {
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		#region Windows Form Designer generated code
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IagErmMainForm));
            this.mainBarManager = new DevExpress.ExpressApp.Win.Templates.Controls.XafBarManager(this.components);
            this._mainMenuBar = new DevExpress.ExpressApp.Win.Templates.Controls.XafBar();
            this.barSubItemFile = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cObjectsCreation = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cFile = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cSave = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cPrint = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cExport = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cExit = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.barSubItemEdit = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cUndoRedo = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cEdit = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cRecordEdit = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cWorkflow = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cOpenObject = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.barSubItemView = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.barSubItemPanels = new DevExpress.XtraBars.BarSubItem();
            this.cPanels = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.cViewsHistoryNavigation = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cViewsNavigation = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.cRecordsNavigation = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cView = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cReports = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cDefault = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cSearch = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cFilters = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cFullTextSearch = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.cAppearance = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.barSubItemTools = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cTools = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.cOptions = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.cDiagnostic = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.barSubItemWindow = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cWindows = new DevExpress.ExpressApp.Win.Templates.ActionContainers.XafBarLinkContainerItem();
            this.barMdiChildrenListItem = new DevExpress.XtraBars.BarMdiChildrenListItem();
            this.Window = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.barSubItemHelp = new DevExpress.ExpressApp.Win.Templates.MainMenuItem();
            this.cAbout = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem();
            this.standardToolBar = new DevExpress.ExpressApp.Win.Templates.Controls.XafBar();
            this.CBusinessLogic = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this._statusBar = new DevExpress.ExpressApp.Win.Templates.Controls.XafBar();
            this.mainBarAndDockingController = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.mainDockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanelNavigation = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelNavigation_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.navigation = new DevExpress.ExpressApp.Win.Templates.ActionContainers.NavigationActionContainer();
            this.cMenu = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.actionContainerBarItem1 = new DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem();
            this.viewSitePanel = new DevExpress.XtraEditors.PanelControl();
            this.formStateModelSynchronizerComponent = new DevExpress.ExpressApp.Win.Core.FormStateModelSynchronizer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xafTabbedMdiManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarAndDockingController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainDockManager)).BeginInit();
            this.dockPanelNavigation.SuspendLayout();
            this.dockPanelNavigation_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewSitePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // actionsContainersManager
            // 
            this.actionsContainersManager.ActionContainerComponents.Add(this.cObjectsCreation);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cFile);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cSave);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cPrint);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cExport);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cExit);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cUndoRedo);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cEdit);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cRecordEdit);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cWorkflow);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cOpenObject);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cPanels);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cViewsHistoryNavigation);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cViewsNavigation);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cRecordsNavigation);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cView);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cReports);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cDefault);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cSearch);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cFilters);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cFullTextSearch);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cAppearance);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cTools);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cOptions);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cDiagnostic);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cAbout);
            this.actionsContainersManager.ActionContainerComponents.Add(this.navigation);
            this.actionsContainersManager.ActionContainerComponents.Add(this.cMenu);
            this.actionsContainersManager.ActionContainerComponents.Add(this.Window);
            this.actionsContainersManager.ActionContainerComponents.Add(this.CBusinessLogic);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cObjectsCreation);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cSave);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cEdit);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cRecordEdit);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cOpenObject);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cUndoRedo);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cPrint);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cView);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cReports);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cExport);
            this.actionsContainersManager.ContextMenuContainers.Add(this.cMenu);
            this.actionsContainersManager.DefaultContainer = this.cDefault;
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
            this.mainBarManager.Controller = this.mainBarAndDockingController;
            this.mainBarManager.DockControls.Add(this.barDockControlTop);
            this.mainBarManager.DockControls.Add(this.barDockControlBottom);
            this.mainBarManager.DockControls.Add(this.barDockControlLeft);
            this.mainBarManager.DockControls.Add(this.barDockControlRight);
            this.mainBarManager.DockManager = this.mainDockManager;
            this.mainBarManager.Form = this;
            this.mainBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItemFile,
            this.barSubItemEdit,
            this.barSubItemView,
            this.barSubItemTools,
            this.barSubItemHelp,
            this.cFile,
            this.cObjectsCreation,
            this.cPrint,
            this.cExport,
            this.cSave,
            this.cEdit,
            this.cOpenObject,
            this.cUndoRedo,
            this.cAppearance,
            this.cReports,
            this.cExit,
            this.cRecordEdit,
            this.cWorkflow,
            this.cRecordsNavigation,
            this.cViewsHistoryNavigation,
            this.cSearch,
            this.cFullTextSearch,
            this.cFilters,
            this.cView,
            this.cDefault,
            this.cTools,
            this.cViewsNavigation,
            this.cDiagnostic,
            this.cOptions,
            this.cAbout,
            this.cWindows,
            this.cPanels,
            this.cMenu,
            this.barSubItemWindow,
            this.barMdiChildrenListItem,
            this.Window,
            this.barSubItemPanels,
            this.actionContainerBarItem1,
            this.CBusinessLogic});
            this.mainBarManager.MainMenu = this._mainMenuBar;
            this.mainBarManager.MaxItemId = 38;
            this.mainBarManager.StatusBar = this._statusBar;
            this.mainBarManager.Disposed += new System.EventHandler(this.mainBarManager_Disposed);
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemWindow),
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
            new DevExpress.XtraBars.LinkPersistInfo(this.cSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cPrint, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cExport, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cExit, true)});
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
            this.cObjectsCreation.Id = 18;
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
            // cSave
            // 
            this.cSave.ApplicationMenuIndex = 7;
            this.cSave.ApplicationMenuItem = true;
            resources.ApplyResources(this.cSave, "cSave");
            this.cSave.ContainerId = "Save";
            this.cSave.Id = 8;
            this.cSave.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cSave.Name = "cSave";
            this.cSave.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            // cExit
            // 
            this.cExit.ApplicationMenuIndex = 900;
            this.cExit.ApplicationMenuItem = true;
            resources.ApplyResources(this.cExit, "cExit");
            this.cExit.ContainerId = "Exit";
            this.cExit.Id = 8;
            this.cExit.MergeOrder = 900;
            this.cExit.Name = "cExit";
            this.cExit.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            this.cUndoRedo.Id = 10;
            this.cUndoRedo.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cUndoRedo.Name = "cUndoRedo";
            this.cUndoRedo.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cEdit
            // 
            resources.ApplyResources(this.cEdit, "cEdit");
            this.cEdit.ContainerId = "Edit";
            this.cEdit.Id = 9;
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
            this.cOpenObject.Id = 9;
            this.cOpenObject.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cOpenObject.Name = "cOpenObject";
            this.cOpenObject.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // barSubItemView
            // 
            resources.ApplyResources(this.barSubItemView, "barSubItemView");
            this.barSubItemView.Id = 2;
            this.barSubItemView.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemPanels, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cViewsHistoryNavigation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cViewsNavigation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cRecordsNavigation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cView, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cReports, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cDefault, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cSearch, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.cFilters, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.cFullTextSearch, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cAppearance, true)});
            this.barSubItemView.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.barSubItemView.Name = "barSubItemView";
            // 
            // barSubItemPanels
            // 
            resources.ApplyResources(this.barSubItemPanels, "barSubItemPanels");
            this.barSubItemPanels.Id = 35;
            this.barSubItemPanels.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cPanels, true)});
            this.barSubItemPanels.Name = "barSubItemPanels";
            // 
            // cPanels
            // 
            resources.ApplyResources(this.cPanels, "cPanels");
            this.cPanels.ContainerId = "Panels";
            this.cPanels.Id = 16;
            this.cPanels.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cPanels.Name = "cPanels";
            this.cPanels.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.cPanels.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cViewsHistoryNavigation
            // 
            resources.ApplyResources(this.cViewsHistoryNavigation, "cViewsHistoryNavigation");
            this.cViewsHistoryNavigation.ContainerId = "ViewsHistoryNavigation";
            this.cViewsHistoryNavigation.Id = 35;
            this.cViewsHistoryNavigation.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cViewsHistoryNavigation.Name = "cViewsHistoryNavigation";
            this.cViewsHistoryNavigation.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cViewsNavigation
            // 
            resources.ApplyResources(this.cViewsNavigation, "cViewsNavigation");
            this.cViewsNavigation.ContainerId = "ViewsNavigation";
            this.cViewsNavigation.Id = 14;
            this.cViewsNavigation.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cViewsNavigation.Name = "cViewsNavigation";
            this.cViewsNavigation.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            this.cReports.ApplicationMenuIndex = 12;
            this.cReports.ApplicationMenuItem = true;
            resources.ApplyResources(this.cReports, "cReports");
            this.cReports.ContainerId = "Reports";
            this.cReports.Id = 11;
            this.cReports.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cReports.Name = "cReports";
            this.cReports.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cDefault
            // 
            resources.ApplyResources(this.cDefault, "cDefault");
            this.cDefault.ContainerId = "Default";
            this.cDefault.Id = 50;
            this.cDefault.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cDefault.Name = "cDefault";
            this.cDefault.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cSearch
            // 
            this.cSearch.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            resources.ApplyResources(this.cSearch, "cSearch");
            this.cSearch.ContainerId = "Search";
            this.cSearch.Id = 11;
            this.cSearch.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cSearch.Name = "cSearch";
            this.cSearch.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // cFilters
            // 
            this.cFilters.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            resources.ApplyResources(this.cFilters, "cFilters");
            this.cFilters.ContainerId = "Filters";
            this.cFilters.Id = 26;
            this.cFilters.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cFilters.Name = "cFilters";
            this.cFilters.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            // cAppearance
            // 
            resources.ApplyResources(this.cAppearance, "cAppearance");
            this.cAppearance.ContainerId = "Appearance";
            this.cAppearance.Id = 9;
            this.cAppearance.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cAppearance.Name = "cAppearance";
            this.cAppearance.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            // barSubItemWindow
            // 
            resources.ApplyResources(this.barSubItemWindow, "barSubItemWindow");
            this.barSubItemWindow.Id = 32;
            this.barSubItemWindow.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cWindows),
            new DevExpress.XtraBars.LinkPersistInfo(this.Window, true)});
            this.barSubItemWindow.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.barSubItemWindow.Name = "barSubItemWindow";
            // 
            // cWindows
            // 
            resources.ApplyResources(this.cWindows, "cWindows");
            this.cWindows.Id = 16;
            this.cWindows.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barMdiChildrenListItem)});
            this.cWindows.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.cWindows.Name = "cWindows";
            this.cWindows.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // barMdiChildrenListItem
            // 
            resources.ApplyResources(this.barMdiChildrenListItem, "barMdiChildrenListItem");
            this.barMdiChildrenListItem.Id = 37;
            this.barMdiChildrenListItem.Name = "barMdiChildrenListItem";
            // 
            // Window
            // 
            resources.ApplyResources(this.Window, "Window");
            this.Window.ContainerId = "Windows";
            this.Window.Id = 34;
            this.Window.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.Window.Name = "Window";
            this.Window.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            this.cAbout.ApplicationMenuIndex = 15;
            this.cAbout.ApplicationMenuItem = true;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.cViewsHistoryNavigation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cObjectsCreation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cEdit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cUndoRedo, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cRecordEdit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cOpenObject),
            new DevExpress.XtraBars.LinkPersistInfo(this.cWorkflow, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cView, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cReports),
            new DevExpress.XtraBars.LinkPersistInfo(this.cDefault, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cRecordsNavigation, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.CBusinessLogic, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cFullTextSearch, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cSearch, true)});
            this.standardToolBar.OptionsBar.UseWholeRow = true;
            this.standardToolBar.TargetPageCategoryColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.standardToolBar, "standardToolBar");
            // 
            // CBusinessLogic
            // 
            resources.ApplyResources(this.CBusinessLogic, "CBusinessLogic");
            this.CBusinessLogic.ContainerId = "BusinessLogic";
            this.CBusinessLogic.Id = 37;
            this.CBusinessLogic.MergeType = DevExpress.XtraBars.BarMenuMerge.MergeItems;
            this.CBusinessLogic.Name = "CBusinessLogic";
            this.CBusinessLogic.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            // mainBarAndDockingController
            // 
            this.mainBarAndDockingController.PropertiesBar.AllowLinkLighting = false;
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
            // mainDockManager
            // 
            this.mainDockManager.Controller = this.mainBarAndDockingController;
            this.mainDockManager.Form = this;
            this.mainDockManager.MenuManager = this.mainBarManager;
            this.mainDockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelNavigation});
            this.mainDockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.ExpressApp.Win.Templates.Controls.XafRibbonControl",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar"});
            // 
            // dockPanelNavigation
            // 
            this.dockPanelNavigation.Controls.Add(this.dockPanelNavigation_Container);
            this.dockPanelNavigation.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanelNavigation.FloatSize = new System.Drawing.Size(146, 428);
            this.dockPanelNavigation.ID = new System.Guid("24977e30-0ea6-44aa-8fa4-9abaeb178b5e");
            resources.ApplyResources(this.dockPanelNavigation, "dockPanelNavigation");
            this.dockPanelNavigation.Name = "dockPanelNavigation";
            this.dockPanelNavigation.Options.AllowDockBottom = false;
            this.dockPanelNavigation.Options.AllowDockTop = false;
            this.dockPanelNavigation.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelNavigation.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanelNavigation.SavedIndex = 2;
            this.dockPanelNavigation.TabStop = false;
            // 
            // dockPanelNavigation_Container
            // 
            this.dockPanelNavigation_Container.Controls.Add(this.navigation);
            resources.ApplyResources(this.dockPanelNavigation_Container, "dockPanelNavigation_Container");
            this.dockPanelNavigation_Container.Name = "dockPanelNavigation_Container";
            // 
            // navigation
            // 
            resources.ApplyResources(this.navigation, "navigation");
            this.navigation.Model = null;
            this.navigation.Name = "navigation";
            // 
            // cMenu
            // 
            resources.ApplyResources(this.cMenu, "cMenu");
            this.cMenu.ContainerId = "Menu";
            this.cMenu.Id = 7;
            this.cMenu.Name = "cMenu";
            this.cMenu.TargetPageCategoryColor = System.Drawing.Color.Empty;
            // 
            // actionContainerBarItem1
            // 
            resources.ApplyResources(this.actionContainerBarItem1, "actionContainerBarItem1");
            this.actionContainerBarItem1.Id = 36;
            this.actionContainerBarItem1.Name = "actionContainerBarItem1";
            this.actionContainerBarItem1.TargetPageCategoryColor = System.Drawing.Color.Empty;
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
            // IagErmMainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BarManager = this.mainBarManager;
            this.Controls.Add(this.viewSitePanel);
            this.Controls.Add(this.dockPanelNavigation);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IsMdiContainer = true;
            this.Name = "IagErmMainForm";
            ((System.ComponentModel.ISupportInitialize)(this.xafTabbedMdiManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarAndDockingController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainDockManager)).EndInit();
            this.dockPanelNavigation.ResumeLayout(false);
            this.dockPanelNavigation_Container.ResumeLayout(false);
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
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cPrint;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cExport;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cSave;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cUndoRedo;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cAppearance;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cReports;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cEdit;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cExit;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cOpenObject;
		private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemFile;
		private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemEdit;
		private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemView;
		private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemTools;
		private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemHelp;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cViewsNavigation;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cRecordEdit;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cWorkflow;		
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cRecordsNavigation;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cViewsHistoryNavigation;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cSearch;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cFullTextSearch;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cFilters;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cView;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cDefault;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cTools;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cDiagnostic;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cOptions;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cAbout;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.XafBarLinkContainerItem cWindows;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerMenuBarItem cPanels;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem cMenu;
		private DevExpress.ExpressApp.Win.Templates.MainMenuItem barSubItemWindow;
		private DevExpress.XtraBars.BarMdiChildrenListItem barMdiChildrenListItem;
		#endregion
		protected DevExpress.ExpressApp.Win.Templates.ActionContainers.NavigationActionContainer navigation;
		protected DevExpress.ExpressApp.Win.Core.FormStateModelSynchronizer formStateModelSynchronizerComponent;
		private BarAndDockingController mainBarAndDockingController;
		private DevExpress.XtraBars.Docking.DockManager mainDockManager;
		protected DevExpress.ExpressApp.Win.Templates.Controls.XafBarManager mainBarManager;
		private DevExpress.XtraBars.Docking.DockPanel dockPanelNavigation;
		private DevExpress.XtraBars.Docking.ControlContainer dockPanelNavigation_Container;
		private DevExpress.XtraEditors.PanelControl viewSitePanel;
		private DevExpress.ExpressApp.Win.Templates.ActionContainers.ActionContainerBarItem Window;
		private BarSubItem barSubItemPanels;
//        private Controls.XafBar xafBar1;
        private ActionContainers.ActionContainerBarItem actionContainerBarItem1;
        private ActionContainers.ActionContainerBarItem CBusinessLogic;
	}
}
