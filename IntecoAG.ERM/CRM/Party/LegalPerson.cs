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
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс LegalPerson, представляющий юридическое лицо как участника (сторону) Договора
    /// </summary>
//    [DefaultClassOptions]
    [Persistent("crmPartyLegalPerson")]
    public partial class crmCLegalPerson : csCComponent, crmIParty, crmIPerson
    {
        public crmCLegalPerson(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            Person = new crmCPerson(this.Session);
            Party = new crmPartyRu(this.Session, null);
            this.ComponentType = typeof(crmCLegalPerson);
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
        public crmCPerson Person;

        crmPartyRu crmIParty.Party {
            get { return Party; }
        }

        [Browsable(false)]
        crmIPerson crmIParty.Person {
            get { return Person; }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Party.Code")]
        [Size(7)]
        //[RuleRequiredField]
        public string Code {
            get {
                return Party.Code;
            }
            set {
                String old = Code;
                if (old != value) {
                    Party.Code = value;
                    OnChanged("Code");
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
                    if (String.IsNullOrEmpty(NameFull) || NameFull == old)
                        NameFull = value;
                    OnChanged("Name");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
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
                    OnChanged("NameFull");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Person.Country")]
        [RuleRequiredField]
        public csCountry Country {
            get {
                return Person.Country;
            }
            set {
                csCountry old = Country;
                if (old != value) {
                    Person.Country = value;
                    OnChanged("Country");
                }
            }
        }
        //
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Person.AddressLegal")]
        public csAddress AddressLegal {
            get { return Person.Address; }
        }
        csAddress crmIPerson.Address {
            get { return AddressLegal; }
        }
        [Browsable(false)]
        [RuleFromBoolProperty("", DefaultContexts.Save, InvertResult = true, UsedProperties = "AddressLegal")]
        public Boolean AddresLegalIsEmpty {
            get { return AddressLegal.IsEmpty; }
        }
        //
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressFact")]
        public csAddress AddressFact {
            get { return Party.AddressFact; }
        }
        csIAddress crmIParty.AddressFact {
            get { return AddressFact; }
        }
        [Browsable(false)]
        [RuleFromBoolProperty("", DefaultContexts.Save, InvertResult = true, UsedProperties = "AddressFact")]
        public Boolean AddresFactIsEmpty {
            get { return AddressFact.IsEmpty; }
        }
        [PersistentAlias("Party.AddressFactString")]
        public string AddressFactString {
            get { return Party.AddressFactString; }
        }
        //
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressPost")]
        public CS.Country.csIAddress AddressPost {
            get { return Party.AddressPost; }
        }

        [PersistentAlias("Person.RegCode")]
        public String RegCode {
            get { return Person.RegCode;  }
            set {
                String old = RegCode;
                if (old != value) {
                    Person.RegCode = value;
                    OnChanged("RegCode");
                }
            }
        }
        [PersistentAlias("Person.INN")]
        [RuleUniqueValue("IntecoAG.ERM.CRM.Party.LegalPerson.INN.Unique", DefaultContexts.Save, TargetCriteria = "Country.CodeAlfa3 == 'RUS'")]
        [RuleRequiredField(TargetCriteria = "Country.CodeAlfa3 == 'RUS'")]
        [Size(15)]
        public String INN {
            get {
                return Person.INN;
            }
            set {
                String old = INN;
                if (old != value) {
                    Party.INN = value;
                    Person.INN = value;
                    foreach(crmCLegalPersonUnit unit in LegalPersonUnits ) {
                        unit.INN = value;
                    }
                    OnChanged("INN");
                }
            }
        }
        [PersistentAlias("Party.KPP")]
        [Size(15)]
        [RuleRequiredField(TargetCriteria = "Country.CodeAlfa3 == 'RUS'")]
        public String KPP {
            get {
                return Party.KPP;
            }
            set {
                String old = KPP;
                if (old != value) {
                    Party.KPP = value;
                    OnChanged("KPP");
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
                    OnChanged("Description");
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
                    OnChanged("PersonType");
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

        [Aggregated]
        [Association("crmLegalPerson-LegalPersonUnits", typeof(crmCLegalPersonUnit))]
        public XPCollection<crmCLegalPersonUnit> LegalPersonUnits {
            get { return GetCollection<crmCLegalPersonUnit>("LegalPersonUnits"); }
        }

    }

}