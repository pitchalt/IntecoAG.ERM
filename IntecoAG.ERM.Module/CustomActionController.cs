using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//using System.Windows.Forms;
using System.Collections;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp.Editors;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Win.SystemModule;

//using System.Linq;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {
    public partial class CustomActionController : OpenObjectController, IMasterDetailViewInfo
    {   //ViewController<ListView> {
        //public LoadTaskDetailViewOnActionController() {
        //    InitializeComponent();
        //    RegisterActions(components);
        //}
        public CustomActionController() {
            //TargetViewType = ViewType.ListView;
            //TargetObjectType = typeof(SimpleContractVersion);
        }

        protected override void OnActivated() {
            base.OnActivated();

            //Frame.Controllers[typeof(DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController)].Actions["TaskInvoiceInstanceDefinitionAdmin.create"].Executed += new EventHandler<ActionBaseEventArgs>(action_Executed);
/*
            foreach (DevExpress.ExpressApp.Actions.ActionBase action in Frame.Controllers[typeof(DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController)].Actions) {
                switch (action.Id) {
                    case "NewVersion":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(NewVersion_Executed);
                        break;
                    //case "BaseUserTask.start":
                    //    action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskStart_Executed);
                    //    break;
                    //case "BaseUserTask.stop":
                    //    action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskStop_Executed);
                    //    break;
                    //case "BaseUserTask.claim":
                    //    action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskClaim_Executed);
                    //    break;
                    //case "BaseUserTask.complete":
                    //    action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskComplete_Executed);
                    //    break;
                    //case "BaseUserTask.Nominate":
                    //    action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskNominate_Executed);
                    //    break;
                }
                
                //if (action.Id == "TaskInvoiceInstanceDefinitionAdmin.create") {
                //    action.Executed += new EventHandler<ActionBaseEventArgs>(actionCreate_Executed);
                //    break;
                //}
            }
*/
            
            /*
            // Полный перебор
            if (View.Id == "SimpleContract_ListView" | View.Id == "ComplexContract_ListView" | View.Id == "WorkPlan_ListView") {
                foreach (Controller controller in Frame.Controllers) {
                    foreach (DevExpress.ExpressApp.Actions.ActionBase action in controller.Actions) {
                        if (action.Id == "OpenObject") {
                            //action.Active["OpenObjectActionDesable"] = false;
                            action.Executed += new EventHandler<ActionBaseEventArgs>(OpenObject_Executed);
                            goto M;
                        }
                    }
                }
            }
        M: ;
            */
        }

        protected override void OnDeactivated() {
            /*
            foreach (DevExpress.ExpressApp.Actions.ActionBase action in Frame.Controllers[typeof(DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController)].Actions) {
                switch (action.Id) {
                    case "NewVersion":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(NewVersion_Executed);
                        break;
                    //case "BaseUserTask.start":
                    //    action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskStart_Executed);
                    //    break;
                    //case "BaseUserTask.stop":
                    //    action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskStop_Executed);
                    //    break;
                    //case "BaseUserTask.claim":
                    //    action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskClaim_Executed);
                    //    break;
                    //case "BaseUserTask.complete":
                    //    action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskComplete_Executed);
                    //    break;
                    //case "BaseUserTask.Nominate":
                    //    action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskNominate_Executed);
                    //    break;
                }
                //if (action.Id == "TaskInvoiceInstanceDefinitionAdmin.create") {
                //    action.Executed -= new EventHandler<ActionBaseEventArgs>(actionCreate_Executed);
                //    break;
                //}
            }
            */
            /*
            // Полный перебор
            if (View.Id == "SimpleContract_ListView" | View.Id == "ComplexContract_ListView" | View.Id == "WorkPlan_ListView") {
                foreach (Controller controller in Frame.Controllers) {
                    foreach (DevExpress.ExpressApp.Actions.ActionBase action in controller.Actions) {
                        if (action.Id == "OpenObject") {
                            //action.Active["OpenObjectActionDesable"] = true;
                            action.Executed -= new EventHandler<ActionBaseEventArgs>(OpenObject_Executed);
                            goto M;
                        }
                    }
                }
            }
        M: ;
            */
            //Frame.Controllers[0].Actions[""].Executed += new EventHandler<ActionBaseEventArgs>(actionCreate_Executed);
            base.OnDeactivated();
        }


        protected override void OpenObject(SimpleActionExecuteEventArgs e) {
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

            base.OpenObject(e);
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
