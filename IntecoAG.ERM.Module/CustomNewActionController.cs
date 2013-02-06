using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;

namespace IntecoAG.ERM.Module {

    // Чтобы смотреть работу контроллера в нестандартном режиме, надо переменной Experiment установить значение true

    // В данном образце контроллера при Experiment = true в методе ObjectCreating будет создаваться объект 
    // crmSimpleContract, на выходе присваиваться crmSimpleContractVersion. После этого все остальные меоды, если не менять их 
    // так и будут работать для crmSimpleContractVersion
    // В CustomNewActionController_CustomAddObjectToCollection можно добавить в коллекцию объектов того или иного типа нужные объекты, возможно тут же созданные
    // В CustomNewActionController_ObjectCreated можно изменить созданный объект
    // В Application_DetailViewCreating можно выбрать подходящее View для показа созданного объекта
    // В CustomNewActionController_Execute можно сделать всё, что угодно с View или самим показываемым объектом, можно
    // забыть о созданном объекте и показать совершенно другой объект и т.п.

    
    public partial class CustomNewActionController : WindowController {

        // Признак эксперимента с NewObjectViewController
        bool Experiment = false;


        //WinNewObjectViewController wovc = null;
        NewObjectViewController wovc = null;

        //object newedObject;

        //ShowNavigationItemController snic = null;

        public CustomNewActionController() {
            InitializeComponent();
            RegisterActions(components);

            //this.Active["CustomNewActionController_Enabled"] = false;
        }

        protected override void OnActivated() {
            base.OnActivated();

            //snic = Window.GetController<ShowNavigationItemController>();
            //if (snic != null) snic.ShowNavigationItemAction.SelectedItemChanged += new EventHandler(ShowNavigationItemAction_SelectedItemChanged);

            //wovc = Window.GetController<WinNewObjectViewController>();
            wovc = Frame.GetController<NewObjectViewController>();
            if (wovc == null) return;

            wovc.CollectCreatableItemTypes += new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            wovc.CollectDescendantTypes += new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);

            // ProcessCreatedView происходит после того как произойдёт wovc.NewObjectAction.Execute, и к его выполнению уже формируется
            // тот вью, который номинально приписан кнопке, т.о. будет вместе с crmContractVersion открываться и crmContract, что не нужно,
            // Поэтому надо перехватывать Execute
            //wovc.NewObjectAction.ProcessCreatedView

            wovc.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);
            
            // 2011-08-08. Задача. Перехват 3-х событий.

            // Список событий, помечены интересные Павлу.
            //wovc.Activated
            //wovc.AfterConstruction
            //wovc.CollectCreatableItemTypes
            //wovc.CollectDescendantTypes
            //wovc.CustomAddObjectToCollection
            //wovc.Disposed
            //wovc.FrameAssigned
            // * wovc.ObjectCreated
            // * wovc.ObjectCreating
            //wovc.ViewControlsCreated

            // Проверка на вложенность фрейма или представления:
            //if (Frame as NestedFrame != null | !View.IsRoot) {
            //    objectSpace = view.ObjectSpace.CreateNestedObjectSpace();
            //    openMode = TargetWindow.NewModalWindow;
            //} else {
            //    objectSpace = Application.CreateObjectSpace();
            //    openMode = TargetWindow.NewWindow;
            //}

            // Замечание. Если открывать объект из списка во вложенном представлении или фрейме, то надо использовать Nested ObjectSpace

            // Если надо открыть объект в отдельном ObjectSpace, его надо в нег передать, а затем вернуть в исходный ObjectSpace.

