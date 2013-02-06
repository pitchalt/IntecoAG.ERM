using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// INewVersionSupport
    /// </summary>
    public interface INewVersionSupport {

        #region онкъ йкюяяю дкъ онддепфйх янгдюмхъ мнбни бепяхх назейрю

        /// <summary>
        /// VersionState - БЕПЯХЪ ГЮОХЯХ
        /// </summary>
        //VersionStates VersionState { get; set; }

        #endregion

        #region лерндш дкъ онддепфйх янгдюмхъ мнбни бепяхх назейрю

        IVersionSupport CreateNewVersion();

        #endregion
    }

}