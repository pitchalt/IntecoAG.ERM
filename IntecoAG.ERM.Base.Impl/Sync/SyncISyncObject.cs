using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
namespace IntecoAG.ERM.Sync {
    /// <summary>
    /// 
    /// </summary>
    public interface SyncISyncObject {
        Boolean IsSyncRequired { get; set; }
    }
}
