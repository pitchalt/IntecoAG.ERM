using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.CS.Security {
    /// <summary>
    /// 
    /// </summary>
    [Persistent("csSecurityRole")]
    [ImageName("BO_Role"), System.ComponentModel.DisplayName("Role")]
    public class csCSecurityRole : SecurityRole {
        public csCSecurityRole(Session session) : base(session) { }

        protected void CheckChildRoleHierarchy(List<csCSecurityRole> collectedHierarchy) {
            foreach (csCSecurityRole childRole in this.ChildRoles) {
                List<csCSecurityRole> childCollectedHierarchy = new List<csCSecurityRole>(collectedHierarchy);
                if (childCollectedHierarchy.Contains(childRole)) {
                    throw new UserFriendlyException("There is a recursion in roles hierarchy.");
                }
                else {
                    childCollectedHierarchy.Add(childRole);
                }
                childRole.CheckChildRoleHierarchy(childCollectedHierarchy);
            }
        }

        protected override List<IOperationPermission> GetPermissionsCore() {
            CheckChildRoleHierarchy(new List<csCSecurityRole>(new csCSecurityRole[] { this }));
            List<IOperationPermission> result = base.GetPermissionsCore();
            foreach (csCSecurityRole childRole in this.ChildRoles) {
                result.AddRange(childRole.GetPermissions());
            }
            return result;
        }
        [Association("csSecurityRole-csSecurityChildRoles"), MemberDesignTimeVisibility(false)]
        public XPCollection<csCSecurityRole> MasterRoles {
            get { return GetCollection<csCSecurityRole>("MasterRoles"); }
        }
        [Association("csSecurityRole-csSecurityChildRoles")]
        public XPCollection<csCSecurityRole> ChildRoles {
            get { return GetCollection<csCSecurityRole>("ChildRoles"); }
        }

        /// <summary>
        /// Список разрешений на Actions
        /// </summary>
        //[Association("csCSecurityRole-csCSecurityRoleActionPermission", typeof(csCSecurityRoleActionPermission))]
        //public XPCollection<csCSecurityRoleActionPermission> ActionPermissions {
        //    get { return GetCollection<csCSecurityRoleActionPermission>("ActionPermissions"); }
        //}

    }

}
