using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CS.Security;

namespace IntecoAG.ERM.Module.CS {
    public partial class csNavigationController : WindowController {
        public csNavigationController() {
            this.TargetWindowType = WindowType.Main;
            InitializeComponent();
            RegisterActions(components);
        }

        private ShowNavigationItemController navigationController;

        protected override void OnFrameAssigned() {
            UnsubscribeFromEvents();
            base.OnFrameAssigned();
            navigationController =
                Frame.GetController<ShowNavigationItemController>();
            if (navigationController != null) {
                navigationController.CustomShowNavigationItem += 
                    new EventHandler<CustomShowNavigationItemEventArgs>(navigationController_CustomShowNavigationItem);
                navigationController.NavigationItemCreated += new EventHandler<NavigationItemCreatedEventArgs>(navigationController_NavigationItemCreated);
            }
        }

        void navigationController_NavigationItemCreated(object sender, NavigationItemCreatedEventArgs e) {
            if (e.NavigationItem.Id == "MyDetails") {
                ViewShortcut shortcut = e.NavigationItem.Data as ViewShortcut;
                if (shortcut != null) {
                    shortcut["ObjectKey"] = SecuritySystem.CurrentUserId.ToString();
                }
            }
        }
        // ≈сли установлен Detail View, то Custom не срабатывает, поскольку видимо перехватываетс€ внутренним обработчиком, 
        // который просто создает новый объект
        void navigationController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e) {
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "ERMCurrentUser") {
//                ChoiceActionItem parent =
//                     e.ActionArguments.SelectedChoiceActionItem.ParentItem;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                ViewShortcut shortcut = e.ActionArguments.SelectedChoiceActionItem.Data as ViewShortcut;
                View view = Application.CreateDetailView(objectSpace, "csCSecurityUser_DetailView", true, objectSpace.GetObject<csCSecurityUser>(SecuritySystem.CurrentUser as csCSecurityUser));
                e.ActionArguments.ShowViewParameters.CreatedView = view;
                e.Handled = true;
            }

        }

        private void UnsubscribeFromEvents() {
            if (navigationController != null) {
                navigationController.CustomShowNavigationItem -= 
                    new EventHandler<CustomShowNavigationItemEventArgs>(navigationController_CustomShowNavigationItem);
                navigationController = null;
            }
        }
//        protected override void Dispose(bool disposing) {
//            UnsubscribeFromEvents();
//            base.Dispose(disposing);
//        }
    }
}
