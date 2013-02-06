using System;
using System.ComponentModel;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Nomenclature;
using DevExpress.Data.Filtering;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.PaymentRequest {

    [Persistent("fmPRUnknownPaymentTaskLine")]
    public class fmCPRUnknownPaymentTaskLine : csCComponent
    {
        public fmCPRUnknownPaymentTaskLine(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRUnknownPaymentTaskLine);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private fmCPRPaymentRequest _PaymentRequest;   // Ссылка на Заявку
        private Decimal _Sum;   // Сумма по Заявке

        private fmCPRUnknownPaymentTask _UnknownPaymentTask; // Ссылка на задачу обработки неопознанного  платежа
        private DateTime _PaymentDate; // Дата прохождения операции по счёту

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Ссылка на Заявку об оплате
        /// </summary>
        public fmCPRPaymentRequest PaymentRequest {
            get { return _PaymentRequest; }
            set {
                SetPropertyValue<fmCPRPaymentRequest>("PaymentRequest", ref _PaymentRequest, value);
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
        /// Сумма по Заявке
        /// </summary>
        public Decimal Sum {
            get { return _Sum; }
            set {
                SetPropertyValue<Decimal>("Sum", ref _Sum, value);
            }
        }

        /// <summary>
        /// Ссылка на задачу сопоставления
        /// </summary>
        [Association("fmCPRUnknownPaymentTask-fmCPRUnknownPaymentTaskLines")]
        public fmCPRUnknownPaymentTask UnknownPaymentTask {
            get {
                return _UnknownPaymentTask;
            }
            set {
                SetPropertyValue<fmCPRUnknownPaymentTask>("UnknownPaymentTask", ref _UnknownPaymentTask, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
