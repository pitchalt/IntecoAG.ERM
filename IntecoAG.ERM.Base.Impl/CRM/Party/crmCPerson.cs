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

using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.Trw.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Person, представляющий абстрактное лицо
    /// </summary>
    [DefaultProperty("Name")]
    [Persistent("crmPartyPerson")]
    [Appearance("", AppearanceItemType.ViewItem, "", TargetItems="TrwParty", Enabled = false)]
    public partial class crmCPerson : csCComponent, crmIPerson
    {
        public crmCPerson(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(crmCPerson);
            this.CID = Guid.NewGuid();
            TrwPartyMarket = TrwPartyMarket.MARKET_UNKNOW;
            TrwPartyType = TrwPartyType.PARTY_UNKNOW;
            this.Address = new csAddress(this.Session);
        }

        #region ПОЛЯ КЛАССА
        //
        private crmPersonType _PersonType;
        private csAddress _Address;
        private String _Name;
        private String _NameFull;
        private String _RegCode;
        private String _INN;
        private TrwPartyParty _TrwParty;
        //
        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// 
        /// </summary>
        public TrwPartyParty TrwParty {
            get { return _TrwParty; }
            set { SetPropertyValue<TrwPartyParty>("TrwParty", ref _TrwParty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public crmPersonType PersonType {
            get { return _PersonType; }
            set { SetPropertyValue<crmPersonType>("PersonType", ref _PersonType, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Address.Country")]
        public csCountry Country {
            get { return Address == null ? null : Address.Country; }
            set {
                csCountry old = this.Country;
                if (old != value && Address != null) {
                    this.Address.Country = value;
                    OnChanged("Country", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        // SHU!!! 2011-08-24 Временно убрал проверку [RuleRequiredField("crmPerson.RequiredAddress", "Save")]
        public csAddress Address {
            get { return _Address; }
            set { 
                SetPropertyValue<csAddress>("Address", ref _Address, value);
                if (!IsLoading && value != null) {
                    UpdateCalcFields();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [Browsable(false)]
        [Association("crmPerson-crmBankAccount",typeof(crmBankAccount))]
        public XPCollection<crmBankAccount> BankAccounts {
            get { return GetCollection<crmBankAccount>("BankAccounts"); }
        }
        //
        IList<crmBankAccount> crmIPerson.BankAccounts {
            get { return BankAccounts; }
            //get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Association("crmPerson-crmParty", typeof(crmCParty))]
        [Browsable(false)]
        public XPCollection<crmCParty> Partys {
            get { return GetCollection<crmCParty>("Partys"); }
        }
        //
        IList<crmIParty> crmIPerson.Partys {
            get { return new ListConverter<crmIParty, crmCParty>(Partys); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(15)]
        public String INN {
            get { return _INN; }
            set { SetPropertyValue<String>("INN", ref _INN, value == null ? String.Empty : value.Trim()); }
        }
        /// <summary>
        /// Name
        /// </summary>
        [Size(200)]
        public String Name {
            get { return _Name; }
            set {
                SetPropertyValue<String>("Name", ref _Name, value);
            }
        }
        /// <summary>
        /// Full Name
        /// </summary>
        [Size(300)]
        public String NameFull {
            get { return _NameFull; }
            set {
                SetPropertyValue<String>("NameFull", ref _NameFull, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(20)]
        public String RegCode {
            get { return _RegCode; }
            set {
                SetPropertyValue<String>("RegCode", ref _RegCode, value);
            }
        }

        #endregion


        #region МЕТОДЫ

        #endregion


        Boolean _IsGovermentCustomer; 
        public Boolean IsGovermentCustomer {
            get { return _IsGovermentCustomer; }
            set { SetPropertyValue<Boolean>("IsGovermentCustomer", ref _IsGovermentCustomer, value); }
        }

        Boolean _IsTrwCorporation;
        public Boolean IsTrwCorporation {
            get { return _IsTrwCorporation; }
            set { 
                SetPropertyValue<Boolean>("IsTrwCorporation", ref _IsTrwCorporation, value);
                if (!IsLoading && !value) {
                    IsNpoCorporation = false;
                    UpdateCalcFields();
                }
            }
        }

        Boolean _IsNpoCorporation;
        public Boolean IsNpoCorporation {
            get { return _IsNpoCorporation; }
            set { 
                SetPropertyValue<Boolean>("IsNpoCorporation", ref _IsNpoCorporation, value);
                if (!IsLoading && value)
                    IsTrwCorporation = true;
            }
        }

        TrwPartyMarket _TrwPartyMarket;
        public TrwPartyMarket TrwPartyMarket {
            get { return _TrwPartyMarket; }
            set { SetPropertyValue<TrwPartyMarket>("TrwPartyMarket", ref _TrwPartyMarket, value); }
        }

        TrwPartyType _TrwPartyType;
        public TrwPartyType TrwPartyType {
            get { return _TrwPartyType; }
            set { SetPropertyValue<TrwPartyType>("TrwPartyType", ref _TrwPartyType, value); }
        }

        public void UpdateCalcFields() {
            if (IsTrwCorporation)
                TrwPartyMarket = TrwPartyMarket.MARKET_TRW;
            else {
                if (!Country.IsVED) {
                    TrwPartyMarket = TrwPartyMarket.MARKET_RUSSIA;
                }
                else {
                    if (Country.IsUIG)
                        TrwPartyMarket = TrwPartyMarket.MARKET_UIG;
                    else
                        TrwPartyMarket = TrwPartyMarket.MARKET_VED;
                }
            }
            if (TrwPartyType == TrwPartyType.PARTY_UNKNOW) {
                if (Country.IsVED) {
                    if (Country.IsUIG)
                        TrwPartyType = TrwPartyType.PARTY_UIG;
                    else
                        TrwPartyType = TrwPartyType.PARTY_VED;
                }
            }
        }
    }

}