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
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
using IntecoAG.ERM.Trw.Subject;
//
namespace IntecoAG.ERM.Trw.Budget {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwBudgetSubjectSale : TrwBudgetSubject {

        private TrwBudgetSale _TrwBudgetMasterChild;
        [Association("TrwBudgetSale-TrwBudgetSubjectSale")]
        public TrwBudgetSale TrwBudgetMasterChild {
            get { return _TrwBudgetMasterChild; }
            set {
                SetPropertyValue<TrwBudgetSale>("TrwBudgetMasterChild", ref _TrwBudgetMasterChild, value);
                if (!IsLoading) {
                    BudgetMaster = value;
                }
            }
        }

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

        public TrwBudgetSubjectSale(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        protected struct LineKey {
            public DateTime Date;
            public fmCSubject Subject;
            public fmCOrder Order;
            public crmContractDeal Deal;
            public crmCParty Party;
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
            os.Delete(this.BudgetValues);
            IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> keys = new Dictionary<TrwBudgetKey, IList<TrwBudgetValue>>();
            foreach (TrwSubjectDealSale deal_sale in TrwSubject.DealsSale) {
                Load(os, deal_sale, keys);
            }
        }
        //
        protected void Load(IObjectSpace os, TrwSubjectDealSale trw_deal_sale, IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> keys ) {
            LineKey key = new LineKey();
            key.TrwSubject = trw_deal_sale.TrwSubject;
            key.Subject = trw_deal_sale.Subject;
            key.Deal = trw_deal_sale.Deal;
            key.Party = key.Deal.Customer;
            if (trw_deal_sale.DealBudget != null) {
                key.TrwContract = trw_deal_sale.DealBudget;
            }
            if (key.Deal != null) {
                key.TrwOrder = key.Subject.TrwOrders.FirstOrDefault(x => x.Deal == key.Deal);
                Load(os, key.Deal, key, keys);
            }
        }

        protected void Load(IObjectSpace os, crmContractDeal deal_sale, LineKey key, IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> keys) {
            crmDealVersion deal_version = deal_sale.Current;
            foreach (crmStage stage in deal_version.StageStructure.Stages) {
                Load(os, deal_version, stage, key, keys);
            }
        }

        protected void Load(IObjectSpace os, crmDealVersion deal_version, crmStage stage, LineKey key, IDictionary<TrwBudgetKey, IList<TrwBudgetValue>> keys) {
            foreach (crmDeliveryUnit unit in stage.DeliveryPlan.DeliveryUnits) {
                foreach (crmDeliveryItem item in unit.DeliveryItems) {
//                    TrwBudgetPeriodValue period_value = TrwBudgetMaster.TrwPeriod.ValueGet(unit.DatePlane);
                    TrwBudgetValue budget_value = null;
                    TrwBudgetKey budget_key = keys.Keys.FirstOrDefault(
                                x => x.DealSale == key.Deal &&
                                    x.Subject == key.Subject &&
                                    x.Order == item.Order &&
                                    x.Party == key.Party &&
                                    x.ObligationValuta == item.Valuta &&
                                    x.TrwContractSale == key.TrwContract &&
                                    x.TrwOrderSale == key.TrwOrder &&
                                    x.TrwSaleNomenclature == item.TrwSaleNomenclature
                                );
                    if (budget_key == null) {
                        budget_key = os.CreateObject<TrwBudgetKey>();
                        BudgetMaster.BudgetKeys.Add(budget_key);
                        // budget_key.SubjectBudget = this;
                        // budget_key.BudgetBase = this;
                        budget_key.DealSale = key.Deal;
                        budget_key.Party = key.Party;
                        budget_key.Subject = key.Subject;
                        budget_key.Order = item.Order;
                        budget_key.ObligationValuta = item.Valuta;
                        budget_key.TrwContractSale = key.TrwContract;
                        budget_key.TrwOrderSale = key.TrwOrder;
                        budget_key.TrwSaleNomenclature = item.TrwSaleNomenclature;
                        keys[budget_key] = new List<TrwBudgetValue>();
                        foreach (TrwBudgetPeriodValue period_value in BudgetMaster.BudgetPeriod.PeriodValues) {
                            budget_value = os.CreateObject<TrwBudgetValue>();
                            budget_key.TrwBudgetValues.Add(budget_value);
                            BudgetValues.Add(budget_value);
                            BudgetMaster.BudgetValues.Add(budget_value);
                            if (period_value.Month < 0 && period_value.Month < 13) {
                                budget_value.Date = new DateTime(BudgetMaster.BudgetPeriod.Year, period_value.Month, 1);
                                budget_value.Date.AddMonths(1);
                                budget_value.Date.AddDays(-1);
                            }
                            else {
                                if (period_value.Month == 0)
                                    budget_value.Date = new DateTime(BudgetMaster.BudgetPeriod.Year - 1, 12, 31);
                                else
                                    budget_value.Date = new DateTime(BudgetMaster.BudgetPeriod.Year + 1, 01, 01);
                            }
                            BudgetMaster.BudgetPeriod.ValueGet(budget_value.Date).BudgetValues.Add(budget_value);
                            if (BudgetMaster.BudgetPeriod.Valuta == budget_key.ObligationValuta)
                                budget_value.ObligationRate = 1;
                            else
                                budget_value.ObligationRate = BudgetMaster.BudgetPeriod.CurrencyExchanges.FirstOrDefault(x => x.Valuta == budget_key.ObligationValuta).Rate;
                            keys[budget_key].Add(budget_value);
                        }
                    }
                    budget_value = keys[budget_key].FirstOrDefault(
                        x => x.Date == unit.DatePlane &&
                            x.ObligationValuta == item.Valuta
                        );
                    if (budget_value == null) {
                        budget_value = os.CreateObject<TrwBudgetValue>();
                        budget_key.TrwBudgetValues.Add(budget_value);
                        BudgetValues.Add(budget_value);
                        BudgetMaster.BudgetValues.Add(budget_value);
                        budget_value.Date = unit.DatePlane;
                        BudgetMaster.BudgetPeriod.ValueGet(budget_value.Date).BudgetValues.Add(budget_value);
                        keys[budget_key].Add(budget_value);
                    }
                    budget_value.SummCost += item.SummCost;
                    budget_value.SummVat += item.SummNDS;
                    budget_value.SummAll = budget_value.SummCost + budget_value.SummVat;
                }
            }
        }
    }

}
