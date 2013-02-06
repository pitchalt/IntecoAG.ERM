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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс RussianIndividualPerson, представляющий индивидуальное частное предприятие как участника (сторону) Договора
    /// </summary>
    //[DefaultClassOptions]
//    [MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("crmPartyBusinessman")]
    public partial class crmCBusinessman : csCComponent, crmIParty, crmIPerson
    {
        public crmCBusinessman(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Person = new crmPhysicalPerson(this.Session);
            Party = new crmPartyRu(this.Session, null);
            this.ComponentType = typeof(crmCBusinessman);
            this.CID = Guid.NewGuid();
            this.Person.ComponentType = this.ComponentType;
            this.Person.CID = this.CID;
            this.Party.ComponentType = this.ComponentType;
            this.Party.CID = this.CID;
        }

        [Aggregated]
        [Browsable(false)]
        public crmPartyRu Party;

        [Aggregated]
        [Browsable(false)]
        public crmPhysicalPerson Person;

        crmPartyRu crmIParty.Party {
            get { return Party; }
        }

        [Browsable(false)]
        crmIPerson crmIParty.Person {
            get { return Person; }
        }

        #region ПОЛЯ КЛАССА
        private String _NameHandmake;
        //private String _RegCode;
        //private String _Name;
        //private String _INN;
        //private csAddress _Address;

        #endregion


        #region СВОЙСТВА КЛАССА

        [PersistentAlias("Party.Code")]
        [Size(7)]
        //[RuleRequiredField]
        public String Code {
            get {
                return Party.Code;
            }
            set {
                String old = Code;
                if (old != value) {
                    Party.Code = value;
                    OnChanged("Code", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Party.IsClosed")]
        public Boolean IsClosed {
            get {
                return Party.IsClosed;
            }
            set {
                Boolean old = IsClosed;
                if (old != value && Party != null) {
                    this.Party.IsClosed = value;
                    OnChanged("IsClosed", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Person.Name")]
        [RuleRequiredField]
        [Size(100)]
        public String Name {
            get {
                return Person.Name;
            }
            set {
                String old = Name;
                if (old != value) {
                    Person.Name = value;
                    Party.Name = value;
                    OnChanged("Name", old, value);
                }
            }
        }
        [PersistentAlias("Person.NameFull")]
        [Size(250)]
        public String NameFull {
            get {
                return Person.NameFull;
            }
            set {
                String old = NameFull;
                if (old != value) {
                    Person.NameFull = value;
                    Party.NameFull = value;
                    OnChanged("NameFull", old, value);
                }
            }
        }
        [PersistentAlias("Person.FirstName")]
        [RuleRequiredField]
        [Size(30)]
        public String FirstName {
            get {
                return Person.FirstName;
            }
            set {
                String old = FirstName;
                if (old != value) {
                    Person.FirstName = value;
                    UpdateCalcField();
                    OnChanged("FirstName", old, value);
                }
            }
        }
        [PersistentAlias("Person.LastName")]
        [RuleRequiredField]
        [Size(30)]
        public String LastName {
            get {
                return Person.LastName;
            }
            set {
                String old = LastName;
                if (old != value) {
                    Person.LastName = value;
                    UpdateCalcField();
                    OnChanged("LastName", old, value);
                }
            }
        }
        [PersistentAlias("Person.MiddleName")]
        [Size(30)]
        public String MiddleName {
            get {
                return Person.MiddleName;
            }
            set {
                String old = MiddleName;
                if (old != value) {
                    Person.MiddleName = value;
                    UpdateCalcField();
                    OnChanged("MiddleName", old, value);
                }
            }
        }

        [PersistentAlias("Person.Country")]
        public csCountry Country {
            get {
                return Person.Country;
            }
            set {
                csCountry old = Country;
                if (old != value) {
                    Person.Country = value;
                    OnChanged("Country", old, value);
                }
            }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Person.AddressLegal")]
        public csAddress AddressLegal {
            get { return Person.Address; }
        }

        csAddress crmIPerson.Address {
            get { return AddressLegal; }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressFact")]
        public csIAddress AddressFact {
            get { return Party.AddressFact; }
        }

        [VisibleInDetailView(false)]
        [PersistentAlias("Party.AddressFactString")]
        public string AddressFactString {
            get { return Party.AddressFactString; }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressPost")]
        public csIAddress AddressPost {
            get { return Party.AddressPost; }
        }

        [PersistentAlias("Person.RegCode")]
        public string RegCode {
            get { return Person.RegCode; }
            set {
                String old = RegCode;
                if (old != value) {
                    Person.RegCode = value;
                    OnChanged("RegCode", old, value);
                }
            }
        }

        [PersistentAlias("Person.INN")]
        [RuleUniqueValue("", DefaultContexts.Save, TargetCriteria = "Country.CodeAlfa3 == 'RUS'")]
        [RuleRequiredField(TargetCriteria = "Country.CodeAlfa3 == 'RUS'")]
        [Size(15)]
        public string INN {
            get {
                return Person.INN;
            }
            set {
                String old = INN;
                if (old != value) {
                    Party.INN = value;
                    Person.INN = value;
                    OnChanged("INN", old, value);
                }
            }
        }
//        [PersistentAlias("Party.KPP")]
        [Size(15)]
//        [RuleRequiredField]
        [Browsable(false)]
        String crmIParty.KPP {
            get {
                return Party.KPP;
            }
            set {
                String old = Party.KPP;
                if (old != value) {
                    Party.KPP = value;
                    OnChanged("KPP", old, value);
                }
            }
        }
        [PersistentAlias("Person.Description")]
        [Size(SizeAttribute.Unlimited)]
        public String Description {
            get {
                return Party.Description;
            }
            set {
                String old = Description;
                if (old != value) {
                    Party.Description = value;
                    OnChanged("Description", old, value);
                }
            }
        }
        [PersistentAlias("Person.PersonType")]
        public crmPersonType PersonType {
            get {
                return Person.PersonType;
            }
            set {
                crmPersonType old = PersonType;
                if (old != value) {
                    Person.PersonType = value;
                    UpdateCalcField();
                    OnChanged("PersonType", old, value);
                }
            }
        }
        
        public String NameHandmake { 
            get { return _NameHandmake; }
            set {
                String old = _NameHandmake;
                _NameHandmake = value;
                if (!IsLoading) {
                    if (old != value) {
                        UpdateCalcField();
                        OnChanged("NameHandmake", old, value);
                    }
                }
            }
        }

        [PersistentAlias("Person.BankAccounts")]
        public IList<crmBankAccount> BankAccounts {
            get { return ((crmIPerson)Person).BankAccounts; }
        }

        IList<crmIParty> crmIPerson.Partys {
            get { return ((crmIPerson)Person).Partys; }
        }

        #endregion

        void UpdateCalcField() {
            if (String.IsNullOrEmpty(NameHandmake)) {
                StringBuilder sb = new StringBuilder(250);
                sb.Append(PersonType == null ? null : PersonType.Code);
                sb.Append(' ');
                sb.Append(LastName);
                sb.Append(' ');
                sb.Append(String.IsNullOrEmpty(FirstName) ? null : FirstName.Substring(0, 1));
                sb.Append('.');
                sb.Append(String.IsNullOrEmpty(MiddleName) ? null : MiddleName.Substring(0, 1));
                sb.Append('.');
                Name = sb.ToString();
            }
            else {
                Name = NameHandmake;
            }
        }

    }

}