using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM.ReportHelper {

    // Перечень неоплаченных счетов счетов на заданную дату

    //[VisibleInReports]
    [NonPersistent]
    public class fmCRHPayedRequestNonPersistent : BaseObject   //csCComponent
    {
        public fmCRHPayedRequestNonPersistent(Session session)
            : base(session) {
        }

        public fmCRHPayedRequestNonPersistent(Session session, DateTime reportDate)
            : base(session) {
                ReportDate = reportDate;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            //this.ComponentType = typeof(fmCRHPayedRequestNonPersistent);
            //this.CID = Guid.NewGuid();
}

        #region ПОЛЯ КЛАССА

        private DateTime _ReportDateStart;  // Дата, начиная с которой собирается отчёт
        private DateTime _ReportDate;  // Дата, к которой собирается отчёт

        private crmCashFlowRegister _CashFlowRecord;   // Запись CashFlow
        private fmCPRPaymentRequestObligation _PaymentRequestObligation;   // Обязательство
        private fmCPRRegistrator _PaymentRegistrator;   // Запись журнала регистрации

        // Из CachFlow
        private csValuta _ValutaByObligation;   // Валюта обязательств
        private Decimal _SumByObligation;   // Сумма по обязательствам общая
        private Decimal _SumByFact;   // Сумма фактическая (выплаченная) общая

        private Decimal _SumObligationOut;   // Сумма по обязательствам
        private Decimal _SumOut;   // Сумма фактическая (выплаченная)

        private csCDocRCB _PaymentDocument;   // Документ оплаты
        private DateTime _OperationDate;   // Дата изменения счёта
        private crmBankAccount _BankAccount;   // Счёт
        private fmCOrder _fmOrder;   // Заказ
        private fmCostItem _CostItem;   // Статья

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата, начиная с которой собирается отчёт
        /// </summary>
        public DateTime ReportDateStart {
            get {
                return _ReportDateStart;
            }
            set {
                SetPropertyValue<DateTime>("ReportDateStart", ref _ReportDateStart, value);
            }
        }

        /// <summary>
        /// Дата, к которой собирается отчёт
        /// </summary>
        public DateTime ReportDate {
            get {
                return _ReportDate;
            }
            set {
                SetPropertyValue<DateTime>("ReportDate", ref _ReportDate, value);
            }
        }

        /// <summary>
        /// Запись CashFlow
        /// </summary>
        public crmCashFlowRegister CashFlowRecord {
            get {
                return _CashFlowRecord;
            }
            set {
                SetPropertyValue<crmCashFlowRegister>("CashFlowRecord", ref _CashFlowRecord, value);
            }
        }

        /// <summary>
        /// Обязательство
        /// </summary>
        public fmCPRPaymentRequestObligation PaymentRequestObligation {
            get {
                return _PaymentRequestObligation;
            }
            set {
                SetPropertyValue<fmCPRPaymentRequestObligation>("PaymentRequestObligation", ref _PaymentRequestObligation, value);
            }
        }

        /// <summary>
        /// Запись журнала регистрации
        /// </summary>
        public fmCPRRegistrator PaymentRegistrator {
            get {
                return _PaymentRegistrator;
            }
            set {
                SetPropertyValue<fmCPRRegistrator>("PaymentRegistrator", ref _PaymentRegistrator, value);
            }
        }

        /// <summary>
        /// Валюта обязательств
        /// </summary>
        public csValuta ValutaByObligation {
            get {
                return _ValutaByObligation;
            }
            set {
                SetPropertyValue<csValuta>("ValutaByObligation", ref _ValutaByObligation, value);
            }
        }

        /// <summary>
        /// Сумма по обязательствам общая
        /// </summary>
        public Decimal SumByObligation {
            get {
                return _SumByObligation;
            }
            set {
                SetPropertyValue<Decimal>("SumByObligation", ref _SumByObligation, value);
            }
        }

        /// <summary>
        /// Сумма фактическая (выплаченная) общая
        /// </summary>
        public Decimal SumByFact {
            get {
                return _SumByFact;
            }
            set {
                SetPropertyValue<Decimal>("SumByFact", ref _SumByFact, value);
            }
        }

        /// <summary>
        /// Сумма по обязательствам
        /// </summary>
        public Decimal SumObligationOut {
            get {
                return _SumObligationOut;
            }
            set {
                SetPropertyValue<Decimal>("SumObligationOut", ref _SumObligationOut, value);
            }
        }

        /// <summary>
        /// Сумма фактическая (выплаченная)
        /// </summary>
        public Decimal SumOut {
            get {
                return _SumOut;
            }
            set {
                SetPropertyValue<Decimal>("SumOut", ref _SumOut, value);
            }
        }

        /// <summary>
        /// Документ оплаты
        /// </summary>
        public csCDocRCB PaymentDocument {
            get {
                return _PaymentDocument;
            }
            set {
                SetPropertyValue<csCDocRCB>("PaymentDocument", ref _PaymentDocument, value);
            }
        }

        /// <summary>
        /// Дата изменения счёта
        /// </summary>
        public DateTime OperationDate {
            get {
                return _OperationDate;
            }
            set {
                SetPropertyValue<DateTime>("OperationDate", ref _OperationDate, value);
            }
        }

        /// <summary>
        /// Счёт
        /// </summary>
        public crmBankAccount BankAccount {
            get {
                return _BankAccount;
            }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
            }
        }

        /// <summary>
        /// Заказ
        /// </summary>
        public fmCOrder fmOrder {
            get {
                return _fmOrder;
            }
            set {
                SetPropertyValue<fmCOrder>("fmOrder", ref _fmOrder, value);
            }
        }

        /// <summary>
        /// Статья
        /// </summary>
        public fmCostItem CostItem {
            get {
                return _CostItem;
            }
            set {
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
