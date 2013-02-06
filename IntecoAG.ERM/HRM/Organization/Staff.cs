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
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.HRM.Organization
{
    /// <summary>
    /// Класс Department, представляющий подразделение
    /// </summary>
    [DefaultProperty("NameFull")]
    //[DefaultClassOptions]
    [Persistent("hrmStaff")]
    //[MiniNavigation("PhysicalPerson", "Физическое лицо", TargetWindow.NewModalWindow, 1)]
    //[MiniNavigation("PhysicalPerson", "Физическое лицо", TargetWindow.NewModalWindow, 1)]
    public partial class hrmStaff : BaseObject
    {
        public hrmStaff(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();

            PhysicalPerson = new crmPhysicalPerson(this.Session);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmPhysicalPerson _PhysicalPerson;
        //[ExpandObjectMembers(ExpandObjectMembers.Always)]
        [Browsable(false)]
        public crmPhysicalPerson PhysicalPerson {
            get { return _PhysicalPerson; }
            set { SetPropertyValue<crmPhysicalPerson>("PhysicalPerson", ref _PhysicalPerson, value); }
        }

        private string _Level;
        [Size(30)]
        public string Level {
            get { return _Level; }
            set { SetPropertyValue<string>("Level", ref _Level, value); }
        }

        [PersistentAlias("PhysicalPerson.FirstName")]
        public string FirstName {
            get {
                return PhysicalPerson.FirstName;
            }
            set {
                String old = FirstName;
                if (old != value) {
                    PhysicalPerson.FirstName = value;
                    UpdateCalcField();
                    OnChanged("FirstName", old, value);
                }
            }
        }

        [PersistentAlias("PhysicalPerson.LastName")]
        public string LastName {
            get {
                return PhysicalPerson.LastName;
            }
            set {
                String old = LastName;
                if (old != value) {
                    PhysicalPerson.LastName = value;
                    UpdateCalcField();
                    OnChanged("LastName", old, value);
                }
            }
        }

        [PersistentAlias("PhysicalPerson.MiddleName")]
        public string MiddleName {
            get {
                return PhysicalPerson.MiddleName;
            }
            set {
                String old = FirstName;
                if (old != value) {
                    PhysicalPerson.MiddleName = value;
                    UpdateCalcField();
                    OnChanged("MiddleName", old, value);
                }
            }
        }

        [PersistentAlias("PhysicalPerson.NameFull")]
        public string NameFull {
            get {
                return PhysicalPerson.NameFull;
            }
        }

        //private string _Description;
        //[Size(SizeAttribute.Unlimited)]
        //public string Description {
        //    get { return _Description; }
        //    set { SetPropertyValue<string>("Description", ref _Description, value); }
        //}

        //private hrmDepartment _Department;
        //[Association("hrmDepartment-Staffs")]
        //[VisibleInListView(false)]
        //public hrmDepartment Department {
        //    get { return _Department; }
        //    set { SetPropertyValue<hrmDepartment>("Department", ref _Department, value); }
        //}

        //[Association("hrmDepartment-Staffs"), Aggregated]
        //public XPCollection<hrmStaff> Staffs {
        //    get { return GetCollection<hrmStaff>("Staffs"); }
        //}
        
        #endregion


        #region МЕТОДЫ

        void UpdateCalcField() {
            String _FirstName = FirstName;
            String _LastName = LastName;
            String _MiddleName = MiddleName;
            if (String.IsNullOrEmpty(_FirstName))
                _FirstName = " ";
            if (String.IsNullOrEmpty(_LastName))
                _LastName = " ";
            if (String.IsNullOrEmpty(_MiddleName))
                _MiddleName = " ";
            StringBuilder sb = new StringBuilder(250);
            sb.Append(this.LastName);
            sb.Append(' ');
            sb.Append(this.FirstName);
            sb.Append(' ');
            sb.Append(this.MiddleName);
            this.PhysicalPerson.NameFull = sb.ToString();
            //
            sb = new StringBuilder(250);
            sb.Append(_LastName);
            sb.Append(' ');
            sb.Append(_FirstName[1]);
            sb.Append('.');
            sb.Append(_MiddleName[1]);
            sb.Append('.');
            this.PhysicalPerson.Name = sb.ToString();
            OnChanged("Name");
            OnChanged("NameFull");
        }

        #endregion

    }
}