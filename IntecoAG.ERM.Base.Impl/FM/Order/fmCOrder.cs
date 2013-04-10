#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.Order
{
    /// <summary>
    /// Класс Order
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("fmOrder")]
    [FriendlyKeyProperty("Code")]
    [DefaultProperty("NameAndCode")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Code")]
    public abstract class fmCOrder : gfmCAnalyticBase, fmIOrder
    {
        public fmCOrder(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА
        private fmCSubject _Subject;
        private hrmStaff _Manager;
        private hrmStaff _ManagerPlanDepartment;
        private crmContractDeal _SourceDeal;
        private String _SourceOther;
        private crmCParty _SourceParty;
        [Persistent("SourceName")]
        private String _SourceName;
        private String _BuhAccount;
        private csNDSRate _AVTRate;
        private Int32 _BuhIntNum;
        private Decimal _KoeffKB;
        private Decimal _KoeffOZM;
        #endregion

        #region СВОЙСТВА КЛАССА

        //[Browsable(false)]
        public string NameAndCode {
            get {
                if (Name != null && Name.Trim() != "")
                    return string.Concat(Code, " ", Name);
                else
                    return this.Code;
            }
        }

        [Association("fmSubject-Orders")]
        public fmCSubject Subject {
            get { return _Subject; }
            set {
                if (IsLoading) {
                    _Subject = value;
                }
                else {
                    fmCSubject old = _Subject;
                    if (value != old) {
                        if (old != null)
                            old.OrdersRemove(this);
                        _Subject = value;
                        if (value != null)
                            value.OrdersAdd(this);
                        OnChanged("Subject", old, value);
                    }
                }
            }
        }

        fmISubject fmIOrder.Subject {
            get { return Subject; }
        }

        ////[PersistentAlias("Subject.SourceName")]
        //public String SourceName {
        //    get {
        //        if (String.IsNullOrEmpty(_SourceName)) {
        //            if (Subject != null)
        //                _SourceName = Subject.SourceName;
        //        }
        //        return _SourceName;
        //    }
        //    set {
        //        SetPropertyValue<String>("SourceName", ref _SourceName, value);
        //    }
        //}

        [DataSourceProperty("ManagerSource")]
        public hrmStaff Manager {
            get { return _Manager; }
            set {
                SetPropertyValue<hrmStaff>("Manager", ref _Manager, value);
            }
        }
        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfOrderStaffs; }
        }

        hrmIStaff fmIOrder.Manager {
            get { return Manager; }
            set {
                hrmIStaff old = Manager;
                Manager = value as hrmStaff;
                if (Manager != old)
                    OnChanged("Manager", old, Manager);
            }
        }

        [DataSourceProperty("ManagerPlanDepartmentSource")]
        public hrmStaff ManagerPlanDepartment {
            get { return _ManagerPlanDepartment; }
            set {
                SetPropertyValue<hrmStaff>("ManagerPlanDepartment", ref _ManagerPlanDepartment, value);
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerPlanDepartmentSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfPlanDepartmentStaffs; }
        }

        hrmIStaff fmIOrder.ManagerPlanDepartment {
            get { return ManagerPlanDepartment; }
            set {
                hrmIStaff old = ManagerPlanDepartment;
                ManagerPlanDepartment = value as hrmStaff;
                if (ManagerPlanDepartment != old)
                    OnChanged("Manager", old, Manager);
            }
        }
        public crmContractDeal SourceDeal {
            get { return _SourceDeal; }
            set {
                SetPropertyValue<crmContractDeal>("SourceDeal", ref _SourceDeal, value);
                if (!IsLoading) {
                    if (value != null)
                        SourceParty = value.Customer;
                    SourceNameUpdate();
                }
            }
        }

        public String SourceOther {
            get { return _SourceOther; }
            set {
                SetPropertyValue<String>("SourceOther", ref _SourceOther, value);
                if (!IsLoading) {
                    SourceNameUpdate();
                }
            }
        }

        public void SourceNameUpdate() {
            if (SourceDeal != null) {
                _SourceName = SourceDeal.Name;
            } else {
                _SourceName = _SourceOther;
            }
        }

        [PersistentAlias("_SourceName")]
        public String SourceName {
            get {
                if (_SourceName == null)
                    SourceNameUpdate();
                return _SourceName;
            }
        }

        //[RuleRequiredField]
        public crmCParty SourceParty {
            get { return _SourceParty; }
            set { SetPropertyValue<crmCParty>("SourceParty", ref _SourceParty, value); }
        }

        crmIParty fmIOrder.SourceParty {
            get { return SourceParty; }
            set {
                crmIParty old = SourceParty;
                SourceParty = value as crmCParty;
            }
        }

        [Size(30)]
        public String BuhAccount {
            get { return _BuhAccount; }
            set { SetPropertyValue<String>("BuhAccount", ref _BuhAccount, value); }
        }

        public csNDSRate AVTRate {
            get { return _AVTRate; }
            set { SetPropertyValue<csNDSRate>("AVTRate", ref _AVTRate, value); }
        }


        [Browsable(false)]
        public Int32 BuhIntNum {
            get { return _BuhIntNum; }
            set { SetPropertyValue<Int32>("BuhIntNum", ref _BuhIntNum, value); }
        }

        public Decimal KoeffKB {
            get { return _KoeffKB; }
            set { SetPropertyValue<Decimal>("KoeffKB", ref _KoeffKB, value); }
        }

        public Decimal KoeffOZM {
            get { return _KoeffOZM; }
            set { SetPropertyValue<Decimal>("KoeffOZM", ref _KoeffOZM, value); }
        }

        private fmСOrderAnalitycAccouterType _AnalitycAccouterType;
        public fmСOrderAnalitycAccouterType AnalitycAccouterType {
            get { return _AnalitycAccouterType; }
            set { SetPropertyValue<fmСOrderAnalitycAccouterType>("AnalitycAccouterType", ref _AnalitycAccouterType, value); }
        }

        private fmСOrderAnalitycAVT _AnalitycAVT;
        public fmСOrderAnalitycAVT AnalitycAVT {
            get { return _AnalitycAVT; }
            set { SetPropertyValue<fmСOrderAnalitycAVT>("AnalitycAccouterType", ref _AnalitycAVT, value); }
        }

        private fmСOrderAnalitycWorkType _AnalitycWorkType;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "!IsClosed")]
        public fmСOrderAnalitycWorkType AnalitycWorkType {
            get { return _AnalitycWorkType; }
            set { SetPropertyValue<fmСOrderAnalitycWorkType>("AnalitycWorkType", ref _AnalitycWorkType, value); }
        }

        private fmСOrderAnalitycOrderSource _AnalitycOrderSource;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "!IsClosed")]
        public fmСOrderAnalitycOrderSource AnalitycOrderSource {
            get { return _AnalitycOrderSource; }
            set { SetPropertyValue<fmСOrderAnalitycOrderSource>("AnalitycOrderSource", ref _AnalitycOrderSource, value); }
        }

        private fmСOrderAnalitycFinanceSource _AnalitycFinanceSource;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "!IsClosed")]
        public fmСOrderAnalitycFinanceSource AnalitycFinanceSource {
            get { return _AnalitycFinanceSource; }
            set { SetPropertyValue<fmСOrderAnalitycFinanceSource>("AnalitycFinanceSource", ref _AnalitycFinanceSource, value); }
        }

        private fmСOrderAnalitycMilitary _AnalitycMilitary;
