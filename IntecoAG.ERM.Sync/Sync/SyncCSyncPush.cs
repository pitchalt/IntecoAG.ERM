using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
//
namespace IntecoAG.ERM.Sync {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SyncCSyncPush<T>: SyncISyncPush, SyncISyncPush<T>  where T : SyncISyncObject {
        public abstract void Send(IObjectSpace os, T obj);
        public void Send(IObjectSpace os, SyncISyncObject obj) {
            if (!(obj is T))
                throw new ArgumentException("Object Type Error");
            T type_obj =  (T) obj;
            this.Send(os, type_obj);
        }
        public abstract Boolean CheckSyncRequired(IObjectSpace os, T obj);
        public Boolean CheckSyncRequired(IObjectSpace os, SyncISyncObject  obj) {
            if (!(obj is T))
                throw new ArgumentException("Object Type Error");
            T type_obj = (T)obj;
            return this.CheckSyncRequired(os, type_obj);
        }
        public Type SyncType {
            get { return typeof(T); }
        }
    }
}
