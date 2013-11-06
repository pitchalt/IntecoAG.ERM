using System;
using System.Configuration;
using System.Collections.Generic;
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
    [Persistent("csSecurityUser")]
    [Appearance("",AppearanceItemType.Action,"",TargetItems="Delete", Enabled=false)]
    [ImageName("BO_User"), System.ComponentModel.DisplayName("User")]
    public class csCSecurityUser : SecurityUser {
        public csCSecurityUser(Session session) : base(session) { }

        private String _StaffBuhCode;
        private hrmStaff _Staff;
        private Boolean _IsFirstEnter;

        private static String _NullRightGroup = ConfigurationManager.AppSettings["SecurityGroups.NullRightRole"];
        public override void AfterConstruction() {
            base.AfterConstruction();

            csCSecurityRole def_role = new XPQuery<csCSecurityRole>(this.Session).Where(role => role.Name == _NullRightGroup).FirstOrDefault();
            this.Roles.Add(def_role);            
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Unable to delete");
        }
        /// <summary>
        /// 
        /// </summary>
        //[Appearance("", AppearanceItemType.ViewItem, "Staff != Null", Enabled=false)]
        public hrmStaff Staff {
            get { return _Staff; }
            set {
                hrmStaff old = _Staff;
                if (old != value) {
                    _Staff = value;
                    if (!IsLoading){
                        if (!_IsFirstEnter) {
                            _IsFirstEnter = true;
                            if (value != null)
                                this.StaffBuhCode = value.BuhCode;
                            else
                                this.StaffBuhCode = null;
                            _IsFirstEnter = false;
                        }
                        OnChanged("Staff", old, value);
                        OnChanged("FirstName");
                        OnChanged("MiddleName");
                        OnChanged("LastName");
                        OnChanged("Department");
                    }
                }
            }
        }
        //[Appearance("", AppearanceItemType.ViewItem, "Staff != Null", Enabled = false)]
        [PersistentAlias("Staff.BuhCode")]
        public String StaffBuhCode
        {
            get {
                if (Staff != null)
                    return Staff.BuhCode;
                return _StaffBuhCode; 
            }
            set {
                String old = _StaffBuhCode;
                if (old != value) {
                    _StaffBuhCode = value;
                    if (!IsLoading) {
                        if (!_IsFirstEnter) {
                            _IsFirstEnter = true;
                            if (String.IsNullOrEmpty(value))
                                Staff = null;
                            else {
                                Staff = new XPQuery<hrmStaff>(this.Session).Where(staff => staff.BuhCode == value).FirstOrDefault();
                            }
                            _IsFirstEnter = false;
                        }
                        OnChanged("StaffBuhCode", old, value);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Staff.FirstName")]
        public String FirstName {
            get {
                if (Staff != null)
                    return Staff.FirstName;
                else
                    return null;
            }
        }
        [PersistentAlias("Staff.LastName")]
        public String LastName {
            get {
                if (Staff != null)
                    return Staff.LastName;
                else
                    return null;
            }
        }
        [PersistentAlias("Staff.MiddleName")]
        public String MiddleName {
            get {
                if (Staff != null)
                    return Staff.MiddleName;
                else
                    return null;
            }
        }

        [PersistentAlias("Staff.Department")]
        public hrmDepartment Department {
            get {
                if (Staff != null)
                    return Staff.Department;
                else
                    return null;
            }
        }

    }
    
}
