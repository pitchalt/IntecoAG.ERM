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
using DevExpress.ExpressApp.ConditionalAppearance;
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
    [DefaultProperty("FamalyIO")]
    //[DefaultClassOptions]
    [Persistent("hrmStaff")]
    //[MiniNavigation("PhysicalPerson", "Физическое лицо", TargetWindow.NewModalWindow, 1)]
    //[MiniNavigation("PhysicalPerson", "Физическое лицо", TargetWindow.NewModalWindow, 1)]
    [Appearance("", AppearanceItemType.Action, "", TargetItems="Delete", Enabled=false)]
    public partial class hrmStaff : BaseObject, hrmIStaff
    {
        public hrmStaff(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();

            PhysicalPerson = new crmPhysicalPerson(this.Session);
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete error");
        }

        #region ПОЛЯ КЛАССА

        private string _Phone;
        private string _Room;
        private hrmIStaffPost _Post;
        private Boolean _IsClosed;
        private DateTime _DateBegin;
        private DateTime _DateEnd;

        #endregion


        #region СВОЙСТВА КЛАССА

        [Browsable(false)]
        [Association("hrmStaffGroupItem", typeof(hrmCStaffGroup), UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<hrmCStaffGroup> Groups {
            get {
                return GetCollection<hrmCStaffGroup>("Groups");
            }
        }

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

        private string _BuhCode;
        [Size(5)]
        public string BuhCode {
            get { return _BuhCode; }
            set { SetPropertyValue<String>("BuhCode", ref _BuhCode, value); }
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

        [PersistentAlias("PhysicalPerson.Sex")]
        public crmPhysicalPersonSex Sex {
            get {
                return PhysicalPerson.Sex;
            }
            set {
                crmPhysicalPersonSex old = Sex;
                if (old != value) {
                    PhysicalPerson.Sex = value;
                    OnChanged("Sex", old, value);
                }
            }
        }
        /// <summary>
        /// Должность
        /// </summary>
        public hrmIStaffPost Post {
            get { return _Post; }
            set { SetPropertyValue<hrmIStaffPost>("Post", ref _Post, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsClosed {
            get { return _IsClosed; }
            set { SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        }

        [PersistentAlias("PhysicalPerson.NameFull")]
        public string NameFull {
            get {
                return PhysicalPerson.NameFull;
            }
        }

        [Size(70)]
        //[VisibleInLookupListView(true)]
        public string Phone {
            get {
                return _Phone;
            }
            set {
                SetPropertyValue<string>("Phone", ref _Phone, value);
            }
        }

        [Size(30)]
        public string Room {
            get {
                return _Room;
            }
            set {
                SetPropertyValue<string>("Room", ref _Room, value);
            }
        }

        /// <summary>
        /// Famaly I.O.
        /// </summary>
        [Size(250)]
        public String FamalyIO {
            get {
                string familyio = "";
                if (!string.IsNullOrEmpty(this.LastName)) {
                    familyio = this.LastName;
                    if (!string.IsNullOrEmpty(this.FirstName)) {
                        familyio = familyio + " " + this.FirstName.Substring(0, 1) + ".";
                    } else {
                        if (!string.IsNullOrEmpty(this.MiddleName)) {
                            familyio = familyio + " -.";
                        }
                    }
                    if (!string.IsNullOrEmpty(this.MiddleName)) {
                        familyio = familyio + " " + this.MiddleName.Substring(0, 1) + ".";
                    }
                } else {
                    if (!string.IsNullOrEmpty(this.FirstName)) {
                        familyio = this.FirstName;
                    }
                }
                return familyio; 
            }
            //set {
            //    SetPropertyValue<String>("FamalyIO", ref _FamalyIO, value);
            //}
        }

        /// <summary>
        /// I.O. Famaly
        /// </summary>
        [Size(250)]
        public String IOFamaly {
            get {
                string iofamily = "";
                if (!string.IsNullOrEmpty(this.LastName)) {
                    iofamily = this.LastName;
                    if (!string.IsNullOrEmpty(this.MiddleName) && !string.IsNullOrEmpty(this.FirstName)) {
                        iofamily = this.MiddleName.Substring(0, 1) + ". " + iofamily;
                    }
                    if (!string.IsNullOrEmpty(this.FirstName)) {
                        iofamily = this.FirstName.Substring(0, 1) + ". " + iofamily;
                    }
                } else {
                    if (!string.IsNullOrEmpty(this.FirstName)) {
                        iofamily = this.FirstName;
                    }
                    if (!string.IsNullOrEmpty(this.MiddleName)) {
                        iofamily = iofamily + " " + this.MiddleName.Substring(0, 1) + ".";
                    }
                }
                return iofamily; 
            }
            //set {
            //    SetPropertyValue<String>("FamalyIO", ref _FamalyIO, value);
            //}
        }


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
            if (_MiddleName.Trim() != "") {
                sb.Append((_FirstName.Trim() != "") ? (_FirstName.Trim())[0].ToString() : "-");
                sb.Append('.');
                sb.Append(_MiddleName[0]);
                sb.Append('.');
            } else if (_FirstName.Trim() != "") {
                sb.Append(_FirstName[0]);
                sb.Append('.');
            }
            this.PhysicalPerson.Name = sb.ToString();
            OnChanged("Name");
            OnChanged("NameFull");
        }

        #endregion

    }
}