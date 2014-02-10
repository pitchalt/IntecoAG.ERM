using System;
//using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//
using FileHelpers;
//
using DevExpress.ExpressApp;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Subject;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Budget.Period {

    public static class TrwBudgetPeriodDocDealLogic {


        private class OrderValue {
            public fmCOrder Order;
            public IList<TrwBudgetPeriodDocBSR.LineBSR> BsrLines; 
            //            public IList<TrwOrder> TrwSaleOrders;
            public IList<TrwBudgetPeriodDocDeal.LineDeal> DealLines ;
            public OrderValue() {
                BsrLines = new List<TrwBudgetPeriodDocBSR.LineBSR>();
                DealLines = new List<TrwBudgetPeriodDocDeal.LineDeal>();
            }
        }

        public static void Calculate(TrwBudgetPeriodDocDeal doc, IObjectSpace os) {
            os.Delete(doc.DocDealLines);
            Dictionary<fmCOrder, OrderValue> rsp = new Dictionary<fmCOrder, OrderValue>();
            foreach (TrwSubject subj in doc.Period.TrwSubjects) {
                LoadTrwSubject(doc, os, rsp, subj);
            }
            foreach (TrwBudgetPeriodDocBSR.LineBSR bsr_line in doc.DocBSR.DocBSRLines) {
                if (bsr_line.FmOrder == null)
                    continue;
                if (!rsp.ContainsKey(bsr_line.FmOrder)) {
                    rsp[bsr_line.FmOrder] = new OrderValue();
                    rsp[bsr_line.FmOrder].Order = bsr_line.FmOrder;
                }
                rsp[bsr_line.FmOrder].BsrLines.Add(bsr_line);
            }
            foreach (fmCOrder order in rsp.Keys) {
                System.Console.WriteLine("Order: " + rsp[order].Order.Code + " : " + rsp[order].BsrLines.Count + " : " + rsp[order].DealLines.Count);
                    foreach (TrwBudgetPeriodDocDeal.LineDeal line_deal in rsp[order].DealLines) {
                        line_deal.FmOrder = order;
                        if (rsp[order].BsrLines.Count == 1) {
                            line_deal.TrwSaleNom = rsp[order].BsrLines[0].SaleNomenclature;
                        }
                        else if (rsp[order].BsrLines.Count > 1) {
                            continue;
                        }
                        else
                            continue;
                    }

            }
        }

        private static void LoadTrwSubject(TrwBudgetPeriodDocDeal doc, IObjectSpace os,  Dictionary<fmCOrder, OrderValue> rsp, TrwSubject trw_subj) {
            TrwSubjectDealSale other_sale = trw_subj.DealsSale.FirstOrDefault(x => x.DealType == TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER);
            TrwSubjectDealBay other_bay = trw_subj.DealsBay.FirstOrDefault(x => x.DealType == TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER);

            foreach (fmCSubject fm_subj in trw_subj.Subjects) {
                foreach (var deal_info in fm_subj.DealInfos) {
                    if (deal_info.Order == null)
                        continue;
                    if (deal_info.DealType == DealInfoDealType.DEAL_INFO_PROCEEDS) { 
                    }
                    else if (deal_info.DealType == DealInfoDealType.DEAL_INFO_EXPENDITURE) { 
                        if (!rsp.ContainsKey(deal_info.Order)) {
                            rsp[deal_info.Order] = new OrderValue();
                            rsp[deal_info.Order].Order = deal_info.Order;
                        }
                        TrwSubjectDealBay deal_bay = CheckDealBase<TrwSubjectDealBay>(trw_subj.DealsBay, deal_info.Deal);
                        TrwBudgetPeriodDocDeal.LineDeal line_deal = rsp[deal_info.Order].DealLines.FirstOrDefault(x => x.TrwSubjectDealBay == deal_bay);
                        if (line_deal == null) {
                            line_deal = os.CreateObject<TrwBudgetPeriodDocDeal.LineDeal>();
                            line_deal.TrwSubjectDealBay = deal_bay;
                            rsp[deal_info.Order].DealLines.Add(line_deal);
                            doc.DocDealLines.Add(line_deal);
                        }
                        if (deal_info.Year == doc.Period.Year) {
                            line_deal[deal_info.Month] += deal_info.SummCost;
                        }
                        else if (deal_info.Year > doc.Period.Year) {
                            line_deal[13] += deal_info.SummCost;
                        }
                        else if (deal_info.Year < doc.Period.Year) {
                            line_deal[0] += deal_info.SummCost;
                        }
                    }
                }
            }
        }

        private static T CheckDealBase<T>(IList<T> trw_subj_deals, crmContractDeal fm_deal) where T : TrwSubjectDealBase  {
            T real_deal = null;
            T cons_deal = null;
            T cons_partner = null;
            T cons_other = null;
            foreach (T trw_subj_deal in trw_subj_deals) {
                switch (trw_subj_deal.DealType) { 
                    case TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL:
                        if (trw_subj_deal.Deal == fm_deal) {
                            real_deal = trw_subj_deal;
                            continue;
                        }
                        break;
                    case TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_DEAL:
                        if (trw_subj_deal.CrmContractDeals.Contains(fm_deal)) {
                            cons_deal = trw_subj_deal;
                            continue;
                        }
                        break;
                    case TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_PARTNER:
                        if (trw_subj_deal.Person == fm_deal.Current.Customer.Party.Person ||
                            trw_subj_deal.Person == fm_deal.Current.Supplier.Party.Person) {
                                cons_partner = trw_subj_deal;
                                continue;
                        }
                        break;
                    case TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER:
                        cons_other = trw_subj_deal;
                        break;
                }
            }
            if (real_deal != null)
                return real_deal;
            if (cons_deal != null)
                return cons_deal;
            if (cons_partner != null)
                return cons_partner;
            return cons_other;
        }
    }
}
