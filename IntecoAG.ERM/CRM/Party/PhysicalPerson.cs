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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
//
namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Êëàññ PhysicalPerson, ïðåäñòàâëÿþùèé ôèçè÷åñêîå ëèöî
    /// </summary>
//    [DefaultClassOptions]
    [Persistent("crmPartyPhysicalPerson")]
    public class crmPhysicalPerson : csCComponent, crmIPerson
    {
        public crmPhysicalPerson(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Person = new crmCPerson(this.Session);
//            Party = new crmPartyRu(this.Session, null);
            //
            this.ComponentType = typeof(crmPhysicalPerson);
            this.CID = Guid.NewGuid();
            this.Person.ComponentType = this.ComponentType;
            this.Person.CID = this.CID;
//            this.Party.ComponentType = this.ComponentType;
//            this.Party.CID = this.CID;
        }

//        [Aggregated]
//        [Browsable(false)]
//        public crmPartyRu Party;

        [Aggregated]
        [Browsable(false)]
        public crmCPerson Person;

        //crmPartyRu crmIParty.Party {
        //    get { return Party; }
        //}

        //[Browsable(false)]
        //crmIPerson crmIParty.Person {
        //    get { return Person; }
        //}

        #region ÏÎËß ÊËÀÑÑÀ
        private string _LastName;
        private string _FirstName;
        private string _MiddleName;
        #endregion


        #region ÑÂÎÉÑÒÂÀ ÊËÀÑÑÀ

        /// <summary>
        /// LastName
        /// </summary>
        [RuleRequiredField]
        [Size(30)]
        public string LastName
        {
            get { return _LastName; }
            set { 
                SetPropertyValue("LastName", ref _LastName, (value == null) ? "" : value.Trim());
//                if (!IsLoading)
//                    UpdateCalcFields();
            }
        }

        /// <summary>
        /// FirstName
        /// </summary>
        [RuleRequiredField]
        [Size(30)]
        public string FirstName {
            get { return _FirstName; }
            set { 
                SetPropertyValue("FirstName", ref _FirstName, (value == null) ? "" : value.Trim());
//                if (!IsLoading)
//                    UpdateCalcFields();
            }
        }

        /// <summary>
        /// MiddleName
        /// </summary>
        [Size(30)]
        public string MiddleName {
            get { return _MiddleName; }
            set { 
                SetPropertyValue("MiddleName", ref _MiddleName, (value == null) ? "" : value.Trim());
//                if (!IsLoading)
//                    UpdateCalcFields();
            }
        }

        [Browsable(false)]
        [PersistentAlias("Person.PersonType")]
        public crmPersonType PersonType {
            get { return Person.PersonType; }
            set {
                crmPersonType old = PersonType;
                if (old != value) {
                    Person.PersonType = value;
                    OnChanged("PersonType", old, value);
                }
            }
        }

        [Browsable(false)]
        [PersistentAlias("Person.Country")]
        public csCountry Country {
            get { return Person.Country; }
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
        [PersistentAlias("Person.Address")]
        public csAddress Address {
            get { return Person.Address; }
        }

        //[PersistentAlias("Person.BankAccounts ")]
        public IList<crmBankAccount> BankAccounts {
            get { return ((crmIPerson)Person).BankAccounts; }
        }

        [Browsable(false)]
        public IList<crmIParty> Partys {
            get { return ((crmIPerson)Person).Partys; }
        }

        [PersistentAlias("Person.Name")]
        public String Name {
            get { return Person.Name; }
            set {
                String old = Name;
                if (old != value) {
                    Person.Name = value;
                    OnChanged("Name", old, value);
                }
            }
        }

        [PersistentAlias("Person.NameFull")]
        public String NameFull {
            get { return Person.NameFull; }
            set {
                String old = NameFull;
                if (old != value) {
                    Person.NameFull = value;
                    OnChanged("NameFull", old, value);
                }
            }
        }

        [Browsable(false)]
        [PersistentAlias("Person.RegCode")]
        public String RegCode {
            get { return Person.RegCode; }
            set {
                String old = NameFull;
                if (old != value) {
                    Person.RegCode = value;
                    OnChanged("RegCode", old, value);
                }
            }
        }

        [Browsable(false)]
        [PersistentAlias("Person.INN")]
        public String INN {
            get { return Person.INN; }
            set {
                String old = INN;
                if (old != value) {
                    Person.INN = value;
                    OnChanged("INN", old, value);
                }
            }
        }

        #endregion


        #region ÌÅÒÎÄÛ

        #endregion
        //protected void UpdateCalcFields() {
        //    StringBuilder sb = new StringBuilder(250);
        //    sb.Append(LastName == null ? null : LastName.Trim());
        //    sb.Append(' ');
        //    sb.Append(FirstName == null ? null : FirstName.Trim());
        //    sb.Append(' ');
        //    sb.Append(MiddleName == null ? null : MiddleName.Trim());
        //    this.Name = sb.ToString();
        //    this.NameFull = this.Name;
        //}

    }

}