using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DC=DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.FM.FinIndex {
    [DC.DomainComponent]
    public interface fmIFinIndexStructureItem {
        fmCFinIndex FinIndex { get; }
        String Code { get; }
        String Name { get; }
        Int32 SortOrder { get; }
        Decimal Value { get; set; }
    }
}
