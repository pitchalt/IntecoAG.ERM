using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Trw.Party {
    public enum TrwPartyLegalType {
        LEGAL_PERSON = 0, 
        PHYSICAL_PERSON = 1
    }
    [Persistent("TrwPartyParty")]
    public class TrwPartyParty : csCComponent {
        public TrwPartyParty(Session session) : base(session) { }
        public override void AfterConstruction() {            
            base.AfterConstruction();
            _PartyLegalType = null;
            Name = String.Empty;
            INN = String.Empty;
            KPP = String.Empty;

        }

        [Persistent("PartyLegalType")]
        TrwPartyLegalType? _PartyLegalType;
        [PersistentAlias("_PartyLegalType")]
        public TrwPartyLegalType? PartyLegalType {
            get { return _PartyLegalType; }
        }
        public Int16 PartyTypeCode {
            get { return (Int16) (PartyLegalType != null ? ((Int16)PartyLegalType) : -1); }
        }

        private TrwPartyMarket _Market;
        public TrwPartyMarket Market {
            get { return _Market; }
            set { SetPropertyValue<TrwPartyMarket>("Market", ref _Market, value); }
        }

        private TrwPartyType _PartyType;
        public TrwPartyType PartyType {
            get { return _PartyType; }
            set { SetPropertyValue<TrwPartyType>("PartyType", ref _PartyType, value); }
        }

        private crmCPerson _Person;
        [RuleRequiredField]
        [ExplicitLoading()]
        public crmCPerson Person {
            get { return _Person; }
            set { 
                SetPropertyValue<crmCPerson>("Person", ref _Person, value);
                if (!IsLoading) {
                    Party = null;
                    UpdateCalcFields();
                }
            }
        }

        private crmCParty _Party;
        [RuleRequiredField]
        [ExplicitLoading()]
        [DataSourceProperty("PartySource")]
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }
        [PersistentAlias("Person.Partys")]
        [Browsable(false)]
        public XPCollection<crmCParty> PartySource {
            get { return Person != null ? Person.Partys : null; }
        }

        String _Name;
        [Size(180)]
        String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        String _INN;
        [Size(12)]
        String INN {
            get { return _INN; }
            set { SetPropertyValue<String>("INN", ref _INN, value); }
        }

        String _KPP;
        [Size(9)]
        String KPP {
            get { return _KPP; }
            set { SetPropertyValue<String>("KPP", ref _KPP, value); }
        }

        protected void UpdateCalcFields() {
            if (Person != null) {
                if (Person.ComponentObject is crmCLegalPerson)
                    _PartyLegalType = TrwPartyLegalType.LEGAL_PERSON;
                else
                    _PartyLegalType = TrwPartyLegalType.PHYSICAL_PERSON;
                Name = Person.Name;
                INN = Person.INN;
            } 
            else {
                _PartyLegalType = null;
                Name = String.Empty;
            }
            OnChanged("PartyLegalType");
            OnChanged("Name");
            OnChanged("INN");
            OnChanged("KPP");
        }
    }
}
