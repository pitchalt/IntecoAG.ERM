using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.CS.Security {
    public partial class csCSecurityActionExecutePermissionViewController : ViewController {
        public csCSecurityActionExecutePermissionViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            // Заполнение списка actions в текущем объекте
            FillActionList();
        }

        private void FillActionList() {
            if (View != null && View.CurrentObject as csCSecurityRoleActionPermission != null) {
            }
        }
    }
}
