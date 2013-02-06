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
    public interface SyncISyncPushManager {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        void Register(SyncISyncPush type_sync_impl);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<SyncISyncPush> GetTypeSyncImpls(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        IList<SyncISyncObject> CheckSyncRequired(IObjectSpace os);
        Boolean CheckSyncRequired(IObjectSpace os, SyncISyncObject obj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        void Send(IObjectSpace os, IList<SyncISyncObject> objs);
        void Send(IObjectSpace os, SyncISyncObject obj);
    }
}
