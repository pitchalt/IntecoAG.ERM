using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.Cost {

    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Sale")]
    public class FmCostPeriod : FmCostTaskList {

        public Int16 Year;
        public Int16 Month;

        //public override String Name {
        //    get { return Year.ToString() + "." + Month.ToString(); }
        //}

        public FmCostPeriod(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
