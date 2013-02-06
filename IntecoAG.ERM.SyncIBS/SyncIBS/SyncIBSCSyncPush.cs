using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IntecoAG.IBS.SyncService;

namespace IntecoAG.ERM.Sync.SyncIBS {

    public abstract class SyncIBSCSyncPush<T> : SyncCSyncPush<T> where T : SyncISyncObject {
        protected ISyncService _SyncService = null;

        public SyncIBSCSyncPush(ISyncService syncservice) {
            _SyncService = syncservice;
        }

        public ISyncService SyncService {
            get { return _SyncService; }
        }
    }
}
