using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Nomenclature;

namespace IntecoAG.ERM.Trw.Budget.Period {

    [Persistent("TrwBudgetPeriodInContractBSR")]
    public class TrwBudgetPeriodInContractBSR : BaseObject {

        private TrwBudgetPeriod _Period;
        [Association("TrwBudgetPeriod-TrwBudgetPeriodInContractBSR")]
        public TrwBudgetPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<TrwBudgetPeriod>("Period", ref _Period, value); }
        }

        private String _SaleNomCode;
        public String SaleNomCode {
            get { return _SaleNomCode; }
            set { SetPropertyValue<String>("SaleNomCode", ref _SaleNomCode, value); }
        }
        private TrwSaleNomenclature _SaleNomenclature;
        public TrwSaleNomenclature SaleNomenclature {
            get { return _SaleNomenclature; }
            set { SetPropertyValue<TrwSaleNomenclature>("SaleNomenclature", ref _SaleNomenclature, value); }
        }
        [PersistentAlias("SaleNomenclature.Order")]
        public fmCOrder FmOrder {
            get {
                return SaleNomenclature != null ? SaleNomenclature.Order : null;
            }
        }
        [PersistentAlias("SaleNomenclature.Order.Subject")]
        public fmCSubject FmSubject {
            get {
                return SaleNomenclature != null ? 
                    SaleNomenclature.Order != null ?
                    SaleNomenclature.Order.Subject : null : null;
            }
        }

        public Decimal Period00;
        public Decimal Period01;
        public Decimal Period02;
        public Decimal Period03;
        public Decimal Period04;
        public Decimal Period05;
        public Decimal Period06;
        public Decimal Period07;
        public Decimal Period08;
        public Decimal Period09;
        public Decimal Period10;
        public Decimal Period11;
        public Decimal Period12;
        public Decimal Period13;

        public TrwBudgetPeriodInContractBSR(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
