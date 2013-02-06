using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.FinIndex {
    [Persistent("fmFinIndexStructure")]
//    [NavigationItem("Finance")]
    public class fmCFinIndexStructure : csCCodedComponent {
        public fmCFinIndexStructure(Session ses) : base(ses) { 
        }

        //[Association("fmFinIndexStructure-fmFinIndexStructureItem", typeof(fmCFinIndexStructureItem))]
        //[Aggregated]
        //XPCollection<fmCFinIndexStructureItem> FinIndexStructureItems {
        //    get { return GetCollection<fmCFinIndexStructureItem>("FinIndexStructureItems"); }
        //}

    }
}
