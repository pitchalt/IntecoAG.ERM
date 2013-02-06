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
    public partial class fmCDirection : gfmCAnalyticBase, fmIDirection
    {
        public fmCDirection(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            this.ComponentType = typeof(fmCDirection);
            this.CID = Guid.NewGuid();
            base.AfterConstruction();
        }

        #region ПОЛЯ КЛАССА
        private hrmStaff _Manager;
        #endregion


        [Aggregated]
        [Association("fmDirection-Subjects", typeof(fmCSubject))]
        public XPCollection<fmCSubject> Subjects {
            get {
                return GetCollection<fmCSubject>("Subjects");
            }
        }

        [Aggregated]
        IList<fmISubject> fmIDirection.Subjects {
            get {
                //                return new ListConverter<fmISubject, fmCSubject>(this.Subjects);
                return new ListConverter<fmISubject, fmCSubject>(this.Subjects);
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

    }

}