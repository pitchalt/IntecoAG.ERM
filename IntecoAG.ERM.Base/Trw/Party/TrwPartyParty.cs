using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Trw.Party {
    public enum TrwPartyState {
        NEW = 1,
        EDITED = 2,
        CONFIRMED = 4,
        IN_TRW = 8 
    }

    public enum TrwPartyLegalType {
        LEGAL_PERSON = 0, 
        PHYSICAL_PERSON = 1
    }

    public enum TrwPartyType {
        PARTY_UNKNOW = 0,
        PARTY_WITH_BANK_ACCOUNT = 1,
        PARTY_WITHOUT_BANK_ACCOUNT = 2,
        PARTY_GROUP = 3,
        PARTY_UIG = 4,
        PARTY_VED = 5,
        PARTY_TAX_AUTHORITY = 6,
        PARTY_INTERMEDIA_TREASURE = 7,
        PARTY_INTERMEDIA_BANK = 8
    }

    public enum TrwPartyMarket {
        MARKET_UNKNOW = 0,
        MARKET_TRW = 1,
        MARKET_UIG = 2,
        MARKET_VED = 4,
        MARKET_RUSSIA = 8
    }

    public enum TrwAccountType { 
        ACCOUNT_UNKNOW  = 0,
        ACCOUNT_CURRENT = 1,
        ACCOUNT_ACCREDITIVE = 2,
        ACCOUNT_TRANSIT = 3,
        ACCOUNT_VEKSEL = 4,
        ACCOUNT_DEPOSIT = 6
    }

    [Persistent("TrwPartyParty")]
    [Appearance("", AppearanceItemType.ViewItem, "Person != null", TargetItems="Person,State", Enabled=false)]
    [Appearance("", AppearanceItemType.ViewItem, "State != 'NEW' && State != 'EDITED'", TargetItems="*", Enabled=false)]
    public class TrwPartyParty : csCComponent {
        public TrwPartyParty(Session session) : base(session) { }
        public override void AfterConstruction() {            
            base.AfterConstruction();
            State = TrwPartyState.NEW;
            _PartyLegalType = null;
            Name = String.Empty;
            INN = String.Empty;
            KPP = String.Empty;
//            UseCounter++;
        }

        public Boolean IsDeal;
        public Boolean IsPay;

        private TrwPartyState _State;
        public TrwPartyState State {
            get { return _State; }
            set { SetPropertyValue<TrwPartyState>("State", ref _State, value); }
        }

        [Persistent("PartyLegalType")]
        TrwPartyLegalType? _PartyLegalType;
        [PersistentAlias("_PartyLegalType")]
        public TrwPartyLegalType? PartyLegalType {
            get { return _PartyLegalType; }
        }
        public Int16 PartyLegalTypeCode {
            get { return (Int16) (PartyLegalType != null ? ((Int16)PartyLegalType) : -1); }
        }

        //private TrwPartyMarket _Market;
        [RuleRequiredField("", "Approve")]
        [PersistentAlias("Person.TrwPartyMarket")]
        public TrwPartyMarket Market {
            get { return Person != null ? Person.TrwPartyMarket : TrwPartyMarket.MARKET_UNKNOW; }
            set {
                if (!IsLoading && Person != null) {
                    Person.TrwPartyMarket = value;
                    OnChanged("Market");
                }
            }
        }
        public String MarketCode {
            get { return TrwPartyMarketGetCode(this.Market); }
        }
        static String TrwPartyMarketGetCode(TrwPartyMarket value) {
            switch (value) {
                case TrwPartyMarket.MARKET_TRW:
                    return "X";
                case TrwPartyMarket.MARKET_RUSSIA:
                    return "RF";
                case TrwPartyMarket.MARKET_VED:
                    return "ЯРМ1";
                case TrwPartyMarket.MARKET_UIG:
                    return "ЯРМ2";
                default:
                    return String.Empty;
            }
        }
        static String TrwPartyMarketGetName(TrwPartyMarket value) {
            switch (value) {
                case TrwPartyMarket.MARKET_TRW:
                    return "Корпорация";
                case TrwPartyMarket.MARKET_RUSSIA:
                    return "Россия";
                case TrwPartyMarket.MARKET_VED:
                    return "Ярмарка 1";
                case TrwPartyMarket.MARKET_UIG:
                    return "Ярмарка 2";
                default:
                    return String.Empty;
            }
        }
        //private TrwPartyType _PartyType;
        [RuleRequiredField("", "Approve")]
        [PersistentAlias("Person.TrwPartyType")]
        public TrwPartyType PartyType {
            get { return Person != null ? Person.TrwPartyType : TrwPartyType.PARTY_UNKNOW; }
            set {
                if (!IsLoading && Person != null) {
                    Person.TrwPartyType = value;
                    OnChanged("PartyType");
                }
            }
        }

        public Int32 PartyTypeCode {
            get { return (Int32)PartyType; }
        }

        public Int32 TrwFACCode {
            get { return 25; }
        }

        private crmCPerson _Person;
        [RuleRequiredField]
        [ExplicitLoading]
        public crmCPerson Person {
            get { return _Person; }
            set {
                if (!IsLoading && value != null && value.TrwParty != null)
                    throw new ArgumentException("Person already linked to TrwParty");
                SetPropertyValue<crmCPerson>("Person", ref _Person, value);
                if (!IsLoading) {
                    if (value != null) {
                        value.TrwParty = this;
                    }
                    UpdateCalcFields();
                }
            }
        }
        [PersistentAlias("Person.PersonType")]
        [RuleRequiredField("", "Approve")]
        public crmPersonType PersonType {
            get { return Person != null ? Person.PersonType : null; }
            set {
                if (!IsLoading && Person != null) {
                    Person.PersonType = value;
                }
            }
        }
        //        public csCountry Country {
//            get { return Person != null ? Person.Country : null; }
//        }
        [PersistentAlias("Person.Country")]
        [RuleRequiredField("", "Approve")]
        public csCountry Country {
            get { return Person != null ? Person.Country : null; }
            set {
                if (!IsLoading && Person != null ) {
                    Person.Country = value;
                }
            }
        }
        public String AddressLegalString {
            get { return Person != null && Person.Address != null? Person.Address.AddressString : null; }
        }

        [PersistentAlias("Person.Address.CityType")]
        [RuleRequiredField("", "Approve")]
        public String CityType {
            get { return Person != null && Person.Address != null ? Person.Address.CityType : null; }
            set {
                if (!IsLoading && Person != null && Person.Address != null) {
                    Person.Address.CityType = value;
                }
            }
        }

        [PersistentAlias("Person.Address.City")]
        [RuleRequiredField("", "Approve")]
        public String City {
            get { return Person != null && Person.Address != null ? Person.Address.City : null; }
            set {
                if (!IsLoading && Person != null && Person.Address != null) {
                    Person.Address.City = value;
                }
            }
        }

        public String CityFull {
            get { return (CityType != null ? CityType : String.Empty) + (City != null ? City : String.Empty); }
        }

        [PersistentAlias("Person.IsGovermentCustomer")]
        public Boolean IsGovermentCustomer {
            get { return Person != null ? Person.IsGovermentCustomer : false; }
            set {
                if (!IsLoading && Person != null) {
                    Person.IsGovermentCustomer = value;
                    OnChanged("IsGovermentCustomer");
                }
            }
        }

        public Int32 IsGovermentCode {
            get { return IsGovermentCustomer ? 1 : 0;  }
        }

        [PersistentAlias("Person.IsNpoCorporation")]
        public Boolean IsNpoCorporation {
            get { return Person != null ? Person.IsNpoCorporation : false; }
            set {
                if (!IsLoading && Person != null) {
                    Person.IsNpoCorporation = value;
                    OnChanged("IsNpoCorporation");
                    OnChanged("IsTrwCorporation");
                }
            }
        }
        [PersistentAlias("Person.IsTrwCorporation")]
        public Boolean IsTrwCorporation {
            get { return Person != null ? Person.IsTrwCorporation : false; }
            set {
                if (!IsLoading && Person != null) {
                    Person.IsTrwCorporation = value;
                    OnChanged("IsTrwCorporation");
                    OnChanged("IsNpoCorporation");
                }
            }
        }

        private crmCParty _Party;
        [RuleRequiredField]
        [ExplicitLoading]
        [DataSourceProperty("PartySource")]
        public crmCParty Party {
            get { return _Party; }
            set { 
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
                if (!IsLoading && value != null) {
                    if (value.Name.Length > 179)
                        this.Name = value.Name.Substring(1, 179);
                    else
                        this.Name = value.Name;
                    value.INN = value.INN != null ? value.INN : String.Empty;
                    value.KPP = value.KPP != null ? value.KPP : String.Empty;
                    if (this.PartyLegalType == TrwPartyLegalType.LEGAL_PERSON) {
                        if (value.INN.Length > 10)
                            this.INN = value.INN.Substring(1,10);
                        else
                            this.INN = value.INN;
                        if (value.KPP.Length > 9)
                            this.KPP = value.KPP.Substring(1, 9);
                        else
                            this.KPP = value.KPP;
                    }
                    else {
                        if (value.INN.Length > 12)
                            this.INN = value.INN.Substring(1, 12);
                        else
                            this.INN = value.INN;
                        this.KPP = String.Empty;
                    }
                }
            }
        }

        [PersistentAlias("Person.Partys")]
        [Browsable(false)]
        public XPCollection<crmCParty> PartySource {
            get { return Person != null ? Person.Partys : null; }
        }

        private String _Name;
        [Size(180)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        [PersistentAlias("Party.Name")]
        public String NameCur {
            get { return Party != null ? Party.Name : String.Empty; }
            set {
                if (!IsLoading) {
                    if (Party != null)
                        Party.Name = value;
                    if (Person != null)
                        Person.Name = value;
                }
            }
        }

        private String _INN;
        [Size(12)]
//        [RuleUniqueValue("", "Approve")]
        public String INN {
            get { return _INN; }
            set {
                value = value == null ? null : value.Trim();
                SetPropertyValue<String>("INN", ref _INN, value); 
            }
        }
        [Browsable(false)]
        [RuleFromBoolProperty("", "Approve", "Некорректный ИНН")]
        public Boolean INNConfirm {
            get { return (PartyLegalType == TrwPartyLegalType.LEGAL_PERSON && INN.Length == 10 || PartyLegalType == TrwPartyLegalType.PHYSICAL_PERSON && INN.Length == 12); }
        }

        [PersistentAlias("Party.INN")]
        public String INNCur {
            get { return Party!= null ? Party.INN : String.Empty; }
            set {
                if (!IsLoading && Party != null) {
                    Party.INN = value;
                }
            }
        }

        private String _KPP;
        [Size(9)]
        public String KPP {
            get { return _KPP; }
            set {
                value = value == null ? null : value.Trim();
                SetPropertyValue<String>("KPP", ref _KPP, value);
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty("", "Approve", "Некорректный КПП")]
        public Boolean KPPConfirm {
            get { return (PartyLegalType == TrwPartyLegalType.LEGAL_PERSON && KPP.Length == 9 || PartyLegalType == TrwPartyLegalType.PHYSICAL_PERSON && KPP.Length == 0); }
        }
        [PersistentAlias("Party.KPP")]
        public String KPPCur {
            get { return Party != null ? Party.KPP : String.Empty; }
            set {
                if (!IsLoading && Party != null) {
                    Party.KPP = value;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Person.PersonInScience")]
        [RuleRequiredField]
        public crmPartyPersonInScience PersonInScience {
            get {
                return Person != null ? Person.PersonInScience : crmPartyPersonInScience.PERSON_NOT_SCIENCE;
            }
            set {
                crmPartyPersonInScience old = PersonInScience;
                if (old != value) {
                    Person.PersonInScience = value;
                    OnChanged("PersonInScience");
                }
            }
        }

        [Aggregated]
        public XPCollection<crmBankAccount> BankAccounts {
            get { return Person != null ? Person.BankAccounts : null; }
        }

        protected void UpdateCalcFields() {
            if (Person != null) {
                if (Person.ComponentObject is crmCLegalPerson)
                    _PartyLegalType = TrwPartyLegalType.LEGAL_PERSON;
                else
                    _PartyLegalType = TrwPartyLegalType.PHYSICAL_PERSON;
            } 
            else {
                _PartyLegalType = null;
            }
            OnChanged("PartyLegalType");
//            OnChanged("Name");
//            OnChanged("INN");
//            OnChanged("KPP");
//            OnChanged("BankAccounts");
        }

        [Action(PredefinedCategory.RecordEdit, Caption = "Утвердить")]
        public void Approve() {
            RuleSetValidationResult check_result = Validator.RuleSet.ValidateTarget(this, "Approve");
            if (check_result.State == ValidationState.Invalid)
                throw new ValidationException(check_result);
//            Validator.RuleSet.ValidateTarget(this, "Approve");
            State = TrwPartyState.CONFIRMED;
            ObjectSpace.FindObjectSpaceByObject(this).CommitChanges();
           
        }
        [Action(PredefinedCategory.RecordEdit, Caption="Отклонить")]
        public void Decline() {
            if (State == TrwPartyState.CONFIRMED ||
                State == TrwPartyState.IN_TRW)
                State = TrwPartyState.EDITED;
        }
        [Action(PredefinedCategory.RecordEdit, Caption = "Отправить")]
        public void SendInTrw() {
            if (State == TrwPartyState.CONFIRMED)
                State = TrwPartyState.IN_TRW;
        }

        [Action(PredefinedCategory.RecordEdit, Caption = "Тек->ТРВ", 
            TargetObjectsCriteria = "State == 'NEW' || State == 'EDITED'")]
        public void CurToTrw () {
            this.Name = NameCur;
            this.INN = INNCur;
            this.KPP = KPPCur;
        }
        public static TrwPartyParty LocateTrwParty(IObjectSpace os, crmCParty party) {
            if (party.Person == null)
                return null;

            TrwPartyParty trw_party = party.Person.TrwParty;
//                trw_partys.FirstOrDefault(x => x.Person == party.Person);
            if (trw_party == null) {
                trw_party = os.CreateObject<TrwPartyParty>();
                trw_party.Person = party.Person;
                trw_party.Party = party;
            }
            return trw_party;
        }
    }
}
