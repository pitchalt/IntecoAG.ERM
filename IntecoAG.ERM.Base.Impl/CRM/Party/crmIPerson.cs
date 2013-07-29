using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Persistent.Base;
using DC = DevExpress.ExpressApp.DC;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;

using IntecoAG.ERM.Trw.Party;

namespace IntecoAG.ERM.CRM.Party {

    public enum crmPartyPersonInScience { 
        PERSON_NOT_SCIENCE = 0,
        PERSON_SCIENCE_RAN = 1,
        PERSON_SCIENCE_OTHER = 2,
        PERSON_SCIENCE_INSTITUTE = 3
    }

    [DC.DomainComponent]
    public interface crmIPerson : csIComponent {

        crmPersonType PersonType { get; set; }
        csCountry Country { get; set; }
        [DC.Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        csAddress Address { get; }
        [DC.Aggregated]
        IList<crmBankAccount> BankAccounts { get;}
        IList<crmIParty> Partys { get; }

        [DC.FieldSize(200)]
        String Name { get; set; } 

        [DC.FieldSize(300)]
        String NameFull { get; set; } 

        [DC.FieldSize(25)]
        String RegCode { get; set; } 

        [DC.FieldSize(15)]
        String INN { get; set; }

        Boolean IsGovermentCustomer { get; set; }
        Boolean IsTrwCorporation {get; set; }
        Boolean IsNpoCorporation {get; set; }

        TrwPartyMarket TrwPartyMarket { get; set; }
        TrwPartyType   TrwPartyType { get; set; }
    }
}
