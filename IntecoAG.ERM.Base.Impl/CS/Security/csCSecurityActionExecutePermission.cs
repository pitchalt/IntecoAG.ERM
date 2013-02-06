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
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp.Security;

namespace IntecoAG.ERM.CS.Security {

    public class ActionExecutePermission : OperationPermissionBase {
        public const string ActionExecuteName = "ActionExecuteName";
        public ActionExecutePermission()
            : base(ActionExecuteName) {
        }
        public override IList<string> GetSupportedOperations() {
            return new string[] { ActionExecuteName };
        }
    }

    public class ActionExecutePermissionRequest : OperationPermissionRequestBase {
        public ActionExecutePermissionRequest()
            : base(ActionExecutePermission.ActionExecuteName) { }

    }

    public class ActionExecutePermissionRequestProcessor : PermissionRequestProcessorBase<ActionExecutePermissionRequest> {
        protected override bool IsRequestFit(
            ActionExecutePermissionRequest permissionRequest, OperationPermissionBase permission,
            IRequestSecurityStrategy securityInstance) {
            ActionExecutePermission ActionExecutePermission = permission as ActionExecutePermission;
            if (permissionRequest == null || ActionExecutePermission == null) {
                return false;
            }
            return permissionRequest.Operation == ActionExecutePermission.Operation;
        }
    }

}
