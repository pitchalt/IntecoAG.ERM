using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.FinIndex {

    [Persistent("FmFinIndexCostItem")]
    public class FmFinIndexCostItem : XPObject {
        public FmFinIndexCostItem(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private fmCFinIndex _FinIndex;
        [Association("FmFinIndex-FmFinIndexCostItem")]
        public fmCFinIndex FinIndex {
            get { return _FinIndex; }
            set { SetPropertyValue<fmCFinIndex>("FinIndex", ref _FinIndex, value); }
        }

        private fmCostItem _CostItem;
        public fmCostItem CostItem {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }

    }

}
