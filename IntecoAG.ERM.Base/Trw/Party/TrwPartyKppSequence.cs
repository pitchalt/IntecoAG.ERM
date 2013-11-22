using System;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.Trw.Party;
//
namespace IntecoAG.ERM.Trw.Party {

    public enum TrwPartyKppSequenceType {
        PARTY_TYPE_OTHER_SUPPLIER = 1,
        PARTY_TYPE_OTHER_CUSTOMER = 2,
        PARTY_TYPE_SINGLE_DEAL = 3,
        PARTY_TYPE_INTERNAL = 4,
        PARTY_TYPE_MARKET1 = 5,
        PARTY_TYPE_MARKET2 = 6
    }

    [Persistent("TrwPartyKppSequence")]
    public class TrwPartyKppSequence : XPObject {
        public TrwPartyKppSequence(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private crmCPerson _Person;
        public crmCPerson Person {
            get { return _Person; }
            set { SetPropertyValue<crmCPerson>("Person", ref _Person, value); }
        }

        private TrwPartyKppSequenceType _SequenceType;
        [Indexed("TrwPartyOid", Unique = true)]
        public TrwPartyKppSequenceType SequenceType {
            get { return _SequenceType; }
            set { SetPropertyValue<TrwPartyKppSequenceType>("SequenceType", ref _SequenceType, value); }
        }

        private Boolean? _IsCurrent;
        [Indexed]
        public Boolean? IsCurrent {
            get { return _IsCurrent; }
            set { SetPropertyValue<Boolean?>("IsCurrent", ref _IsCurrent, value); }
        }

        private Int32 _Number;
        [Indexed(Unique=true)]
        public Int32 Number {
            get { return _Number; }
            set { SetPropertyValue<Int32>("Number", ref _Number, value); }
        }

        private Guid _TrwPartyOid;
        public Guid TrwPartyOid {
            get { return _TrwPartyOid; }
            set { SetPropertyValue<Guid>("TrwPartyOid", ref _TrwPartyOid, value); }
        }
    }

}