//        [RuleRequiredField]
        [RuleRequiredField(TargetCriteria = "!IsClosed")]
        public fmСOrderAnalitycMilitary AnalitycMilitary {
            get { return _AnalitycMilitary; }
            set { SetPropertyValue<fmСOrderAnalitycMilitary>("AnalitycMilitary", ref _AnalitycMilitary, value); }
        }
        private fmСOrderAnalitycFedProg _AnalitycFedProg;
        public fmСOrderAnalitycFedProg AnalitycFedProg {
            get { return _AnalitycFedProg; }
            set { SetPropertyValue<fmСOrderAnalitycFedProg>("AnalitycFedProg", ref _AnalitycFedProg, value); }
        }
        private fmСOrderAnalitycOKVED _AnalitycOKVED;
        public fmСOrderAnalitycOKVED AnalitycOKVED {
            get { return _AnalitycOKVED; }
            set { SetPropertyValue<fmСOrderAnalitycOKVED>("AnalitycOKVED", ref _AnalitycOKVED, value); }
        }
        private fmСOrderAnalitycRegion _AnalitycRegion;
        public fmСOrderAnalitycRegion AnalitycRegion {
            get { return _AnalitycRegion; }
            set { SetPropertyValue<fmСOrderAnalitycRegion>("AnalitycRegion", ref _AnalitycRegion, value); }
        }
        private fmСOrderAnalitycBigCustomer _AnalitycBigCustomer;
        public fmСOrderAnalitycBigCustomer AnalitycBigCustomer {
            get { return _AnalitycBigCustomer; }
            set { SetPropertyValue<fmСOrderAnalitycBigCustomer>("AnalitycBigCustomer", ref _AnalitycBigCustomer, value); }
        }

        #endregion

        #region fmIOrder

        public void CopyTo(fmIOrder to) {
            fmIOrderLogic.CopyTo(this, to);
        }

        #endregion
    }

}