// Developer Express Code Central Example:
// How to: Implement Custom Permission, Role and User Objects
// 
// This example illustrates how to create custom security objects, such as
// permissions, roles and users. We will implement a permission that allows
// administrators to secure the exporting functionality in an XAF application. The
// complete description is available in the How to: Implement Custom Permission,
// Role and User Objects
// (http://documentation.devexpress.com/#Xaf/CustomDocument3384) topic.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E3794

using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security;
using System.Collections.Generic;

namespace IntecoAG.ERM.CS.Security {

    [System.ComponentModel.DisplayName("ActionExecuteName Permission")]
    public class csCSecurityActionExecutePermissionData : PermissionData {
        private bool canExecute = true;
        protected override string GetPermissionInfoCaption() {
            return "ActionExecuteName";
        }

        public csCSecurityActionExecutePermissionData(Session session)
            : base(session) {
        }

        public override IList<IOperationPermission> GetPermissions() {
            IList<IOperationPermission> result = new List<IOperationPermission>();
            if (canExecute) {
                result.Add(new ActionExecutePermission());
            }
            return result;
        }

        public bool CanExecute {
            get { return canExecute; }
            set { SetPropertyValue("CanExecute", ref canExecute, value); }
        }
    }
}
