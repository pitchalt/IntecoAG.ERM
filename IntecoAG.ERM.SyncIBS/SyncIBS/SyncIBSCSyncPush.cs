using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IntecoAG.IBS.SyncService;

namespace IntecoAG.ERM.Sync.SyncIBS {

    public abstract class SyncIBSCSyncPush<T> : SyncCSyncPush<T> where T : SyncISyncObject {
        protected IIBSSyncService _SyncService = null;

        public SyncIBSCSyncPush(IIBSSyncService syncservice) {
            _SyncService = syncservice;
        }

        public IIBSSyncService SyncService {
            get { return _SyncService; }
        }
    }
}
