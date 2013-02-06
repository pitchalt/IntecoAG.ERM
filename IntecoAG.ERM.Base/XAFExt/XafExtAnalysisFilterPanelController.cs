using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.SystemModule;

using System.Windows.Forms;

using DevExpress.ExpressApp.PivotChart;
using DevExpress.ExpressApp.PivotChart.Win;
using DevExpress.XtraCharts;
using DevExpress.XtraPivotGrid;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.XAFExt;

namespace IntecoAG.ERM.XAFExt {
    public partial class XafExtAnalysisFilterPanelController : ViewController {

        public string criteriaBuilderString = "";
        public string criteriaLikeSearchString = "";
        public string criteriaFormSearchString = "";
        private ICustomFilter wp = null;
        DevExpress.XtraEditors.PanelControl filterPanel = null;
        DevExpress.XtraEditors.SplitterControl splitter = null;
        DevExpress.XtraEditors.PanelControl dataPanel = null;

        public AnalysisDataBindController analysisDataBindController = null;

        string currentFilterId = "";
        bool FilterIsShowihg = false;

        public static string DO_NOT_ENABLED = "FilteringCriterionListAction_DO_NOT_ENABLED";
        public static string DO_NOT_ACTIVE = "DO_NOT_ACTIVE";


        private Frame frame;
        private DevExpress.ExpressApp.View view;
        private DetailView detailView;

        //private DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo;


        public XafExtAnalysisFilterPanelController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnDeactivated() {
            if (analysisDataBindController != null) {
                analysisDataBindController.BindDataAction.Active.SetItemValue("ResultValue", true);
                analysisDataBindController.UnbindDataAction.Active.SetItemValue("ResultValue", true);
            }
            base.OnDeactivated();
        }

        private void DetailViewAnalysisFilterPanelController_Activated(object sender, EventArgs e) {

            frame = Frame;
            view = View;
            detailView = view as DetailView;

            if (detailView == null) return;

            DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo = detailView.ObjectTypeInfo;



            // Подключаемся к ListViewProcessCurrentObjectController
            analysisDataBindController = frame.GetController<AnalysisDataBindController>();

            if (frame.Context.Name == TemplateContext.LookupControl ||
                frame.Context.Name == TemplateContext.LookupWindow) return;

            Type typeAnalysingObject = GetChoiceType();  // typeof(object);

            ExecStandartAction(null);

            if (typeAnalysingObject == null) return;

            #region ОБРАБОТКА ОПЦИЙ ФИЛЬТРАЦИИ ИЗ ПОДУЗЛА Filters текущего ListView СРЕДСТВАМИ ДАННОГО КОНТРОЛЛЕРА ВМЕСТО СТАНДАРТНОГО КОНТРОЛЛЕРА FilterController

            View.ControlsCreated += new EventHandler(View_ControlsCreated);
            AnalysisCriterionListAction.Items.Clear();
            #endregion

            // Виды фильтрации
            // Список DetailView, которые представляют формы ввода параметров
            // Считаем пока, что имя DetailView с фильром формируется из имени типа фильтуемого списка прибавлением
            // слова Filter и номера, например, WorkPlanFilter01_DetailView.

            foreach (IModelDetailView node in View.Model.Application.Views.GetNodes<IModelDetailView>()) {
                //if (node.ModelClass == View.Model.ModelClass) {
                if (node.ModelClass.TypeInfo.Type.Name.StartsWith(typeAnalysingObject.Name + "Filter")) {
                    ChoiceActionItem current = new ChoiceActionItem(node.Id, node.Caption, node);
                    AnalysisCriterionListAction.Items.Add(current);
                    //if (node == View.Model) SelectViewAction.SelectedItem = current;
                }
            }

//            AnalysisCriterionListAction.SelectedIndex = 0;

            ClearFilterFields.Active.Clear();
            ClearFilterFields.Enabled.Clear();
            ClearFilterFields.Active.SetItemValue("ResultValue", true);
        }

        private Type GetChoiceType() {
            Type typeAnalysingObject = null;
            if (detailView == null) return typeAnalysingObject;

            foreach (PropertyEditor item in detailView.GetItems<PropertyEditor>()) {
                if (item.PropertyName == "DataType") {
                    typeAnalysingObject = item.PropertyValue as System.Type;
                    break;
                }
            }

            return typeAnalysingObject;
        }

