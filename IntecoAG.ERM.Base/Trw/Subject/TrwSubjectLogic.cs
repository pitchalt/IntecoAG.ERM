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

        public static IList<crmContractDeal> GetDeals(TrwSubject trw_subj, DealInfoDealType deal_type, Int16 year, Int32 max_count, Decimal percent) {
            IDictionary<crmContractDeal, Decimal> dict = new Dictionary<crmContractDeal, Decimal>(trw_subj.Subject.Deals.Count);
            foreach (fmCSubject.DealInfo info in trw_subj.Subject.DealInfos) {
                if (info.DealType != deal_type ||
                    info.Subject != trw_subj.Subject ||
                    info.Year != year ||
                    info.NomType != DealInfoNomType.DEAL_INFO_DELIVERY)
                    continue;
                if (dict.ContainsKey(info.Deal)) {
                    dict[info.Deal] += info.DeliverySummCost;
                }
                else
                    dict[info.Deal] = info.DeliverySummCost;
            }
            IOrderedEnumerable<crmContractDeal> deals = dict.Keys.OrderBy(x => -dict[x]);
            IList<crmContractDeal> result = new List<crmContractDeal>(max_count);
            Decimal all_summ = dict.Values.Sum();
            Decimal current_summ = 0;
            Int32 current_count = 0;
            foreach (crmContractDeal deal in deals) {
                current_summ += dict[deal];
                current_count++;
                result.Add(deal);
                if (max_count > 0 && current_count >= max_count)
                    break;
                if (percent > 0 && current_summ / all_summ >= percent)
                    break;
            }
            return result;
        }

        public static void FillSaleDeals(IObjectSpace os, TrwSubject trw_subj, Int16 year, Int32 max_count, Decimal percent) {
            foreach (crmContractDeal deal in trw_subj.Subject.Deals) {
                if (deal.TRVType != null && deal.TRVType.TrwContractSuperType == Contract.TrwContractSuperType.DEAL_SALE) {
                    TrwSubjectDealSale subj_deal = trw_subj.DealsSale.FirstOrDefault(x => x.Deal == deal);
                    if (subj_deal == null) {
                        subj_deal = os.CreateObject<TrwSubjectDealSale>();
                        subj_deal.TrwSubject = trw_subj;
                        subj_deal.Deal = deal;
                        subj_deal.DealBudget = trw_subj.DealOtherSale;
                    }
                }
            }
            IList<crmContractDeal> deals = GetDeals(trw_subj, DealInfoDealType.DEAL_INFO_PROCEEDS, year, max_count, percent);
            foreach (crmContractDeal deal in deals) {
                TrwSubjectDealSale subj_deal = trw_subj.DealsSale.FirstOrDefault(x => x.Deal == deal);
                    if (subj_deal != null) {
                        subj_deal.DealBudget = null;
                    }
            }
        }
        public static void FillBayDeals(IObjectSpace os, TrwSubject trw_subj, Int16 year, Int32 max_count, Decimal percent) {
            foreach (crmContractDeal deal in trw_subj.Subject.Deals) {
                if (deal.TRVType != null && deal.TRVType.TrwContractSuperType != Contract.TrwContractSuperType.DEAL_SALE) {
                    TrwSubjectDealBay subj_deal = trw_subj.DealsBay.FirstOrDefault(x => x.Deal == deal);
                    if (subj_deal == null) {
                        subj_deal = os.CreateObject<TrwSubjectDealBay>();
                        subj_deal.TrwSubject = trw_subj;
                        subj_deal.Deal = deal;
                        subj_deal.DealBudget = trw_subj.DealOtherBay;
                    }
                }
            }
            IList<crmContractDeal> deals = GetDeals(trw_subj, DealInfoDealType.DEAL_INFO_EXPENDITURE, year, max_count, percent);
            foreach (crmContractDeal deal in deals) {
                TrwSubjectDealBay subj_deal = trw_subj.DealsBay.FirstOrDefault(x => x.Deal == deal);
                if (subj_deal != null) {
                    subj_deal.DealBudget = null;
                }
            }
        }
    }
}
