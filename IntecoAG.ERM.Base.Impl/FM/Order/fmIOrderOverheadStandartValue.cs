using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.FM.Order {

    [DomainComponent]
    public interface fmIOrderOverheadStandartValue {
        Decimal KoeffKB  { get; set; }
        Decimal KoeffOZM { get; set; }
    }
}
