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
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
//
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
using IntecoAG.ERM.Trw.Subject;
//
namespace IntecoAG.ERM.Trw.Budget {

    [Persistent("TrwBudgetKey")]
    public class TrwBudgetKey : XPObject {

        private TrwBudgetMaster _BudgetMaster;
        [Association("TrwBudgetMaster-TrwBudgetKey")]
        public TrwBudgetMaster BudgetMaster {
            get { return _BudgetMaster; }
            set { SetPropertyValue<TrwBudgetMaster>("BudgetMaster", ref _BudgetMaster, value); }
        }

        [Association("TrwBudgetKey-TrwBudgetValue")]
        public XPCollection<TrwBudgetValue> TrwBudgetValues {
            get { return GetCollection<TrwBudgetValue>("TrwBudgetValues"); }
        }

        private fmCSubject _Subject;
        public fmCSubject Subject {
            get { return _Subject; }
            set { SetPropertyValue<fmCSubject>("Subject", ref _Subject, value); }
        }

        private fmCOrder _Order;
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        private fmCostItem _CostItem;
        public fmCostItem CostItem {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }

        private crmCParty _Party;
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }

        private crmContractDeal _DealSale;
        public crmContractDeal DealSale {
            get { return _DealSale; }
            set { SetPropertyValue<crmContractDeal>("DealSale", ref _DealSale, value); }
        }

        private TrwContract _TrwContractSale;
        [Browsable(false)]
        public TrwContract TrwContractSale {
            get { return _TrwContractSale; }
            set { SetPropertyValue<TrwContract>("TrwContractSale", ref _TrwContractSale, value); }
        }

        public TrwIContract TrwIContractSale {
            get {
                if (TrwContractSale != null)
                    return TrwContractSale;
                else
                    return DealSale;
            }
        }
        private TrwOrder _TrwOrderSale;
        public TrwOrder TrwOrderSale {
            get { return _TrwOrderSale; }
            set { SetPropertyValue<TrwOrder>("TrwOrderSale", ref _TrwOrderSale, value); }
        }

        private TrwSaleNomenclature _TrwSaleNomenclature;
        public TrwSaleNomenclature TrwSaleNomenclature {
            get { return _TrwSaleNomenclature; }
            set { SetPropertyValue<TrwSaleNomenclature>("TrwSaleNomenclature", ref _TrwSaleNomenclature, value); }
        }

        private crmContractDeal _DealBay;
        public crmContractDeal DealBay {
            get { return _DealBay; }
            set { SetPropertyValue<crmContractDeal>("DealBay", ref _DealBay, value); }
        }

        private TrwContract _TrwContractBay;
        [Browsable(false)]
        public TrwContract TrwContractBay {
            get { return _TrwContractBay; }
            set { SetPropertyValue<TrwContract>("TrwContractBay", ref _TrwContractBay, value); }
        }

        public TrwIContract TrwIContractBay {
            get {
                if (TrwContractBay != null)
                    return TrwContractBay;
                else
                    return DealBay;
            }
        }

        private csValuta _ObligationValuta;
        public csValuta ObligationValuta {
            get { return _ObligationValuta; }
            set { SetPropertyValue<csValuta>("ObligationValuta", ref _ObligationValuta, value); }
        }

        private csValuta _PaymentValuta;
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set { SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value); }
        }

        public TrwBudgetKey(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
