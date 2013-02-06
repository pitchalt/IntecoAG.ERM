using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Sync {

    public class SyncCSyncPushManager : SyncISyncPushManager {

        protected IDictionary<Type, IList<SyncISyncPush>> _TypeSyncImpls;

        public SyncCSyncPushManager() {
            _TypeSyncImpls = new Dictionary<Type, IList<SyncISyncPush>>();
        }

        public void Register(SyncISyncPush type_sync_impl) {
            if (!_TypeSyncImpls.ContainsKey(type_sync_impl.SyncType))
                _TypeSyncImpls[type_sync_impl.SyncType] = new List<SyncISyncPush>();
            if (!_TypeSyncImpls[type_sync_impl.SyncType].Contains(type_sync_impl))
                _TypeSyncImpls[type_sync_impl.SyncType].Add(type_sync_impl);
        }

        public IList<SyncISyncPush> GetTypeSyncImpls(Type type) {
            if (_TypeSyncImpls.ContainsKey(type))
                return _TypeSyncImpls[type];
            else
                return new List<SyncISyncPush>(0);
        }

        public IList<SyncISyncObject> CheckSyncRequired(DevExpress.ExpressApp.IObjectSpace os) {
            IList<SyncISyncObject> result = new List<SyncISyncObject>();
            foreach (var obj in os.ModifiedObjects) {
                if (obj is SyncISyncObject) {
                    SyncISyncObject tobj = (SyncISyncObject)obj;
                    if (this.CheckSyncRequired(os, tobj))
                        result.Add(tobj);                    
                }
            }
            return result;
        }

        public Boolean CheckSyncRequired(DevExpress.ExpressApp.IObjectSpace os, SyncISyncObject obj) {
            Type type = obj.GetType();
            foreach (SyncISyncPush type_sync in this.GetTypeSyncImpls(type)) {
                type_sync.CheckSyncRequired(os, obj);
            }
            return obj.IsSyncRequired;
        }


        public void Send(DevExpress.ExpressApp.IObjectSpace os, IList<SyncISyncObject> objs) {
            foreach (SyncISyncObject obj in objs) {
                if (obj.IsSyncRequired)
                    this.Send(os, obj);
            }
        }

        public void Send(DevExpress.ExpressApp.IObjectSpace os, SyncISyncObject obj) {
            Type type = obj.GetType();
            Boolean is_fail = false;
            foreach (SyncISyncPush type_sync in this.GetTypeSyncImpls(type)) {
                try {
                    type_sync.Send(os, obj);
                } catch (Exception e) {
                    is_fail = true;
                    Tracing.Tracer.LogError(e);
                }
            }
            if (!is_fail)
                obj.IsSyncRequired = false;
        }
    }
}
