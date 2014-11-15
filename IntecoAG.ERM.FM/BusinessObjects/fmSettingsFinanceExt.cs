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
using DevExpress.ExpressApp.Security;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Settings; 
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CS.Security;
//
namespace IntecoAG.ERM.FM {

    [Persistent("fmSettingsFinance")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class fmCSettingsFinance: csCSettings {
        protected internal fmCSettingsFinance(Session ses)
            : base(ses) {
        }

        public static fmCSettingsFinance GetInstance(IObjectSpace os) {
            fmCSettingsFinance result = os.FindObject<fmCSettingsFinance>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = os.CreateObject<fmCSettingsFinance>();
                result = InstanceInit(result);
                result.Save();
            }
            return result;
        } 
        public static fmCSettingsFinance GetInstance(Session session) {
            //Get the Singleton's instance if it exists 
            fmCSettingsFinance result = session.FindObject<fmCSettingsFinance>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = new fmCSettingsFinance(session);
                result = InstanceInit(result);
                result.Save();
            }
            return result;
        }
        public static fmCSettingsFinance InstanceInit(fmCSettingsFinance instance) {
            instance.UseCounter = 1;
            instance.Code = "Финансы";
            instance.Name = "Настройки финансового модуля";
            return instance;
        }
        hrmCStaffGroup _ManagerGroupOfDirection;
        public hrmCStaffGroup ManagerGroupOfDirection {
            get { return _ManagerGroupOfDirection; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfDirection;
                if (old != value) {
                    _ManagerGroupOfDirection = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfDirection", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfDirectionStaffs {
            get {
                if (ManagerGroupOfDirection == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfDirection.Staffs;
            }
        }

        hrmCStaffGroup _ManagerGroupOfSubject;
        public hrmCStaffGroup ManagerGroupOfSubject {
            get { return _ManagerGroupOfSubject; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfSubject;
                if (old != value) {
                    _ManagerGroupOfSubject = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfSubject", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfSubjectStaffs {
            get {
                if (ManagerGroupOfSubject == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfSubject.Staffs;
            }
        }

        hrmCStaffGroup _ManagerGroupOfOrder;
        public hrmCStaffGroup ManagerGroupOfOrder {
            get { return _ManagerGroupOfOrder; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfOrder;
                if (old != value) {
                    _ManagerGroupOfOrder = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfOrder", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfOrderStaffs {
            get {
                if (ManagerGroupOfOrder == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfOrder.Staffs;
            }
        }

        hrmCStaffGroup _ManagerGroupOfSignAccountDepartment;
        public hrmCStaffGroup ManagerGroupOfSignAccountDepartment {
            get { return _ManagerGroupOfSignAccountDepartment; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfSignAccountDepartment;
                if (old != value) {
                    _ManagerGroupOfSignAccountDepartment = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfSignAccountDepartment", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfSignAccountDepartmentStaffs {
            get {
                if (ManagerGroupOfSignAccountDepartment == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfSignAccountDepartment.Staffs;
            }
        }


        hrmCStaffGroup _ManagerGroupOfSignPlanDepartment;
        public hrmCStaffGroup ManagerGroupOfSignPlanDepartment {
            get { return _ManagerGroupOfSignPlanDepartment; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfSignPlanDepartment;
                if (old != value) {
                    _ManagerGroupOfSignPlanDepartment = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfSignPlanDepartment", old, value);
                }
            }
        }
        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfSignPlanDepartmentStaffs {
            get {
                if (ManagerGroupOfSignPlanDepartment == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfSignPlanDepartment.Staffs;
            }
        }

        hrmCStaffGroup _ManagerGroupOfPlanDepartment;
        public hrmCStaffGroup ManagerGroupOfPlanDepartment {
            get { return _ManagerGroupOfPlanDepartment; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfPlanDepartment;
                if (old != value) {
                    _ManagerGroupOfPlanDepartment = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfPlanDepartment", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfPlanDepartmentStaffs {
            get {
                if (ManagerGroupOfPlanDepartment == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfPlanDepartment.Staffs;
            }
        }

        hrmCStaffGroup _ManagerGroupOfBudgetDepartment;
        public hrmCStaffGroup ManagerGroupOfBudgetDepartment {
            get { return _ManagerGroupOfBudgetDepartment; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfBudgetDepartment;
                if (old != value) {
                    _ManagerGroupOfBudgetDepartment = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfBudgetDepartment", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfBudgetDepartmentStaffs {
            get {
                if (ManagerGroupOfBudgetDepartment == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfBudgetDepartment.Staffs;
            }
        }

        hrmCStaffGroup _ManagerGroupOfFinancialDepartment;
        public hrmCStaffGroup ManagerGroupOfFinancialDepartment {
            get { return _ManagerGroupOfFinancialDepartment; }
            set {
                hrmCStaffGroup old = _ManagerGroupOfFinancialDepartment;
                if (old != value) {
                    _ManagerGroupOfFinancialDepartment = value;
                    if (old != null)
                        old.UseCounter--;
                    if (value != null)
                        value.UseCounter++;
                    OnChanged("ManagerGroupOfFinancialDepartment", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerGroupOfFinancialDepartmentStaffs {
            get {
                if (ManagerGroupOfFinancialDepartment == null)
                    return new XPCollection<hrmStaff>(this.Session);
                else
                    return ManagerGroupOfFinancialDepartment.Staffs;
            }
        }

        private csCSecurityRole _MainBuhRole;
        /// <summary>
        /// Головная роль для групп бухгалтерии (для распределения доступа к служебным запискам)
        /// </summary>
        public csCSecurityRole MainBuhRole {
            get {
                return _MainBuhRole;
            }
            set {
                SetPropertyValue<csCSecurityRole>("MainBuhRole", ref _MainBuhRole, value);
            }
        }

        // 
        String _CurrencyCodeRub;
        public String CurrencyCodeRub {
            get {
                return _CurrencyCodeRub;
            }
            set {
                SetPropertyValue<String>("CurrencyCodeRub", ref _CurrencyCodeRub, value);
            }
        }

        Decimal _CurrencyDefaultCourceRub;
        public Decimal CurrencyDefaultCourceRub {
            get {
                return _CurrencyDefaultCourceRub;
            }
            set {
                SetPropertyValue<Decimal>("CurrencyDefaultCourceRub", ref _CurrencyDefaultCourceRub, value);
            }
        }

        private hrmDepartment _FBKDepartment; // Подразделение, в которое направлена служебная записка
        /// <summary>
        /// Подразделение, в которое направлена служебная записка
        /// </summary>
        public hrmDepartment FBKDepartment {
            get {
                return _FBKDepartment;
            }
            set {
                SetPropertyValue<hrmDepartment>("FBKDepartment", ref _FBKDepartment, value);
            }
        }

    }
}
