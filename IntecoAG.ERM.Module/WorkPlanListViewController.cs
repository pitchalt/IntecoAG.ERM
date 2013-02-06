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

using System.Reflection;
using DevExpress.XtraGrid;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {
    public partial class WorkPlanListViewController : ViewController {

        private SimpleAction showDVAction;

        public WorkPlanListViewController() {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof(WorkPlan);
            //showDVAction = new SimpleAction(this, "Show DetailView For WorkPlan_ListView", PredefinedCategory.Edit);
            showDVAction = new SimpleAction(this, "", PredefinedCategory.View);
            //showDVAction.ToolTip = "Show version of record";
            showDVAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //showDVAction.ImageName = "BO_Contact";
            showDVAction.Execute += showDVAction_Execute;
        }

        protected override void OnActivated() {
            base.OnActivated();
            ListViewProcessCurrentObjectController controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem += controller_CustomProcessSelectedItem;
            }
        }

        protected override void OnDeactivated() {
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
            WorkPlan currentObject = (WorkPlan)e.CurrentObject;

            if (currentObject == null) return;

            Type objType = typeof(object);
            WorkPlanVersion rgn = null;
            string DetailViewID = "";

            rgn = currentObject.Current;

            if (rgn == null) return;

            // Показ DetailView стандартного формирования, т.е. имя типа + суффикс 
            objType = (System.Type)((System.Reflection.MemberInfo)(rgn.GetType()));   // ((System.Reflection.MemberInfo)(rgn.GetType())).DeclaringType;
            DetailViewID = objType.Name + "_DetailView";

            // ПОЯСНЕНИЕ.
            // Если есть запись в статусе VERSION_NEW, то её и показываем.
            // В противном случае показываем DetailView определённый для самого объекта WorkPlan (не для версии)

            if (currentObject.Current.VersionState == VersionStates.VERSION_NEW) {
                ShowConcreteDetailView<WorkPlanVersion>(DetailViewID, currentObject.Current);
            } else {
                ShowConcreteDetailView<WorkPlan>("WorkPlan_DetailView", currentObject);
            }
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(string DetailViewID, classType but) {
            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(but);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            //TaskInvoiceInstanceDefinition objTaskInvoiceInstanceDefinition = objectSpace.CreateObject<TaskInvoiceInstanceDefinition>();
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);
            //ListView dv = Frame.Application.CreateListView(objectSpace, typeof(TaskInvoiceInstanceDefinition), true);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            //Specify various display settings.
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            // Here we show our detail view.
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));
        }
    }
}

