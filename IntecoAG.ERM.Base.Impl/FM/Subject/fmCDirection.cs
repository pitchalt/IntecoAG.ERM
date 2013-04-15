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
using System.Linq;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    /// Класс Direction
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Finance")]
    [Persistent("fmDirection")]
    [FriendlyKeyProperty("Code")]
    [DefaultProperty("Name")]
    [Appearance("", AppearanceItemType.ViewItem, "", Enabled=false, TargetItems="IsClosed")]
    [Appearance("", AppearanceItemType.Action, "Status != 'PROJECT' || !IsDeleteAllow", Enabled = false, TargetItems = "Delete")]
    public partial class fmCDirection : gfmCAnalyticBase, fmIDirection, IStateMachineProvider
    {
        public fmCDirection(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            this.ComponentType = typeof(fmCDirection);
            this.CID = Guid.NewGuid();
            base.AfterConstruction();
            Status = fmIDirectionStatus.PROJECT;
        }

        protected override void OnDeleting() {
            if (this.IsSaving && (Status != fmIDirectionStatus.PROJECT || !IsDeleteAllow)) {
                throw new InvalidOperationException("Delete is not allowed");
            }
        }

        #region ПОЛЯ КЛАССА
        private hrmStaff _Manager;
        private fmIDirectionStatus _Status; 
        #endregion

//        [Aggregated]
        [Association("fmDirection-Subjects", typeof(fmCSubject))]
        public XPCollection<fmCSubject> Subjects {
            get {
                return GetCollection<fmCSubject>("Subjects");
            }
        }

//        [Aggregated]
        IList<fmISubject> fmIDirection.Subjects {
            get {
                //                return new ListConverter<fmISubject, fmCSubject>(this.Subjects);
                return new ListConverter<fmISubject, fmCSubject>(this.Subjects);
            }
        }
        //
        public fmIDirectionStatus Status {
            get { return _Status; }
            set {
                fmIDirectionStatus old = _Status;
                if (old != value) {
                    _Status = value;
                    if (!IsLoading) {
                        OnChanged("Status", old, value);
                        if (value == fmIDirectionStatus.CLOSED)
                            IsClosed = true;
                        else
                            IsClosed = false;
                    }
                }
            }
        }
        [Browsable(false)]
        public Boolean IsDeleteAllow {
            get {
                return Subjects.Count == 0;
            }
        }
        [Browsable(false)]
        public Boolean IsStatusOpenedAllow {
            get {
                return true;
            }
        }
        [Browsable(false)]
        public Boolean IsStatusClosedAllow {
            get {
                return this.Subjects.Aggregate(true, (x, subj) => x && subj.IsClosed);
            }
        }
        //
        [DataSourceProperty("ManagerSource")]
        public hrmStaff Manager {
            get { return _Manager; }
            set {
                SetPropertyValue<hrmStaff>("Manager", ref _Manager, value);
            }
        }
        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfDirectionStaffs; }
        }
        //
        hrmIStaff fmIDirection.Manager {
            get { return Manager; }
            set {
                hrmIStaff old = Manager;
                Manager = value as hrmStaff;
                if (Manager != old)
                    OnChanged("Manager", old, Manager);
            }
        }

        public IList<IStateMachine> GetStateMachines() {
            List<IStateMachine> result = new List<IStateMachine>();
            result.Add(new fmCDirectionSM());
            return result;
        }
    }

}