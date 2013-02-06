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

    // ����� �������� ������ ����������� � ������������� ������, ���� ���������� Experiment ���������� �������� true

    // � ������ ������� ����������� ��� Experiment = true � ������ ObjectCreating ����� ����������� ������ 
    // crmSimpleContract, �� ������ ������������� crmSimpleContractVersion. ����� ����� ��� ��������� �����, ���� �� ������ �� 
    // ��� � ����� �������� ��� crmSimpleContractVersion
    // � CustomNewActionController_CustomAddObjectToCollection ����� �������� � ��������� �������� ���� ��� ����� ���� ������ �������, �������� ��� �� ���������
    // � CustomNewActionController_ObjectCreated ����� �������� ��������� ������
    // � Application_DetailViewCreating ����� ������� ���������� View ��� ������ ���������� �������
    // � CustomNewActionController_Execute ����� ������� ��, ��� ������ � View ��� ����� ������������ ��������, �����
    // ������ � ��������� ������� � �������� ���������� ������ ������ � �.�.

    
    public partial class CustomNewActionController : WindowController {

        // ������� ������������ � NewObjectViewController
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

            // ProcessCreatedView ���������� ����� ���� ��� ��������� wovc.NewObjectAction.Execute, � � ��� ���������� ��� �����������
            // ��� ���, ������� ���������� �������� ������, �.�. ����� ������ � crmContractVersion ����������� � crmContract, ��� �� �����,
            // ������� ���� ������������� Execute
            //wovc.NewObjectAction.ProcessCreatedView

            wovc.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);
            
            // 2011-08-08. ������. �������� 3-� �������.

            // ������ �������, �������� ���������� �����.
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

            // �������� �� ����������� ������ ��� �������������:
            //if (Frame as NestedFrame != null | !View.IsRoot) {
            //    objectSpace = view.ObjectSpace.CreateNestedObjectSpace();
            //    openMode = TargetWindow.NewModalWindow;
            //} else {
            //    objectSpace = Application.CreateObjectSpace();
            //    openMode = TargetWindow.NewWindow;
            //}

            // ���������. ���� ��������� ������ �� ������ �� ��������� ������������� ��� ������, �� ���� ������������ Nested ObjectSpace

            // ���� ���� ������� ������ � ��������� ObjectSpace, ��� ���� � ��� ��������, � ����� ������� � �������� ObjectSpace.

            if (Experiment) {
                // �������� �� �������
                // �� ��������������� wovc.FrameAssigned += new EventHandler(CustomNewActionController_FrameAssigned);
                // ���������� ������ ��� ����� ������ �������� (�������� �� 2011-08-10) 
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
            // ��������, ����� ����� ��������� view � ����������� ��� ������������ ������
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

            // �������: ��� ������ ������������� � ����������� �� �������
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


            if (false) { // �������� 1
                /*  �����������, �.�. ������ ������� �� ������� ����������� ���������
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
                
            //    // ��� �������, ������� ���� ����������
            //    Type ChoiceType = e.Obj.GetType();   //((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;   //typeof(object);
            //    //if ((view.ObjectTypeInfo).Type == typeof(crmContractImplementation)) {
            //    //    ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;
            //    //}
            //    //if (ChoiceType == typeof(object)) {
            //    //    ChoiceType = (view.ObjectTypeInfo).Type;
            //    //}


            //    // ���� �������������� ���������:
            //    //if ((view.ObjectTypeInfo).Type.GetInterface("IVersionMainObject") != null) {
            //    if (ChoiceType.GetInterface("IVersionMainObject") != null) {
            //        IObjectSpace objectSpace = Application.CreateObjectSpace();

            //        //object obj = objectSpace.CreateObject((view.ObjectTypeInfo).Type);
            //        object obj = objectSpace.CreateObject(ChoiceType);
            //        IVersionMainObject mainObj = obj as IVersionMainObject;
            //        if (mainObj == null) return;

            //        VersionRecord objCurrent = mainObj.GetCurrent();
            //        if (objCurrent == null) return;

            //        // ���������� DetailView
            //        string DetailViewId = frame.Application.FindDetailViewId(objCurrent.GetType());

            //        // ������ ������������ ���������
            //        //e.Action.Active[DesableStandartNewAction] = false;
            //        //e.ShowViewParameters.CreatedView = null;
            //        e.ViewID = DetailViewId;

            //        //// �����:
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
        /// ���� ����� ��������� ������� ������ ������, ��� ���, ��� ������� ������� ������ ���� ��� ������� New.
        /// ������, �� ������ ���� �������� ���� ������ ������ � ����� CustomNewActionController_CustomAddObjectToCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomNewActionController_ObjectCreating(object sender, ObjectCreatingEventArgs e) {

            if (true) {   // �������� 1. ������ �������� ��������� e
                /*  �����������, �.�. ������ ������� �� ������� ����������� ���������
                //e.Cancel
                //e.ShowDetailView

                //e.ObjectSpace
                e.ObjectSpace = Application.CreateObjectSpace();

                //e.NewObject - ������� ������������ �������. ��� �������� �������� ������ �������� �� ���� � ��� �� �� ����
                // � ������ ������ crmSimpleContract, �.�. � ����� ����� ����������, ��� ������ ������ New, ����� �����������
                // ����� crmSimpleContract. � ������ DetailViewCreating ��� ���������� ������� ����� ����������� ������������
                // � ��������� �������� ���������� ������������� �������, �.�. crmSimpleContractVersion

                crmSimpleContract sc = e.ObjectSpace.CreateObject<crmSimpleContract>();
                if (sc == null && sc.Current == null) return;
                e.NewObject = sc.Current;   // e.ObjectSpace.CreateObject<crmSimpleContract>();
                */
            }

            //if (false) {   // �������� 0.
            //    // ������/���������� ������������ �������� �������
            //    e.Cancel = false;
            //    //e.ShowDetailView = false;  // DetailView �� ����� �������


            //    #region ������ ���� ������������ ������� ��� ������ ObjectSpace

            //    /*
            //    // ����� �������� ObjectSpace
            //    objectSpace = Application.CreateObjectSpace();
            //    e.ObjectSpace = objectSpace;

            //    // ����� �������� ��������� �������, ��� ��������� ������� ����� ������ ������.
            //    // CustomNewActionController_Execute
            //    ObjectCreatingEventArgs args = new ObjectCreatingEventArgs(objectSpace, typeof(crmWorkPlan));

            //    // ������/���������� ������������ �������� �������
            //    args.Cancel = false;
            //    //args.ShowDetailView = false;  // DetailView �� ����� �������
            //    e = args;
            //    */

            //    #endregion
            //}

            //if (false) {   // �������� 2. ������� �������� �������� ��������� e
            //    // ��� ������ ��������� e �������� ��� ������������ ������� �� ����������
            //    IObjectSpace objectSpace = e.ObjectSpace;
            //    object newObj = objectSpace.CreateObject(e.ObjectType);
            //    //newedObject = newObj;

            //    // ����� ����� ���-������ ������� � ������������� ��������.
            //    /*
            //    crmWorkPlan wpObj = newObj as crmWorkPlan;
            //    if (wpObj != null) {
            //        crmWorkPlanVersion wpvObj = wpObj.Current as crmWorkPlanVersion;
            //        if (wpObj != null && wpvObj != null) {
            //            //wpObj.Current.crmContractDocument.Number = "� " + System.DateTime.Now.ToString();
            //            wpvObj.Description = System.DateTime.Now.ToString();
            //        }
            //    }

            //    FM.Order.fmOrder orderObj = newObj as FM.Order.fmOrder;
            //    if (orderObj != null) {
            //        orderObj.Name = "Order � " + System.DateTime.Now.ToString();
            //    }
            //    */

            //    // ������ ��������� � CustomNewActionController_CustomAddObjectToCollection
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
            //    // ��������� � ������ CustomNewActionController_ObjectCreating ������ ��������� � ���� ����� ����������
            //    // � ����� �������� ��� � ��������� �������� ��� ���� ������ ������

            //    Frame frame = Frame;
            //    View view = frame.View;

            //    // ���������� ������ ������

            //    // ��������� ����������� ������
            //    e.Handled = true;

            //    //e.NewObject = newedObject;
            //    //e.CurrentCollectionSource = new CollectionSource(e.ObjectSpace, newedObject.GetType());

            //    //DevExpress.XtraEditors.XtraMessageBox.Show("��������� ���������� �������� (e.ObjectSpace): " + e.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("��������� ���������� �������� (view.ObjectSpace): " + view.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());

            //    // e.ObjectSpace � view.ObjectSpace - ������

            //    IObjectSpace nestedObjectSpace = view.ObjectSpace.CreateNestedObjectSpace();

            //    object newObj = nestedObjectSpace.GetObject(e.NewObject);
            //    e.CurrentCollectionSource.Add(newObj);

            //    //e.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null);

            //    e.ObjectSpace.GetObject(newObj);

            //    //DevExpress.XtraEditors.XtraMessageBox.Show("���������� �������� ����� ���������� (e.ObjectSpace): " + e.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("���������� �������� ����� ���������� (view.ObjectSpace): " + view.ObjectSpace.GetObjectsCount(e.NewObject.GetType(), null).ToString());
            //    //DevExpress.XtraEditors.XtraMessageBox.Show("���������� �������� ��� ���������� (e.ObjectSpace): " + e.ObjectSpace.GetObjectsToSave(false).Count.ToString());
            //}
        }

        private void CustomNewActionController_ObjectCreated(object sender, ObjectCreatedEventArgs e) {

            // � ��������� ������� ������ ������ ������ � ����� ��������� �������� ��������, ��������, �������������� 
            // ���� ����������� �������� � �.�.
            //if (false) {
            //    //((Task)e.CreatedObject).StartDate = DateTime.Now;

            //    Frame frame = Frame;
            //    View view = frame.View;

            //    IObjectSpace objectSpace = e.ObjectSpace;

            //    /*
            //    #region ������ ���� ������������ ������� ��� ������ ObjectSpace - �� ������
            
            //    //// ����� �������� ObjectSpace
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
                // ������� �� �������
                // �� ��������������� wovc.FrameAssigned -= new EventHandler(CustomNewActionController_FrameAssigned);
                // ���������� ������ ��� ����� ������ �������� (�������� �� 2011-08-10) 
                wovc.ObjectCreating -= new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
                wovc.CustomAddObjectToCollection -= new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
                wovc.ObjectCreated -= new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);
            }

            base.OnDeactivated();
        }

        void CustomNewActionController_Execute(object sender, ActionBaseEventArgs e) {

            // ���� ����� ������������ ��, ��� ���������� � ������� ObjectCreating � DetailViewCreating
            // � ��������� �������� ����� ������.
            // �� ����� ����, � ���� ������ ����� ������� ��, ��� ������, �� ��� ���� ����� ��������� ��� 
            // ������� �������� ������ ������� �������� ������������� �������.

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

            //    // ��� �������, ������� ���� ����������
            //    Type ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;   //typeof(object);
            //    //if ((view.ObjectTypeInfo).Type == typeof(crmContractImplementation)) {
            //    //    ChoiceType = ((System.Type)(((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Data)).UnderlyingSystemType;
            //    //}
            //    //if (ChoiceType == typeof(object)) {
            //    //    ChoiceType = (view.ObjectTypeInfo).Type;
            //    //}


            //    // ���� �������������� ���������:
            //    //if ((view.ObjectTypeInfo).Type.GetInterface("IVersionMainObject") != null) {
            //    if (ChoiceType.GetInterface("IVersionMainObject") != null) {
            //        IObjectSpace objectSpace = Application.CreateObjectSpace();

            //        //object obj = objectSpace.CreateObject((view.ObjectTypeInfo).Type);
            //        object obj = objectSpace.CreateObject(ChoiceType);
            //        IVersionMainObject mainObj = obj as IVersionMainObject;
            //        if (mainObj == null) return;

            //        VersionRecord objCurrent = mainObj.GetCurrent();
            //        if (objCurrent == null) return;

            //        // ���������� DetailView
            //        string DetailViewId = frame.Application.FindDetailViewId(objCurrent.GetType());

            //        // ������ ������������ ���������
            //        //e.Action.Active[DesableStandartNewAction] = false;
            //        e.ShowViewParameters.CreatedView = null;

            //        // �����:
            //        TargetWindow openMode = TargetWindow.NewWindow;
            //        CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, objCurrent, openMode);

            //        if (openMode == TargetWindow.NewModalWindow && (view as ListView) != null) ((ListView)view).CollectionSource.Reload();
            //    }

            //    // bool isPers = (((((IModelDetailView)modelDetailView).AsObjectView).ModelClass).TypeInfo).IsPersistent;
            //}
        }

        #region "������� �� �����"

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

            // ������� �� �������� NonPersistent
            // ObjectTypeInfo - Obsolete
            //NonPersistentAttribute attr = snic.Frame.View.ObjectTypeInfo.FindAttribute<NonPersistentAttribute>();
            //if (attr == null) return;


            Frame frame = Frame;

            //////IObjectSpace objectSpace = Application.CreateObjectSpace();

            //////crmContractNewForm newObj = objectSpace.CreateObject<crmContractNewForm>();

            //////// ���������� DetailView
            //////string DetailViewId = frame.Application.FindDetailViewId(newObj.GetType());
            
            //////// ����������:
            //////TargetWindow openMode = TargetWindow.Current;
            //////CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newObj, openMode);

        }
*/

    }
}
