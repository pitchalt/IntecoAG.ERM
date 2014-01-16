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
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.Trw.Subject {

    public static class TrwSubjectDealLogic {

        public static void RefreshDeal(IObjectSpace os, TrwSubjectDealSale subj_deal) {
            if (subj_deal.DealType == TrwSubjectDealType.TRW_SUBJECT_DEAL_UNKNOW ||
                subj_deal.TrwSubjectBase == null)
                return;
            if (subj_deal.DealType == TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL) {
                if (subj_deal.Deal == null)
                    return;
                DateTime period_end = new DateTime(subj_deal.TrwSubject.Period.Year, 12, 31);
                foreach (fmCSubject subj in subj_deal.TrwSubjectBase.Subjects) {
                    TrwOrder trw_order = subj_deal.Deal.TrwOrders.FirstOrDefault(x => x.Subject == subj);
                    if (trw_order == null) {
                        subj.Deals.Add(subj_deal.Deal);
                        trw_order = subj_deal.Deal.TrwOrders.FirstOrDefault(x => x.Subject == subj);
                    }
                    if (subj_deal.Nomenclature != null) {
                        foreach (fmCOrder order in subj.Orders) {
                            if (order.IsClosed || order.DateBegin > period_end)
                                continue;
                            TrwSubjectDealSaleOrder trw_subj_deal_order = subj_deal.DealSaleOrders.FirstOrDefault(x => x.Order == order);
                            if (trw_subj_deal_order == null) {
                                trw_subj_deal_order = os.CreateObject<TrwSubjectDealSaleOrder>();
                                subj_deal.DealSaleOrders.Add(trw_subj_deal_order);
                                trw_subj_deal_order.TrwOrderSet(trw_order);
                                trw_subj_deal_order.Order = order;
                                trw_order.TrwSaleNomenclatures.Add(trw_subj_deal_order.TrwSaleNomenclature);
                            }
                        }
                    }
                }
            }
            else {
                if (subj_deal.DealBudget == null)
                    return;
                DateTime period_end = new DateTime(subj_deal.TrwSubject.Period.Year, 12, 31);
                foreach (fmCSubject subj in subj_deal.TrwSubjectBase.Subjects) {
                    if (subj_deal.DealBudget == null)
                        return;
                    TrwOrder trw_order = subj_deal.DealBudget.TrwOrders.FirstOrDefault(x => x.Subject == subj);
                    if (trw_order == null) {
                        trw_order = os.CreateObject<TrwOrder>();
                        trw_order.Subject = subj;
                        trw_order.TrwContractInt = subj_deal.DealBudget;
                    }
                    trw_order.TrwDateFrom = subj_deal.DealBudget.TrwDateValidFrom;
                    trw_order.TrwDateToPlan = subj_deal.DealBudget.TrwDateValidToFact;
                    trw_order.TrwOrderWorkType = subj_deal.TrwOrderWorkType;
                    if (subj_deal.Nomenclature != null) {
                        foreach (fmCOrder order in subj.Orders) {
                            if (order.IsClosed || order.DateBegin > period_end)
                                continue;
                            TrwSubjectDealSaleOrder trw_subj_deal_order = subj_deal.DealSaleOrders.FirstOrDefault(x => x.Order == order);
                            if (trw_subj_deal_order == null) {
                                trw_subj_deal_order = os.CreateObject<TrwSubjectDealSaleOrder>();
                                subj_deal.DealSaleOrders.Add(trw_subj_deal_order);
                                trw_subj_deal_order.TrwOrderSet(trw_order);
                                trw_subj_deal_order.Order = order;
                                trw_order.TrwSaleNomenclatures.Add(trw_subj_deal_order.TrwSaleNomenclature);
                            }
                        }
                    }
                }
            }
        }
        public static void RefreshDeal(IObjectSpace os, TrwSubjectDealBay subj_deal) {
        }
    }
}
