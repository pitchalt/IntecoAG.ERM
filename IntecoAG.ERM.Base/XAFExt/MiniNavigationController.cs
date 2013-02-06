using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;

using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.SystemModule;

using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.Module {

    public partial class MiniNavigationController : ObjectViewController, IMasterDetailViewInfo {
        private const string messageCaption = "Сообщение об ошибке";
        private const string MiniNavigationActionShowReason = "MiniNavigationActionShowReason";
        //static string selId = null;
        ListViewProcessCurrentObjectController controller = null;
        bool WhenControllerWorking = false;
        /// <summary>
        /// Сообщения
        /// </summary>
        public enum MiniNavigatorControllerMessages {
            [Description("Объект, который должен быть показан, не определён")]
            SHOWING_OBJECT_NOT_DEFINED = 1
        }

        public MiniNavigationController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            WhenControllerWorking = false;
            MiniNavigationAction.Items.Clear();
            MiniNavigationAction.Active[MiniNavigationActionShowReason] = false;

            if (Frame.Context.Name == TemplateContext.LookupControl || Frame.Context.Name == TemplateContext.LookupWindow) {
                return;
            }

            // Заполнение выпадающего списка переходов
            IModelMiniNavigations models = FindMiniNavigationModel(View);
            if (models != null) {
                FillNavigationDDLFromModel(models);
            }
            else {
                IList<MiniNavigationAttribute> mAttr = new List<MiniNavigationAttribute>(View.ObjectTypeInfo.FindAttributes<MiniNavigationAttribute>());
                if (mAttr.Count > 0)
                    FillNavigationDDLFromAttributes(mAttr);
                else
                    return;
            }

            // Подключаемся к ListViewProcessCurrentObjectController
            controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem += new EventHandler<CustomProcessListViewSelectedItemEventArgs>(controller_CustomProcessSelectedItem);
            }
            WhenControllerWorking = true;
            MiniNavigationAction.Active[MiniNavigationActionShowReason] = true;
            this.MiniNavigationAction.Execute += new SingleChoiceActionExecuteEventHandler(this.MiniNavigationAction_Execute);

        }

        protected override void OnDeactivated() {
            if (controller != null) {
                controller.CustomProcessSelectedItem -= new EventHandler<CustomProcessListViewSelectedItemEventArgs>(controller_CustomProcessSelectedItem);
                controller = null;
            }
            if (WhenControllerWorking) {
                this.MiniNavigationAction.Execute -= new SingleChoiceActionExecuteEventHandler(this.MiniNavigationAction_Execute);
                WhenControllerWorking = false;
            }
            base.OnDeactivated();
        }


        #region Методы для работы с навигацией через модель или атрибуты

        private BaseObject GetObjectOnPathEnd(string path, BaseObject parentObj) {
            string[] mPath = path.Split(new string[] { "." }, StringSplitOptions.None);
            BaseObject obj = (parentObj.GetMemberValue("This")) as BaseObject;
            for (int i = 0; i < mPath.Length; i++) {
                obj = obj.GetMemberValue(mPath[i]) as BaseObject;
            }

            return obj;
        }

        private IModelMiniNavigations FindMiniNavigationModel(View prmView) {
            IModelView node = View.Model.Application.Views[prmView.Id];
            if (node != null) {
                // Перебираем все подузлы
                // node.GetNode(1)	{ModelListViewFilters}	DevExpress.ExpressApp.Model.IModelNode {ModelListViewFilters}
                for (int i = 0; i < node.NodeCount; i++) {
                    IModelMiniNavigations miniNavigationNode = node.GetNode(i) as IModelMiniNavigations;
                    if (miniNavigationNode != null && miniNavigationNode.NodeCount != 0) {
                        return miniNavigationNode;
                    }
                }
            }
            return null;
        }

        private void FillNavigationDDLFromModel(IModelMiniNavigations miniNavigationNode) {
            if (miniNavigationNode != null) {
                // Сначала заполняем тот, что прописан по умолчанию
                IModelMiniNavigationItem miniNavigationDefaultItem = miniNavigationNode.DefaultMiniNavigationNode;
                if (miniNavigationDefaultItem != null) {
                    string path = miniNavigationDefaultItem.NavigationPath;
                    string caption = miniNavigationDefaultItem.NavigationCaption;
                    TargetWindow tw = miniNavigationDefaultItem.TargetWindow;

                    ChoiceActionItem cai = new ChoiceActionItem(path, caption, tw);
                    MiniNavigationAction.Items.Add(cai);
                }

                foreach (IModelMiniNavigationItem miniNavigationItem in miniNavigationNode.GetNodes<IModelMiniNavigationItem>()) {
                    if (miniNavigationItem != miniNavigationDefaultItem) {
                        string path = miniNavigationItem.NavigationPath;
                        string caption = miniNavigationItem.NavigationCaption;
                        TargetWindow tw = miniNavigationItem.TargetWindow;

                        ChoiceActionItem cai = new ChoiceActionItem(path, caption, tw);
                        MiniNavigationAction.Items.Add(cai);
                    }
                }
            }
        }

        private void FillNavigationDDLFromAttributes(IEnumerable<MiniNavigationAttribute> prmAttrs) {
            SortedDictionary<int, MiniNavigationAttribute> sd = new SortedDictionary<int, MiniNavigationAttribute>();

            IEnumerator<MiniNavigationAttribute> _enum = prmAttrs.GetEnumerator();
            while (_enum.MoveNext()) {
                MiniNavigationAttribute attr = ((MiniNavigationAttribute)(_enum.Current));
                sd.Add(attr.Order, attr);
            }

            foreach (KeyValuePair<int, MiniNavigationAttribute> kvp in sd) {
                MiniNavigationAttribute attr = kvp.Value;

                string path = attr.NavigationPath;
                string caption = attr.NavigationCaptin;
                TargetWindow tw = attr.TargetWindow;

                ChoiceActionItem cai = new ChoiceActionItem(path, caption, tw);
                MiniNavigationAction.Items.Add(cai);
            }
        }

        #endregion


        #region ОБРАБОТКА переходов по мининавигации

        private void MiniNavigationAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {

            if (e.CurrentObject == null) return;

            if (MiniNavigationAction.SelectedItem == null) return;

            object currentObj = e.CurrentObject;

            Frame frame = Frame;
            View view = View;
            string path = e.SelectedChoiceActionItem.Id;
            //if (selId == path) return;
            String selId = path;

            if ((view as DetailView) != null & selId == "This") {
                return;
            }

            BaseObject obj = GetObjectOnPathEnd(path, (BaseObject)currentObj);

            if (obj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    CommonMethods.GetMessage(MiniNavigatorControllerMessages.SHOWING_OBJECT_NOT_DEFINED),
                    messageCaption,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Exclamation);
                return;
            }

            Type ChoiceType = (System.Type)(obj.GetType());   //.UnderlyingSystemType;   //typeof(object);
            string ViewID = Application.FindDetailViewId(ChoiceType); // По умолчанию - DetailView

            // Считывание из модели view, через которое надо показать объект и оно задано
            IModelView modelView = null;
            DevExpress.ExpressApp.ListView listView = view as DevExpress.ExpressApp.ListView;
            DevExpress.ExpressApp.DetailView detailView = view as DevExpress.ExpressApp.DetailView;

            //if (IsMiniNavigationDefined) {
            //    // Узел в модели
            //    IModelView node = null;
            //    node = View.Model.Application.Views[((listView != null) ? listView.Id : "") + ((detailView != null) ? detailView.Id : "")];
            //    if (node != null) {
            //        // Перебираем все подузлы
            //        // node.GetNode(1)	{ModelListViewFilters}	DevExpress.ExpressApp.Model.IModelNode {ModelListViewFilters}
            //        for (int i = 0; i < node.NodeCount; i++) {
            //IModelMiniNavigations miniNavigationNode = node.GetNode(i) as IModelMiniNavigations;
            IModelMiniNavigations miniNavigationNode = FindMiniNavigationModel(View);
            if (miniNavigationNode != null) {
                foreach (IModelMiniNavigationItem miniNavigationItem in miniNavigationNode.GetNodes<IModelMiniNavigationItem>()) {
                    if (miniNavigationItem.NavigationPath == selId) {
                        modelView = miniNavigationItem.View;
                        break;
                    }
                }
                //break;
            }
            //        }
            //    }
            //}


            Frame resultFrame = frame; // Application.CreateFrame("new");
            TargetWindow openMode = (TargetWindow)e.SelectedChoiceActionItem.Data;

            // Сбрасываем состояние списка
            MiniNavigationAction.SelectedItem = null;
            MiniNavigationAction.SelectedIndex = 0;

            // Небольшая разборка с сочетанием значений openMode и типа текущего View - List или Detail - в целях
            // определения фрейма, в который грузить
            if (MasterDetailViewFrame != null && (openMode == TargetWindow.Current & (view as ListView) != null & ((frame as NestedFrame) != null | !view.IsRoot))) {
                // Тогда Current трактуем как корневое окно
                resultFrame = MasterDetailViewFrame;
                {
                    //IObjectSpace objectSpace = resultFrame.View.ObjectSpace;
                    IObjectSpace objectSpace = resultFrame.Application.CreateObjectSpace();   // Всё равно предыдущий ObjectSpace теряется в этом случае
                    object representativeObj = objectSpace.GetObject(obj);

                    View dv = null;

                    if (modelView != null) {
                        if ((modelView as IModelDetailView) != null) {
                            ViewID = (modelView as IModelDetailView).Id;
                            dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                        }
                        else if ((modelView as IModelListView) != null) {
                            ViewID = (modelView as IModelListView).Id;
                            //ListView dv = resultFrame.Application.CreateListView(objectSpace, ChoiceType, true);
                            CollectionSource colSource = new CollectionSource(objectSpace, ChoiceType);
                            if (!colSource.IsLoaded) colSource.Reload();
                            dv = resultFrame.Application.CreateListView((modelView as IModelListView), colSource, true) as ListView;
                        }
                    }
                    else {
                        ViewID = Application.FindDetailViewId(ChoiceType);
                        dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                    }

                    resultFrame.SetView(dv, true, resultFrame);
                }
                return;
            }

            // Общий алгоритм
            {
                IObjectSpace objectSpace = null;

                // Анализ того, какой ObjectSpace нужен
                if ((frame as NestedFrame) != null | !view.IsRoot) {
                    objectSpace = resultFrame.View.ObjectSpace.CreateNestedObjectSpace();
                }
                //objectSpace = resultFrame.View.ObjectSpace.CreateNestedObjectSpace();
                if (objectSpace == null) objectSpace = resultFrame.Application.CreateObjectSpace();

                object representativeObj = objectSpace.GetObject(obj);

                //ViewID = Application.FindDetailViewId(ChoiceType);
                //DetailView dv = frame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj);

                View dv = null;

                if (modelView != null) {
                    if ((modelView as IModelDetailView) != null) {
                        ViewID = (modelView as IModelDetailView).Id;
                        dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                    }
                    else if ((modelView as IModelListView) != null) {
                        ViewID = (modelView as IModelListView).Id;
                        //ListView dv = resultFrame.Application.CreateListView(objectSpace, ChoiceType, true);
                        //objectSpace = resultFrame.Application.CreateObjectSpace(); 
                        CollectionSource colSource = new CollectionSource(objectSpace, ChoiceType);
                        if (!colSource.IsLoaded) colSource.Reload();
                        dv = resultFrame.Application.CreateListView((modelView as IModelListView), colSource, true) as ListView;
                    }
                }
                else {
                    ViewID = Application.FindDetailViewId(ChoiceType);
                    dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                }

                ShowViewParameters svp = new ShowViewParameters();
                svp.CreatedView = dv;
                svp.TargetWindow = openMode;
                svp.Context = TemplateContext.View;
                svp.CreateAllControllers = true;

                e.ShowViewParameters.Assign(svp);
            }
        }

        #endregion


        #region ОБРАБОТКА 2-клик и Ввод на строках списков (кроме Lookup)

        private void controller_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {
            //if (Frame.Context.Name == TemplateContext.LookupControl || Frame.Context.Name == TemplateContext.LookupWindow) return;
            if (WhenControllerWorking) {
                showDV(sender, e);
                e.Handled = true;
            }
        }

        void showDV(object sender, CustomProcessListViewSelectedItemEventArgs e) {
            Frame frame = Frame;
            View view = View;
            DevExpress.ExpressApp.ListView listView = view as DevExpress.ExpressApp.ListView;
            IEnumerable<MiniNavigationAttribute> mAttr = null;
            bool AttrExists = false;

            object currentObject = (object)view.CurrentObject;
            if (currentObject == null) return;

            Type objType = typeof(object);
            //string DetailViewID = "";
            TargetWindow openMode = TargetWindow.Default;
            IObjectSpace objectSpace;

            Type NavigationChoiceType = (System.Type)(view.CurrentObject.GetType());   //.UnderlyingSystemType;   //typeof(object);


            // Вставка механизма проверки модели на предмет умолчательного view для показа по 2-клик или Ввод
            {
                string path = "";
                //string caption = "";
                //TargetWindow openMode = TargetWindow.Default;
                IModelView modelView = null;
                bool IsMiniNavigationDefined = false;

                // Узел в модели
                IModelView node = null;
                node = View.Model.Application.Views[listView.Id];
                //IModelListView node  = View.Model.Application.Views.GetNode<IModelListView>(listView.Id);
                if (node != null) {
                    // Перебираем все подузлы
                    // node.GetNode(1)	{ModelListViewFilters}	DevExpress.ExpressApp.Model.IModelNode {ModelListViewFilters}
                    for (int i = 0; i < node.NodeCount; i++) {
                        IModelMiniNavigations miniNavigationNode = node.GetNode(i) as IModelMiniNavigations;
                        if (miniNavigationNode != null) {

                            // Сначала заполняем тот, что прописан по умолчанию
                            IModelMiniNavigationItem miniNavigationDefaultItem = miniNavigationNode.DefaultMiniNavigationNode;
                            if (miniNavigationDefaultItem != null) {
                                path = miniNavigationDefaultItem.NavigationPath;
                                //caption = miniNavigationDefaultItem.NavigationCaption;
                                openMode = miniNavigationDefaultItem.TargetWindow;
                                modelView = miniNavigationDefaultItem.View;

                                IsMiniNavigationDefined = true;
                            }

                            //if (modelView == null) {
                            if (!IsMiniNavigationDefined) {
                                foreach (IModelMiniNavigationItem miniNavigationItem in miniNavigationNode.GetNodes<IModelMiniNavigationItem>()) {
                                    if (miniNavigationItem != miniNavigationDefaultItem) {
                                        path = miniNavigationItem.NavigationPath;
                                        //caption = miniNavigationItem.NavigationCaption;
                                        openMode = miniNavigationItem.TargetWindow;
                                        modelView = miniNavigationItem.View;

                                        IsMiniNavigationDefined = true;
                                        break; // Берём первый по расположению
                                    }
                                }
                            }
                            break;
                        }
                    }

                    // Если в модели ничего не найдено, то читаем атрибуты.
                    if (!IsMiniNavigationDefined) {

                        DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo = null;
                        objectTypeInfo = listView.CollectionSource.ObjectTypeInfo;
                        //typeObjectOfListView = objectTypeInfo.Type;

                        mAttr = objectTypeInfo.FindAttributes<MiniNavigationAttribute>();

                        // Если ничего нет, то покидаем
                        if (mAttr == null & !IsMiniNavigationDefined) return;

                        SortedDictionary<int, MiniNavigationAttribute> sd = new SortedDictionary<int, MiniNavigationAttribute>();

                        IEnumerator<MiniNavigationAttribute> _enum = mAttr.GetEnumerator();
                        while (_enum.MoveNext()) {
                            MiniNavigationAttribute attr = ((MiniNavigationAttribute)(_enum.Current));
                            sd.Add(attr.Order, attr);
                        }

                        foreach (KeyValuePair<int, MiniNavigationAttribute> kvp in sd) {
                            AttrExists = true;
                            MiniNavigationAttribute attr = kvp.Value;
                            path = attr.NavigationPath;
                            //caption = attr.NavigationCaptin;
                            openMode = attr.TargetWindow;
                            break;  // Берём первый по порядку Order (он же и тот, что по умолчанию)
                        }
                    }

                    if (AttrExists || IsMiniNavigationDefined) {

                        // Получаем объект obj для показа
                        BaseObject obj = GetObjectOnPathEnd(path, currentObject as BaseObject);

                        if (obj == null) {
                            DevExpress.XtraEditors.XtraMessageBox.Show(
                                CommonMethods.GetMessage(MiniNavigatorControllerMessages.SHOWING_OBJECT_NOT_DEFINED),
                                messageCaption,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Exclamation);
                            return;
                        }

                        Type ChoiceType = (System.Type)(obj.GetType());   //.UnderlyingSystemType;   //typeof(object);
                        string ViewID = Application.FindDetailViewId(ChoiceType); // По умолчанию - DetailView

                        Frame resultFrame = frame; // Application.CreateFrame("new");
                        //TargetWindow openMode = (TargetWindow)e.SelectedChoiceActionItem.Data;

                        // Небольшая разборка с сочетанием значений openMode в целях определения фрейма, в который грузить
                        if (MasterDetailViewFrame != null && (openMode == TargetWindow.Current & ((frame as NestedFrame) != null | !view.IsRoot))) {
                            // Тогда Current трактуем как корневое окно
                            resultFrame = MasterDetailViewFrame;
                            {
                                objectSpace = resultFrame.Application.CreateObjectSpace();   //.View.ObjectSpace;
                                object representativeObj = objectSpace.GetObject(obj);

                                View dv = null;

                                if (modelView != null) {
                                    if ((modelView as IModelDetailView) != null) {
                                        ViewID = (modelView as IModelDetailView).Id;
                                        dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                                    }
                                    else if ((modelView as IModelListView) != null) {
                                        ViewID = (modelView as IModelListView).Id;
                                        //ListView dv = resultFrame.Application.CreateListView(objectSpace, ChoiceType, true);
                                        CollectionSource colSource = new CollectionSource(objectSpace, ChoiceType);
                                        if (!colSource.IsLoaded) colSource.Reload();
                                        dv = resultFrame.Application.CreateListView((modelView as IModelListView), colSource, true) as ListView;
                                    }
                                }
                                else {
                                    // Если навигация орпделена в модели, а view не определено, то образуем DetailView по умолчанию
                                    ViewID = Application.FindDetailViewId(ChoiceType);
                                    dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                                }

                                resultFrame.SetView(dv, true, resultFrame);
                            }
                            return;
                        }

                        // Общий алгоритм
                        {
                            objectSpace = resultFrame.Application.CreateObjectSpace();
                            object representativeObj = objectSpace.GetObject(obj);

                            View dv = null;

                            if (modelView != null) {
                                if ((modelView as IModelDetailView) != null) {
                                    ViewID = (modelView as IModelDetailView).Id;
                                    dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                                }
                                else if ((modelView as IModelListView) != null) {
                                    ViewID = (modelView as IModelListView).Id;
                                    //ListView dv = resultFrame.Application.CreateListView(objectSpace, ChoiceType, true);
                                    //objectSpace = resultFrame.Application.CreateObjectSpace(); 
                                    CollectionSource colSource = new CollectionSource(objectSpace, ChoiceType);
                                    if (!colSource.IsLoaded) colSource.Reload();
                                    dv = resultFrame.Application.CreateListView((modelView as IModelListView), colSource, true) as ListView;
                                }
                            }
                            else {
                                // Если навигация определена в модели, а view не определено, то образуем DetailView по умолчанию
                                ViewID = Application.FindDetailViewId(ChoiceType);
                                dv = resultFrame.Application.CreateDetailView(objectSpace, ViewID, true, representativeObj) as DetailView;
                            }

                            ShowViewParameters svp = new ShowViewParameters();
                            svp.CreatedView = dv;
                            svp.TargetWindow = openMode;
                            svp.Context = TemplateContext.View;
                            svp.CreateAllControllers = true;

                            e.InnerArgs.ShowViewParameters.Assign(svp);
                            return;
                        }

                    }

                }
            }


            // ПРОЧИЕ LISTVIEW
            // Грузим в отдельном окне

            e.Handled = false;
            openMode = TargetWindow.NewWindow;
            if (Frame as NestedFrame != null | !View.IsRoot) openMode = TargetWindow.NewModalWindow;
            e.InnerArgs.ShowViewParameters.TargetWindow = openMode;
        }

        #endregion


        #region IMasterDetailViewInfo Members

        private string masterDetailViewIdCore = String.Empty;
        public Frame masterDetailViewFrameCore = null;
        //public View masterViewCore = null;

        public string MasterDetailViewId {
            get { return masterDetailViewIdCore; }
        }

        public Frame MasterDetailViewFrame {
            get { return masterDetailViewFrameCore; }
        }

        //public View MasterView {
        //    get { return masterViewCore; }
        //}

        public void AssignMasterDetailViewId(string id) {
            masterDetailViewIdCore = id;
        }

        public void AssignMasterDetailViewFrame(Frame frame) {
            masterDetailViewFrameCore = frame;
        }

        //public void AssignMasterView(View view) {
        //    masterViewCore = view;
        //}

        #endregion

    }
}
