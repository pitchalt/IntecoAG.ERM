using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// В этом контроллере доступны все вообще частные контроллеры для каждого действия (кнопки, события и т.п.)
    /// Например, в объекте Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.WinNewObjectViewController>()
    /// можно найти семейство всех контроллеров нужного типа <WinNewObjectViewController>
    /// А с помощью Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions
    /// можно орудовать всеми Action, полученными из методов класса с помощью атрибута [Action] 
    /// Примеры см. в этом контроллере (часть образцов закомментарена, но проверена в работе)
    /// </summary>
    public partial class HideOrShowMenuItemsViewController : ViewController {
        public HideOrShowMenuItemsViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            Frame frame = Frame;
            View view = View;

            DetailView detailView = view as DetailView;
            
            // Запрещаем показ действия в режиме DetailView

            Type typeObjectOfDetailView;
            if (detailView != null) {
                typeObjectOfDetailView = detailView.ObjectTypeInfo.Type;   // ((System.Type)(listView.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;
                if (typeObjectOfDetailView.IsInstanceOfType(typeof(BaseTaskAdmin))) {
                    // 
                    //Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.WinNewObjectViewController>().NewObjectAction.Active.SetItemValue("create", false);

                    // http://www.devexpress.com/Support/Center/p/Q265465.aspx
                    //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseTaskInstanceDefinitionAdmin.create"].Active.SetItemValue("create", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseTaskInstanceDefinitionAdmin.create"].Active.SetItemValue("CreateEnabled", false);
                }

                if (typeObjectOfDetailView.IsInstanceOfType(typeof(BaseTask))) {

                    DealWithoutStageTaskInstanceDefinition curObj = (DealWithoutStageTaskInstanceDefinition)View.CurrentObject;

                    // Разбор случаев
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.activate"].Active.SetItemValue("activateEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.start"].Active.SetItemValue("startEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.stop"].Active.SetItemValue("stopEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.complete"].Active.SetItemValue("completeEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.Nominate"].Active.SetItemValue("NominateEnabled", false);

                    switch (curObj.State) {
                        case UserTaskState.Created:
                            Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", true);
                            break;
                        case UserTaskState.Ready:
                            Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", true);
                            break;
                        case UserTaskState.Reserved:
                            Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.start"].Active.SetItemValue("startEnabled", true);
                            break;
                        case UserTaskState.InProgress:
                            Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.stop"].Active.SetItemValue("stopEnabled", true);
                            Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.complete"].Active.SetItemValue("completeEnabled", true);

                            if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"] != null) {
                                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"].Active.SetItemValue("ApproveEnabled", true);
                            }
                            if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"] != null) {
                                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"].Active.SetItemValue("DeclainEnabled", true);
                            }
                            break;
                        case UserTaskState.Completed:
                            break;
                        case UserTaskState.Failed:
                            break;
                        case UserTaskState.Error:
                            break;
                        case UserTaskState.Exited:
                            break;
                        case UserTaskState.Obsolete:
                            break;
                    }
                }

            }


            /*
            if (View.Id == "BaseTaskInstanceDefinitionAdmin_DetailView") {
                // 
                //Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.WinNewObjectViewController>().NewObjectAction.Active.SetItemValue("create", false);

                // http://www.devexpress.com/Support/Center/p/Q265465.aspx
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseTaskInstanceDefinitionAdmin.create"].Active.SetItemValue("create", false);
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseTaskInstanceDefinitionAdmin.create"].Active.SetItemValue("CreateEnabled", false);
            }

            if (View.Id == "TaskInvoiceInstanceDefinition_DetailView" | View.Id == "BaseUserTask_DetailView") {

                TaskInvoiceInstanceDefinition curObj = (TaskInvoiceInstanceDefinition)View.CurrentObject;

                // Разбор случаев
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.activate"].Active.SetItemValue("activateEnabled", false);
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", false);
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.start"].Active.SetItemValue("startEnabled", false);
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.stop"].Active.SetItemValue("stopEnabled", false);
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.complete"].Active.SetItemValue("completeEnabled", false);
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.Nominate"].Active.SetItemValue("NominateEnabled", false);

                switch (curObj.State) {
                    case UserTaskState.Created :
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", true);
                        break;
                    case UserTaskState.Ready :
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", true);
                        break;
                    case UserTaskState.Reserved :
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.start"].Active.SetItemValue("startEnabled", true);
                        break;
                    case UserTaskState.InProgress :
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.stop"].Active.SetItemValue("stopEnabled", true);
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.complete"].Active.SetItemValue("completeEnabled", true);

                        if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"] != null) {
                            Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"].Active.SetItemValue("ApproveEnabled", true);
                        }
                        if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"] != null) {
                            Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"].Active.SetItemValue("DeclainEnabled", true);
                        }
                        break;
                    case UserTaskState.Completed :
                        break;
                    case UserTaskState.Failed :
                        break;
                    case UserTaskState.Error :
                        break;
                    case UserTaskState.Exited :
                        break;
                    case UserTaskState.Obsolete :
                        break;
                }

            }
            */

            ListView listView = view as ListView;
            Type typeObjectOfListView;
            if (listView != null) {
                typeObjectOfListView = ((System.Type)(listView.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;
                if (typeObjectOfListView.IsInstanceOfType(typeof(BaseUserTask))) {
                    // Все кнопки запрещаем
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.activate"].Active.SetItemValue("activateEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.start"].Active.SetItemValue("startEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.stop"].Active.SetItemValue("stopEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.complete"].Active.SetItemValue("completeEnabled", false);
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.Nominate"].Active.SetItemValue("NominateEnabled", false);

                    if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"] != null) {
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"].Active.SetItemValue("ApproveEnabled", false);
                    }
                    if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"] != null) {
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"].Active.SetItemValue("DeclainEnabled", false);
                    }
                }
            }


            //if (View.Id == "TaskInvoiceInstanceDefinition_ListView" | View.Id == "BaseUserTask_ListView") {

            //    // Все кнопки запрещаем
            //    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.activate"].Active.SetItemValue("activateEnabled", false);
            //    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", false);
            //    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.start"].Active.SetItemValue("startEnabled", false);
            //    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.stop"].Active.SetItemValue("stopEnabled", false);
            //    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.complete"].Active.SetItemValue("completeEnabled", false);
            //    Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.Nominate"].Active.SetItemValue("NominateEnabled", false);

            //    if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"] != null) {
            //        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Approve"].Active.SetItemValue("ApproveEnabled", false);
            //    }
            //    if (Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"] != null) {
            //        Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinition.Declain"].Active.SetItemValue("DeclainEnabled", false);
            //    }

            //}

        }
    }
}
