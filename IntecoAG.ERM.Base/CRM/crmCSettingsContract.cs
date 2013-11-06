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
using IntecoAG.ERM.CS.Settings; 
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.CRM {

    [Persistent("crmSettingsContract")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class crmCSettingsContract: csCSettings {
        protected internal crmCSettingsContract(Session ses)
            : base(ses) {
        }

        public static crmCSettingsContract GetInstance(Session session) {
            //Get the Singleton's instance if it exists 
            crmCSettingsContract result = session.FindObject<crmCSettingsContract>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = new crmCSettingsContract(session);
                result.UseCounter = 1;
                result.Code = "Договора";
                result.Name = "Настройки договорного модуля";
                result.Save();
            }
            return result;
        }

        hrmCStaffGroup _ManagerGroupOfContract;
        public hrmCStaffGroup ManagerGroupOfContract {
            get { return _ManagerGroupOfContract; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfContract;
                if (old != value) {
                    _ManagerGroupOfContract = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfContract", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfContractStaffs {
            get {
                if (ManagerGroupOfContract == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfContract.Staffs;
            }
        }

    }
}
