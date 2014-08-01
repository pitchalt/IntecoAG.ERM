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
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;
//
using IntecoAG.ERM.Trw.Budget;
//
namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Статус ручной проверки объекта
    /// </summary>
    public enum ManualCheckStateEnum {
        /// <summary>
        /// Не проверено
        /// </summary>
        NO_CHECKED = 1,
        /// <summary>
        /// Проверено
        /// </summary>
        IS_CHECKED = 2
    }

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
    public class crmCParty : csCComponent, crmIParty
    {
        public crmCParty(Session ses) : base(ses) {
        }

        public crmCParty(Session ses, crmCPerson person) : base(ses) {
            Person = person;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.AddressFact = new csAddress(this.Session);
            this.AddressPost = new csAddress(this.Session);
            this.ComponentType = typeof(crmCParty);
            this.CID = Guid.NewGuid();
            this.ManualCheckStatus = ManualCheckStateEnum.IS_CHECKED;
        }

        #region ПОЛЯ КЛАССА
        private Boolean _IsClosed;
        private string _Code;
        private string _Name;
        private string _Description;
        private string _NameFull;
        //private crmCPerson _Person;
        private string _INN;
        private string _KPP;
        private csAddress _AddressFact;
        private csAddress _AddressPost;
        private Boolean _IsSyncRequired;
        private ManualCheckStateEnum _ManualCheckStatus;

        #endregion


        #region Component

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Code
        /// </summary>
        [Size(7)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        /// <summary>
        /// Name
        /// </summary>
        [Size(200)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String Description {
            get { return _Description; }
            set { SetPropertyValue<String>("Description", ref _Description, value); }
        }

        /// <summary>
        /// NameFull
        /// </summary>
        [Size(300)]
        public String NameFull {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsClosed {
            get { return _IsClosed; }
            set { SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value); }
        }

        /// <summary>
        /// Требуется синхронизация
        /// </summary>
        public Boolean IsSyncRequired {
            get { return _IsSyncRequired; }
            set { SetPropertyValue<Boolean>("IsSyncRequired", ref _IsSyncRequired, value); }
        }

        [Browsable(true)]
        public ManualCheckStateEnum ManualCheckStatus {
            get {
                return _ManualCheckStatus;
            }
            set {
                SetPropertyValue<ManualCheckStateEnum>("ManualCheckStatus", ref _ManualCheckStatus, value);
            }
        }


        ///// <summary>
        ///// Name - описание
        ///// </summary>
        //public override crmPerson RealPerson {
        //    get { return Person; }
        //}

        //[Browsable(false)]
        //[Association("crmPerson-crmParty")]
        //[RuleRequiredField("crmParty.RequiredPerson", "Save")]
        //[Delayed]
        //public crmCPerson Person {
        //    get { return GetDelayedPropertyValue<crmCPerson>("Person"); }
        //    set { SetDelayedPropertyValue<crmCPerson>("Person", value); }
        //}
        crmCPerson _Person;
        [Association("crmPerson-crmParty")]
        [ExplicitLoading]
        public crmCPerson Person {
            get { return _Person; }
            set { SetPropertyValue<crmCPerson>("Person", ref _Person, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Person")]
        crmIPerson crmIParty.Person {
            get { return Person; }
        }
        /// <summary>
        /// 
        /// </summary>
        crmCParty crmIParty.Party {
            get { return this; }
        }
        /// <summary>
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

        [Association("TrwBudgetPeriod-crmParty")]
        [Browsable(false)]
        public XPCollection<TrwBudgetPeriod> TrwBudgetPeriods {
            get { return GetCollection<TrwBudgetPeriod>("TrwBudgetPeriods"); }
        }
    
        #endregion


        #region МЕТОДЫ

        #endregion

    }

}