using System;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw;

namespace IntecoAG.ERM.FM.PaymentRequest {

    /// <summary>
    /// Оплачиваемые расчётные обязательства 
    /// </summary>
    [Persistent("fmPRPaymentRequestObligation")]
    public class fmCPRPaymentRequestObligation : csCComponent
    {
        public fmCPRPaymentRequestObligation(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            //ImportResult = new fmImportResult(this.Session);
            this.ComponentType = typeof(fmCPRPaymentRequestObligation);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private crmContractDeal _ContractDeal;   // Доп. соглашение
        private crmFinancialDeal _FinancialDeal; // Финансовый этап
        private crmPaymentItem _PaymentItem;   // Оплата, т.е. пункт обязательства оплаты
        private fmCostItem _CostItem;   // Статья затрат
        private fmCOrderExt _Order;
        private fmPRPayType _PayType;

        private decimal _Summ; // Сумма
//        private csValuta _Valuta; // Валюта
//        private csValuta _PaymentValuta; // Валюта расчёта

        private fmCPRPaymentRequest _PaymentRequestBase; // Ссылка на Заявку на оплату

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Доп. соглашение
        /// </summary>
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value); }
        }

        /// <summary>
        /// Финансовый этап
        /// </summary>
        public crmFinancialDeal FinancialDeal {
            get { return _FinancialDeal; }
            set { SetPropertyValue<crmFinancialDeal>("FinancialDeal", ref _FinancialDeal, value); }
        }

        /// <summary>
        /// Пункт обязательств оплаты
        /// </summary>
        public virtual crmPaymentItem PaymentItem {
            get { return _PaymentItem; }
            set { SetPropertyValue<crmPaymentItem>("PaymentItem", ref _PaymentItem, value); }
        }
        /// <summary>
        /// Вариант оплаты
        /// </summary>
        [RuleRequiredField]
        public fmPRPayType PayType {
            get { return _PayType; }
            set { 
                SetPropertyValue<fmPRPayType>("PayType", ref _PayType, value);
                if (!IsLoading) {
                    TrwRefCashFlowUpdate();
                }
            }
        }
        /// <summary>
        /// Заказ
        /// </summary>
        [RuleRequiredField]
        public fmCOrderExt Order {
            get { return _Order; }
            set { 
                SetPropertyValue<fmCOrderExt>("Order", ref _Order, value);
                if (!IsLoading) {
                    TrwRefCashFlowUpdate();
                }
            }
        }
        /// <summary>
        /// Статья
        /// </summary>
        [RuleRequiredField]
        public fmCostItem CostItem {
            get { return _CostItem; }
            set { 
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
                if (!IsLoading) {
                    TrwRefCashFlowUpdate();
                }
            }
        }

        private TrwRefCashFlow _TrwRefCashFlow;
        /// <summary>
        /// ТРВ Статья ДДС
        /// </summary>
        public TrwRefCashFlow TrwRefCashFlow {
            get { return _TrwRefCashFlow; }
            set { 
                SetPropertyValue<TrwRefCashFlow>("TrwRefCashFlow", ref _TrwRefCashFlow, value);
            }
        }
        //
        private void TrwRefCashFlowUpdate() {
            if (Order == null || CostItem == null || !(PayType == fmPRPayType.PREPAYMENT || PayType == fmPRPayType.POSTPAYMENT))
                return;
            TrwRefCashFlow = TrwRefCashFlowLogic.AutoDetect(ObjectSpace.FindObjectSpaceByObject(this), false, Order, PayType, CostItem);
        }
        /// <summary>
        /// Сумма
        /// </summary>
        [RuleRequiredField]
        public Decimal Summ {
            get { return _Summ; }
            set {
                SetPropertyValue<decimal>("Summ", ref _Summ, value);
                if (!IsLoading && PaymentRequestBase != null)
                    PaymentRequestBase.UpdateSumm();
                    
            }
        }

        /// <summary>
        /// Валюта платежа
        /// </summary>
        //[RuleRequiredField]
        public csValuta PaymentValuta {
            get {
                if (PaymentRequestBase != null)
                    return PaymentRequestBase.PaymentValuta;
                else
                    return null;
            }
//            set {
//                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
//            }
        }

        /// <summary>
        /// Валюта обязательств
        /// </summary>
        //[RuleRequiredField]
        public csValuta Valuta {
            get {
                if (PaymentRequestBase != null)
                    return PaymentRequestBase.Valuta;
                else
                    return null;
            }
//            set {
//                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
//            }
        }

        /// <summary>
        /// Валюта расчётов
        /// </summary>
        //[RuleRequiredField]
        //public csValuta ValutaPayment {
        //    get { return _ValutaPayment; }
        //    set {
        //        SetPropertyValue<csValuta>("ValutaPayment", ref _ValutaPayment, value);
        //    }
        //}

        [Association("fmPRPaymentRequest-fmPRPaymentRequestObligation")]
        public fmCPRPaymentRequest PaymentRequestBase {
            get { return _PaymentRequestBase; }
            set { SetPropertyValue<fmCPRPaymentRequest>("PaymentRequestBase", ref _PaymentRequestBase, value); }
        }

        #endregion

        #region МЕТОДЫ

        protected override void OnDeleting() {
            if (PaymentRequestBase != null) {
                Summ = 0;
                PaymentRequestBase.UpdateSumm();
            }
            base.OnDeleting();
        }
        #endregion

    }

}
