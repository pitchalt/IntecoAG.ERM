using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;

namespace IntecoAG.ERM.Sync {
    public sealed partial class ERMSyncModule : ModuleBase {
        public ERMSyncModule() {
            InitializeComponent();
        }

        public SyncISyncPushManager SyncPushManager;

        public override void Setup(XafApplication application) {
            base.Setup(application);
            SyncPushManager = new SyncCSyncPushManager();
        }
    }
}
