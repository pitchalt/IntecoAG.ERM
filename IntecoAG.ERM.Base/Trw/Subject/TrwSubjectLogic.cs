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

        public static IList<crmContractDeal> GetDeals(TrwSubject trw_subj, DealInfoDealType deal_type, TrwSubjectImportDealParameters parms) {
            IDictionary<crmContractDeal, Decimal> dict = new Dictionary<crmContractDeal, Decimal>();
            foreach (fmCSubject subj in trw_subj.Subjects) {
                foreach (fmCSubject.DealInfo info in subj.DealInfos) {
                    if (info.DealType != deal_type ||
                        info.Subject != trw_subj.Subject ||
                        info.Year != trw_subj.Period.Year ||
                        info.NomType != DealInfoNomType.DEAL_INFO_DELIVERY)
                        continue;
                    if (dict.ContainsKey(info.Deal)) {
                        dict[info.Deal] += info.DeliverySummCost;
                    }
                    else
                        dict[info.Deal] = info.DeliverySummCost;
                }
            }
            IOrderedEnumerable<crmContractDeal> deals = dict.Keys.OrderBy(x => -dict[x]);
            IList<crmContractDeal> result = new List<crmContractDeal>(parms.MaxCount);
            Decimal all_summ = dict.Values.Sum();
            Decimal current_summ = 0;
            Int32 current_count = 0;
            foreach (crmContractDeal deal in deals) {
                current_summ += dict[deal];
                current_count++;
                result.Add(deal);
                if (parms.MaxCount > 0 && current_count >= parms.MaxCount)
                    break;
                if (parms.VolumePercent > 0 && current_summ / all_summ >= parms.VolumePercent)
                    break;
            }
            return result;
        }

        public static void FillSaleDeals(IObjectSpace os, TrwSubject trw_subj, TrwSubjectImportDealParameters parms) {
            IList<crmContractDeal> deals = GetDeals(trw_subj, DealInfoDealType.DEAL_INFO_PROCEEDS, parms);
            foreach (crmContractDeal deal in deals) {
                TrwSubjectDealSale subj_deal = trw_subj.DealsSale.FirstOrDefault(x => x.Deal == deal);
                if (subj_deal == null) {
                    subj_deal = os.CreateObject<TrwSubjectDealSale>();
                    trw_subj.DealsSale.Add(subj_deal);
                    subj_deal.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL;
                    subj_deal.Deal = deal;
                    subj_deal.DealBudget = null;
                }
            }
            if (parms.CreateOtherDeal) {
                TrwSubjectDealSale sale_other = trw_subj.DealsSale.FirstOrDefault(x => x.DealType == TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER);
                if (sale_other == null) {
                    sale_other = os.CreateObject<TrwSubjectDealSale>();
                    trw_subj.DealsSale.Add(sale_other);
                    sale_other.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER;
                    sale_other.PersonInternal = TrwSettings.GetInstance(os).PersonOtherSale;
                }
            }
        }
        public static void FillBayDeals(IObjectSpace os, TrwSubject trw_subj, TrwSubjectImportDealParameters parms) {
            IList<crmContractDeal> deals = GetDeals(trw_subj, DealInfoDealType.DEAL_INFO_EXPENDITURE, parms);
            foreach (crmContractDeal deal in deals) {
                TrwSubjectDealBay subj_deal = trw_subj.DealsBay.FirstOrDefault(x => x.Deal == deal);
                if (subj_deal == null) {
                    subj_deal = os.CreateObject<TrwSubjectDealBay>();
                    trw_subj.DealsBay.Add(subj_deal);
                    subj_deal.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL;
                    subj_deal.Deal = deal;
                    subj_deal.DealBudget = null;
                }
            }
            if (parms.CreateOtherDeal) {
                TrwSubjectDealBay bay_other = trw_subj.DealsBay.FirstOrDefault(x => x.DealType == TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER);
                if (bay_other == null) {
                    bay_other = os.CreateObject<TrwSubjectDealBay>();
                    trw_subj.DealsBay.Add(bay_other);
                    bay_other.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER;
                    bay_other.PersonInternal = TrwSettings.GetInstance(os).PersonOtherBay;
                }
            }
        }
    }
}
