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
            throw new NotImplementedException();
            XWZKXMOA msg_out;
            msg_in.CMD = "SET";
            msg_in.UUID = order.Oid.ToString();
            msg_in.OGCODE = 1000;
            msg_in.ZKCODE = order.Code;
            msg_in.ZKBUHTYPE = order.AnalitycAccouterType.Code;
            msg_in.ZKSHORTNAME = order.Name;
            msg_in.ZKFULLNAME = order.NameFull;
            msg_in.ZKDESCRIPTION = order.Description;
            Decimal acccode = 0;
            Decimal.TryParse(order.BuhAccountCode, out acccode);
            msg_in.ZKACBUHCODE = acccode;
            msg_in.ZKKOEFFKB = order.FixKoeff;
            msg_in.ZKKOEFFOZM = order.FixKoeffOZM;
            if (order.Status == fmIOrderStatus.Closed)
                msg_in.ZKISCLOSED = true;
            else
                msg_in.ZKISCLOSED = false;
            msg_out = SyncService.XWZKXM0N(msg_in);
            order.BuhIntNum = msg_out.ZKINTNUM;
        }

        public override bool CheckSyncRequired(IObjectSpace os, fmCOrderExt order) {
            if (os.IsNewObject(order) || os.IsObjectToSave(order) || os.IsObjectToDelete(order)) 
                if (order.Status == fmIOrderStatus.Opened || 
                    order.Status == fmIOrderStatus.Closed)
                    order.IsSyncRequired = true;
                else
                    order.IsSyncRequired = false;
            return order.IsSyncRequired;
        }
    }

}
