using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
//
using IntecoAG.ERM.FM.Order;
//
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.XZK;

namespace IntecoAG.ERM.SyncIBS {
    public static class OrderExchangeLogic {

        public static void ExportTo(IObjectSpace os, fmCOrderExt order) {

        }

        public struct OrderShort {
            public String UserOrgCode;
            public String Code;
            public String SubjectCode;
            public Boolean IsClosed;
            public DateTime DateOpen;
            public DateTime DateClose;
        }

        public static IList<OrderShort> Catalog(IObjectSpace os) {
            ISyncService syncservice = new HTTPSyncService(ConfigurationManager.AppSettings["IBS.SyncService"]);
            XWZKXCIA lprm = new XWZKXCIA();
            lprm.CMD = "CATALOG";
            lprm.OGCODE = 1000;
            XWZKXCOA lres = syncservice.XWZKXC0N(lprm);
            List<OrderShort> result = new List<OrderShort>(lres.ZKLIST.Count);
            foreach (XWZKXCOAZKLIST item in lres.ZKLIST) {
                OrderShort ritem = new OrderShort();
                ritem.UserOrgCode = "1000";
                ritem.Code = item.ZKCODE;
                ritem.IsClosed = item.ZKISCLOSED;
                ritem.SubjectCode = item.ZKSUBJECTCODE;
                DateTime.TryParseExact(item.ZKDTOPEN, "yy-MM-dd", null, default(System.Globalization.DateTimeStyles), out ritem.DateOpen);
                DateTime.TryParseExact(item.ZKDTCLOSE, "yy-MM-dd", null, default(System.Globalization.DateTimeStyles), out ritem.DateClose);
                result.Add(ritem);
            }

            return result;
        }

//        public static void 
    }
}
