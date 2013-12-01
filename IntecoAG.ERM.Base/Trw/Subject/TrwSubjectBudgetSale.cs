using System;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw.Budget;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwSubjectBudgetSale : TrwSubjectBudget {

        private const String IntName = "Ѕюджет выручки";

        protected override void NameUpdate() {

            if (TrwSubject != null && TrwSubject.Subject != null) {
                _Name = TrwSubject.Subject.Code + " " + IntName;
            }
            else {
                _Name = IntName;
            }
        }

        private XPCollection<TrwSubjectBudgetSaleLine> _SaleLines;
        [Aggregated]
        public XPCollection<TrwSubjectBudgetSaleLine> SaleLines {
            get {
                if (_SaleLines == null)
                    _SaleLines = new XPCollection<TrwSubjectBudgetSaleLine>(Keys);
                return _SaleLines;
            }
        }

        public TrwSubjectBudgetSale(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private struct LineKey {
            public fmCOrder Order;
            public crmContractDeal Deal;
            public TrwOrder TrwOrder;
            public TrwSaleNomenclature TrwNomenclature;
        }
        private struct LineKeyValue {
            public Decimal SummCost;
            public Decimal SummVat;
            public Decimal SummAll;
        }
        public override void Calculate(IObjectSpace os) {
            foreach(TrwSubjectDealSale deal_sale in TrwSubject.DealsSale) {
                fmCOrder order = null;
                crmContractDeal deal = null;
                TrwOrder trw_order = null;
                TrwSaleNomenclature trw_sale_nom = null;
                TrwPeriodValue trw_period;

                TrwSubjectBudgetSaleLine line = null;
                foreach (TrwSubjectBudgetSaleLine cur_line in SaleLines) {
                    if (cur_line.Order == order &&
                        cur_line.Deal == deal &&
                        cur_line.TrwOrderSale == trw_order &&
                        cur_line.TrwSaleNomenclature == trw_sale_nom ) {
                        line = cur_line;
                        break;
                    }
                }
                if (line != null) {
                    line = os.CreateObject<TrwSubjectBudgetSaleLine>();

                    foreach (TrwPeriodValue period in TrwPeriod.TrwPeriodValues) {
                        
                    }
                }
            }
        }

    }

}
