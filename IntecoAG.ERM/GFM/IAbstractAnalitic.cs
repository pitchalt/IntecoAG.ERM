using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.GFM {

    [DomainComponent]
    public interface IAbstractAnalitic {
        String Code {get; set; }
        String Name {get; set; }
        DateTime DateBegin { get; set; }
        DateTime DateEnd { get; set; }
        Boolean IsClosed { get; }

        void ReOpen();
    }
}
