using System;
using System.Collections.Generic;
using System.Configuration;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
//using DevExpress.ExpressApp.SystemModule;
//using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.Sync;
using IntecoAG.ERM.Sync.SyncIBS;
using IntecoAG.XAFExt.CDS;
//
using IntecoAG.IBS.SyncService;
//
namespace IntecoAG.ERM.ERM {
    public sealed partial class ERMModule : ModuleBase {
        public ERMModule() {
            InitializeComponent();
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            ERMSyncModule sync_module = application.Modules.FindModule<ERMSyncModule>();
            if (sync_module == null) return;
            HTTPSyncService sync_service = new HTTPSyncService(ConfigurationManager.AppSettings["IBS.SyncService"]);
            sync_module.SyncPushManager.Register(new SyncIBSCSyncPushCrmParty(sync_service));

            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(GCR.Codif.gcrCodifDeal));

            CustomCollectionSourceManager.Register(typeof(GCR.Codif.gcrCodifDeal));
        }
    }
}
