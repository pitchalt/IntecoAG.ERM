using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw.Party {

    public static class TrwPartyPartyLogic {

        public static void SetNumbers(IObjectSpace os, TrwPartyParty party, IObjectSpace seq_os) {
            if (party.Country != null && party.Country.IsVED) {
                if (party.Country.IsUIG)
                    party.Market = TrwPartyMarket.MARKET_UIG;
                else
                    party.Market = TrwPartyMarket.MARKET_VED;
            }
            TrwPartyKppSequenceType seq_type;
            if (party.Market == TrwPartyMarket.MARKET_UIG)
                seq_type = TrwPartyKppSequenceType.PARTY_TYPE_MARKET2;
            else {
                if (party.Market == TrwPartyMarket.MARKET_VED)
                    seq_type = TrwPartyKppSequenceType.PARTY_TYPE_MARKET1;
                else
                    throw new ArgumentOutOfRangeException("Unsupported market type");
            }
            Int32 kpp = TrwPartyKppSequenceLogic.GetNextNumber(seq_os, seq_type, party.Oid);
            party.INN = "2500000000";
            party.KPP = "25" + kpp.ToString("D7");
        }
    }

}
