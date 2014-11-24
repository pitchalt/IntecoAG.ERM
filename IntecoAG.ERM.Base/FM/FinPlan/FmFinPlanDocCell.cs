using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.FinPlan.View;

namespace IntecoAG.ERM.FM.FinPlan {

    [Persistent]
    public class FmFinPlanDocCell : XPObject, ITableViewCell {
        public FmFinPlanDocCell(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public string Value {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }
    }

}