        void View_ControlsCreated(object sender, EventArgs e) {
            ((System.Windows.Forms.Control)View.Control).HandleCreated += new EventHandler(ViewController2_HandleCreated);
        }

        void ViewController2_HandleCreated(object sender, EventArgs e) {
            // Вставка Layout
            if (AnalysisCriterionListAction.Items.Count == 0) return;

            //DevExpress.XtraEditors.PanelControl filterPanel = new DevExpress.XtraEditors.PanelControl();
            filterPanel = new DevExpress.XtraEditors.PanelControl();
            filterPanel.Height = 80;
            filterPanel.MinimumSize = new System.Drawing.Size(0, 0);
            filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            filterPanel.AutoSize = false;
            //filterPanel.GetPreferredSize(
            filterPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //filterPanel.Hide();
            //filterPanel.Show();
            //filterPanel.Controls.Add(splitter);

            splitter = new DevExpress.XtraEditors.SplitterControl();
            splitter.Height = 50;
            splitter.Dock = System.Windows.Forms.DockStyle.Top;
            splitter.AutoSize = false;
            //splitter.Hide();
            //splitter.Show();
            //splitter.Controls.Add(filterPanel);

            //DevExpress.XtraEditors.PanelControl dataPanel = new DevExpress.XtraEditors.PanelControl();
            dataPanel = new DevExpress.XtraEditors.PanelControl();
            dataPanel.Height = 50;
            dataPanel.MinimumSize = new System.Drawing.Size(0, 0);
            dataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            dataPanel.AutoSize = false;
            dataPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //dataPanel.Hide();
            //dataPanel.Show();
            //dataPanel.Controls.Add(splitter);

            if (((System.Windows.Forms.Control)sender).Parent == null) return;
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(dataPanel);
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(splitter);
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(filterPanel);

            // Находим контрол, показывающий DetailView
            System.Windows.Forms.Control ctrl = ((System.Windows.Forms.Control)sender).Parent.Controls[0];
            ctrl.Parent = dataPanel;

            // DetailView с фильтром
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            // Находим 1-й элемент списка фильтров
            IModelDetailView node = AnalysisCriterionListAction.Items[0].Data as IModelDetailView;
            // Тип объекта-фильтра
            Type filterObjType = node.ModelClass.TypeInfo.Type;

            wp = objectSpace.CreateObject(filterObjType) as ICustomFilter;
            if (wp == null) return;

            string DetailViewId = frame.Application.FindDetailViewId(wp.GetType());
            //TargetWindow openMode = TargetWindow.Current;

            DetailView newView = frame.Application.CreateDetailView(objectSpace, DetailViewId, false, wp);
            newView.CreateControls();

            // Назначения:
            //DevExpress.ExpressApp.ListView listView = sender as DevExpress.ExpressApp.ListView;
            DevExpress.ExpressApp.DetailView detailView = View as DevExpress.ExpressApp.DetailView;
            if (detailView == null) return;
            // ((System.Windows.Forms.Control)sender).Parent.Parent.Parent.Parent.Parent  as DevExpress.ExpressApp.ListView
            DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo = detailView.ObjectTypeInfo;

            wp.LV = null;
            wp.DV = detailView;
            //wp.objectType = typeObjectOfListView;
            wp.objectTypeInfo = objectTypeInfo;
            wp.CriteriaController = this;

            DevExpress.ExpressApp.Win.Layout.XafLayoutControl ctrlFilter = ((DevExpress.ExpressApp.Win.Layout.XafLayoutControl)newView.Control);

            filterPanel.Controls.Add(ctrlFilter);

            // Показ панели с фильтром
            FilterPanelShow(GetFilterHight(newView) + 30);
        }

