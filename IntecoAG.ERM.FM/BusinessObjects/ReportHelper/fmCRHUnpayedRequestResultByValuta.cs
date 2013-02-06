using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM.ReportHelper {

    // Перечень сумм на заданную дату в разрезе валют

    [NonPersistent]
    public abstract class fmCRHUnpayedRequestResultByValuta : csCComponent
    {
        public fmCRHUnpayedRequestResultByValuta(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCRHUnpayedRequestResultByValuta);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА


        private DateTime _CourseDate;  // Дата, к которой собирается отчёт
        private csValuta _Valuta;   // Валюта
        private String _ValutaName;   // Наименование валюты
        private Decimal _Course;   // Курс
        private Decimal _ConversionCount;   // Какое количество валюты берётся

        private Decimal _ReportResultInPaymentValuta;   // Сумма по всему отчёту для заданной в Valuta валюте платежа
        private Decimal _ReportResultInObligationValuta;   // Сумма по всему отчёту для заданной в Valuta валюте обязательств
        
        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата, к которой собирается отчёт
        /// </summary>
        public DateTime CourseDate {
            get {
                return _CourseDate;
            }
            set {
                SetPropertyValue<DateTime>("CourseDate", ref _CourseDate, value);
            }
        }

        /// <summary>
        /// Валюта
        /// </summary>
        public csValuta Valuta {
            get {
                return _Valuta;
            }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        /// <summary>
        /// Наименование валюты
        /// </summary>
        public String ValutaName {
            get {
                return _ValutaName;
            }
            set {
                SetPropertyValue<String>("ValutaName", ref _ValutaName, value);
            }
        }

        /// <summary>
        /// Курс
        /// </summary>
        public Decimal Course {
            get {
                return _Course;
            }
            set {
                SetPropertyValue<Decimal>("Course", ref _Course, value);
            }
        }

        /// <summary>
        /// Какое количество валюты берётся
        /// </summary>
        public Decimal ConversionCount {
            get {
                return _ConversionCount;
            }
            set {
                SetPropertyValue<Decimal>("ConversionCount", ref _ConversionCount, value);
            }
        }

        /// <summary>
        /// Сумма по всему отчёту для заданной в Valuta валюте платежа
        /// </summary>
        public Decimal ReportResultInPaymentValuta {
            get {
                return _ReportResultInPaymentValuta;
            }
            set {
                SetPropertyValue<Decimal>("ReportResultInPaymentValuta", ref _ReportResultInPaymentValuta, value);
            }
        }

        /// <summary>
        /// Сумма по всему отчёту для заданной в Valuta валюте обязательств
        /// </summary>
        public Decimal ReportResultInObligationValuta {
            get {
                return _ReportResultInObligationValuta;
            }
            set {
                SetPropertyValue<Decimal>("ReportResultInObligationValuta", ref _ReportResultInObligationValuta, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
