using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.XVO;
//
namespace IntecoAG.ERM.Sync.SyncIBS {

    public class SyncIBSCSyncPushCrmParty : SyncIBSCSyncPush<crmCParty> {

        public SyncIBSCSyncPushCrmParty(ISyncService syncservice) : base(syncservice) { }

        public override void Send(IObjectSpace os, crmCParty comp) {
            crmCParty party = ((crmIParty)comp).Party;
            XWVOXMIA msg_in = new XWVOXMIA();
            XWVOXMOA msg_out;
            if (String.IsNullOrEmpty(party.Code))
                msg_in.CMD = "INSERT";
            else {
                msg_in.CMD = "UPDATE";
                msg_in.VOCODE = Decimal.Parse(party.Code == null ? String.Empty : party.Code);
            }
            msg_in.OGCODE = 1000;
            msg_in.UUID = party.Oid.ToString();
            msg_in.VOADDR = party.AddressFact.ToString();
            msg_in.VOCOUNTRYCODE = party.AddressFact.Country.CodeAlfa3;
            msg_in.VOINN = party.INN;
            msg_in.VOKPP = party.KPP;
            msg_in.VONAME = party.Name;
            msg_in.VONAMEFULL = party.NameFull;
            msg_in.VOTYPE = party.ComponentType.FullName;
            msg_in.VOUPUSER = "USER";
            msg_in.VOISCLOSED = party.IsClosed;
            msg_out = SyncService.XWVOXM0N(msg_in);
            if (String.IsNullOrEmpty(party.Code))
               party.Code = msg_out.VOCODE.ToString();
        }

        public override bool CheckSyncRequired(IObjectSpace os, crmCParty comp) {
            crmCParty party = ((crmIParty)comp).Party;
            if (party.ManualCheckStatus != ManualCheckStateEnum.NO_CHECKED)
                if (os.IsNewObject(party) || os.IsObjectToSave(party) || os.IsObjectToDelete(party))
                    comp.IsSyncRequired = true;
            return comp.IsSyncRequired;
        }
    }

}
