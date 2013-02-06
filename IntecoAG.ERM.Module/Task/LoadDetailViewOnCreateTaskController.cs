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

using System.Linq;

using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.SystemModule;

namespace IntecoAG.ERM.Module {
    public partial class LoadDetailViewOnCreateTaskController : ViewController<ListView> {
        //public LoadTaskOnCreateController() {
        //    InitializeComponent();
        //    RegisterActions(components);
        //}
        public LoadDetailViewOnCreateTaskController() {
            TargetViewType = ViewType.ListView;
            TargetObjectType = typeof(DealWithoutStageTaskInstanceDefinitionAdmin);
        }
        protected override void OnActivated() {
            base.OnActivated();

            //Frame.Controllers[typeof(DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController)].Actions["TaskInvoiceInstanceDefinitionAdmin.create"].Executed += new EventHandler<ActionBaseEventArgs>(action_Executed);

            foreach (DevExpress.ExpressApp.Actions.ActionBase action in Frame.Controllers[typeof(DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController)].Actions) {
                switch (action.Id) {
                    case "TaskInvoiceInstanceDefinitionAdmin.create":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(actionAdminCreateTask_Executed);
                        break;
                    case "BaseUserTask.activate":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskActivate_Executed);
                        break;
                    case "BaseUserTask.start":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskStart_Executed);
                        break;
                    case "BaseUserTask.stop":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskStop_Executed);
                        break;
                    case "BaseUserTask.claim":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskClaim_Executed);
                        break;
                    case "BaseUserTask.complete":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskComplete_Executed);
                        break;
                    case "BaseUserTask.Nominate":
                        action.Executed += new EventHandler<ActionBaseEventArgs>(actionTaskNominate_Executed);
                        break;
                }

                //if (action.Id == "TaskInvoiceInstanceDefinitionAdmin.create") {
                //    action.Executed += new EventHandler<ActionBaseEventArgs>(actionCreate_Executed);
                //    break;
                //}
            }

            /*
            // Полный перебор
            foreach (Controller controller in Frame.Controllers) {
                foreach (DevExpress.ExpressApp.Actions.ActionBase action in controller.Actions) {
                    if (action.Id == "create") {
                        action.Executed += new EventHandler<ActionBaseEventArgs>(action_Executed);
                    }
                }
            }
            */
        }
        protected override void OnDeactivated() {
            foreach (DevExpress.ExpressApp.Actions.ActionBase action in Frame.Controllers[typeof(DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController)].Actions) {
                switch (action.Id) {
                    case "TaskInvoiceInstanceDefinitionAdmin.create":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(actionAdminCreateTask_Executed);
                        break;
                    case "BaseUserTask.activate":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskActivate_Executed);
                        break;
                    case "BaseUserTask.start":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskStart_Executed);
                        break;
                    case "BaseUserTask.stop":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskStop_Executed);
                        break;
                    case "BaseUserTask.claim":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskClaim_Executed);
                        break;
                    case "BaseUserTask.complete":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskComplete_Executed);
                        break;
                    case "BaseUserTask.Nominate":
                        action.Executed -= new EventHandler<ActionBaseEventArgs>(actionTaskNominate_Executed);
                        break;
                }
                //if (action.Id == "TaskInvoiceInstanceDefinitionAdmin.create") {
                //    action.Executed -= new EventHandler<ActionBaseEventArgs>(actionCreate_Executed);
                //    break;
                //}
            }

            //Frame.Controllers[0].Actions[""].Executed += new EventHandler<ActionBaseEventArgs>(actionCreate_Executed);
            base.OnDeactivated();
        }

        /// <summary>
        /// Поиск заданного пункта навигационного меню
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public ChoiceActionItem FindNavigationItemByViewId(string viewId) {
            foreach (ChoiceActionItem navigationGroup in Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.Items) {
                foreach (ChoiceActionItem navigationItem in navigationGroup.Items) {
                    if (navigationItem.Id == viewId) {
                        return navigationItem;
                    }
                }
            }
            return null;
        }

        void actionAdminCreateTask_Executed(object sender, ActionBaseEventArgs e) {
            // Загружаем новый Task

            DealWithoutStageTaskInstanceDefinitionAdmin currentObject = (DealWithoutStageTaskInstanceDefinitionAdmin)View.CurrentObject;
            currentObject.Save();
            currentObject.Session.FlushChanges();

            // Находим новую задачу (она должна быть одна для админской (singleton)) по ссылке в задаче на админскую
            XPCollection<BaseUserTask> tasks = new XPCollection<BaseUserTask>(currentObject.Session);
            if (!tasks.IsLoaded) tasks.Load();

            var ltasks = (from task in tasks where task.TaskAdminGuid.Equals(currentObject.Oid) select task);
            //if (((XPCollection<BaseUserTask>)ltasks).Count == 0) return;

            BaseUserTask but = null;

            // Выполнение пункта меню
            //string viewId = "BaseUserTask";
            //ChoiceActionItem item = FindNavigationItemByViewId(viewId);
            //if (item != null && item.Enabled.ResultValue) {
            //    Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.DoExecute(item);
            //} else {
            //    throw new Exception(String.Format("Unable to open {0} at startup", viewId));
            //}
            //return;

            //ChoiceActionItem item = FindNavigationItemByViewId("TaskInvoiceInstanceDefinition_ListView");



            IEnumerator enumltasks = ltasks.GetEnumerator();
            while (enumltasks.MoveNext()) {
                but = (BaseUserTask)enumltasks.Current;
                break;
            }
            if (but == null) return;


            //string detailViewId = "TaskInvoiceInstanceDefinition_DetailView";
            string detailViewId = "BaseUserTask_DetailView";
            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            BaseUserTask objCurrent = objectSpace.GetObject<BaseUserTask>(but);

            //TaskInvoiceInstanceDefinitionAdmin objTaskInvoiceInstanceDefinitionAdmin = objectSpace.CreateObject<TaskInvoiceInstanceDefinitionAdmin>();
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, detailViewId, true, objCurrent);
            //ListView dv = Frame.Application.CreateListView(objectSpace, typeof(TaskInvoiceInstanceDefinitionAdmin), true);
            dv.AllowEdit.SetItemValue("BaseUserTaskReadOnly", true);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            //Specify various display settings.
            svp.TargetWindow = TargetWindow.Current;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            // Here we show our detail view.
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));
        }

        void actionTaskActivate_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            //currentObject.Session.FlushChanges(); // При start и complete сохраняем

            ShowConcreteDetailView("BaseUserTask_DetailView", currentObject);
        }

        void actionTaskStart_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            currentObject.Session.FlushChanges(); // При start и complete сохраняем

            ShowConcreteDetailView("TaskInvoiceInstanceDefinition_DetailView", currentObject);
        }

        void actionTaskStop_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            //currentObject.Session.FlushChanges(); // При start и complete сохраняем

            ShowConcreteDetailView("BaseUserTask_DetailView", currentObject);
        }

        void actionTaskClaim_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            currentObject.Session.FlushChanges(); // При start и complete сохраняем

            ShowConcreteDetailView("BaseUserTask_DetailView", currentObject);
        }

        void actionTaskComplete_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            currentObject.Session.FlushChanges(); // При start и complete сохраняем

            ShowConcreteDetailView("TaskInvoiceInstanceDefinition_DetailView", currentObject);
        }

        void actionTaskNominate_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            //currentObject.Session.FlushChanges(); // При start и complete сохраняем

            ShowConcreteDetailView("TaskInvoiceInstanceDefinition_DetailView", currentObject);
        }

        /// <summary>
        /// Загрузка указанного параметром DeatilView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView(string DetailViewID, BaseUserTask but) {
            //BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            //currentObject.Save();
            //currentObject.Session.FlushChanges(); // При start и complete сохраняем

            string detailViewId = DetailViewID;
            //IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            BaseUserTask objCurrent = objectSpace.GetObject<BaseUserTask>(but);

            //TaskInvoiceInstanceDefinition objTaskInvoiceInstanceDefinition = objectSpace.CreateObject<TaskInvoiceInstanceDefinition>();
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, detailViewId, true, objCurrent);
            //ListView dv = Frame.Application.CreateListView(objectSpace, typeof(TaskInvoiceInstanceDefinition), true);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            //Specify various display settings.
            svp.TargetWindow = TargetWindow.Current;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            // Here we show our detail view.
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));

        }

    }
}
