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

namespace IntecoAG.ERM.CRM.Party {
    /// <summary>
    ///  Î‡ÒÒ Person, ÔÂ‰ÒÚ‡‚Îˇ˛˘ËÈ Û˜‡ÒÚÌËÍ‡ (Í‡Í ÒÚÓÓÌÛ) ƒÓ„Ó‚Ó‡
    /// </summary>
//    [DefaultClassOptions]
    [Persistent("crmPartyLegalPersonUnit")]
    public partial class crmCLegalPersonUnit : csCComponent, crmIParty
    {
        public crmCLegalPersonUnit(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.Party = new crmPartyRu(this.Session);
            this.ComponentType = typeof(crmCLegalPersonUnit);
            this.CID = Guid.NewGuid();
            this.Party.ComponentType = this.ComponentType;
            this.Party.CID = this.CID;
        }

        #region œŒÀﬂ  À¿——¿

        private crmCLegalPerson _LegalPerson;
        private crmPartyRu _Party;

        #endregion


        #region Component
        /// <summary>
        /// KPP
        /// </summary>
        [Browsable(false)]
        [Aggregated]
        public crmPartyRu Party {
            get { return _Party; }
            set { SetPropertyValue<crmPartyRu>("Party", ref _Party, value); }
        }

        #endregion

        #region —¬Œ…—“¬¿  À¿——¿

        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Party.Code")]
        [Size(7)]
        public string Code {
            get {
                return Party.Code;
            }
            set {
                value = value == null ? null : value.Trim();
                String old = Code;
                if (old != value && Party != null) {
                    this.Party.Code = value;
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
        [RuleRequiredField]
        [PersistentAlias("Party.Name")]
        [Size(100)]
        public string Name {
            get {
                return Party.Name;
            }
            set {
                value = value == null ? null : value.Trim();
                if (String.IsNullOrEmpty(value))
                    value = LegalPerson == null ? null : LegalPerson.Name;
                String old = Name;
                if (old != value && Party != null) {
                    this.Party.Name = value;
                    OnChanged("Name", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Party.Description")]
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get {
                return Party.Description;
            }
            set {
                value = value == null ? null : value.Trim();
                String old = Description;
                if (old != value ) {
                    this.Party.Description = value;
                    OnChanged("Description", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Party.NameFull")]
        [Size(250)]
        public string NameFull {
            get {
                return Party.NameFull;
            }
            set {
                value = value == null ? null : value.Trim();
                if (String.IsNullOrEmpty(value))
                    value = LegalPerson == null ? null : LegalPerson.NameFull;
                String old = NameFull;
                if (old != value && Party != null) {
                    this.Party.NameFull = value;
                    OnChanged("Name", old, value);
                }
            }
        }
        /// <summary>
        /// INN
        /// </summary>
        [PersistentAlias("Party.INN")]
        public string INN {
            get { return Party.INN; }
            set {
                value = value == null ? null : value.Trim();
                String old = INN;
                if (old != value) {
                    Party.INN = value;
                    OnChanged("INN", old, value);
                }
            }
        }
        /// <summary>
        /// KPP
        /// </summary>
        [PersistentAlias("Party.KPP")]
        [RuleRequiredField(TargetCriteria = "Country.CodeAlfa3 == 'RUS'")]
        public string KPP {
            get { return Party.KPP; }
            set {
                value = value == null ? null : value.Trim();
                String old = KPP;
                if (old != value) {
                    this.Party.KPP = value;
                    OnChanged("KPP", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("LegalPerson.AddressLegal.AddressString")]
        public String AddressLegal {
            get { return LegalPerson == null ? null : LegalPerson.AddressLegal == null ? null : LegalPerson.AddressLegal.ToString(); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressFact")]
        public csAddress AddressFact {
            get { return Party.AddressFact ; }
        }
        [Browsable(false)]
        [RuleFromBoolProperty("", DefaultContexts.Save, InvertResult = true, UsedProperties = "AddressFact")]
        public Boolean AddresFactIsEmpty {
            get { return AddressFact.IsEmpty; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [PersistentAlias("Party.AddressPost")]
        public csAddress AddressPost {
            get { return Party.AddressPost ; }
        }
        //
        [Browsable(false)]
        [Association("crmLegalPerson-LegalPersonUnits")]
        public crmCLegalPerson LegalPerson {
            get { return _LegalPerson; }
            set { 
                SetPropertyValue<crmCLegalPerson>("LegalPerson", ref _LegalPerson, value);
                if (!IsLoading ) {
                    if (value != null) {
                        if (String.IsNullOrEmpty(this.Name)) {
                            this.Name = value.Name;
                        }
                        if (String.IsNullOrEmpty(this.NameFull)) {
                            this.NameFull = value.NameFull;
                        }
                        this.INN = value.INN;
                        this.KPP = value.KPP;
                    }
                    OnChanged("LegalPersonManage");
                }
            }
        }
        //
        [PersistentAlias("LegalPerson")]
        public crmCLegalPerson LegalPersonManage {
            get { return LegalPerson; }
            set { 
                crmCLegalPerson old = LegalPerson;
                if (old == value) return;
                this.LegalPerson = value;
                OnChanged("LegalPersonManage", old, value);
            }
        }
        #endregion


        #region Ã≈“Œƒ€

        #endregion


        crmIPerson crmIParty.Person {
            get { return LegalPerson; }
        }

        [PersistentAlias("Party.Country")]
        public csCountry Country {
            get { return Party.Country; }
        }

        csIAddress crmIParty.AddressFact {
            get { return ((crmIParty)Party).AddressFact;  }
        }

        public string AddressFactString {
            get { return AddressFact.AddressString; }
        }

        csIAddress crmIParty.AddressPost {
            get { return ((crmIParty)Party).AddressPost; }
        }

        [PersistentAlias("LegalPerson.RegCode")]
        public string RegCode {
            get { return LegalPerson == null ? null : LegalPerson.RegCode; }
        }

    }

}