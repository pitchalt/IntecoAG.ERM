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



             // Запрещаем показ действия в режиме DetailView
            if (View.Id == "TaskInvoiceInstanceDefinitionAdmin_DetailView") {
                // 
                //Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.WinNewObjectViewController>().NewObjectAction.Active.SetItemValue("create", false);

                // http://www.devexpress.com/Support/Center/p/Q265465.aspx
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinitionAdmin.create"].Active.SetItemValue("create", false);
                Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["TaskInvoiceInstanceDefinitionAdmin.create"].Active.SetItemValue("CreateEnabled", false);
            }

            if (View.Id == "SimpleContract_ListView" | View.Id == "ComplexContract_ListView" | View.Id == "WorkPlan_ListView") {

                //Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.WinNewObjectViewController>().Actions..NewObjectAction.Active.SetItemValue("create", false);


                //// Разбор случаев
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.activate"].Active.SetItemValue("activateEnabled", false);
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.claim"].Active.SetItemValue("claimEnabled", false);
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.start"].Active.SetItemValue("startEnabled", false);
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.stop"].Active.SetItemValue("stopEnabled", false);
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.complete"].Active.SetItemValue("completeEnabled", false);
                //Frame.GetController<DevExpress.ExpressApp.SystemModule.ObjectMethodActionsViewController>().Actions["BaseUserTask.Nominate"].Active.SetItemValue("NominateEnabled", false);

            }
        }

    }
}
