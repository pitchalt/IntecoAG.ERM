using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.FM.Order {

    public enum fmIOrderOverheadStandartType { 
        NO_OVERHEAD = 1,
        VARIABLE = 2,
        FIX_SINGLE = 3,
        FIX_NPO = 11
    }

    [DomainComponent]
    public interface fmIOrderOverheadStandartValue {
        Decimal KoeffKB  { get; set; }
        Decimal KoeffOZM { get; set; }
    }
}
