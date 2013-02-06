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

using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

//using System.Linq;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {
    public partial class LoadDetailViewOnNewActionController : ViewController<DetailView> {
        //public LoadTaskDetailViewOnActionController() {
        //    InitializeComponent();
        //    RegisterActions(components);
        //}
        public LoadDetailViewOnNewActionController() {
            TargetViewType = ViewType.DetailView;
            TargetObjectType = typeof(SimpleContractVersion);
        }

        protected override void OnActivated() {
            base.OnActivated();

            //Frame.Controllers[typeof(DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController)].Actions["TaskInvoiceInstanceDefinitionAdmin.create"].Executed += new EventHandler<ActionBaseEventArgs>(action_Executed);

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

            //Frame.Controllers[0].Actions[""].Executed += new EventHandler<ActionBaseEventArgs>(actionCreate_Executed);
            base.OnDeactivated();
        }

        void NewVersion_Executed(object sender, ActionBaseEventArgs e) {
            SimpleContractVersion currentObject = (SimpleContractVersion)View.CurrentObject;
            currentObject.Save();
            //currentObject.Session.FlushChanges(); // При start и complete сохраняем
            ObjectSpace.Refresh();

            ShowConcreteDetailView<SimpleContractVersion>("SimpleContractVersion_DetailView", currentObject);
        }

/*
        void actionTaskStart_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            currentObject.Session.FlushChanges(); // При start и complete сохраняем
            ObjectSpace.Refresh();

            ShowConcreteDetailView("TaskInvoiceInstanceDefinition_DetailView", currentObject);
        }

        void actionTaskStop_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            //currentObject.Session.FlushChanges(); // При start и complete сохраняем
            ObjectSpace.Refresh();

            ShowConcreteDetailView("BaseUserTask_DetailView", currentObject);
        }

        void actionTaskClaim_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            currentObject.Session.FlushChanges(); // При start и complete сохраняем
            ObjectSpace.Refresh();

            ShowConcreteDetailView("BaseUserTask_DetailView", currentObject);
        }

        void actionTaskComplete_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            currentObject.Session.FlushChanges(); // При start и complete сохраняем
            ObjectSpace.Refresh();

            ShowConcreteDetailView("TaskInvoiceInstanceDefinition_DetailView", currentObject);
        }

        void actionTaskNominate_Executed(object sender, ActionBaseEventArgs e) {
            BaseUserTask currentObject = (BaseUserTask)View.CurrentObject;
            currentObject.Save();
            //currentObject.Session.FlushChanges(); // При start и complete сохраняем
            ObjectSpace.Refresh();

            ShowConcreteDetailView("TaskInvoiceInstanceDefinition_DetailView", currentObject);
        }
*/

        /// <summary>
        /// Загрузка указанного параметром DeatilView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(string DetailViewID, classType but) {
            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(but);

            //TaskInvoiceInstanceDefinition objTaskInvoiceInstanceDefinition = objectSpace.CreateObject<TaskInvoiceInstanceDefinition>();
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);
            //ListView dv = Frame.Application.CreateListView(objectSpace, typeof(TaskInvoiceInstanceDefinition), true);
            
            // ReadOnly - если нужно
            //dv.AllowEdit.SetItemValue("BaseUserTaskReadOnly", true);

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
