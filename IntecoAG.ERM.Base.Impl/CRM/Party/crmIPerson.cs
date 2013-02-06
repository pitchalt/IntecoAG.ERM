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

namespace IntecoAG.ERM.CRM.Party {

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
        
    }
}
