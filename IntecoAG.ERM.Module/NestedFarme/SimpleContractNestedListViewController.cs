using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.SystemModule;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {
    public partial class SimpleContractNestedListViewController : NestedListViewControllerBase {

        private SimpleAction saMasterDetailViewInfoAction = null;
        private SimpleAction showDVAction = null;

        public SimpleContractNestedListViewController() {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof(SimpleContractVersion);
            saMasterDetailViewInfoAction = new SimpleAction(this, "MasterDetailViewInfoAction", DevExpress.Persistent.Base.PredefinedCategory.View);
            saMasterDetailViewInfoAction.Execute += new SimpleActionExecuteEventHandler(saMasterDetailViewInfoAction_Execute);
            MasterDetailViewInfo();

            showDVAction = new SimpleAction(this, "", PredefinedCategory.View);
            showDVAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            showDVAction.Execute += showDVAction_Execute;

        }

        void View_ControlsCreated(object sender, EventArgs e) {
            MasterDetailViewInfo();
        }


        private bool ActionCondition() {
            return (
                View.Id == "SimpleContract_SimpleContractVersions_ListView" |
                View.Id == "ComplexContract_ComplexContractVersions_ListView" |
                View.Id == "WorkPlan_WorkPlanVersions_ListView"
                );
        }

        void saMasterDetailViewInfoAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            MasterDetailViewInfo();
        }

        private void MasterDetailViewInfo() {
            string MasterViewId = string.Format("MasterViewId = '{0}'", this.MasterDetailViewId);
            Frame frame = this.Frame;
        }

        protected override void OnActivated() {
            base.OnActivated();
            View.ControlsCreated += new EventHandler(View_ControlsCreated);

            if (!ActionCondition()) return;

            ListViewProcessCurrentObjectController controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem += controller_CustomProcessSelectedItem;
            }
        }

        protected override void OnDeactivated() {
            if (!ActionCondition()) return;

            ListViewProcessCurrentObjectController controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem -= controller_CustomProcessSelectedItem;
            }
            base.OnDeactivated();
        }


        private void controller_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {
            e.Handled = true;
            showDVAction.DoExecute();
        }

        void showDVAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;

            object currentObject = (object)e.CurrentObject;
            if (currentObject == null) return;

            Type objType = typeof(object);
            string DetailViewID = "";

            if (View.Id == "SimpleContract_SimpleContractVersions_ListView") {
                SimpleContractVersion ob = currentObject as SimpleContractVersion;
                if (ob != null) {
                    objType = (System.Type)((System.Reflection.MemberInfo)((ob).GetType()));
                    DetailViewID = objType.Name + "_DetailView";

                    ShowConcreteDetailView<SimpleContractVersion>(view, DetailViewID, (SimpleContractVersion)ob, this.MasterDetailViewFrame);
                }
                return;
            }

            if (View.Id == "ComplexContract_ComplexContractVersions_ListView") {
                ComplexContractVersion ob = currentObject as ComplexContractVersion;
                if (ob != null) {
                    objType = (System.Type)((System.Reflection.MemberInfo)((ob).GetType()));
                    DetailViewID = objType.Name + "_DetailView";
                    ShowConcreteDetailView<ComplexContractVersion>(view, DetailViewID, (ComplexContractVersion)ob, TargetWindow.Current);
                }
                return;
            }

            if (View.Id == "WorkPlan_WorkPlanVersions_ListView") {
                WorkPlanVersion ob = currentObject as WorkPlanVersion;
                if (ob != null) {
                    objType = (System.Type)((System.Reflection.MemberInfo)((ob).GetType()));
                    DetailViewID = objType.Name + "_DetailView";
                    ShowConcreteDetailView<WorkPlanVersion>(view, DetailViewID, (WorkPlanVersion)ob, TargetWindow.Current);
                }
                return;
            }

        }



        #region Методы показа View

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteShortCutDetailView<classType>(View view, classType Obj, TargetWindow tw) {
            BaseObject obj = Obj as BaseObject;
            if (obj == null) return;

            ShowViewParameters svp = new ShowViewParameters();
            ViewShortcut shortcut = new ViewShortcut(Application.FindDetailViewId(typeof(classType)), obj.Oid);
            svp.CreatedView = Application.ProcessShortcut(shortcut);

            //Specify various display settings.
            svp.TargetWindow = tw;   // TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));

            view.ObjectSpace.CommitChanges();
            view.Close();
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(View view, string DetailViewID, classType Obj, TargetWindow tw) {
            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = tw;  // TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));

            //objectSpace.CommitChanges();
            //view.Close();
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(View view, string DetailViewID, classType Obj, Frame frame) {
            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = TargetWindow.Current;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));

            //objectSpace.CommitChanges();
            //view.Close();
        }

        /// <summary>
        /// Загрузка указанного параметром типа ListView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteListView<classType>(TargetWindow tw) {
            IObjectSpace objectSpaceList = Application.CreateObjectSpace();
            ListView lv = Frame.Application.CreateListView(objectSpaceList, typeof(classType), true);

            ShowViewParameters svpList = new ShowViewParameters();
            svpList.CreatedView = lv;

            //Specify various display settings.
            svpList.TargetWindow = tw;   // TargetWindow.Current;
            svpList.Context = TemplateContext.View;
            svpList.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svpList, new ShowViewSource(Frame, null));
        }

        #endregion

    }
}
