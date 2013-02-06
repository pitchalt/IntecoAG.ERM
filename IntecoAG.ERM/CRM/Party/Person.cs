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
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;
using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Person, представляющий абстрактное лицо
    /// </summary>
//    [DefaultClassOptions]
    [DefaultProperty("FullName")]
    [Persistent("crmPerson")]
    public abstract partial class crmPerson : BaseObject
    {
        public crmPerson(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }

        #region ПОЛЯ КЛАССА
        private csAddress _Address;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Division
        /// </summary>
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [RuleRequiredField("crmPerson.RequiredAddress", "Save")]
        public csAddress Address {
            get { return _Address; }
            set { SetPropertyValue<csAddress>("Address", ref _Address, value); }
        }
        /// <summary>
        /// Division
        /// </summary>
        [Aggregated]
        [Association("crmPerson-crmBankAccount",typeof(crmBankAccount))]
        public XPCollection<crmBankAccount> BankAccounts {
            get { return GetCollection<crmBankAccount>("BankAccounts"); }
        }
        [Association("crmPerson-crmParty", typeof(crmParty))]
        public XPCollection<crmParty> Partys {
            get { return GetCollection<crmParty>("Partys"); }
        }
        /// <summary>
        /// Name
        /// </summary>
        private string _INN;
        [Size(15)]
        public string INN {
            get { return _INN; }
            set { SetPropertyValue("INN", ref _INN, value == null ? String.Empty : value.Trim()); }
        }
        /// <summary>
        /// Name
        /// </summary>
        public abstract string FullName { get; }

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