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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;

//using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.HRM.Organization
{
    /// <summary>
    /// Класс Department, представляющий подразделение
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("hrmDepartment")]
    public partial class hrmDepartment : BaseObject, ITreeNode
    {
        public hrmDepartment(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА
        private Boolean _IsClosed;
        private string _Code;
        private string _BuhCode;
        private string _PostCode;
        private string _Name;
        private string _Phone;
        private hrmDepartment _UpDepartment;
        #endregion


        #region СВОЙСТВА КЛАССА

        [Size(10)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        public Boolean IsClosed {
            get { return _IsClosed; }
            set { SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value); }
        }

        [Size(10)]
        [VisibleInLookupListView(true)]
        public string BuhCode {
            get { return _BuhCode; }
            set { SetPropertyValue<string>("BuhCode", ref _BuhCode, value); }
        }

        [Size(10)]
        public string PostCode {
            get { return _PostCode; }
            set { SetPropertyValue<string>("PostCode", ref _PostCode, value); }
        }

        [Size(70)]
        [VisibleInLookupListView(true)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        [Size(70)]
        [VisibleInLookupListView(true)]
        public string Phone {
            get {
                return _Phone;
            }
            set {
                SetPropertyValue<string>("Phone", ref _Phone, value);
            }
        }

        [Association("hrmDepartment-Departments")]
        [VisibleInListView(false)]
        public hrmDepartment UpDepartment {
            get { return _UpDepartment; }
            set { SetPropertyValue<hrmDepartment>("UpDepartment", ref _UpDepartment, value); }
        }
        [Association("hrmDepartment-Departments", typeof(hrmDepartment))]
        public XPCollection<hrmDepartment> DownDepartments {
            get { return GetCollection<hrmDepartment>("DownDepartments"); }
        }
        
        #endregion


        #region МЕТОДЫ

        #endregion


        IBindingList ITreeNode.Children {
            get { return DownDepartments; }
        }

        string ITreeNode.Name {
            get { return Name; }
        }

        ITreeNode ITreeNode.Parent {
            get { return UpDepartment; }
        }
    }

}