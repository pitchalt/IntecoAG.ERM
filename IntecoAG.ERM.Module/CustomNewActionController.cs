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

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {
    public partial class CustomNewActionController : WindowController {

        WinNewObjectViewController wovc = null;
        ShowNavigationItemController snic = null;

        public CustomNewActionController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            snic = Window.GetController<ShowNavigationItemController>();
            if (snic != null) snic.ShowNavigationItemAction.SelectedItemChanged += new EventHandler(ShowNavigationItemAction_SelectedItemChanged);

            wovc = Window.GetController<WinNewObjectViewController>();
            if (wovc == null) return;

            wovc.CollectCreatableItemTypes += new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            wovc.CollectDescendantTypes += new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);

            // ProcessCreatedView происходит после того как произойдёт wovc.NewObjectAction.Execute, и к его выполнению уже формируется
            // тот вью, который номинально приписан кнопке, т.о. будет вместе с ContractVersion открываться и Contract, что не нужно,
            // Поэтому надо перехватывать Execute
            //wovc.NewObjectAction.ProcessCreatedView

            wovc.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);
        }

        protected override void OnDeactivated() {
            if (snic != null) snic.ShowNavigationItemAction.SelectedItemChanged -= new EventHandler(CustomNewActionController_SelectedItemChanged);

            if (wovc == null) {
                base.OnDeactivated();
                return;
            }
            wovc.CollectCreatableItemTypes -= new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            wovc.CollectDescendantTypes -= new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);
            wovc.NewObjectAction.Execute -= new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);

            base.OnDeactivated();
        }


        void CustomNewActionController_Execute(object sender, ActionBaseEventArgs e) {
            Frame frame = Frame;
            if (frame == null) return;

            View view = frame.View;
            if (view == null) return;

            // View Id:
            string ViewId = view.Id;   // ((DevExpress.ExpressApp.ViewShortcut)(args.SelectedChoiceActionItem.Data)).ViewId;

            //IModelView modelView = Application.FindModelView(ViewId) as IModelView;
            //IModelDetailView modelDetailView = Application.FindModelView(ViewId) as IModelDetailView;
            //IModelListView modelListView = Application.FindModelView(ViewId) as IModelListView;

            //if (modelView == null) return;

            //ITypeInfo typeInfo = ((((IModelView)modelView).AsObjectView).ModelClass).TypeInfo; // is IntecoAG.ERM.CS.IVersionMainObject
            //ITypeInfo typeInfoDetail = (modelDetailView != null) ? ((((IModelDetailView)modelDetailView).AsObjectView).ModelClass).TypeInfo : null;
            //ITypeInfo typeInfoList = (modelListView != null) ? ((((IModelListView)modelListView).AsObjectView).ModelClass).TypeInfo : null;

            // Тип объекта, который надо обработать
            Type ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;   //typeof(object);
            //if ((view.ObjectTypeInfo).Type == typeof(ContractImplementation)) {
            //    ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;
            //}
            //if (ChoiceType == typeof(object)) {
            //    ChoiceType = (view.ObjectTypeInfo).Type;
            //}


            // Если поддерживается интрфейс:
            //if ((view.ObjectTypeInfo).Type.GetInterface("IVersionMainObject") != null) {
            if (ChoiceType.GetInterface("IVersionMainObject") != null) {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                //object obj = objectSpace.CreateObject((view.ObjectTypeInfo).Type);
                object obj = objectSpace.CreateObject(ChoiceType);
                IVersionMainObject mainObj = obj as IVersionMainObject;
                if (mainObj == null) return;

                VersionRecord objCurrent = mainObj.GetCurrent();
                if (objCurrent == null) return;

                // Определяем DetailView
                string DetailViewId = frame.Application.FindDetailViewId(objCurrent.GetType());

                // Запрет стандартного поведения
                //e.Action.Active[DesableStandartNewAction] = false;
                e.ShowViewParameters.CreatedView = null;

                // Показ:
                TargetWindow openMode = TargetWindow.NewWindow;
                CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, objCurrent, openMode);

                if (openMode == TargetWindow.NewModalWindow && (view as ListView) != null) ((ListView)view).CollectionSource.Reload();
            }

            // bool isPers = (((((IModelDetailView)modelDetailView).AsObjectView).ModelClass).TypeInfo).IsPersistent;
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
            if (!types.Contains(typeof(crmContractNewForm)))
                types.Add(typeof(crmContractNewForm));
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




        private void ShowNavigationItemAction_SelectedItemChanged(object sender, EventArgs e) {

            // Отсечка по признаку NonPersistent
            // ObjectTypeInfo - Obsolete
            //NonPersistentAttribute attr = snic.Frame.View.ObjectTypeInfo.FindAttribute<NonPersistentAttribute>();
            //if (attr == null) return;


            Frame frame = Frame;
/*
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            crmContractNewForm newObj = objectSpace.CreateObject<crmContractNewForm>();

            // Определяем DetailView
            string DetailViewId = frame.Application.FindDetailViewId(newObj.GetType());
            
            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newObj, openMode);
*/
        }

    }
}
