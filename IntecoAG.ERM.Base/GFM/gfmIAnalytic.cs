using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DC=DevExpress.ExpressApp.DC;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.GFM {

    [DC.DomainComponent]
    public interface gfmIAnalytic: csICodedComponent {
//        String Code {get; set; }
//        String Name {get; set; }
        [DC.FieldSize(250)]
        String NameFull { get; set; }

        [RuleRequiredField]
        DateTime DateBegin { get; set; }
        DateTime DateEnd { get; set; }
        
        Boolean IsClosed { get; }

//        void ReOpen();
    }
}
