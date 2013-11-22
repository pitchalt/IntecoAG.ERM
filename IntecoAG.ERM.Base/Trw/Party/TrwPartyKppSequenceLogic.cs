using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw.Party {

    public static class TrwPartyKppSequenceLogic {
        public static Int32 GetNextNumber(IObjectSpace os, TrwPartyKppSequenceType seq_type, Guid party_oid) {
            var cur_num = os.FindObject<TrwPartyKppSequence>(
                new BinaryOperator("SequenceType", seq_type) &
                new BinaryOperator("IsCurrent", true),
                true);
            if (cur_num == null) {
                cur_num = os.CreateObject<TrwPartyKppSequence>();
                switch (seq_type) {
                    case TrwPartyKppSequenceType.PARTY_TYPE_OTHER_CUSTOMER:
                        cur_num.Number = 1;
                        break;
                    case TrwPartyKppSequenceType.PARTY_TYPE_OTHER_SUPPLIER:
                        cur_num.Number = 2;
                        break;
                    case TrwPartyKppSequenceType.PARTY_TYPE_SINGLE_DEAL:
                        cur_num.Number = 50;
                        break;
                    case TrwPartyKppSequenceType.PARTY_TYPE_INTERNAL:
                        cur_num.Number = 3;
                        break;
                    case TrwPartyKppSequenceType.PARTY_TYPE_MARKET2:
                        cur_num.Number = 2000;
                        break;
                    case TrwPartyKppSequenceType.PARTY_TYPE_MARKET1:
                        cur_num.Number = 100;
                        break;
                    case TrwPartyKppSequenceType.PARTY_TYPE_PAY_INTERMEDIA:
                        cur_num.Number = 1000;
                        break;
                }
                cur_num.SequenceType = seq_type;
                cur_num.IsCurrent = true;
                cur_num.TrwPartyOid = party_oid;
                os.CommitChanges();
                return cur_num.Number;
            }
            var num = os.FindObject<TrwPartyKppSequence>(
                new BinaryOperator("TrwPartyOid", party_oid) &
                new BinaryOperator("SequenceType", seq_type),
                true);
            if (num != null) {
                return num.Number;
            }
            switch (seq_type) {
                case TrwPartyKppSequenceType.PARTY_TYPE_OTHER_CUSTOMER:
                case TrwPartyKppSequenceType.PARTY_TYPE_OTHER_SUPPLIER:
                case TrwPartyKppSequenceType.PARTY_TYPE_SINGLE_DEAL:
                    throw new InvalidOperationException("Can be only single");
                case TrwPartyKppSequenceType.PARTY_TYPE_INTERNAL:
                case TrwPartyKppSequenceType.PARTY_TYPE_MARKET1:
                case TrwPartyKppSequenceType.PARTY_TYPE_MARKET2:
                    num = os.CreateObject<TrwPartyKppSequence>();
                    num.SequenceType = seq_type;
                    num.IsCurrent = true;
                    num.Number = cur_num.Number + 1;
                    num.TrwPartyOid = party_oid;
                    cur_num.IsCurrent = null;
                    os.CommitChanges();
                    return num.Number;
                default:
                    throw new ArgumentOutOfRangeException("Unknow");
            }
        }
    }

}
