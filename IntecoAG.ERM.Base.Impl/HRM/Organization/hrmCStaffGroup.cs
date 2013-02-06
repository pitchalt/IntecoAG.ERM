using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.HRM.Organization {

    [Persistent("hrmStaffGroup")]
    //[NavigationItem("Settings.SettingsCommon")]
    public class hrmCStaffGroup : csCCodedComponent {
        public hrmCStaffGroup(Session ses)
            : base(ses) {
        }

        [RuleUniqueValue("", DefaultContexts.Save)]
        public override string Code {
            get {
                return base.Code;
            }
            set {
                base.Code = value;
            }
        }

        [Aggregated]
        [Association("hrmStaffGroup-hrmStaffItem")]
        [Browsable(false)]
        public XPCollection<hrmCStaffGroupItem> StaffGroupItems {
            get { return GetCollection<hrmCStaffGroupItem>("StaffGroupItems"); }
        }

        [Association("hrmStaffGroupItem",typeof(hrmStaff), UseAssociationNameAsIntermediateTableName=true)]
        public XPCollection<hrmStaff> Staffs {
            get { 
                return GetCollection<hrmStaff>("Staffs"); 
            }
        }
        //public class StaffCollection : XPCollection<hrmStaff> {
        //    public StaffCollection(Session session, hrmCStaffGroup theOwner, XPMemberInfo refProperty) :
        //            base(session, theOwner, refProperty) {
        //        _Group = theOwner;
        //    }

        //    private hrmCStaffGroup _Group;

        //    public hrmCStaffGroup Group {
        //        get { return _Group; }
        //    }

        //    public override int BaseAdd(object newObject) {
        //        if (!IsLoaded) {
        //        }
        //        return base.BaseAdd(newObject);
        //    }

        //    public override bool BaseRemove(object theObject) {
        //        return base.BaseRemove(theObject);
        //    }
        //}
    }
}
