using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.Trw {

    public static class TrwRefCashFlowLogic {
        public static TrwRefCashFlow AutoDetect(IObjectSpace os, Boolean CashDirect, fmCOrder order, fmPRPayType pay_type, fmCostItem cost_item) {
            IList<TrwRefCashFlow> all_refs = os.GetObjects<TrwRefCashFlow>();
            TrwRefCashFlow current = null;
            if (CashDirect) {
                return null;
            }
            else {
                if (cost_item.Code == "2000" ||
                    cost_item.Code == "2001") {
                    return GetRefByFullCode(os, all_refs, null, "2.1.1.2.1");
                }
                if (cost_item.Code == "2003" ||
                    cost_item.Code == "2006") {
                    return GetRefByFullCode(os, all_refs, null, "2.1.1.2.2");
                }
                if (cost_item.Code == "2002" ||
                    cost_item.Code == "2004" ||
                    cost_item.Code == "2005") {
                    return GetRefByFullCode(os, all_refs, null, "2.1.1.2.3");
                }
                if (order.Subject != null && order.Subject.Direction != null && order.AnalitycFinanceSource != null && order.AnalitycFinanceSource.Code == "Êîììåð÷åñêèé" &&
                    (order.Subject.Direction.Code != "ÏÒÐ" || order.Subject.Direction.Code != "ÍÐ" || order.Subject.Direction.Code != "ÑÏ")) {
                    current = GetRefByFullCode(os, all_refs, null, "2.1.1.1");
                    if (cost_item.Code == "6003") {
                        if (order.AnalitycRegion.IsVED) {
                            if (pay_type == fmPRPayType.PREPAYMENT)
                                return GetRefByFullCode(os, all_refs, current, "1.1.1");
                            if (pay_type == fmPRPayType.POSTPAYMENT)
                                return GetRefByFullCode(os, all_refs, current, "1.1.2");
                        }
                        else if (order.AnalitycOrderSource.IsGZ) {
                            if (pay_type == fmPRPayType.PREPAYMENT)
                                return GetRefByFullCode(os, all_refs, current, "1.2.1");
                            if (pay_type == fmPRPayType.POSTPAYMENT)
                                return GetRefByFullCode(os, all_refs, current, "1.2.2");
                        }
                        else {
                            if (pay_type == fmPRPayType.PREPAYMENT)
                                return GetRefByFullCode(os, all_refs, current, "1.3.1");
                            if (pay_type == fmPRPayType.POSTPAYMENT)
                                return GetRefByFullCode(os, all_refs, current, "1.3.2");
                        }
                    }
                    else if (cost_item.Code == "6000" || cost_item.Code == "6001" || cost_item.Code == "6002" ||
                        cost_item.Code == "6004" || cost_item.Code == "6005" || cost_item.Code == "6006" || cost_item.Code == "6007") {
                        return GetRefByFullCode(os, all_refs, current, "3");
                    }
                    else if (cost_item.Code == "7001") {
                        if (pay_type == fmPRPayType.PREPAYMENT)
                            return GetRefByFullCode(os, all_refs, current, "2.1.1");
                        if (pay_type == fmPRPayType.POSTPAYMENT)
                            return GetRefByFullCode(os, all_refs, current, "2.2.1");
                    }
                    else {
                        if (pay_type == fmPRPayType.PREPAYMENT)
                            return GetRefByFullCode(os, all_refs, current, "2.1.2");
                        if (pay_type == fmPRPayType.POSTPAYMENT)
                            return GetRefByFullCode(os, all_refs, current, "2.2.2");
                    }
                }
                else if (order.Subject != null && order.Subject.Direction != null && order.Subject.Direction.Code == "ÏÒÐ") {
                    current = GetRefByFullCode(os, all_refs, null, "2.1.3");
                }
                else if (order.Subject != null && order.Subject.Direction != null && order.Subject.Direction.Code == "ÍÐ") {
                    current = GetRefByFullCode(os, all_refs, null, "2.1.1.3");
                    String smeta = "";
                    if (order.Code.Length == 8)
                        smeta = order.Code.Substring(6, 2);
                    if (smeta == "10" || smeta == "11" || smeta == "12" || smeta == "22" || smeta == "09" && order.Code != "26015509") {
                        if (cost_item.Code == "6000" || cost_item.Code == "6001" || cost_item.Code == "6002" ||
                            cost_item.Code == "6004" || cost_item.Code == "6005" || cost_item.Code == "6006" ||
                            cost_item.Code == "6007" || cost_item.Code == "6003") {
                            return GetRefByFullCode(os, all_refs, current, "1.1");
                        }
                        else
                            return GetRefByFullCode(os, all_refs, current, "1.2");
                    }
                    else if (order.Code == "26020500") {
                        return GetRefByFullCode(os, all_refs, current, "13");
                    }
                    else if ((order.Code == "26003100" || order.Code == "26503100") &&
                             (cost_item.Code == "7007")) {
                        return GetRefByFullCode(os, all_refs, current, "15");
                    }
                    else if (order.Code == "26020000" && cost_item.Code == "7012" ||
                             order.Code == "26520300" && cost_item.Code == "7012") {
                        return GetRefByFullCode(os, all_refs, current, "6");
                    }
                    else if ((order.Code == "26020000" || order.Code == "26002000" || order.Code == "26502100" || order.Code == "26520300") &&
                             (cost_item.Code == "2101" || cost_item.Code == "2102" || order.Code == "2103")) {
                        return GetRefByFullCode(os, all_refs, current, "7");
                    }
                    else if (order.Code == "23210000" || order.Code == "23220000") {
                        return GetRefByFullCode(os, all_refs, current, "4");
                    }
                    else if (order.Code == "23230000") {
                        return GetRefByFullCode(os, all_refs, current, "5");
                    }
                    else if (order.Code == "26003116") {
                        return GetRefByFullCode(os, all_refs, current, "8");
                    }
                    else if (order.Code == "26015407" || order.Code == "25006407") {
                        return GetRefByFullCode(os, all_refs, current, "9");
                    }
                    else if (order.Code == "26020400") {
                        return GetRefByFullCode(os, all_refs, current, "17");
                    }
                    else if (smeta == "37" || smeta == "34" ||
                        order.Code == "26017400" || order.Code == "26017300") {
                        return GetRefByFullCode(os, all_refs, current, "16");
                    }
                    else if (smeta == "39" ||
                        order.Code == "26015107" || order.Code == "26015307" || order.Code == "26020607" ||
                        order.Code == "25006107" || order.Code == "25006307") {
                        return GetRefByFullCode(os, all_refs, current, "12");
                    }
                    else if (smeta == "13" || smeta == "14" || smeta == "15" || smeta == "16" && order.Code != "26003116" ||
                            smeta == "17" || smeta == "18" || smeta == "21" || smeta == "26" || smeta == "40" ||
                            order.Code == "26015509") {
                        return GetRefByFullCode(os, all_refs, current, "2");
                    }
                    else if (smeta == "19" || smeta == "20" || order.Code == "26004000" || order.Code == "26012500") {
                        return GetRefByFullCode(os, all_refs, current, "3");
                    }
                    else if (smeta == "31" || smeta == "33") {
                        return GetRefByFullCode(os, all_refs, current, "10");
                    }
                    else if (smeta == "27") {
                        return GetRefByFullCode(os, all_refs, current, "11");
                    }
                    else if (smeta == "35") {
                        return GetRefByFullCode(os, all_refs, current, "18");
                    }
                    else if (smeta == "29" || smeta == "30") {
                        return GetRefByFullCode(os, all_refs, current, "19");
                    }
                    else if (smeta == "23" || smeta == "24") {
                        return GetRefByFullCode(os, all_refs, current, "21");
                    }
                    else if (smeta == "25" || smeta == "02" || smeta == "03" || smeta == "04" || smeta == "05" ||
                             smeta == "06" || smeta == "36" || smeta == "28" || smeta == "41" ||
                        order.Code == "26016000" || order.Code == "26014600" || order.Code == "23210008" ||
                        order.Code == "23220008" || order.Code == "26020000" || order.Code == "26520300" ||
                        order.Code == "25008300") {
                        return GetRefByFullCode(os, all_refs, current, "23");
                    }
                }
                else if (order.Subject != null && order.Subject.Direction != null && order.Subject.Direction.Code == "ÑÏ") {
                    current = GetRefByFullCode(os, all_refs, null, "2.1.1.7");
                }
            }
            return null;
        }
        public static TrwRefCashFlow GetRefByFullCode(IObjectSpace os, IList<TrwRefCashFlow> all_refs, TrwRefCashFlow base_item, String path) {
            String code = path;
            if (base_item != null) {
                code = base_item.Code + "." + code;
            }
            return all_refs.FirstOrDefault(x => x.Code == code);
        }
        //    public static TrwRefCashFlow GetRefByFullCode(IObjectSpace os, IList<TrwRefCashFlow> all_refs, TrwRefCashFlow base_item, String path) {
        //        if (base_item != null) {
        //            return GetRefByFullCode(os, all_refs, base_item.Childs, path.Split('.'));
        //        }
        //        else {
        //            IList<TrwRefCashFlow> childs = new List<TrwRefCashFlow>(
        //                all_refs.Where( x => x.TopRef == null)
        //                );
        //            return GetRefByFullCode(os, all_refs, childs, path.Split('.'));
        //        }
        //    }

        //    public static TrwRefCashFlow GetRefByFullCode(IObjectSpace os, IList<TrwRefCashFlow> all_refs, IList<TrwRefCashFlow> sub_items, String[] path_items) {
        //        TrwRefCashFlow item = sub_items.FirstOrDefault(x => x.Code == path_items[0]);
        //        if (path_items.Length == 1) {
        //            return item;
        //        }
        //        else { 
        //            String[] new_path_items = new String[path_items.Length-1];
        //            for(int i = 0; i < new_path_items.Count(); i++) {
        //                new_path_items[i] = path_items[i + 1];
        //            }
        //            return GetRefByFullCode(os, all_refs, item.Childs, new_path_items);
        //        }
        //    }
    }

}
