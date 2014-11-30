using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.FinIndex {
    public enum fmCFinIndexType { 
        Primary = 1,
        Calculated = 2
    }

    [Persistent("fmFinIndex")]
//    [NavigationItem("Finance")]
    [Appearance("",  AppearanceItemType.Action, "", TargetItems="Delete", Enabled=false )]
    public class fmCFinIndex: csCCodedComponent {
        public fmCFinIndex(Session ses) : base(ses) { 
        }

        private Boolean _IsClosed;
        private Int32 _CodeBuh;
        private Int16 _SortOrder;

        [RuleRange("", DefaultContexts.Save, 1, 99)]
        public Int32 CodeBuh {
            get { return _CodeBuh; }
            set { SetPropertyValue<Int32>("CodeBuh", ref _CodeBuh, value); }
        }

        [RuleRange("", DefaultContexts.Save, 1, 99)]
        public Int16 SortOrder {
            get { return _SortOrder; }
            set { SetPropertyValue<Int16>("SortOrder", ref _SortOrder, value); }
        }

        public Boolean IsClosed {
            get { return _IsClosed; }
            set { SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value); }
        }


        [Association("FmFinIndex-FmFinIndexCostItem"), Aggregated]
        [VisibleInDetailView(false)]
        public XPCollection<FmFinIndexCostItem> FinIndexCostItems {
            get {
                return GetCollection<FmFinIndexCostItem>("FinIndexCostItems");
            }
        }

        [ManyToManyAlias("FinIndexCostItems", "CostItem")]
        public IList<fmCostItem> CostItems {
            get {
                return GetList<fmCostItem>("CostItems");
            }
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Invalid deleting operation for type: " + this.GetType().FullName);
        }

        
        public static fmCFinIndex FinIndexGet(Session session, fmCostItem cost_item, DateTime date) {
            fmCFinIndex result = null;
            foreach (FmFinIndexCostItem index_cost_item in session.GetObjects(session.GetClassInfo<FmFinIndexCostItem>(),null,null,0,false,false)) {
                if (cost_item == index_cost_item.CostItem) {
                    result = index_cost_item.FinIndex;
                    break;
                }
            }
            return result;
        }

    }
}
