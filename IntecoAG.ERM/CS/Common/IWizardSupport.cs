using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace IntecoAG.ERM.CS.Common {
    public interface IWizardSupport {
        BaseObject Complete();
    }
}
