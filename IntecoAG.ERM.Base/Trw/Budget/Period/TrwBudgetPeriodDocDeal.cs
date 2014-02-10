using System;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.FM.Docs.Fm;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Subject;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Budget.Period {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwBudgetPeriodDocDeal : TrwBudgetPeriodDoc {

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class LineDeal: Line {
            private TrwBudgetPeriodDocDeal _DocDeal;
            [Association("TrwBudgetPeriodDocDeal-TrwBudgetPeriodDocDealLine")]
            public TrwBudgetPeriodDocDeal DocDeal {
                get { return _DocDeal; }
                set {
                    SetPropertyValue<TrwBudgetPeriodDocDeal>("DocDeal", ref _DocDeal, value);
                    if (!IsLoading) {
                        Doc = value;
                    }
                }
            }

            public TrwSubjectDealSale TrwSubjectDealSale;
            public TrwOrder TrwOrder;
            public TrwSaleNomenclature TrwSaleNom;
            public TrwSubjectDealBay TrwSubjectDealBay;

            public fmCOrder FmOrder;

            public LineDeal(Session session) : base(session) { }
            public override void AfterConstruction() {
                base.AfterConstruction();
            }
        }

        public FmDocsFmInData DocInData;
        public TrwBudgetPeriodDocBSR DocBSR;

        [Association("TrwBudgetPeriodDocDeal-TrwBudgetPeriodDocDealLine"), Aggregated]
        public XPCollection<LineDeal> DocDealLines {
            get { return GetCollection<LineDeal>("DocDealLines"); }
        }

        public TrwBudgetPeriodDocDeal(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        [Action]
        public void Calculate() {
            TrwBudgetPeriodDocDealLogic.Calculate(this, ObjectSpace.FindObjectSpaceByObject(this));
        }
    }

}
