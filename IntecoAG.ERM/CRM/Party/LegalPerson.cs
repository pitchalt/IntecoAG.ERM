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
    /// Класс LegalPerson, представляющий юридическое лицо как участника (сторону) Договора
    /// </summary>
//    [DefaultClassOptions]
    [Persistent("crmPartyLegalPerson")]
    public abstract partial class crmLegalPerson : crmPerson
    {
        public crmLegalPerson(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            Name = String.Empty;
            RegNumber = String.Empty;
        }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// Name
        /// </summary>
        private string _RegNumber;
        [Size(20)]
        public string RegNumber {
            get { return _RegNumber; }
            set { SetPropertyValue<string>("RegNumber", ref _RegNumber, value == null ? String.Empty : value.Trim()); }
        }
        /// <summary>
        /// Name
        /// </summary>
        private string _Name;
        [RuleRequiredField("crmLegalPerson.RequiredName", "Save")]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value == null ? String.Empty : value.Trim()); }
        }

        #endregion


        #region МЕТОДЫ
        public override string FullName {
            get {
                return Name;
            }
        }

        #endregion

    }

}