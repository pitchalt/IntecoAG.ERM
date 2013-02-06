using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.FinIndex {

    [Persistent("fmFinIndexStructureItem")]
    public abstract class fmCFinIndexStructureItem : csCComponent, fmIFinIndexStructureItem {
        public fmCFinIndexStructureItem(Session session)
            : base(session) {
        }

        public enum fmCFinIndexStructureItemType {
            Index = 1,
            Calculated = 2,
        }

        [Persistent("fmFinIndexStructureItemCalculated ")]
        public class fmCFinIndexStructureItemCalculated : BaseObject {
            public fmCFinIndexStructureItemCalculated(Session session)
                : base(session) {

            }

            public Decimal Koefficient;
            public fmCFinIndexStructureItem SourceItem;
            [Association("fmFinStructureItem-fmFinStructureItemCalculated")]
            public fmCFinIndexStructureItem TargetItem;

        }

        private fmCFinIndexStructureItemType _FinIndexStructureItemType;
        private Decimal _Value;
        private fmCFinIndex _FinIndex;

        public fmCFinIndexStructureItemType FinIndexStructureItemType {
            get { return _FinIndexStructureItemType; }
            set { SetPropertyValue<fmCFinIndexStructureItemType>("FinIndexStructureItemType", ref _FinIndexStructureItemType, value); }
        }

        [RuleRequiredField]
        [VisibleInListView(false)]
        public fmCFinIndex FinIndex {
            get { return _FinIndex; }
            set {
                SetPropertyValue<fmCFinIndex>("FinIndex", ref _FinIndex, value);
                if (!IsLoading) {
                    OnChanged("SortOrder");
                    OnChanged("Name");
                }
            }
        }

        [PersistentAlias("FinIndex.SortOrder")]
        public Int32 SortOrder {
            get {
                return FinIndex.SortOrder;
            }
        }

        [PersistentAlias("FinIndex.Code")]
        public String Code {
            get {
                return FinIndex.Code;
            }
        }

        [PersistentAlias("FinIndex.Name")]
        public String Name {
            get {
                return FinIndex.Name;
            }
        }

        public Decimal Value {
            get { return _Value; }
            set { SetPropertyValue<Decimal>("Value", ref _Value, value); }
        }

        [Association("fmFinStructureItem-fmFinStructureItemCalculated", typeof(fmCFinIndexStructureItemCalculated))]
        [Aggregated]
        [Browsable(false)]
        public XPCollection<fmCFinIndexStructureItemCalculated> SourceItems {
            get { return GetCollection<fmCFinIndexStructureItemCalculated>("SourceItems"); }
        }
    }

}
