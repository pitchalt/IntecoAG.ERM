using System;
using System.Runtime;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.XtraNavBar;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {
    //public partial class ReplaceWinNewObjectViewController : WindowController {
    //    public ReplaceWinNewObjectViewController() {
    //        InitializeComponent();
    //        RegisterActions(components);
    //    }
    //}

    /// <summary>
    /// http://www.devexpress.com/Support/Center/p/K18080.aspx
    /// http://www.devexpress.com/Support/Center/e/E229.aspx
    /// http://documentation.devexpress.com/#Xaf/CustomDocument2915
    /// http://documentation.devexpress.com/#Xaf/CustomDocument2920
    /// </summary>
    public partial class ReplaceWinNewObjectViewController : WinNewObjectViewController {

        public ReplaceWinNewObjectViewController() {
            //InitializeComponent();
            //RegisterActions(components);

            //TargetViewType = ViewType.ListView;
            //TargetObjectType = typeof(SimpleContract);
        }

        //public ReplaceWinNewObjectViewController() {
        //    //TargetViewType = ViewType.ListView;
        //    //TargetObjectType = typeof(SimpleContract);
        //}

        //Subscribe the required events 
        protected override void OnActivated() {
            base.OnActivated();

            // <<< Вставка от Павла:
            //Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.SelectedItemChanged += new EventHandler(ShowNavigationItemAction_SelectedItemChanged);
            //CollectCreatableItemTypes += new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(MyController_CollectCreatableItemTypes);
            //CollectDescendantTypes += new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(MyController_CollectDescendantTypes);
            // >>>

        }

        /*
        #region "Вставка от Павла"

        void MyController_CollectDescendantTypes(object sender, DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs e) {
            //CustomizeList(e.Types);
        }
        
        void MyController_CollectCreatableItemTypes(object sender, DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs e) {
            CustomizeList(e.Types);
        }
        
        void ShowNavigationItemAction_SelectedItemChanged(object sender, EventArgs e) {
            this.UpdateActionState();
        }
        
        public void CustomizeList(ICollection<Type> types) {
            if (!types.Contains(typeof(crmContractNewForm)))
                types.Add(typeof(crmContractNewForm));
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
        }

        #endregion
        */

        protected override void OnDeactivated() {

            // <<< Вставка от Павла:
            //Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.SelectedItemChanged -= new EventHandler(ShowNavigationItemAction_SelectedItemChanged);
            //CollectCreatableItemTypes -= new EventHandler<CollectTypesEventArgs>(MyController_CollectCreatableItemTypes);
            //CollectDescendantTypes -= new EventHandler<CollectTypesEventArgs>(MyController_CollectDescendantTypes);
            // >>>

            base.OnDeactivated();
        }

        private void ObjectSpace_Disposed(object sender, EventHandler e) {
            IObjectSpace objectSpace = (ObjectSpace)sender;
            objectSpace.CommitChanges();
            base.Dispose();
        }

        /*
        protected override void New(SingleChoiceActionExecuteEventArgs args) {
            View view = View;
            Frame frame = Frame;

            if (view == null) { base.New(args); return; }
            if (view.GetType() != typeof(ListView)) { base.New(args); return; }
            //if (
            //    (((ListView)view).ObjectTypeInfo.Type != typeof(SimpleContract))
            //    & (((ListView)view).ObjectTypeInfo.Type != typeof(ComplexContract))
            //    & (((ListView)view).ObjectTypeInfo.Type != typeof(WorkPlan))
            //    ) 
            //{
            //    base.New(args);
            //    return;
            //}

            //Type nType = ((System.RuntimeType)(args.SelectedChoiceActionItem.Data)).UnderlyingSystemType;
            //Type dataType = (args.SelectedChoiceActionItem.Data).GetType();

            string ChoiceId = "";

            if (((ListView)view).ObjectTypeInfo.Type == typeof(ContractImplementation)) {
                ChoiceId = args.SelectedChoiceActionItem.Id;
            }

            if (((ListView)view).ObjectTypeInfo.Type == typeof(SimpleContract) || ChoiceId == "SimpleContract") {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                SimpleContract objSimpleContract = objectSpace.CreateObject<SimpleContract>();
                SimpleContractVersion objSimpleContractVersion = (SimpleContractVersion)objSimpleContract.Current;

                TargetWindow openMode = TargetWindow.NewWindow;
                ShowConcreteDetailView<SimpleContractVersion>(frame, objectSpace, "SimpleContractVersion_DetailView", objSimpleContractVersion, openMode);

                //TargetWindow openMode = TargetWindow.NewModalWindow;
                //ShowConcreteDetailView(frame, objectSpace, "SimpleContractVersion_DetailView", objSimpleContractVersion, openMode, ref args);
                //base.New(args);

                if (openMode == TargetWindow.NewModalWindow) ((ListView)view).CollectionSource.Reload();

                return;
            }

            if (((ListView)view).ObjectTypeInfo.Type == typeof(ComplexContract) || ChoiceId == "ComplexContract") {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                ComplexContract objComplexContract = objectSpace.CreateObject<ComplexContract>();
                ComplexContractVersion objComplexContractVersion = (ComplexContractVersion)objComplexContract.Current;

                TargetWindow openMode = TargetWindow.NewWindow;
                ShowConcreteDetailView<ComplexContractVersion>(frame, objectSpace, "ComplexContractVersion_DetailView", objComplexContractVersion, openMode);

                if (openMode == TargetWindow.NewModalWindow) ((ListView)view).CollectionSource.Reload();
      
                return;
            }

            if (((ListView)view).ObjectTypeInfo.Type == typeof(WorkPlan) || ChoiceId == "WorkPlan") {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                WorkPlan objWorkPlan = objectSpace.CreateObject<WorkPlan>();
                WorkPlanVersion objWorkPlanVersion = (WorkPlanVersion)objWorkPlan.Current;

                TargetWindow openMode = TargetWindow.NewWindow;
                ShowConcreteDetailView<WorkPlanVersion>(frame, objectSpace, "WorkPlanVersion_DetailView", objWorkPlanVersion, openMode);

                if (openMode == TargetWindow.NewModalWindow) ((ListView)view).CollectionSource.Reload();

                return;
            }


            if (((ListView)view).ObjectTypeInfo.Type == typeof(WorkPlanVersion) & view.Id == "ComplexContractVersion_WorkPlanVersions_ListView" & frame as NestedFrame != null) {

                // След. строка - способ из вложенного фрейма получить View из мастер-фрейма
                CompositeView parentView = ((NestedFrame)this.Frame).DetailViewItem.View;
                ComplexContractVersion ccv = parentView.CurrentObject as ComplexContractVersion;

                if (ccv != null) {
                    //IObjectSpace objectSpace = view.ObjectSpace.CreateNestedObjectSpace();   // Application.CreateObjectSpace();
                    IObjectSpace objectSpace = Application.CreateObjectSpace();

                    // Получаем версию контракта в новом ObjectSpace
                    ComplexContractVersion ccvObj = objectSpace.GetObject<ComplexContractVersion>(ccv);

                    WorkPlan objWorkPlan = objectSpace.CreateObject<WorkPlan>();
                    WorkPlanVersion objWorkPlanVersion = (WorkPlanVersion)objWorkPlan.Current;

                    // Делаем привязку к версии ComplexContract
                    objWorkPlanVersion.ComplexContractVersion = ccvObj;
                    ccvObj.WorkPlanVersions.Add(objWorkPlanVersion);

                    // Показываем
                    TargetWindow openMode = TargetWindow.NewModalWindow;
                    ShowConcreteDetailView<WorkPlanVersion>(frame, objectSpace, "WorkPlanVersion_DetailView", objWorkPlanVersion, openMode);

                    if (openMode == TargetWindow.NewModalWindow) {

                        WorkPlan newObjWorkPlan = view.ObjectSpace.GetObject<WorkPlan>((WorkPlan)objWorkPlan);
                        WorkPlanVersion newObjWorkPlanVersion = view.ObjectSpace.GetObject<WorkPlanVersion>((WorkPlanVersion)objWorkPlanVersion);

                        //((ListView)view).ObjectSpace.CommitChanges();

                        // След. строка - способ из вложенного фрейма получить View из мастер-фрейма
                        ((ListView)view).CollectionSource.Reload();
                        //CompositeView pView = ((NestedFrame)this.Frame).DetailViewItem.View;
                        //((DetailView)pView).ObjectSpace.Refresh();
                    }
                } else {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Не доступна версия контраста для привязки к ней новой версии рабочего плана");
                }

                return;
            }

            //


            if (frame as NestedFrame != null | !view.IsRoot) args.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;

            base.New(args);
        }
        */

        #region Методы показа View

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void SetConcreteListView<classType>(Frame frame) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //SimpleContract objCurrent1 = objectSpace1.GetObject<SimpleContract>((SimpleContract)currentObject);
            ListView lv = frame.Application.CreateListView(objectSpace, typeof(classType), true);
            frame.SetView(lv, frame);
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailViewInWindow(Frame frame, Type objType, string DetailViewID, object currentObject, TargetWindow tw) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = frame.View.ObjectSpace.CreateNestedObjectSpace();
            object objCurrent = objectSpace.GetObject(currentObject);
            //objType ob = objCurrent as objType;
            if (objCurrent == null) objCurrent = objectSpace.CreateObject(objType);

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = tw;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void SetConcreteDetailView<classType>(Frame frame, classType Obj, string DeatilViewId) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DeatilViewId, true, objCurrent);
            frame.SetView(dv, frame);
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void SetConcreteDetailView<classType>(Frame frame, IObjectSpace objectSpace, classType Obj, string DeatilViewId) {
            //IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //classType objCurrent = objectSpace.GetObject<classType>(Obj);
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DeatilViewId, true, Obj);
            frame.SetView(dv, frame);
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteShortCutDetailView<classType>(Frame frame, classType Obj, TargetWindow tw) {
            BaseObject obj = Obj as BaseObject;
            if (obj == null) return;

            ShowViewParameters svp = new ShowViewParameters();
            ViewShortcut shortcut = new ViewShortcut(frame.Application.FindDetailViewId(typeof(classType)), obj.Oid);
            svp.CreatedView = frame.Application.ProcessShortcut(shortcut);

            //Specify various display settings.
            svp.TargetWindow = tw;   // TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(Frame frame, string DetailViewID, classType Obj, TargetWindow tw) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = frame.View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);
            
            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = tw;  // TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView(Frame frame, IObjectSpace objectSpace, string DetailViewID, object Obj, TargetWindow tw, ref SingleChoiceActionExecuteEventArgs args) {
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, Obj);
            args.ShowViewParameters.CreatedView = dv;
            args.ShowViewParameters.TargetWindow = tw;
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(Frame frame, IObjectSpace objectSpace, string DetailViewID, classType Obj, TargetWindow tw) {
            //IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = frame.View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = Obj;   // objectSpace.GetObject<classType>(Obj);
            //if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);
            
            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = tw;  // TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }


        /// <summary>
        /// Загрузка указанного параметром типа ListView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteListView<classType>(Frame frame, TargetWindow tw) {
            if (frame.Application == null) return;
            IObjectSpace objectSpaceList = frame.Application.CreateObjectSpace();
            ListView lv = frame.Application.CreateListView(objectSpaceList, typeof(classType), true);

            ShowViewParameters svpList = new ShowViewParameters();
            svpList.CreatedView = lv;

            //Specify various display settings.
            svpList.TargetWindow = tw;   // TargetWindow.Current;
            svpList.Context = TemplateContext.View;
            svpList.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svpList, new ShowViewSource(frame, null));
        }
        
        #endregion

    }

}
