using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CS.Security {

    public class csCSecurityUserId : ICustomFunctionOperator {

        const String Name = "ERMCurrentUserId";

        static csCSecurityUserId() {
            if (CriteriaOperator.GetCustomFunction(Name) == null)
                CriteriaOperator.RegisterCustomFunction(new csCSecurityUserId());
        }

        String ICustomFunctionOperator.Name {
            get { return Name; }
        }

        public object Evaluate(params object[] operands) {
            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            if (user == null) return null;
            return user.Oid;
        }

        public Type ResultType(params Type[] operands) {
            return typeof(Guid);
        }

        public static void Register() { }
    }

    public class csCSecurityUserStaff : ICustomFunctionOperator {

        const String Name = "ERMCurrentUserStaff";
        
        static csCSecurityUserStaff() {
            if (CriteriaOperator.GetCustomFunction(Name) == null)
                CriteriaOperator.RegisterCustomFunction(new csCSecurityUserStaff());
        }

        String ICustomFunctionOperator.Name {
            get { return Name; }
        }

        public object Evaluate(params object[] operands) {
            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            if (user == null) return null;
            return user.Staff;
        }

        public Type ResultType(params Type[] operands) {
            return typeof(hrmStaff);
        }

        public static void Register() { }
    }

    public class csCSecurityUserDepartment : ICustomFunctionOperator {

        const String Name = "ERMCurrentUserDepartment";

        static csCSecurityUserDepartment() {
            if (CriteriaOperator.GetCustomFunction(Name) == null)
                CriteriaOperator.RegisterCustomFunction(new csCSecurityUserDepartment());
        }

        String ICustomFunctionOperator.Name {
            get { return Name; }
        }

        public object Evaluate(params object[] operands) {
            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            if (user == null) return null;
            if (user.Staff == null) return null;
            return user.Staff.Department;
        }

        public Type ResultType(params Type[] operands) {
            return typeof(hrmDepartment);
        }

        public static void Register() { }
    }

    //public class csCSecurityUserIdParameter : ReadOnlyParameter {
    //    public csCSecurityUserIdParameter() : base("ERMCurrentUserId", typeof(Guid)) { }
    //    public override object CurrentValue {
    //        get {
    //            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
    //            if (user == null) return null;
    //            return user.Oid;
    //        }
    //    }
    //}

}