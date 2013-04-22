using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DC = DevExpress.ExpressApp.DC;
//
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM.Order {

    [DC.DomainComponent]
    public interface fmIOrderExt : fmIOrder, fmIFinIndexStructure {
        fmIOrderStatus Status { get; }
        IList<fmIOrderManageDoc> ManageDocs { get; }
    }
}
