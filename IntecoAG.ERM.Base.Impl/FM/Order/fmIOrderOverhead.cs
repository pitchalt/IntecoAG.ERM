using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.FM.Order {
    public enum fmIOrderOverheadValueType {
        NO_OVERHEAD = 1,
        VARIABLE = 2,
        FIX_SINGLE = 3,
        FIX_NPO = 11
    }

    [DomainComponent]
    public interface fmIOrderOverhead {
        fmIOrderOverheadValueType PlanOverheadType { get; set; }
        fmIOrderOverheadValueType BuhOverheadType { get; set; }
        Decimal FixKoeff { get; set; }
        Decimal FixKoeffOZM { get; set; }
    }

    public interface fmIOrderOverheadIndividual : fmIOrderOverhead {
    }

    public interface fmIOrderOverheadStandart : fmIOrderOverhead {
    }
}
