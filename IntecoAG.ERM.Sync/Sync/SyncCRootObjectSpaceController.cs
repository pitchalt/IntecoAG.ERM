using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Sync {
    public partial class SyncCRootObjectSpaceController : ViewController {
        public SyncCRootObjectSpaceController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected SyncISyncPushManager SyncPushManager;

        protected override void OnActivated() {
            base.OnActivated();
            this.ObjectSpace.Committing += new EventHandler<CancelEventArgs>(ObjectSpace_Committing);
            this.ObjectSpace.Committed += new EventHandler(ObjectSpace_Committed);
            SyncPushManager = this.Application.Modules.FindModule<ERMSyncModule>().SyncPushManager;
        }

        protected override void OnDeactivated() {
            SyncPushManager = null;
            this.ObjectSpace.Committed -= new EventHandler(ObjectSpace_Committed);
            this.ObjectSpace.Committing -= new EventHandler<CancelEventArgs>(ObjectSpace_Committing);
            base.OnDeactivated();
        }

        IList<SyncISyncObject> _SyncRequireds = null;

        void ObjectSpace_Committing(object sender, CancelEventArgs e) {
            if (_SyncRequireds == null)
                _SyncRequireds = SyncPushManager.CheckSyncRequired(this.ObjectSpace);
            else
                _SyncRequireds = null;
        }

        void ObjectSpace_Committed(object sender, EventArgs e) {
            if (_SyncRequireds != null) {
                SyncPushManager.Send(this.ObjectSpace, _SyncRequireds);
                this.ObjectSpace.CommitChanges();
            }
        }
    }
}
