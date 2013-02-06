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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Office, представляющий участника (как сторону) Договора
    /// </summary>
    [LikeSearchPathList(new string[] { 
        "Name", 
        "INN", 
        "AddressFact.AddressString",
        "Person.Name", 
        "Person.Address.AddressString"
    })]
    [MiniNavigation("ComponentObject", "Сторона", TargetWindow.Default, 1)]
    [MiniNavigation("Person", "Лицо", TargetWindow.Default, 2)]
    [MiniNavigation("This", "Party", TargetWindow.Default, 3)]
    [Persistent("crmPartyParty")]
    [DefaultProperty("Name")]
    [FriendlyKeyProperty("Code")]
    public class crmPartyRu : crmCParty, crmIParty
    {
        public crmPartyRu(Session ses) : base(ses) {
        }

        public crmPartyRu(Session ses, crmCPerson person) : base(ses) {
            Person = person;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.AddressFact = new csAddress(this.Session);
            this.AddressPost = new csAddress(this.Session);
            this.ComponentType = typeof(crmPartyRu);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА
        private crmCPerson _Person;
        private string _INN;
        private string _KPP;
        private csAddress _AddressFact;
        private csAddress _AddressPost;
        #endregion


        #region Component

        #endregion

        #region СВОЙСТВА КЛАССА
        ///// <summary>
        ///// Name - описание
        ///// </summary>
        //public override crmPerson RealPerson {
        //    get { return Person; }
        //}

        [Browsable(false)]
        [Association("crmPerson-crmParty")]
        //[RuleRequiredField("crmParty.RequiredPerson", "Save")]
        public crmCPerson Person {
            get { return _Person; }
            set {
                SetPropertyValue<crmCPerson>("Person", ref _Person, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Person")]
        crmIPerson crmIParty.Person {
            get { return Person; }
        }
        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        crmPartyRu crmIParty.Party {
            get { return this; }
        }
        /// 
        /// </summary>
        [PersistentAlias("Person.RegCode")]
        public String RegCode {
            get { return Person == null ? null : Person.RegCode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String INN {
            get { return _INN; }
            set { SetPropertyValue("INN", ref _INN, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(15)]
        public string KPP {
            get { return _KPP; }
            set { if (_KPP != value) SetPropertyValue("KPP", ref _KPP, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Person.PersonType")]
        public crmPersonType PersonType {
            get { return Person == null ? null : Person.PersonType; }
        }
        /// <summary>
        /// AddressFact - описание
        /// </summary>
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public csAddress AddressFact {
            get { return _AddressFact; }
            set { SetPropertyValue<csAddress>("AddressFact", ref _AddressFact, value as csAddress); }
        }
        csIAddress crmIParty.AddressFact {
            get { return AddressFact;  }
        }
        [VisibleInDetailView(false)]
        [NonPersistent]
        public String AddressFactString {
            get { return AddressFact.AddressString; }
        }

        /// <summary>
        /// AddressFact - описание
        /// </summary>
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public csAddress AddressPost {
            get { return _AddressPost; }
            set { SetPropertyValue<csAddress>("AddressPost", ref _AddressPost, value as csAddress); }
        }
        csIAddress crmIParty.AddressPost {
            get { return AddressPost; }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("AddressFact.Country")]
        public csCountry Country {
            get { return AddressFact == null ? null : AddressFact.Country; }
            set {
                csCountry old = this.Country;
                if (old != value && AddressFact != null) {
                    this.AddressFact.Country = value;
                    OnChanged("Country", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [VisibleInListView(false)]
        [PersistentAlias("Person.Address")]
        public csAddress AddressLegal {
            get { return this.Person == null ? null : this.Person.Address; }
        }
        /// <summary>
        /// Паша!!! Переделать список счетов
        /// </summary>
        [PersistentAlias("Person.BankAccounts")]
        public XPCollection<crmBankAccount> BankAccounts {
            get { return null; }
        }
        
        #endregion


        #region МЕТОДЫ

        #endregion

    }

}