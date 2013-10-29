using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.HRM.Organization {

    [Persistent("hrmStaffPost")]
    public class hrmCStaffPost: csCCodedComponent, hrmIStaffPost {
        public hrmCStaffPost(Session ses): base(ses) { 
        }
    }
}