            if (Experiment) {
                // Подписка на события
                // Не перехватывается wovc.FrameAssigned += new EventHandler(CustomNewActionController_FrameAssigned);
                // Используем только для очень особых действий (разговор от 2011-08-10) 
                wovc.ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
                wovc.CustomAddObjectToCollection += new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
                wovc.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);
            }
            wovc.NewObjectAction.ShowItemsOnClick = !(wovc.NewObjectAction.Items.Count < 2);
        }

        #region Experiment

        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
        }

        protected override void OnAfterConstruction() {
            base.OnAfterConstruction();
        }

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
        }

        protected override void OnWindowChanging(Window window) {
            base.OnWindowChanging(window);
        }

        protected override void SubscribeToViewEvents(View view) {
            if (!Experiment) {
                base.SubscribeToViewEvents(view);
                return;
            }

            Frame frame = Frame;

            /*
            // Наверное, здесь можно подменить view и создаваемый или показываемый объект
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            object wp = objectSpace.CreateObject(typeof(crmWorkPlan));
            string DetailViewId = frame.Application.FindDetailViewId(wp.GetType());
            //TargetWindow openMode = TargetWindow.Current;

            DetailView newView = frame.Application.CreateDetailView(objectSpace, DetailViewId, true, wp);
            frame.SetView(newView, true, frame);

            base.SubscribeToViewEvents(newView);
            */
            
            base.SubscribeToViewEvents(view);

            frame.Application.DetailViewCreating += new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);
            //view.InfoChanging += new EventHandler(view_InfoChanging);
            //view.InfoChanged += new EventHandler(view_InfoChanged);
        }

        protected override void UnsubscribeFromViewEvents(View view) {
            if (!Experiment) {
                base.UnsubscribeFromViewEvents(view);
                return;
            }

            //view.InfoChanging += new EventHandler(view_InfoChanging);
            //view.InfoChanged += new EventHandler(view_InfoChanged);
            Frame.Application.DetailViewCreating -= new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);
            //Frame.Application.ViewShowing +=new EventHandler<ViewShowingEventArgs>(Application_ViewShowing);

            base.UnsubscribeFromViewEvents(view);
        }

        private void Application_DetailViewCreating(object sender, DetailViewCreatingEventArgs e) {

            // Образец: как менять представления в зависимости от условий
            //((XafApplication)sender).DetailViewCreating -= new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);
            //if (e.Obj is Transaction && Frame is NestedFrame) {
            //    Type parentType = ((NestedFrame)Frame).ViewItem.ObjectType;
            //    if (typeof(Consumer).IsAssignableFrom(parentType)) {
            //        e.ViewID = "Transaction_DetailView_FromConsumer";
            //    }
            //    if (typeof(Producer).IsAssignableFrom(parentType)) {
            //        e.ViewID = "Transaction_DetailView_FromProducer";
            //    }
            //}
            //...


            //Frame.Application.DetailViewCreating -= new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);
            ((XafApplication)sender).DetailViewCreating -= new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);
            
            Frame frame = Frame;

            //IObjectSpace objectSpace = Application.CreateObjectSpace();
            //object wp = objectSpace.CreateObject(typeof(crmWorkPlan));
            //string DetailViewId = frame.Application.FindDetailViewId(wp.GetType());


            if (false) { // Проверка 1
                /*  Закоменчено, т.к. убраны объекты на которых проводилось испытание
                crmSimpleContract sc = e.Obj as crmSimpleContract;
                if (sc != null) {
                    object scv = sc.CurrentVersion;
                    string DetailViewId = frame.Application.FindDetailViewId(scv.GetType());

                    TemplateContext tc = new TemplateContext();
                    Frame newFrame = frame.Application.CreateFrame(tc);

                    //IObjectSpace objectSpace = Application.CreateObjectSpace();
                    IObjectSpace objectSpace = e.ObjectSpace;

                    //object shownObject = objectSpace.GetObject(scv);
                    object shownObject = scv;

                    DetailView newView = newFrame.Application.CreateDetailView(objectSpace, DetailViewId, false, shownObject);
                    newFrame.SetView(newView, true, frame);

                    //e.ObjectSpace = objectSpace;
                    //.View = newView;

                    DetailViewCreatingEventArgs args = new DetailViewCreatingEventArgs(DetailViewId, objectSpace, scv, false);

                    args.View = newView;
                    e = args;
                }
                */
            }


            //if (false) {
                
            //    // Тип объекта, который надо обработать
            //    Type ChoiceType = e.Obj.GetType();   //((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;   //typeof(object);
            //    //if ((view.ObjectTypeInfo).Type == typeof(crmContractImplementation)) {
            //    //    ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;
            //    //}
            //    //if (ChoiceType == typeof(object)) {
            //    //    ChoiceType = (view.ObjectTypeInfo).Type;
            //    //}


            //    // Если поддерживается интерфейс:
            //    //if ((view.ObjectTypeInfo).Type.GetInterface("IVersionMainObject") != null) {
            //    if (ChoiceType.GetInterface("IVersionMainObject") != null) {
            //        IObjectSpace objectSpace = Application.CreateObjectSpace();

            //        //object obj = objectSpace.CreateObject((view.ObjectTypeInfo).Type);
            //        object obj = objectSpace.CreateObject(ChoiceType);
            //        IVersionMainObject mainObj = obj as IVersionMainObject;
            //        if (mainObj == null) return;

            //        VersionRecord objCurrent = mainObj.GetCurrent();
            //        if (objCurrent == null) return;

            //        // Определяем DetailView
            //        string DetailViewId = frame.Application.FindDetailViewId(objCurrent.GetType());

            //        // Запрет стандартного поведения
            //        //e.Action.Active[DesableStandartNewAction] = false;
            //        //e.ShowViewParameters.CreatedView = null;
            //        e.ViewID = DetailViewId;

            //        //// Показ:
            //        //TargetWindow openMode = TargetWindow.NewWindow;
            //        //CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, objCurrent, openMode);

            //        //if (openMode == TargetWindow.NewModalWindow && (view as ListView) != null) ((ListView)view).CollectionSource.Reload();
            //    }
            //}


            //Frame.Application.DetailViewCreating += new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);
            ((XafApplication)sender).DetailViewCreating += new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);
        }

        private void view_InfoChanging(object sender, EventArgs e) {

        }

        private void view_InfoChanged(object sender, EventArgs e) {

        }

        private void CustomNewActionController_FrameAssigned(object sender, EventArgs e) {
            //NewObjectViewController standardController = Frame.GetController<NewObjectViewController>();
            //standardController.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);
        }

        /// <summary>
        /// Этот метод позволяет создать другой объект, чем тот, что задаётся выбором пункта меню или подменю New.
        /// Однако, не удаётся пока передать этот объект дальше в метод CustomNewActionController_CustomAddObjectToCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomNewActionController_ObjectCreating(object sender, ObjectCreatingEventArgs e) {

            if (true) {   // Првоерка 1. Замена свойства аргумента e
                /*  Закоменчено, т.к. убраны объекты на которых проводилось испытание
                //e.Cancel
                //e.ShowDetailView

                //e.ObjectSpace
                e.ObjectSpace = Application.CreateObjectSpace();

                //e.NewObject - подмена создаваемого объекта. Для простоты проверки всегда меняется на один и тот же по типу
                // В данном случае crmSimpleContract, т.е. в любом месте интерфейса, где нажата кнопка New, будет создаваться
                // новый crmSimpleContract. В методе DetailViewCreating для созданного объекта будет проверяться версионность
                // и запускать создание версионной разновидности объекта, т.е. crmSimpleContractVersion

                crmSimpleContract sc = e.ObjectSpace.CreateObject<crmSimpleContract>();
                if (sc == null && sc.Current == null) return;
                e.NewObject = sc.Current;   // e.ObjectSpace.CreateObject<crmSimpleContract>();
                */
            }

            //if (false) {   // Првоерка 0.
            //    // Запрет/разрешение стандартного создания объекта
            //    e.Cancel = false;
            //    //e.ShowDetailView = false;  // DetailView не будет показан


            //    #region Замена типа создаваемого объекта или только ObjectSpace

            //    /*
            //    // Можно заменить ObjectSpace
            //    objectSpace = Application.CreateObjectSpace();
            //    e.ObjectSpace = objectSpace;

            //    // Можно заменить аргументы события, что позволяет создать любой другой объект.
            //    // CustomNewActionController_Execute
            //    ObjectCreatingEventArgs args = new ObjectCreatingEventArgs(objectSpace, typeof(crmWorkPlan));

            //    // Запрет/разрешение стандартного создания объекта
            //    args.Cancel = false;
            //    //args.ShowDetailView = false;  // DetailView не будет показан
            //    e = args;
            //    */

            //    #endregion
            //}

            //if (false) {   // Првоерка 2. Пробуем заменить свойства аргумента e
            //    // Без замены параметра e заменить тип создаваемого объекта не получилось
            //    IObjectSpace objectSpace = e.ObjectSpace;
            //    object newObj = objectSpace.CreateObject(e.ObjectType);
            //    //newedObject = newObj;

            //    // Здесь можно что-нибудь сделать с новосозданным объектом.
            //    /*
            //    crmWorkPlan wpObj = newObj as crmWorkPlan;
            //    if (wpObj != null) {
            //        crmWorkPlanVersion wpvObj = wpObj.Current as crmWorkPlanVersion;
            //        if (wpObj != null && wpvObj != null) {
            //            //wpObj.Current.crmContractDocument.Number = "№ " + System.DateTime.Now.ToString();
            //            wpvObj.Description = System.DateTime.Now.ToString();
            //        }
            //    }

            //    FM.Order.fmOrder orderObj = newObj as FM.Order.fmOrder;
            //    if (orderObj != null) {
            //        orderObj.Name = "Order № " + System.DateTime.Now.ToString();
            //    }
            //    */

            //    // объект передаётся в CustomNewActionController_CustomAddObjectToCollection
            //    e.NewObject = newObj;

            //    //CollectionSource colSource = new CollectionSource(objectSpace, typeof(crmWorkPlan));
            //    //if (!colSource.IsLoaded) colSource.Reload();

            //    //ProcessNewObjectEventArgs pnoArgs = new ProcessNewObjectEventArgs(newObj, objectSpace, colSource);
            //    //CustomNewActionController_CustomAddObjectToCollection(this, pnoArgs);
            //}            
            
        }

        private void CustomNewActionController_CustomAddObjectToCollection(object sender, ProcessNewObjectEventArgs e) {

            //if (true) {
            //}

            //if (false) {
            //    // Созданный в методе CustomNewActionController_ObjectCreating объект передаётся в этот метод параметром
            //    // и можно включить его в коллекцию объектов его типа своими силами

            //    Frame frame = Frame;
            //    View view = frame.View;

            //    // Добавление своими силами

            //    // Отключаем стандартный способ
            //    e.Handled = true;

            //    //e.NewObject = newedObject;
            //    //e.CurrentCollectionSource = new CollectionSource(e.ObjectSpace, newedObject.GetType());

            //    //DevExpress.XtraEditors.XtraMessageBox.Show("Начальное количество объектов (e.ObjectSpace): " + e.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("Начальное количество объектов (view.ObjectSpace): " + view.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());

            //    // e.ObjectSpace и view.ObjectSpace - разные

            //    IObjectSpace nestedObjectSpace = view.ObjectSpace.CreateNestedObjectSpace();

            //    object newObj = nestedObjectSpace.GetObject(e.NewObject);
            //    e.CurrentCollectionSource.Add(newObj);

            //    //e.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null);

            //    e.ObjectSpace.GetObject(newObj);

            //    //DevExpress.XtraEditors.XtraMessageBox.Show("Количество объектов после добавления (e.ObjectSpace): " + e.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("Количество объектов после добавления (view.ObjectSpace): " + view.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("Количество объектов для сохранения (e.ObjectSpace): " + e.ObjectSpace.GetObjectsToSave(false).Count.ToString());
            //}
        }

        private void CustomNewActionController_ObjectCreated(object sender, ObjectCreatedEventArgs e) {

            // В контексте данного метода объект создан и можно проделать попутные действия, например, статистический 
            // учёт создаваемых объектов и т.п.
            //if (false) {
            //    //((Task)e.CreatedObject).StartDate = DateTime.Now;

            //    Frame frame = Frame;
            //    View view = frame.View;

            //    IObjectSpace objectSpace = e.ObjectSpace;

            //    /*
            //    #region Замена типа создаваемого объекта или только ObjectSpace - не канает
            
            //    //// Можно заменить ObjectSpace
            //    //objectSpace = Application.CreateObjectSpace();
            //    //e.ObjectSpace = objectSpace;
            //    //e.CreatedObject = objectSpace.CreateObject(typeof(crmSimpleContract));

            //    object newObj = objectSpace.CreateObject(typeof(crmSimpleContract));
            //    ObjectCreatedEventArgs args = new ObjectCreatedEventArgs(newObj, objectSpace);
            //    ((crmSimpleContract)(args.CreatedObject)).DateSign = DateTime.Now;
            //    e = args;

            //    #endregion
            //    */
            //}
        }

        #endregion



        protected override void OnDeactivated() {
            //if (snic != null) snic.ShowNavigationItemAction.SelectedItemChanged -= new EventHandler(CustomNewActionController_SelectedItemChanged);

            if (wovc == null) {
                base.OnDeactivated();
                return;
            }
            wovc.CollectCreatableItemTypes -= new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            wovc.CollectDescendantTypes -= new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);
            
            wovc.NewObjectAction.Execute -= new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);


            if (Experiment) {
                // Отписка от событий
                // Не перехватывается wovc.FrameAssigned -= new EventHandler(CustomNewActionController_FrameAssigned);
                // Используем только для очень особых действий (разговор от 2011-08-10) 
                wovc.ObjectCreating -= new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
                wovc.CustomAddObjectToCollection -= new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
                wovc.ObjectCreated -= new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);
            }

            base.OnDeactivated();
        }

        void CustomNewActionController_Execute(object sender, ActionBaseEventArgs e) {

            // Этот метод воспринимает то, что образовано в методах ObjectCreating и DetailViewCreating
            // и выполняет действия этого метода.
            // На самом деле, в этом методе можно сделать всё, что угодно, но при этом лучше отключать при 
            // условии создания нового объекта действия вышеуказанных методов.

            //if (false) {
            //    Frame frame = Frame;
            //    if (frame == null) return;

            //    View view = frame.View;
            //    if (view == null) return;

            //    // View Id:
            //    string ViewId = view.Id;   // ((DevExpress.ExpressApp.ViewShortcut)(args.SelectedChoiceActionItem.Data)).ViewId;

            //    //IModelView modelView = Application.FindModelView(ViewId) as IModelView;
            //    //IModelDetailView modelDetailView = Application.FindModelView(ViewId) as IModelDetailView;
            //    //IModelListView modelListView = Application.FindModelView(ViewId) as IModelListView;

            //    //if (modelView == null) return;

            //    //ITypeInfo typeInfo = ((((IModelView)modelView).AsObjectView).ModelClass).TypeInfo; // is IntecoAG.ERM.CS.IVersionMainObject
            //    //ITypeInfo typeInfoDetail = (modelDetailView != null) ? ((((IModelDetailView)modelDetailView).AsObjectView).ModelClass).TypeInfo : null;
            //    //ITypeInfo typeInfoList = (modelListView != null) ? ((((IModelListView)modelListView).AsObjectView).ModelClass).TypeInfo : null;

            //    // Тип объекта, который надо обработать
            //    Type ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;   //typeof(object);
            //    //if ((view.ObjectTypeInfo).Type == typeof(crmContractImplementation)) {
            //    //    ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;
            //    //}
            //    //if (ChoiceType == typeof(object)) {
            //    //    ChoiceType = (view.ObjectTypeInfo).Type;
            //    //}


            //    // Если поддерживается интерфейс:
            //    //if ((view.ObjectTypeInfo).Type.GetInterface("IVersionMainObject") != null) {
            //    if (ChoiceType.GetInterface("IVersionMainObject") != null) {
            //        IObjectSpace objectSpace = Application.CreateObjectSpace();

            //        //object obj = objectSpace.CreateObject((view.ObjectTypeInfo).Type);
            //        object obj = objectSpace.CreateObject(ChoiceType);
            //        IVersionMainObject mainObj = obj as IVersionMainObject;
            //        if (mainObj == null) return;

            //        VersionRecord objCurrent = mainObj.GetCurrent();
            //        if (objCurrent == null) return;

            //        // Определяем DetailView
            //        string DetailViewId = frame.Application.FindDetailViewId(objCurrent.GetType());

            //        // Запрет стандартного поведения
            //        //e.Action.Active[DesableStandartNewAction] = false;
            //        e.ShowViewParameters.CreatedView = null;

            //        // Показ:
            //        TargetWindow openMode = TargetWindow.NewWindow;
            //        CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, objCurrent, openMode);

            //        if (openMode == TargetWindow.NewModalWindow && (view as ListView) != null) ((ListView)view).CollectionSource.Reload();
            //    }

            //    // bool isPers = (((((IModelDetailView)modelDetailView).AsObjectView).ModelClass).TypeInfo).IsPersistent;
            //}
        }

        #region "Вставка от Павла"

        void CustomNewActionController_CollectDescendantTypes(object sender, DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs e) {
            //CustomizeList(e.Types);
        }

        void CustomNewActionController_CollectCreatableItemTypes(object sender, DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs e) {
            CustomizeList(e.Types);
        }

        void CustomNewActionController_SelectedItemChanged(object sender, EventArgs e) {
            // ? wovc.UpdateActionState();
        }

        public void CustomizeList(ICollection<Type> types) {
            //if (!types.Contains(typeof(crmContractNewForm)))
            //    types.Add(typeof(crmContractNewForm));

            if (!types.Contains(typeof(crmDealRegistrationForm)))
                types.Add(typeof(crmDealRegistrationForm));

            if (!types.Contains(typeof(crmContractRegistrationForm)))
                types.Add(typeof(crmContractRegistrationForm));

            /*
            ////Get the ShowNavigationItemController, then get its ShowNavigationItemAction
            //SingleChoiceAction showNavigationItemAction =
            //    Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction;
            ////Get the item selected in the navigation control
            //ChoiceActionItem selectedItem = showNavigationItemAction.SelectedItem;
            //ChoiceActionItem currentGroup = null;
            //if(selectedItem != null) {
            //    //Get the selected item's parent group
            //    currentGroup = selectedItem.ParentItem;
            //    List<Type> unusableTypes = new List<Type>();
            //    //Collect the types that must be deleted
            //    foreach(Type type in types) {
            //        bool deletionRequired = true;
            //        foreach(ChoiceActionItem item in currentGroup.Items) {
            //            ViewShortcut shortcut = item.Data as ViewShortcut;
            //            if(shortcut.ViewId == Application.FindListViewId(type)) {
            //                deletionRequired = false;
            //            }
            //        }
            //        if(deletionRequired == true)
            //            unusableTypes.Add(type);
            //    }
            //    //Remove the collected types
            //    foreach(Type type in unusableTypes)
            //        types.Remove(type);
            //}
            */
        }

        #endregion


/*
        private void ShowNavigationItemAction_SelectedItemChanged(object sender, EventArgs e) {

            // Отсечка по признаку NonPersistent
            // ObjectTypeInfo - Obsolete
            //NonPersistentAttribute attr = snic.Frame.View.ObjectTypeInfo.FindAttribute<NonPersistentAttribute>();
            //if (attr == null) return;


            Frame frame = Frame;

            //////IObjectSpace objectSpace = Application.CreateObjectSpace();

            //////crmContractNewForm newObj = objectSpace.CreateObject<crmContractNewForm>();

            //////// Определяем DetailView
            //////string DetailViewId = frame.Application.FindDetailViewId(newObj.GetType());
            
            //////// Показываем:
            //////TargetWindow openMode = TargetWindow.Current;
            //////CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newObj, openMode);

        }
*/

    }
}
