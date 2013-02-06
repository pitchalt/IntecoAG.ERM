using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
//using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Win;

using System.Reflection;
using DevExpress.XtraGrid;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {
    public partial class ListViewProcessController : ViewController, IMasterDetailViewInfo {

        private SimpleAction showDVAction;

        public ListViewProcessController() {
            InitializeComponent();
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

            RegisterActions(components);

            //TargetObjectType = typeof(SimpleContract);
            TargetViewType = ViewType.ListView;
            //showDVAction = new SimpleAction(this, "Show DetailView For SimpleContract_ListView", PredefinedCategory.Edit);
            //showDVAction = new SimpleAction(this, "Show DetailView For SimpleContract_ListView", PredefinedCategory.Edit);
            //showDVAction = new SimpleAction(this, "showDVAction", PredefinedCategory.View);
            showDVAction = new SimpleAction(this, "", PredefinedCategory.View);
            showDVAction.Caption = String.Empty;
            //showDVAction.ToolTip = "Show version of record";
            showDVAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            showDVAction.ImageName = "BO_Open";
            showDVAction.Execute += showDVAction_Execute;
        }

        protected override void OnActivated() {
            base.OnActivated();


            //======================================================

            // След. строка - способ из вложенного фрейма получить View из мастер-фрейма
            //CompositeView parentView = ((NestedFrame)this.Frame).DetailViewItem.View;
            
            /*
            if (View.CollectionSource is PropertyCollectionSource) {
                PropertyCollectionSource collectionSource = (PropertyCollectionSource)View.CollectionSource;
                //collectionSource.MasterObjectChanged += OnMasterObjectChanged;
                //if (collectionSource.MasterObject != null) UpdateMasterObject(collectionSource.MasterObject);
            }

            foreach (Controller controller in Frame.Controllers) {
                foreach (DevExpress.ExpressApp.Actions.ActionBase action in controller.Actions) {
                    if (action.Id == "OpenObject") {
                    }
                }
            }
            
            */

            //foreach (PropertyEditor item in ((ListView)View).GetItems<PropertyEditor>()) {
            //    object obj = item.CurrentObject;
            //}

            //foreach (ViewItem item in ((ListView)View).Items) {
            //    CompositeView obj = item.View;
            //}

            if (Frame.Context.Name == TemplateContext.LookupControl ||
                Frame.Context.Name == TemplateContext.LookupWindow) return;

            ListViewProcessCurrentObjectController controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem += controller_CustomProcessSelectedItem;
            }
        }

        protected override void OnDeactivated() {
            if (Frame.Context.Name == "LookupControlContext") return;

            ListViewProcessCurrentObjectController controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem -= controller_CustomProcessSelectedItem;
            }
            base.OnDeactivated();
        }

        private void controller_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {
            if (Frame.Context.Name == TemplateContext.LookupControl || Frame.Context.Name == TemplateContext.LookupWindow) return;
            
            e.Handled = true;
            showDVAction.DoExecute();
        }

        void showDVAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;

            object currentObject = (object)e.CurrentObject;
            if (currentObject == null) return;

            Type objType = typeof(object);
            string DetailViewID = "";
            TargetWindow openMode;
            IObjectSpace objectSpace;

            Type NavigationChoiceType = (System.Type)(((SimpleActionExecuteEventArgs)(e)).CurrentObject.GetType());   //.UnderlyingSystemType;   //typeof(object);
            //Type ChoiceType = view.ObjectTypeInfo.Type;

            if (NavigationChoiceType.GetInterface("IVersionMainObject") != null) {
                //VersionRecord ob = currentObject as VersionRecord;
                IVersionMainObject mainObj = currentObject as IVersionMainObject;
                if (mainObj == null) return;

                objectSpace = Application.CreateObjectSpace();
                openMode = TargetWindow.NewWindow;
                object obj = null;

                if (mainObj.GetCurrent() != null && mainObj.GetCurrent().VersionState == VersionStates.VERSION_NEW) {
                    objType = (System.Type)((System.Reflection.MemberInfo)((mainObj.GetCurrent()).GetType()));
                    DetailViewID = Application.FindDetailViewId(objType);
                    obj = objectSpace.GetObject(mainObj.GetCurrent());
                } else {
                    objType = (System.Type)((System.Reflection.MemberInfo)((currentObject).GetType()));   // ((System.Reflection.MemberInfo)(rgn.GetType())).DeclaringType;
                    DetailViewID = Application.FindDetailViewId(objType);
                    obj = objectSpace.GetObject(currentObject);
                }
                CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewID, obj, openMode);

                if (openMode == TargetWindow.NewModalWindow && (view as ListView) != null) ((ListView)view).CollectionSource.Reload();
                return;
            }

            if (NavigationChoiceType.GetInterface("IVersionSupport") != null) {
                VersionRecord versObj = currentObject as VersionRecord;
                object mainObj = versObj.MainObject;
                if (versObj != null && mainObj != null && (mainObj.GetType()).GetInterface("IVersionMainObject") != null) {
                    objType = (System.Type)((System.Reflection.MemberInfo)((currentObject).GetType()));
                    DetailViewID = Application.FindDetailViewId(objType);

                    Frame resultFrame = (this.MasterDetailViewFrame != null) ? this.MasterDetailViewFrame : frame;
                    openMode = (this.MasterDetailViewFrame != null) ? TargetWindow.Current : TargetWindow.NewModalWindow; 

                    objectSpace = resultFrame.Application.CreateObjectSpace();
                    object obj = objectSpace.GetObject(currentObject);
                    CommonMethods.ShowConcreteDetailViewInWindow(resultFrame, objectSpace, DetailViewID, obj, TargetWindow.Current);
                }
                return;
            }

            // ПРОЧИЕ LISTVIEW
            // Грузим в отдельном окне
            objType = (System.Type)((System.Reflection.MemberInfo)((currentObject).GetType()));   // ((System.Reflection.MemberInfo)(rgn.GetType())).DeclaringType;
            DetailViewID = Application.FindDetailViewId(objType);

            if (Frame as NestedFrame != null | !View.IsRoot) {
                objectSpace = view.ObjectSpace.CreateNestedObjectSpace();
                openMode = TargetWindow.NewModalWindow;
            } else {
                objectSpace = Application.CreateObjectSpace();
                openMode = TargetWindow.NewWindow;
            }

            object passedObject = objectSpace.GetObject(currentObject);
            CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewID, passedObject, openMode);
        }


        #region IMasterDetailViewInfo Members

        private string masterDetailViewIdCore = String.Empty;
        public Frame masterDetailViewFrameCore = null;
        
        public string MasterDetailViewId {
            get { return masterDetailViewIdCore; }
        }

        public Frame MasterDetailViewFrame {
            get { return masterDetailViewFrameCore; }
        }

        public void AssignMasterDetailViewId(string id) {
            masterDetailViewIdCore = id;
        }

        public void AssignMasterDetailViewFrame(Frame frame) {
            masterDetailViewFrameCore = frame;
        }

        #endregion

    }
}
