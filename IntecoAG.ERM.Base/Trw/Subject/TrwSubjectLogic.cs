using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Subject;
//
namespace IntecoAG.ERM.Trw.Subject {

    public static class TrwSubjectLogic {

        public IList<crmContractDeal> GetDeals(TrwSubject trw_subj, DealInfoDealType deal_type, Int16 year, Int16 max_count, Int16 percent) {
            IDictionary<crmContractDeal, Decimal> dict = new Dictionary<crmContractDeal, Decimal>(trw_subj.Subject.Deals.Count);
            foreach (fmCSubject.DealInfo info in trw_subj.Subject.DealInfos) {
                if (info.DealType != deal_type ||
                    info.Subject != trw_subj.Subject ||
                    info.Year != year ||
                    info.NomType != DealInfoNomType.DEAL_INFO_DELIVERY)
                    continue;
            }

            IList<crmContractDeal> result = new List<crmContractDeal>();
            return result;
        }

        public static void FillSaleDeals(IObjectSpace os, TrwSubject subj, Int16 year, Int16 max_count, Int16 percent) {
        }
    }
}
