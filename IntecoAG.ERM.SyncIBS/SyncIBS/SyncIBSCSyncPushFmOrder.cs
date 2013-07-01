using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.FM.Order;
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.XZK;
//
namespace IntecoAG.ERM.Sync.SyncIBS {

    public class SyncIBSCSyncPushFmOrder : SyncIBSCSyncPush<fmCOrderExt> {

        public SyncIBSCSyncPushFmOrder(IIBSSyncService syncservice) : base(syncservice) { }

        public override void Send(IObjectSpace os, fmCOrderExt order) {
            XWZKXMIA msg_in = new XWZKXMIA();
//            throw new NotImplementedException();
            XWZKXMOA msg_out;
            msg_in.UUID = order.Oid.ToString();
            msg_in.OGCODE = 1000;
            msg_in.ZKINTNUM = order.BuhIntNum;
            msg_in.ZKCODE = order.Code;
            msg_in.ZKDTOPEN = order.DateBegin;
            msg_in.ZKDTCLOSE = order.DateEnd;
            msg_in.ZKISCLOSED = order.IsClosed;
            if (order.AnalitycAccouterType != null)
                msg_in.ZKBUHTYPE = order.AnalitycAccouterType.BuhCode;
            if (String.IsNullOrEmpty(msg_in.ZKBUHTYPE))
                msg_in.ZKBUHTYPE = "0";
            if (order.Status == fmIOrderStatus.Opened) {
                msg_in.CMD = "OPEN";
                msg_in.ZKSUBJECTCODE = order.Subject.Code;
                msg_in.ZKSHORTNAME = order.Name;
                msg_in.ZKFULLNAME = order.NameFull;
                msg_in.ZKDESCRIPTION = order.Description;
                msg_in.ZKSOURCE = order.SourceName;
                Decimal acccode = 0;
                Decimal.TryParse(order.BuhAccount.BuhCode, out acccode);
                msg_in.ZKNDSMODE = order.AnalitycAVT.Code;
                msg_in.ZKACBUHCODE = acccode;
                if (order.BuhOverheadType == fmIOrderOverheadValueType.FIX_NPO) {
                    msg_in.ZKKOEFFKB = order.FixKoeff;
                    msg_in.ZKKOEFFOZM = order.FixKoeffOZM;
                }
                else if (order.BuhOverheadType == fmIOrderOverheadValueType.FIX_SINGLE) {
                    msg_in.ZKKOEFFKB = -1;
                    msg_in.ZKKOEFFOZM = order.FixKoeff;
                }
                else if (order.BuhOverheadType == fmIOrderOverheadValueType.NO_OVERHEAD) {
                    msg_in.ZKKOEFFKB = -1;
                    msg_in.ZKKOEFFOZM = -1;
                }
                else if (order.BuhOverheadType == fmIOrderOverheadValueType.VARIABLE) {
                    msg_in.ZKKOEFFKB = 0;
                    msg_in.ZKKOEFFOZM = 0;
                }
                msg_out = SyncService.XWZKXM0N(msg_in);
                order.BuhIntNum = msg_out.ZKINTNUM;
            }
            if (order.Status == fmIOrderStatus.Closed) {
                msg_in.CMD = "CLOSE";
                msg_out = SyncService.XWZKXM0N(msg_in);
            }
            if (order.Status == fmIOrderStatus.Blocked) {
                msg_in.CMD = "BLOCK";
                msg_out = SyncService.XWZKXM0N(msg_in);
            }
        }

        public override bool CheckSyncRequired(IObjectSpace os, fmCOrderExt order) {
            if (os.IsNewObject(order) || os.IsObjectToSave(order) || os.IsObjectToDelete(order)) 
                if (order.Status == fmIOrderStatus.Opened || 
                    order.Status == fmIOrderStatus.Closed ||
                    order.Status == fmIOrderStatus.Blocked)
                    order.IsSyncRequired = true;
                else
                    order.IsSyncRequired = false;
            return order.IsSyncRequired;
        }
    }

}
