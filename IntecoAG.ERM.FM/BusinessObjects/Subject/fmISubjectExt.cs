using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DC = DevExpress.ExpressApp.DC;
//
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM.Subject {

    [DC.DomainComponent]
    public interface fmISubjectExt: fmISubject {
        new IList<fmIOrderExt> Orders {get;}
    }
}
