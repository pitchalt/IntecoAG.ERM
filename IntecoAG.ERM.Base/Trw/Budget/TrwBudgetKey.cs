using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
//
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Budget {

    [Persistent("TrwBudgetLine")]
    public abstract class TrwBudgetKey : XPObject {

        private TrwBudgetBase _BudgetBase;
        [Association("TrwBudgetBase-TrwBudgetLineBase")]
        public TrwBudgetBase BudgetBase {
            get { return _BudgetBase; }
            set { SetPropertyValue<TrwBudgetBase>("BudgetBase", ref _BudgetBase, value); }
        }

        private TrwPeriodValue _PeriodValue;
        public TrwPeriodValue PeriodValue {
            get { return _PeriodValue; }
            set { SetPropertyValue<TrwPeriodValue>("PeriodValue", ref _PeriodValue, value); }
        }

        private fmCostItem _CostItem;
        public fmCostItem CostItem {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }

        private crmContractDeal _Deal;
        public crmContractDeal Deal {
            get { return _Deal; }
            set { SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value); }
        }

        private crmCParty _Party;
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }


        private TrwContract _TrwContractSale;
        public TrwContract TrwContractSale {
            get { return _TrwContractSale; }
            set { SetPropertyValue<TrwContract>("TrwContractSale", ref _TrwContractSale, value); }
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

        private TrwContract _TrwContractBay;
        public TrwContract TrwContractBay {
            get { return _TrwContractBay; }
            set { SetPropertyValue<TrwContract>("TrwContractBay", ref _TrwContractBay, value); }
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

        public Boolean IsCalcVat;
        public Boolean IsCalcCost;

        private Decimal _Count;
        public Decimal Count {
            get { return _Count; }
            set {
                value = Decimal.Round(value, 4);
                SetPropertyValue<Decimal>("Count", ref _Count, value);
            }
        }


        private Decimal _Price;
        public Decimal Price {
            get { return _Price; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("Price", ref _Price, value);
            }
        }

        private Decimal _SummCost;
        public Decimal SummCost {
            get { return _SummCost; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("SummCost", ref _SummCost, value);
                if (!IsLoading) {
                    SummAll = SummCost + SummVat;
                }
            }
        }

        private Decimal _SummVat;
        public Decimal SummVat {
            get { return _SummVat; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("SummVat", ref _SummVat, value);
                if (!IsLoading) {
                    SummAll = SummCost + SummVat;
                }
            }
        }

        private Decimal _SummAll;
        public Decimal SummAll {
            get { return _SummAll; }
            set {
                value = Decimal.Round(value, 2);
                SetPropertyValue<Decimal>("SummAll", ref _SummAll, value); 

            }
        }

        public TrwBudgetKey(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
