using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.CS {

    public class LockConflictException : Exception {
    }

    public interface ICommitInfo {
        Boolean OnCommiting();
        void OnCommited();
    }
}
