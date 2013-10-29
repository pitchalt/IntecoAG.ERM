using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.HRM.Organization {

    [Persistent("hrmStaffGroupItem_TMP")]
    public class hrmCStaffGroupItem: csCCodedComponent {
        public hrmCStaffGroupItem(Session ses) : base(ses) { 

        }

        private hrmStaff _Staff;
        private hrmCStaffGroup _StaffGroup;
        
        [Association("hrmStaffGroup-hrmStaffItem")]
        public hrmCStaffGroup StaffGroup {
            get { return _StaffGroup; }
            set { SetPropertyValue<hrmCStaffGroup>("StaffGroup", ref _StaffGroup, value); }
        }

        public hrmStaff Staff {
            get { return _Staff; }
            set { SetPropertyValue<hrmStaff>("Staff", ref _Staff, value); }
        }
    }
}
