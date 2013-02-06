using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.XVO;

namespace IntecoAG.ERM.CRM.Party {
    public partial class PartySyncController : ViewController {
        public PartySyncController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private crmIParty party;
        private HTTPSyncService syncservice = null;

        protected override void OnActivated() {
            base.OnActivated();
            ObjectView oview = View as ObjectView;
            party = oview.CurrentObject as crmIParty;
            PartySyncAction.Active.SetItemValue("Is_crmIParty", false);
            if (party == null) return;
            PartySyncAction.Active.SetItemValue("Is_crmIParty", true);
            ObjectSpace.Committing += new EventHandler<CancelEventArgs>(ObjectSpace_Committing);
            ObjectSpace.Committed += new EventHandler(ObjectSpace_Committed);
            PartySyncAction.Active.SetItemValue("ObjectSpaceChanged", ObjectSpace.IsModified);
            ObjectSpace.ModifiedChanged += new EventHandler(ObjectSpace_ModifiedChanged);
            syncservice = new HTTPSyncService(ConfigurationManager.AppSettings["IBS.SyncService"]);
        }

        void ObjectSpace_ModifiedChanged(object sender, EventArgs e) {
            PartySyncAction.Active.SetItemValue("ObjectSpaceChanged", ObjectSpace.IsModified);
        }

        void ObjectSpace_Committing(object sender, CancelEventArgs e) {
            if (ObjectSpace.IsModified)
                SyncParty(party);
            //throw new NotImplementedException();
        }

        void ObjectSpace_Committed(object sender, EventArgs e) {
            //throw new NotImplementedException();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (party == null) return;
            party = null;
            ObjectSpace.Committing -= new EventHandler<CancelEventArgs>(ObjectSpace_Committing);
            ObjectSpace.Committed -= new EventHandler(ObjectSpace_Committed);
            ObjectSpace.ModifiedChanged -= new EventHandler(ObjectSpace_ModifiedChanged);
        }

        private void PartySyncAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (ObjectSpace.IsModified) {
                ObjectSpace.CommitChanges();
            }
        }

        private void SyncParty(crmIParty party) {
            XWVOXMIA msg_in = new XWVOXMIA();
            XWVOXMOA msg_out;
            if (String.IsNullOrEmpty(party.Code))
                msg_in.CMD = "INSERT";
            else {
                msg_in.CMD = "UPDATE";
                msg_in.VOCODE = Decimal.Parse(party.Code == null ? String.Empty : party.Code);
            }
            msg_in.OGCODE = 1000;

            msg_in.VOADDR = party.AddressFact.ToString();
            msg_in.VOCOUNTRYCODE = party.AddressFact.Country.CodeAlfa3;
            msg_in.VOINN = party.INN;
            msg_in.VOKPP = party.KPP;
            msg_in.VONAME = party.Name;
            msg_in.VONAMEFULL = party.NameFull;
            msg_in.VOTYPE = party.ComponentType.FullName;
            msg_in.VOUPUSER = "USER";
            msg_in.VOISCLOSED = party.IsClosed;
            try {
                msg_out = syncservice.XWVOXM0N(msg_in);
                if (String.IsNullOrEmpty(party.Code))
                    party.Code = msg_out.VOCODE.ToString();
            }
            catch (Exception e) {
                Tracing.Tracer.LogError(e);
            }

        }
    }
}
