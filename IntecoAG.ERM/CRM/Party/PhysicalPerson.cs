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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс PhysicalPerson, представляющий физическое лицо
    /// </summary>
//    [DefaultClassOptions]
    [Persistent("crmPartyPhysicalPerson")]
    public abstract partial class crmPhysicalPerson : crmPerson
    {
        public crmPhysicalPerson(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// LastName
        /// </summary>
        private string _LastName;
        [RuleRequiredField("crmPhysicalPerson.RequiredLastName", "Save")]
        public string LastName
        {
            get { return _LastName; }
            set { SetPropertyValue("LastName", ref _LastName, (value == null) ? "" : value.Trim()); }
        }

        /// <summary>
        /// FirstName
        /// </summary>
        private string _FirstName;
        [RuleRequiredField("crmPhysicalPerson.RequiredFirstName", "Save")]
        public string FirstName {
            get { return _FirstName; }
            set { SetPropertyValue("FirstName", ref _FirstName, (value == null) ? "" : value.Trim()); }
        }

        /// <summary>
        /// MiddleName
        /// </summary>
        private string _MiddleName;
        public string MiddleName {
            get { return _MiddleName; }
            set { SetPropertyValue("MiddleName", ref _MiddleName, (value == null) ? "" : value.Trim()); }
        }

        /// <summary>
        /// FullName
        /// </summary>
        public override string FullName {
            get {
                string Res = "";
                string ln = (string.IsNullOrEmpty(_LastName)) ? "" : _LastName.Trim();
                string fn = (string.IsNullOrEmpty(_FirstName)) ? "" : _FirstName.Trim();
                string mn = (string.IsNullOrEmpty(_MiddleName)) ? "" : _MiddleName.Trim();
                Res = (ln + " " + fn + " " + mn).Trim();
                return Res;
            }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullName;
        }

        #endregion

    }

}