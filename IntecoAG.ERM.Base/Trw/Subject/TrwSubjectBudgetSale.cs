using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
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

        //        private XPCollection<TrwSubjectBudgetSaleLine> _SaleLines;
        //        [Aggregated]
        //        public XPCollection<TrwSubjectBudgetSaleLine> SaleLines {
        //            get {
        //                if (_SaleLines == null)
        //                    _SaleLines = new XPCollection<TrwSubjectBudgetSaleLine>(Keys);
        //                return _SaleLines;
        //            }
        //        }

        public TrwSubjectBudgetSale(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        protected struct LineKey {
            public DateTime Date;
            public fmCSubject Subject;
            public fmCOrder Order;
            public crmContractDeal Deal;
            public TrwSubject TrwSubject;
            public TrwContract TrwContract;
            public TrwOrder TrwOrder;
            public TrwSaleNomenclature TrwNomenclature;
        }
        private struct LineKeyValue {
            public Decimal SummCost;
            public Decimal SummVat;
            public Decimal SummAll;
        }
        //
        public override void Calculate(IObjectSpace os) {
            os.Delete(this.SubjectBudgetKeys);
            os.Delete(this.BudgetValues);
            IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> Keys = new Dictionary<TrwBudgetKey, IList<TrwBudgetValue>>();
            foreach (TrwSubjectDealSale deal_sale in TrwSubject.DealsSale) {
                Load(deal_sale, Keys);
            }
        }
        //
        protected void Load(TrwSubjectDealSale trw_deal_sale, IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> keys ) {
            LineKey key = new LineKey();
            key.TrwSubject = trw_deal_sale.TrwSubject;
            key.Subject = trw_deal_sale.Subject;
            key.Deal = trw_deal_sale.Deal;
            if (trw_deal_sale.DealBudget != null) {
                key.TrwContract = trw_deal_sale.DealBudget;
            }
            if (key.Deal != null) {
                Load(key.Deal, key, keys);
            }
        }

        protected void Load(crmContractDeal deal_sale, LineKey key, IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> keys) {
            crmDealVersion deal_version = deal_sale.Current;
            foreach (crmStage stage in deal_version.StageStructure.Stages) {
                Load(deal_version, stage, key, keys);
            }
        }

        protected void Load(crmDealVersion deal_version, crmStage stage, LineKey key, IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> keys) {
            foreach (crmDeliveryUnit unit in stage.DeliveryUnits) {
                foreach (crmDeliveryItem item in unit.DeliveryItems) { 
                    TrwBudgetKey budget_key = keys.Keys.FirstOrDefault(
                                x => x.Deal == key.Deal &&
                                    x.Subject == key.Subject &&
                                    x.Order == item.Order &&
                                    x.TrwSaleNomenclature == item.TrwSaleNomenclature
                                );
                    if (budget_key == null) { 
                    }
                }
            }
        }
    }

}