        private void ApplyAnalysisFilter_Execute(object sender, SimpleActionExecuteEventArgs e) {
            //((XafExtAnalysis)View.CurrentObject).IsFilterVisible = true;   // !((XafExtAnalysis)View.CurrentObject).IsFilterVisible;

            XafExtAnalysis currentObject = detailView.CurrentObject as XafExtAnalysis;
            if (currentObject == null) return;

            // Добавляем преднастроенный фильтр, если он определён
            if (wp != null) {
                if (string.IsNullOrEmpty(currentObject.AdminCriteria)) {
                    currentObject.Criteria = wp.CriterionString;
                } else {
                    currentObject.Criteria = "(" + currentObject.AdminCriteria + ")" + "(" + wp.CriterionString + ")";
                }
            } else {
                if (!string.IsNullOrEmpty(currentObject.AdminCriteria)) {
                    currentObject.Criteria = currentObject.AdminCriteria;
                }
            }

            // Подключаемся к ListViewProcessCurrentObjectController
            if (analysisDataBindController != null) {
                AnalysisEditorWin analysisEditor = null;
                IList<AnalysisEditorWin> analysisEditors = detailView.GetItems<AnalysisEditorWin>();
                if (analysisEditors.Count == 1) {
                    analysisEditor = analysisEditors[0];

                    if (analysisEditor.IsDataSourceReady) {
                        ChartControl chartControl = ((AnalysisControlWin)analysisEditor.Control).ChartControl;
                        PivotGridControl pivotGridControl = ((AnalysisControlWin)analysisEditor.Control).PivotGrid;

                    //    Type currentType = currentObject.DataType.UnderlyingSystemType;
                    //    XPCollection xpDataSource = new XPCollection(currentObject.Session, currentType);
                    //    xpDataSource.CriteriaString = currentObject.Criteria;
                    //    if (!xpDataSource.IsLoaded) xpDataSource.Load();

                    //    pivotGridControl.DataSource = xpDataSource;
                    ////pivotGridControl.Fields.A
                    //    //(pivotGridControl.DataSource as XPCollection).CriteriaString = currentObject.Criteria.ToString();

                        pivotGridControl.RefreshData();
                        chartControl.RefreshData();
                        //pivotGridControl.Update();
                        //// Do what you want with the chart and pivot grid controls 
                        //foreach (PivotGridField field in pivotGridControl.Fields) {
                        //    field.Visible = true;
                        //}
                    } else {
                        ExecStandartAction(analysisDataBindController.BindDataAction);
                    }
                } 
            }

        }

        private void ClearAnalusisFilter_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (detailView == null) return;

            XafExtAnalysis currentObject = detailView.CurrentObject as XafExtAnalysis;
            if (currentObject == null) return;

            // Устанавливаем преднастроенный фильтр, если он определён
            currentObject.Criteria = currentObject.AdminCriteria;

            // Скрываем саму кнопку
            AnalysisCriterionListAction.Active[DO_NOT_ACTIVE] = true;

            // Разблокируем две кнопки
            AnalysisCriterionListAction.Enabled.Clear();
            AnalysisCriterionListAction.Enabled[DO_NOT_ENABLED] = true;


            AnalysisEditorWin analysisEditor = null;
            IList<AnalysisEditorWin> analysisEditors = detailView.GetItems<AnalysisEditorWin>();
            if(analysisEditors.Count == 1) {
                analysisEditor = analysisEditors[0];
            }

            // Скрытие панели с фильтром
            FilterPanelHide();
            currentFilterId = "";

            if (analysisDataBindController != null) {
                if (analysisEditor.IsDataSourceReady) {
                    ExecStandartAction(analysisDataBindController.UnbindDataAction);
                }
            }

            if (wp == null) return;
            wp.ClearFilter();
        }

        private void FilteringCriterionListAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            // В зависимости от выбранного пункта вызывается та или иная форма
            if (detailView == null) return;
            DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo = detailView.ObjectTypeInfo;

            XafExtAnalysis currentObject = detailView.CurrentObject as XafExtAnalysis;
            if (currentObject == null) return;

            // Находим, если задан, фильтр из модели
            string choiceActionItemFilterString = "";
            FilterController fc = Frame.GetController<FilterController>();
            ChoiceActionItem choiceActionItem = ((SingleChoiceAction)(fc.Actions["SetFilter"])).SelectedItem;
            if (choiceActionItem != null) choiceActionItemFilterString = choiceActionItem.Data.ToString();

