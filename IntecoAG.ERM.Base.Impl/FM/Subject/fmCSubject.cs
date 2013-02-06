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
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    /// Класс Subject
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    [FriendlyKeyProperty("Code")]
    [DefaultProperty("Name")]
    [Persistent("fmSubject")]
    public abstract class fmCSubject : gfmCAnalyticBase, fmISubject
    {
        public fmCSubject(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        #region ПОЛЯ КЛАССА
        private fmCDirection _Direction;
        private fmСOrderAnalitycWorkType _AnalitycWorkType;
        private fmСOrderAnalitycFinanceSource _AnalitycFinanceSource;
        private crmContractDeal _SourceDeal;
        private String _SourceOther;
        private crmCParty _SourceParty;
        [Persistent("SourceName")]
        private String _SourceName;
        private hrmStaff _Manager;
        private hrmStaff _ManagerPlanDepartment;
        #endregion

        #region Associations

        [Association("fmDirection-Subjects")]
        public fmCDirection Direction {
            get { return _Direction; }
            set { SetPropertyValue<fmCDirection>("Direction", ref _Direction, value); }
        }
        fmIDirection fmISubject.Direction {
            get {
                return this.Direction;
            }
        }
        //
        [Association("fmSubject-Orders", typeof(Order.fmCOrder))]
        //[Aggregated]
        public XPCollection<fmCOrder> Orders {
            get {
                return GetCollection<Order.fmCOrder>("Orders");
            }
        }
        //[Aggregated]
        IList<fmIOrder> fmISubject.Orders {
            get {
                return new ListConverter<fmIOrder, fmCOrder>(this.Orders);
            }
        }

        public void OrdersAdd(fmCOrder order) {
            order.SourceDeal = this.SourceDeal;
            order.SourceOther = this.SourceOther;
            order.SourceParty = this.SourceParty;
            order.AnalitycWorkType = this.AnalitycWorkType;
            order.AnalitycFinanceSource = this.AnalitycFinanceSource;
            order.Manager = this.Manager;
            order.ManagerPlanDepartment = this.ManagerPlanDepartment;
        }

        public void OrdersRemove(fmCOrder order) { 
        }

        [DataSourceProperty("ManagerSource")]
        public hrmStaff Manager {
            get { return _Manager; }
            set {
                SetPropertyValue<hrmStaff>("Manager", ref _Manager, value);
            }
        }
        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfSubjectStaffs; }
        }

        hrmIStaff fmISubject.Manager {
            get { return Manager; }
            set {
                hrmIStaff old = Manager;
                Manager = value as hrmStaff;
//                if (Manager != old)
//                    OnChanged("Manager", old, Manager);
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

        hrmIStaff fmISubject.ManagerPlanDepartment {
            get { return Manager; }
            set {
                hrmIStaff old = ManagerPlanDepartment;
                ManagerPlanDepartment = value as hrmStaff;
            }
        }

        public crmCParty SourceParty {
            get { return _SourceParty; }
            set { SetPropertyValue<crmCParty>("SourceParty", ref _SourceParty, value); }
        }

        crmIParty fmISubject.SourceParty {
            get { return SourceParty; }
            set {
                crmIParty old = SourceParty;
                SourceParty = value as crmCParty;
            }
        }

        #endregion

        #region СВОЙСТВА КЛАССА

        public crmContractDeal SourceDeal {
            get { return _SourceDeal; }
            set { 
                SetPropertyValue<crmContractDeal >("SourceDeal", ref _SourceDeal, value );
                if (!IsLoading) {
                    if (value != null)
                        SourceParty = value.Customer;
                    SourceNameUpdate();
                }
            }
        }

        public String SourceOther {
            get { return _SourceOther ; }
            set { 
                SetPropertyValue<String>("SourceOther", ref _SourceOther, value );
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

        public fmСOrderAnalitycWorkType AnalitycWorkType {
            get { return _AnalitycWorkType; }
            set { SetPropertyValue<fmСOrderAnalitycWorkType>("AnalitycWorkType", ref _AnalitycWorkType, value); }
        }

        public fmСOrderAnalitycFinanceSource AnalitycFinanceSource {
            get { return _AnalitycFinanceSource; }
            set { SetPropertyValue<fmСOrderAnalitycFinanceSource>("AnalitycFinanceSource", ref _AnalitycFinanceSource, value); }
        }

        #endregion

    }

}