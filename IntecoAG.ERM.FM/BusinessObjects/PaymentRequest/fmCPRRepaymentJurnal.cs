using System;
//
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM.PaymentRequest {

    // Погашение заявок на оплату (не счетов), а счета прикреплены к заявкам и поэтому тоже погашаются, но могут быть погашены и заявки по прочим основаниям. 
    // Этот объект ассоциирует Заявки на оплату с соответствующими документами оплаты, снабжая ассоциацию признаками:
    // Ссылка на выписку; дата, сумма и валюта оплаты

    // В эурнале "параллельно" ведутся два вида сумм - в валюте расчёта и в валюте учёта (обязательств). "расстояние" между ними - коэффициент
    // пересчёта - это курс на дату изменения счёта.

    //[NavigationItem("Money")]
    [Persistent("fmPRRepaymentJurnal")]
    public class fmCPRRepaymentJurnal : csCComponent
    {
        public fmCPRRepaymentJurnal(
            Session session, 
            fmCPRRepaymentTask repaymentTask,
            fmCDocRCB paymentDocument, 
            fmCPRPaymentRequest paymentRequest, 
            crmBankAccount bankAccount, 
            Decimal sumIn,
            Decimal sumObligationIn,
            Decimal sumOut,
            Decimal sumObligationOut,
            DateTime paymentDate,
            csValuta valutaPayment,
            csValuta valutaObligation,
            CRM.Contract.Analitic.PlaneFact planFact)
            : base(session) {
                this.RepaymentTask = repaymentTask;
                this.PaymentDocument = paymentDocument;
                this.PaymentRequest = paymentRequest;
                this.BankAccount = bankAccount;
                this.SumIn = sumIn;
                this.SumObligationIn = sumObligationIn;
                this.SumOut = sumOut;
                this.SumObligationOut = sumObligationOut;
                this.ValutaPayment = valutaPayment;
                this.ValutaObligation = valutaObligation;
                this.PaymentDate = paymentDate;
                this.PlaneFact = planFact;
        }

        public fmCPRRepaymentJurnal(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRRepaymentJurnal);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private fmCPRPaymentRequest _PaymentRequest; // Заявка.
        private fmCDocRCB _PaymentDocument;   // Платёжный документ, для которого подбирается документ выписки
        private crmBankAccount _BankAccount;   // Счёт
        private DateTime _PaymentDate; // Дата прохождения операции по счёту
        private fmCPRRepaymentTask _RepaymentTask;   // Ссылка на задачу сопоставления, как на основание заведения данной записи
        private PlaneFact _PlaneFact;

        private csValuta _ValutaPayment; // Валюта расчёта
        private decimal _SumIn;   // Сумма поступления в валюте расчётов
        private decimal _SumOut;   // Сумма убытия в валюте расчётов
        private decimal _SumBalance;   // SumIn - SumOut

        private csValuta _ValutaObligation; // Валюта обязательств
        private decimal _SumObligationIn;   // Сумма поступления в валюте учёта (обязательств)
        private decimal _SumObligationOut;   // Сумма убытия в валюте учёта (обязательств)
        private decimal _SumObligationBalance;   // SumObligationIn - SumObligationOut  (в валюте учёта (обязательств))

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Заявка
        /// </summary>
        //[RuleRequiredField]
        public fmCPRPaymentRequest PaymentRequest {
            get { return _PaymentRequest; }
            set {
                SetPropertyValue<fmCPRPaymentRequest>("PaymentRequest", ref _PaymentRequest, value);
            }
        }

        /// <summary>
        /// Платёжный документ, для которого подбирается документ выписки
        /// </summary>
        //[DataSourceCriteria("StatementAccountIndicator = false")]   // Те платёжные документы, что непогашены, т.е. сумма которых не исчерпана выписками
        public fmCDocRCB PaymentDocument {
            get { return _PaymentDocument; }
            set {
                SetPropertyValue<fmCDocRCB>("PaymentDocument", ref _PaymentDocument, value);
            }
        }
        
        /// <summary>
        /// BankAccount - банковский счёт
        /// </summary>
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
            }
        }

        /// <summary>
        /// Дата прохождения операции по счёту
        /// </summary>
        //[RuleRequiredField]
        public DateTime PaymentDate {
            get { return _PaymentDate; }
            set {
                SetPropertyValue<DateTime>("PaymentDate", ref _PaymentDate, value);
            }
        }


        /// <summary>
        /// Валюта расчёта
        /// </summary>
        public csValuta ValutaPayment {
            get {
                return _ValutaPayment;
            }
            set {
                SetPropertyValue<csValuta>("ValutaPayment", ref _ValutaPayment, value);
            }
        }

        /// <summary>
        ///Сумма поступления в валюте расчётов
        /// </summary>
        public decimal SumIn {
            get { return _SumIn; }
            set {
                SetPropertyValue<decimal>("SumIn", ref _SumIn, value);
                if (!IsLoading) {
                    UpdateBalance();
                }
            }
        }

        /// <summary>
        /// Сумма убытия в валюте расчётов
        /// </summary>
        public decimal SumOut {
            get { return _SumOut; }
            set {
                SetPropertyValue<decimal>("SumOut", ref _SumOut, value);
                if (!IsLoading) {
                    UpdateBalance();
                }
            }
        }

        /// <summary>
        /// Баланс SumIn - SumOut
        /// </summary>
        public decimal SumBalance {
            get { return _SumBalance; }
            set { SetPropertyValue<decimal>("SumBalance", ref _SumBalance, value); }
        }



        /// <summary>
        /// Валюта обязательств
        /// </summary>
        public csValuta ValutaObligation {
            get {
                return _ValutaObligation;
            }
            set {
                SetPropertyValue<csValuta>("ValutaObligation", ref _ValutaObligation, value);
            }
        }

        /// <summary>
        /// Сумма поступления в валюте обязательств
        /// </summary>
        public decimal SumObligationIn {
            get {
                return _SumObligationIn;
            }
            set {
                SetPropertyValue<decimal>("SumObligationIn", ref _SumObligationIn, value);
                if (!IsLoading) {
                    UpdateObligationBalance();
                }
            }
        }

        /// <summary>
        /// Сумма убытия в валюте обязательств
        /// </summary>
        public decimal SumObligationOut {
            get {
                return _SumObligationOut;
            }
            set {
                SetPropertyValue<decimal>("SumObligationOut", ref _SumObligationOut, value);
                if (!IsLoading) {
                    UpdateObligationBalance();
                }
            }
        }

        /// <summary>
        /// Баланс SumObligationIn - SumObligationOut в валюте обязательств
        /// </summary>
        public decimal SumObligationBalance {
            get {
                return _SumObligationBalance;
            }
            set {
                SetPropertyValue<decimal>("SumObligationBalance", ref _SumObligationBalance, value);
            }
        }


        /// <summary>
        /// Ссылка на задачу сопоставления, как на основание заведения данной записи
        /// </summary>
        //[RuleRequiredField]
        public fmCPRRepaymentTask RepaymentTask {
            get { return _RepaymentTask; }
            set {
                SetPropertyValue<fmCPRRepaymentTask>("RepaymentTask", ref _RepaymentTask, value);
            }
        }

        /// <summary>
        /// План/Факт
        /// </summary>
        public PlaneFact PlaneFact {
            get { return _PlaneFact; }
            set {
                SetPropertyValue<PlaneFact>("PlaneFact", ref _PlaneFact, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        private void UpdateBalance() {
            SumBalance = SumIn - SumOut;
        }

        private void UpdateObligationBalance() {
            SumObligationBalance = SumObligationIn - SumObligationOut;
        }

        #endregion

    }

}