            if (e.SelectedChoiceActionItem.Data != null) {
                IModelDetailView node = e.SelectedChoiceActionItem.Data as IModelDetailView;

                // Тип объекта-фильтра
                Type filterObjType = node.ModelClass.TypeInfo.Type;

                string strCriteria = this.criteriaBuilderString;

                IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
                ICustomFilter objFilter = objectSpace.CreateObject(filterObjType) as ICustomFilter;
                if (objFilter == null) return;
                objFilter.LV = null;
                objFilter.DV = detailView;
                objFilter.objectTypeInfo = objectTypeInfo;
                objFilter.CriteriaController = this;

                objFilter.AdditionalCriterionString = choiceActionItemFilterString;

                //objFilter.Criterion = strCriteria;

                string DetailViewId = frame.Application.FindDetailViewId(objFilter.GetType());

                if (FilterIsShowihg & currentFilterId == DetailViewId) {
                    FilterPanelHide();
                    return;
                }

                currentFilterId = DetailViewId;
                FilterIsShowihg = true;

                wp = objFilter as ICustomFilter;
                if (wp == null) return;
                wp.ClearFilter();

                DetailView newView = frame.Application.CreateDetailView(objectSpace, DetailViewId, false, objFilter);
                newView.CreateControls();

                filterPanel.Controls.Clear();
                DevExpress.ExpressApp.Win.Layout.XafLayoutControl ctrlFilter = ((DevExpress.ExpressApp.Win.Layout.XafLayoutControl)newView.Control);
                filterPanel.Controls.Add(ctrlFilter);

                AnalysisCriterionListAction.SelectedIndex = 0;

                //FilterPanelShow(100);
                //FilterPanelShow(GetFilterHight(newView) + 30);

                // Устанавливаем преднастроенный фильтр, если он определён
                currentObject.Criteria = currentObject.AdminCriteria;

                //AnalysisEditorWin analysisEditor = null;
                //IList<AnalysisEditorWin> analysisEditors = detailView.GetItems<AnalysisEditorWin>();
                //if (analysisEditors.Count == 1) {
                //    analysisEditor = analysisEditors[0];
                //}
                //if (analysisEditor != null) {
                //    ChartControl chartControl = ((AnalysisControlWin)analysisEditor.Control).ChartControl;
                //    PivotGridControl pivotGridControl = ((AnalysisControlWin)analysisEditor.Control).PivotGrid;
                //    // Do what you want with the chart and pivot grid controls 
                //    pivotGridControl.RefreshData();
                //    chartControl.RefreshData();
                //    //pivotGridControl.Update();
                //}

                ExecStandartAction(analysisDataBindController.UnbindDataAction);

                FilterPanelShow(GetFilterHight(newView) + 30);

                return;
            }
        }

        private void ExecStandartAction(SimpleAction action) {
            if (action == null) {
                //analysisDataBindController.BindDataAction.Active.Clear();
                //analysisDataBindController.UnbindDataAction.Active.Clear();

                analysisDataBindController.BindDataAction.Active.SetItemValue("ResultValue", false);
                analysisDataBindController.UnbindDataAction.Active.SetItemValue("ResultValue", false);
                return;
            }

            analysisDataBindController.BindDataAction.Active.Clear();
            analysisDataBindController.UnbindDataAction.Active.Clear();

            analysisDataBindController.BindDataAction.Enabled.Clear();
            analysisDataBindController.UnbindDataAction.Enabled.Clear();


            analysisDataBindController.BindDataAction.Active.SetItemValue("ResultValue", true);
            analysisDataBindController.UnbindDataAction.Active.SetItemValue("ResultValue", true);

            action.DoExecute();

            analysisDataBindController.BindDataAction.Active.SetItemValue("ResultValue", false);
            analysisDataBindController.UnbindDataAction.Active.SetItemValue("ResultValue", false);
        }

        private void FilterPanelHide() {
            if (filterPanel == null) return;
            filterPanel.Hide();
            splitter.Hide();
            FilterIsShowihg = false;
            currentFilterId = "";
        }

        private void FilterPanelShow(int Hight) {
            if (filterPanel == null) return;
            if (Hight > -1) filterPanel.Height = Hight;
            filterPanel.Show();
            splitter.Show();
            filterPanel.Refresh();
            FilterIsShowihg = true;
        }

        private int GetFilterHight(DetailView detailView) {
            int height = -1;

            foreach (PropertyEditor editor in detailView.GetItems<PropertyEditor>()) {
                DXPropertyEditor dxEditor = editor as DXPropertyEditor;
                if (dxEditor == null) continue;
                //if (!dxEditor.Control.Visible) continue;
                int summa = dxEditor.Control.ClientSize.Height + dxEditor.Control.Location.Y;
                if (height < summa) height = summa; // +dxEditor.Control.Bottom;
            }

            return height;
        }

        private void ClearFilterFields_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (wp == null) return;
            wp.ClearFilter();
        }
    }
}
