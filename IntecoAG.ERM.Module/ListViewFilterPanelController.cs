using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Data.Filtering;

using System.Windows.Forms;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Module {
    public partial class ListViewFilterPanelController : ViewController {

        private Frame frame;
        private DevExpress.ExpressApp.View view;
        private DevExpress.ExpressApp.ListView listView;
        private string listViewCriteria = "";

        public string criteriaBuilderString = "";
        public string criteriaLikeSearchString = "";
        public string criteriaFormSearchString = "";

        // Для атрибута LikeSearchPathListAttribute
        private string criteriaForLikeSearchPathListAttribute = "";
        IList<string> propertiesForLikeSearchPathListAttribute = new List<string>();

        private ICustomFilter wp = null;
        private DevExpress.XtraEditors.PanelControl filterPanel = null;
        private DevExpress.XtraEditors.SplitterControl splitter = null;
        private DevExpress.XtraEditors.PanelControl dataPanel = null;

        private FilterController standardFilterController;
        private ParametrizedAction standardFullTextFilterAction;
        private SingleChoiceAction standardSetFilterAction;

        private DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo;
        private Type typeObjectOfListView;

        private string currentFilterId = "";
        private bool FilterIsShowihg = false;

        public static string DO_NOT_ENABLED = "FilteringCriterionListAction_DO_NOT_ENABLED";
        public static string FTS_DO_NOT_ENABLED = "FTS_DO_NOT_ENABLED";
        public static string DO_NOT_ACTIVE = "DO_NOT_ACTIVE";


        public ListViewFilterPanelController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            if (View.ObjectTypeInfo.Type == typeof(crmIParty)) {
                if (standardFilterController != null) 
                    standardFilterController.FullTextFilterAction.Active["IsPersistentType"] = true;
            }
        }

        protected override void OnDeactivated() {
            if (standardFilterController != null) {
                standardFilterController.CustomBuildCriteria -= new EventHandler<CustomBuildCriteriaEventArgs>(standardFilterController_CustomBuildCriteria);
                standardFilterController.CustomGetFullTextSearchProperties -= new EventHandler<CustomGetFullTextSearchPropertiesEventArgs>(standardFilterController_CustomGetFullTextSearchProperties);
            }
            base.OnDeactivated();
        }


        private void ListViewFilterPanelController_Activated(object sender, EventArgs e) {

            // Скрываем саму кнопку
            ListViewClearFilter.Enabled[DO_NOT_ENABLED] = false;
            ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = false;

            frame = Frame;
            view = View;
            listView = view as DevExpress.ExpressApp.ListView;
            if (listView == null) return;

            objectTypeInfo = listView.CollectionSource.ObjectTypeInfo;
            typeObjectOfListView = objectTypeInfo.Type;

            // Критерий из модели для самого ListView
            listViewCriteria = listView.Model.Criteria;

            View.ControlsCreated += new EventHandler(View_ControlsCreated);
            FilteringCriterionListAction.Items.Clear();

            foreach (IModelDetailView node in View.Model.Application.Views.GetNodes<IModelDetailView>()) {
                if (node.ModelClass.TypeInfo.Type.Name.StartsWith(typeObjectOfListView.Name + "Filter")) {
                    ChoiceActionItem current = new ChoiceActionItem(node.Id, node.Caption, node);
                    FilteringCriterionListAction.Items.Add(current);
                }
            }

            IModelDetailView imdvFullTextSearch = Application.FindModelView(Application.FindDetailViewId(typeof(FilteringCriterion))) as IModelDetailView;
            string FullTextSearchCaption = imdvFullTextSearch.Caption;

            FilteringCriterionListAction.Items.Add(new ChoiceActionItem("CriteriaBuilder", FullTextSearchCaption, null));
            FilteringCriterionListAction.SelectedIndex = 0;

            //if (standardFullTextFilterAction != null) {
            //    string searchText = "";
            //    if (standardFullTextFilterAction.Value != null) searchText = standardFullTextFilterAction.Value.ToString();
            //    InitVariablesForLikeSearchPathListAttribute(searchText, out criteriaForLikeSearchPathListAttribute, out propertiesForLikeSearchPathListAttribute);
            //}
        }


        /// <summary>
        /// После организации фрейма обработка контроллера FilterController
        /// </summary>
        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();

            standardFilterController = Frame.GetController<FilterController>();
            if (standardFilterController != null) {
                standardFullTextFilterAction = standardFilterController.FullTextFilterAction;
                standardSetFilterAction = standardFilterController.SetFilterAction;

                standardFilterController.FullTextSearchTargetPropertiesMode = FullTextSearchTargetPropertiesMode.AllSearchableMembers;

                standardFilterController.CustomBuildCriteria += new EventHandler<CustomBuildCriteriaEventArgs>(standardFilterController_CustomBuildCriteria);
                standardFilterController.CustomGetFullTextSearchProperties += new EventHandler<CustomGetFullTextSearchPropertiesEventArgs>(standardFilterController_CustomGetFullTextSearchProperties);
            }
        }

        private void InitVariablesForLikeSearchPathListAttribute(string searchText, out string criteria, out IList<string> properties) {
            criteria = "";
            properties = new List<string>();

            // Анализ атрибута LikeSearchPathListAttribute
            IEnumerable<LikeSearchPathListAttribute> attrs = objectTypeInfo.FindAttributes<LikeSearchPathListAttribute>();

            IEnumerator<LikeSearchPathListAttribute> _enum = attrs.GetEnumerator();
            while (_enum.MoveNext()) {
                LikeSearchPathListAttribute attr = (LikeSearchPathListAttribute)(_enum.Current);
                if (attr.Values.Length > 0) {
                    if (!string.IsNullOrEmpty(searchText)) {
                        for (int i = 0; i < attr.Values.Length; i++) {
                            string elem = attr.Values[i];
                            if (string.IsNullOrEmpty(elem)) continue;
                            // проверить бы правильность пути, записанного в elem!
                            criteria += ((criteria == "") ? "" : " OR ") + "(" + elem + " like '%" + searchText + "%'" + ")";
                            properties.Add(elem);
                        }
                    }
                }
            }
        }

        // Формирование собственного списка свойств для поиска
        private void standardFilterController_CustomGetFullTextSearchProperties(object sender, CustomGetFullTextSearchPropertiesEventArgs e) {
            // Если список свойств в атрибуте LikeSearchPathList не пуст
            // По наблюдениям система подхватывает фильтры из модели и текущий фильтр из SetFilterAction контроллера FilterController.
            // Если бы было, пришлось бы удалить данный метод и раскоментарить кусок в standardFilterController_CustomBuildCriteria
            if (propertiesForLikeSearchPathListAttribute.Count > 0) {
                e.Properties.AddRange(propertiesForLikeSearchPathListAttribute);

                // Настраиваем видимости объектов на форме
                FilteringCriterionListAction.Enabled[DO_NOT_ENABLED] = false;
                ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = false;

                ListViewClearFilter.Enabled[DO_NOT_ENABLED] = true;
                //ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = true;

                e.Handled = true;
            }
        }

        /// <summary>
        /// Формирование собственного критерия в зависимости от наличия формы и полей полнотектсового поиска в атрибуте LikeSearchPathList,
        /// а также предустановленного фильтра в модели. Этот метод теперь всегда должен работать (из-за проверок указанных вещей)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void standardFilterController_CustomBuildCriteria(object sender, CustomBuildCriteriaEventArgs e) {

            // 2011-12-12. Фиксация понимания алгоитма работы и взаимодействия различных поисков.
            // Стандартный полнотекстовый
            // Поиск по полям, определённым в атрибутах LikeSearchPathListAttribute
            // Поиск по одной из собственных форм поиска
            //   Если LikeSearchPathListAttribute не определён и форма собственного поиска скрыта (т.е. currentFilterId == ""), 
            //      то работает чисто стандартный полнотекстовый поиск
            //   Если LikeSearchPathListAttribute определён и форма собственного поиска скрыта (т.е. currentFilterId == ""), 
            //      то работает полнотекстовый поиск только по полям, определённым в атрибутах LikeSearchPathListAttribute
            //   Если форма собственного поиска явлена (т.е. currentFilterId != ""),
            //      то поиск производится исключительно по параметрам формы

            string searchText = e.SearchText;   // ((FilterController)(sender)).FullTextFilterAction.Value;

            if (standardFullTextFilterAction != null) {
                InitVariablesForLikeSearchPathListAttribute(searchText, out criteriaForLikeSearchPathListAttribute, out propertiesForLikeSearchPathListAttribute);
            }

            string res = "";

            ListViewClearFilter.Enabled[DO_NOT_ENABLED] = true; // Кнопка очистки фильтра делается видимой
            FilteringCriterionListAction.Enabled[DO_NOT_ENABLED] = true;


            // Собственная форма поиска определена и открыта перед пользователем
            if (currentFilterId != "") {

                // Учитываем фильтр из формы фильтрации, если она определена
                if (wp != null && !string.IsNullOrEmpty(wp.CriterionString)) {
                    res = ((res == "") ? "" : "(" + res + ")" + " AND ") + "(" + wp.CriterionString + ")";
                }

                // Добавляем неизбежную нагрузку из модели

                // Фильтр из ListView в модели: listViewCriteria - присоединяем через AND
                if (!string.IsNullOrEmpty(listViewCriteria)) {
                    res = ((res == "") ? "" : "(" + res + ")" + " AND ") + "(" + listViewCriteria + ")";
                }

                // Учитываем фильтры из коллекции фильтров в модели
                if (standardSetFilterAction != null && standardSetFilterAction.SelectedItem != null && standardSetFilterAction.SelectedItem.Data != null) {
                    if (!string.IsNullOrEmpty(standardSetFilterAction.SelectedItem.Data.ToString()))
                        res = "(" + res + ")" + " AND " + "(" + standardSetFilterAction.SelectedItem.Data.ToString() + ")";
                }

                criteriaBuilderString = res;

                e.Criteria = CriteriaOperator.Parse(res);

                // Настраиваем видимости объектов на форме
                FilteringCriterionListAction.Enabled[DO_NOT_ENABLED] = true;
                ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = false;

                ListViewClearFilter.Enabled[DO_NOT_ENABLED] = false;
                //ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = true;

                // Скрытие панели с фильтром
                FilterPanelHide();

                e.Handled = true;
                return;
            }

            /*
            // Собственная форма поиска не явлена

            // Анализ атрибута LikeSearchPathListAttribute
            res = criteriaForLikeSearchPathListAttribute;

            // Если свой атрибут определён и заполнен, ищем только по полям заданным в атрибуте, которых может быть несколько
            if (!string.IsNullOrEmpty(res)) {

                // Список полей, которые сам контроллер поиска обнаруживает: 
                // Работает, но не нужен в данном случае по соглашению об алгоритме работы контроллера
                //foreach (string propName in standardFilterController.GetFullTextSearchProperties()) {
                //    res += ((res == "") ? "" : " OR ") + "(" + propName + " like '%" + searchText + "%'" + ")";
                //}

                // Добавляем неизбежную нагрузку из модели

                // Фильтр из ListView в модели: listViewCriteria - присоединяем через AND
                if (!string.IsNullOrEmpty(listViewCriteria)) {
                    res = ((res == "") ? "" : "(" + res + ")" + " AND ") + "(" + listViewCriteria + ")";
                }

                // Учитываем фильтры из коллекции фильтров в модели
                if (standardSetFilterAction != null && standardSetFilterAction.SelectedItem != null && standardSetFilterAction.SelectedItem.Data != null) {
                    if (!string.IsNullOrEmpty(standardSetFilterAction.SelectedItem.Data.ToString()))
                        res = "(" + res + ")" + " AND " + "(" + standardSetFilterAction.SelectedItem.Data.ToString() + ")";
                }

                //// Учитываем фильтр из формы фильтрации, если она определена
                //if (wp != null && !string.IsNullOrEmpty(wp.CriterionString)) {
                //    res = ((res == "") ? "" : "(" + res + ")" + " AND ") + "(" + wp.CriterionString + ")";
                //}

                e.Criteria = CriteriaOperator.Parse(res);

                // Настраиваем видимости объектов на форме
                FilteringCriterionListAction.Enabled[DO_NOT_ENABLED] = false;
                ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = false;

                ListViewClearFilter.Enabled[DO_NOT_ENABLED] = true;
                //ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = true;

                e.Handled = true;
            }
            */

        }

        void View_ControlsCreated(object sender, EventArgs e) {
            ((System.Windows.Forms.Control)View.Control).HandleCreated += new EventHandler(ViewController2_HandleCreated);
        }

        void ViewController2_HandleCreated(object sender, EventArgs e) {
            // Вставка Layout
            frame = Frame;
            //DetailView dv = View as DetailView;

            //DevExpress.XtraEditors.PanelControl filterPanel = new DevExpress.XtraEditors.PanelControl();
            filterPanel = new DevExpress.XtraEditors.PanelControl();
            filterPanel.Height = 80;
            filterPanel.MinimumSize = new System.Drawing.Size(0, 0);
            filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            filterPanel.AutoSize = false;
            //filterPanel.GetPreferredSize(
            filterPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            splitter = new DevExpress.XtraEditors.SplitterControl();
            splitter.Height = 50;
            splitter.Dock = System.Windows.Forms.DockStyle.Top;
            splitter.AutoSize = false;

            //DevExpress.XtraEditors.PanelControl dataPanel = new DevExpress.XtraEditors.PanelControl();
            dataPanel = new DevExpress.XtraEditors.PanelControl();
            dataPanel.Height = 50;
            dataPanel.MinimumSize = new System.Drawing.Size(0, 0);
            dataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            dataPanel.AutoSize = false;
            dataPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            if (((System.Windows.Forms.Control)sender).Parent == null) return;
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(dataPanel);
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(splitter);
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(filterPanel);


            // Находим контрол, показывающий ListView
            System.Windows.Forms.Control ctrl = ((System.Windows.Forms.Control)sender).Parent.Controls[0];
            ctrl.Parent = dataPanel;
            FilterPanelHide();
        }

        private void ListViewApplyFilter_Execute(object sender, SimpleActionExecuteEventArgs e) {
            bool actionVisible = standardFilterController.FullTextFilterAction.Enabled[FTS_DO_NOT_ENABLED];
            standardFilterController.FullTextFilterAction.Enabled[FTS_DO_NOT_ENABLED] = true;
            standardFullTextFilterAction.DoExecute(null);
            standardFilterController.FullTextFilterAction.Enabled[FTS_DO_NOT_ENABLED] = actionVisible;
            ListViewClearFilter.Enabled[DO_NOT_ENABLED] = true;
        }

        private void ListViewClearFilter_Execute(object sender, SimpleActionExecuteEventArgs e) {
            // Разблокируем стандартный полнотекстовый поиск
            standardFilterController.FullTextFilterAction.Enabled[FTS_DO_NOT_ENABLED] = true;
            // Скрываем саму кнопку
            //FilteringCriterionListActionClose1.Active[DO_NOT_ACTIVE] = false;
            FilteringCriterionListAction.Active[DO_NOT_ACTIVE] = true;

            // Разблокируем две кнопки
            FilteringCriterionListAction.Enabled.Clear();
            FilteringCriterionListAction.Enabled[DO_NOT_ENABLED] = true;

            ListViewApplyFilter.Enabled.Clear();
            ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = false;

            DevExpress.ExpressApp.ListView listView = View as DevExpress.ExpressApp.ListView;
            listView.CollectionSource.Criteria.Clear();

            if (wp != null) wp.ClearFilter();

            // Очищаем полнотекстовый поиск
            standardFilterController.FullTextFilterAction.DoExecute("");

            //if (wp == null) return;
            //wp.ClearFilter();
            ////listView.CollectionSource.Criteria.Clear();
            //listView.CollectionSource.Reload();   // Не нужно в связи standardFilterController.FullTextFilterAction.DoExecute(null)?

            // Скрытие панели с фильтром
            FilterPanelHide();
            currentFilterId = "";
        }

        private void FilteringCriterionListAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            // Глушим стандартный полнотекстовый поиск
            standardFilterController.FullTextFilterAction.Enabled[FTS_DO_NOT_ENABLED] = false;
            ListViewClearFilter.Enabled[DO_NOT_ENABLED] = true;
            ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = true;


            // В зависимости от выбранного пункта вызывается та или иная форма

            //frame = Frame;
            //view = View;
            //listView = view as DevExpress.ExpressApp.ListView;

            if (listView == null) return;

            // Находим, если задан, фильтр из модели
            string choiceActionItemFilterString = "";
            FilterController fc = Frame.GetController<FilterController>();
            ChoiceActionItem choiceActionItem = ((SingleChoiceAction)(fc.Actions["SetFilter"])).SelectedItem;
            if (choiceActionItem != null) choiceActionItemFilterString = choiceActionItem.Data.ToString();


            if (e.SelectedChoiceActionItem.Id == "CriteriaBuilder") {
                // Показ формы построения запроса с помощью построителя критериев

                if (FilterIsShowihg) {
                    if (currentFilterId == "CriteriaBuilder") {
                        FilterPanelHide();
                        return;
                    }
                }

                currentFilterId = "CriteriaBuilder";
                FilterIsShowihg = true;

                // Разблокируем кнопку применения фильтра
                ListViewApplyFilter.Enabled.Clear();
                ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = true;

                // Тип объекта-фильтра
                Type filterObjType = typeof(FilteringCriterion);

                string strCriteria = this.criteriaBuilderString;

                IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
                FilteringCriterion objFilter = objectSpace.CreateObject(filterObjType) as FilteringCriterion;  // objectSpace.GetObject(currentObject);
                if (objFilter == null) return;
                //objFilter.LV = listView;
                objFilter.objectTypeInfo = objectTypeInfo;
                //objFilter.CriteriaController = this;

                objFilter.Criterion = strCriteria; // Остаётся для построителя запросов в форме FilteringCriterion.cs
                //objFilter.AdditionalCriterionString = choiceActionItemFilterString;

                wp = objFilter as ICustomFilter;
                if (wp == null) return;

                string DetailViewId = frame.Application.FindDetailViewId(objFilter.GetType());

                DetailView newView = frame.Application.CreateDetailView(objectSpace, DetailViewId, false, objFilter);
                newView.CreateControls();

                filterPanel.Controls.Clear();
                DevExpress.ExpressApp.Win.Layout.XafLayoutControl ctrlFilter = ((DevExpress.ExpressApp.Win.Layout.XafLayoutControl)newView.Control);
                filterPanel.Controls.Add(ctrlFilter);

                FilteringCriterionListAction.SelectedIndex = 0;

                FilterPanelShow(130);

                return;
            }

            if (e.SelectedChoiceActionItem.Data != null) {

                // Показ формы с полями от NonPersistent объекта, через которую формировать критерий поиска, присваивать его 
                // списку и reload() этого списка

                // Разблокируем кнопку применения фильтра
                ListViewApplyFilter.Enabled.Clear();
                ListViewApplyFilter.Enabled[DO_NOT_ENABLED] = true;

                IModelDetailView node = e.SelectedChoiceActionItem.Data as IModelDetailView;

                // Тип объекта-фильтра
                Type filterObjType = node.ModelClass.TypeInfo.Type;

                string strCriteria = this.criteriaBuilderString;

                IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
                ICustomFilter objFilter = objectSpace.CreateObject(filterObjType) as ICustomFilter;
                if (objFilter == null) return;
                //objFilter.LV = listView;
                objFilter.objectTypeInfo = objectTypeInfo;
                //objFilter.CriteriaController = this;

                //objFilter.AdditionalCriterionString = choiceActionItemFilterString;

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

                DetailView newView = frame.Application.CreateDetailView(objectSpace, DetailViewId, false, objFilter);
                newView.CreateControls();

                filterPanel.Controls.Clear();
                DevExpress.ExpressApp.Win.Layout.XafLayoutControl ctrlFilter = ((DevExpress.ExpressApp.Win.Layout.XafLayoutControl)newView.Control);
                filterPanel.Controls.Add(ctrlFilter);

                FilteringCriterionListAction.SelectedIndex = 0;

                //FilterPanelShow(100);
                FilterPanelShow(GetFilterHight(newView) + 30);

                return;
            }
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

    }
}
