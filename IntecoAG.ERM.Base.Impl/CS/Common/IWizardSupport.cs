using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.CS.Common {

    [DomainComponent]
    public interface IWizardSupport {
        BaseObject Complete();
    }
}
