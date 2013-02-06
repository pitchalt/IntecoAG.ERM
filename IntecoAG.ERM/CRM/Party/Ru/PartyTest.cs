using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Party.Ru {

    [Persistent("crmPartyTest")]
    public class PartyTest : csCComponent, crmIParty, crmIPerson {
        public PartyTest(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Person = new crmCPerson(this.Session);
            Party = new crmPartyRu(this.Session, null);
            this.ComponentType = typeof(PartyTest);
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

        [PersistentAlias("Party.NameFull")]
        public string NameFull {
            get {
                return Party.NameFull;
            }
            set {
                if (!IsLoading) {
                    Party.NameFull = value;
                    OnChanged("NameFull");
                }
            }
        }

        [PersistentAlias("Person.Country")]
        public csCountry Country {
            get {
                return Person.Country;
            }
            set {
                if (!IsLoading) {
                    Person.Country = value;
                    OnChanged("Country");
                }
            }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressFact")]
        public CS.Country.csIAddress AddressFact {
            get { return Party.AddressFact; }
        }

        [PersistentAlias("Party.AddressFactString")]
        public string AddressFactString {
            get { return Party.AddressFactString; }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressPost")]
        public CS.Country.csIAddress AddressPost {
            get { return Party.AddressPost; }
        }
        [PersistentAlias("Person.RegCode")]
        public string RegCode {
            get { return Person.RegCode;  }
            set {
                if (!IsLoading) {
                    Person.RegCode = value;
                    OnChanged("RegCode");
                }
            }
        }
        [PersistentAlias("Person.INN")]
        public string INN {
            get {
                return Person.INN;
            }
            set {
                if (!IsLoading) {
                    Party.INN = value;
                    Person.INN = value;
                    OnChanged("INN");
                }
            }
        }
        [PersistentAlias("Party.KPP")]
        public string KPP {
            get {
                return Party.KPP;
            }
            set {
                if (!IsLoading) {
                    Party.KPP = value;
                    OnChanged("KPP");
                }
            }
        }
        [PersistentAlias("Party.Code")]
        public string Code {
            get {
                return Party.Code;
            }
            set {
                if (!IsLoading) {
                    Person.Code = value;
                    Party.Code = value;
                    OnChanged("Code");
                }
            }
        }
        [PersistentAlias("Person.Name")]
        public string Name {
            get {
                return Party.Name;
            }
            set {
                if (!IsLoading) {
                    Person.Name = value;
                    Party.Name = value;
                    OnChanged("Name");
                }
            }
        }
        [PersistentAlias("Person.Description")]
        public string Description {
            get {
                return Party.Description;
            }
            set {
                if (!IsLoading) {
                    Person.Description = value;
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
                if (!IsLoading) {
                    Person.PersonType = value;
                    OnChanged("PersonType");
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

        [PersistentAlias("Person.BankAccounts")]
        public IList<crmBankAccount> BankAccounts {
            get { return ((crmIPerson)Person).BankAccounts; }
        }

        IList<crmIParty> crmIPerson.Partys {
            get { return ((crmIPerson)Person).Partys; }
        }

    }

}