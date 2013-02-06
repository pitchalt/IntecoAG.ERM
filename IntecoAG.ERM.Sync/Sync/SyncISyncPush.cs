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
    public interface SyncISyncPush {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="obj"></param>
        void Send(IObjectSpace os, SyncISyncObject obj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        Boolean CheckSyncRequired(IObjectSpace os, SyncISyncObject obj);
        /// <summary>
        /// 
        /// </summary>
        Type SyncType { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface SyncISyncPush<T> : SyncISyncPush where T : SyncISyncObject {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="obj"></param>
        void Send(IObjectSpace os, T obj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        Boolean CheckSyncRequired(IObjectSpace os, T obj);
    }
}
