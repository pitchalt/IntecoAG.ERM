using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.FM.ReportHelper {

    // Перечень неоплаченных счетов счетов на заданную дату

    //[VisibleInReports]
    [NonPersistent]
    public class fmCRHUnpayedRequestNonPersistent : csCComponent
    {
        public fmCRHUnpayedRequestNonPersistent(Session session)
            : base(session) {
        }

        public fmCRHUnpayedRequestNonPersistent(Session session, DateTime reportDate)
            : base(session) {
                ReportDate = reportDate;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCRHUnpayedRequestNonPersistent);
            this.CID = Guid.NewGuid();

            UnpayedRequestContractLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestContractVEDLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestContractOtherLines = new List<fmCRHUnpayedRequestContractList>();

            UnpayedRequestNonContractLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestNonContractZPLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestNonContractNLGLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestNonContractOtherLines = new List<fmCRHUnpayedRequestContractList>();
            
            ValutaCourses = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestContractSectionLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestNonContractSectionLines = new List<fmCRHUnpayedRequestValutaCourseList>();

            UnpayedRequestKazahkstanLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestKazahkstanSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestEnergeticLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestEnergeticSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestPPRLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestPPRSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();

            UnpayedRequestContractPTRLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestContractPTRSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestNonContractPTRLines = new List<fmCRHUnpayedRequestContractList>();
            UnpayedRequestNonContractPTRSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();

        
            UnpayedRequestContractSectionGZSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestContractSectionVEDSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestContractSectionOtherSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();

            UnpayedRequestNonContractMATSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestNonContractZRSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestNonContractNLGSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
            UnpayedRequestNonContractOtherSumLines = new List<fmCRHUnpayedRequestValutaCourseList>();
}

        #region ПОЛЯ КЛАССА

        private DateTime _ReportDateStart;  // Дата, начиная с которой собирается отчёт
        private DateTime _ReportDate;  // Дата, к которой собирается отчёт

        List<fmCPRPaymentRequestObligation> usedObligationList = new List<fmCPRPaymentRequestObligation>();

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractLines; // ГОСЗАКАЗ
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionGZSumLines;
        
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractVEDLines; // ВЭД
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionVEDSumLines;
        
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractOtherLines; // Прочие
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionOtherSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestKazahkstanLines; // Казахстан
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestKazahkstanSumLines; // Казахстан суммы
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestEnergeticLines; // Энергетика
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestEnergeticSumLines; // Энергетика суммы
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestPPRLines; // ППР
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestPPRSumLines; // ППР суммы

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractPTRLines; // ПТР в Счетах по договорам
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractPTRSumLines; // ПТР в Счетах по договорам суммы
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractPTRLines; // ПТР в Прочих счета
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractPTRSumLines; // ПТР в Прочих счета суммы


        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractLines; // Материалы
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractMATSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractZPLines; // Зарплата
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractZRSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractNLGLines; // Налоги
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractNLGSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractOtherLines; // Прочее
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractOtherSumLines;



        // Общие суммы
        private List<fmCRHUnpayedRequestValutaCourseList> _ValutaCourses;
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionLines;
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractSectionLines;

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

        /* ========= ПО ДОГОВОРАМ ========== */

        /// <summary>
        /// Список строк отчёта по договорам (ГОСЗАКАЗ)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestContractLines {
            get {
                return _UnpayedRequestContractLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestContractLines", ref _UnpayedRequestContractLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта ПО ДОГОВОРАМ (ВЭД)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestContractVEDLines {
            get {
                return _UnpayedRequestContractVEDLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestContractVEDLines", ref _UnpayedRequestContractVEDLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта ПО ДОГОВОРАМ (ПРОЧЕЕ)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestContractOtherLines {
            get {
                return _UnpayedRequestContractOtherLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestContractOtherLines", ref _UnpayedRequestContractOtherLines, value);
            }
        }

        /* ========= НЕ ПО ДОГОВОРАМ ========== */

        /// <summary>
        /// Список строк отчёта НЕ ПО ДОГОВОРАМ (МАТЕРИАЛЫ)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestNonContractLines {
            get {
                return _UnpayedRequestNonContractLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestNonContractLines", ref _UnpayedRequestNonContractLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта НЕ ПО ДОГОВОРАМ (ЗАРПЛАТА)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestNonContractZPLines {
            get {
                return _UnpayedRequestNonContractZPLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestNonContractZPLines", ref _UnpayedRequestNonContractZPLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта НЕ ПО ДОГОВОРАМ (НАЛОГИ)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestNonContractNLGLines {
            get {
                return _UnpayedRequestNonContractNLGLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestNonContractNLGLines", ref _UnpayedRequestNonContractNLGLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта НЕ ПО ДОГОВОРАМ (ПРОЧЕЕ)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestNonContractOtherLines {
            get {
                return _UnpayedRequestNonContractOtherLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestNonContractOtherLines", ref _UnpayedRequestNonContractOtherLines, value);
            }
        }

        /* ======== ИТОГИ =========== */

        /// <summary>
        /// Список курсов валют с итогами по всему отчёту (итоги в разрезе валют)
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> ValutaCourses {
            get {
                return _ValutaCourses;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("ValutaCourses", ref _ValutaCourses, value);
            }
        }

        /// <summary>
        /// Итоги по разделу Счета по договорам (итоги в разрезе валют)
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestContractSectionLines {
            get {
                return _UnpayedRequestContractSectionLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestContractSectionLines", ref _UnpayedRequestContractSectionLines, value);
            }
        }

        /// <summary>
        /// Итоги по разделу Прочие счета (итоги в разрезе валют)
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestNonContractSectionLines {
            get {
                return _UnpayedRequestNonContractSectionLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestNonContractSectionLines", ref _UnpayedRequestNonContractSectionLines, value);
            }
        }


        /* ======== ИТОГИ ПО ОТДЕЛЬНЫМ ПОДРАЗДЕЛАМ =========== */

        /// <summary>
        /// Итоги По договорам - Госзаказ
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestContractSectionGZSumLines {
            get {
                return _UnpayedRequestContractSectionGZSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestContractSectionGZSumLines", ref _UnpayedRequestContractSectionGZSumLines, value);
            }
        }

        /// <summary>
        /// Итоги По договорам - ВЭД
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestContractSectionVEDSumLines {
            get {
                return _UnpayedRequestContractSectionVEDSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestContractSectionVEDSumLines", ref _UnpayedRequestContractSectionVEDSumLines, value);
            }
        }

        /// <summary>
        /// Итоги По договорам - Прочие
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestContractSectionOtherSumLines {
            get {
                return _UnpayedRequestContractSectionOtherSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestContractSectionOtherSumLines", ref _UnpayedRequestContractSectionOtherSumLines, value);
            }
        }

        /// <summary>
        /// Итоги Прочие - Материалы
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestNonContractMATSumLines {
            get {
                return _UnpayedRequestNonContractMATSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestNonContractMATSumLines", ref _UnpayedRequestNonContractMATSumLines, value);
            }
        }

        /// <summary>
        /// Итоги Прочие - Зарплата
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestNonContractZRSumLines {
            get {
                return _UnpayedRequestNonContractZRSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestNonContractZRSumLines", ref _UnpayedRequestNonContractZRSumLines, value);
            }
        }

        /// <summary>
        /// Итоги Прочие - Налоги
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestNonContractNLGSumLines {
            get {
                return _UnpayedRequestNonContractNLGSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestNonContractNLGSumLines", ref _UnpayedRequestNonContractNLGSumLines, value);
            }
        }

        /// <summary>
        /// Итоги Прочие - Прочие
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestNonContractOtherSumLines {
            get {
                return _UnpayedRequestNonContractOtherSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestNonContractOtherSumLines", ref _UnpayedRequestNonContractOtherSumLines, value);
            }
        }
 

        // ******************************************* //

        /// <summary>
        /// Список строк отчёта Казахстан
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestKazahkstanLines {
            get {
                return _UnpayedRequestKazahkstanLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestKazahkstanLines", ref _UnpayedRequestKazahkstanLines, value);
            }
        }

        /// <summary>
        /// Итоги Казахстан
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestKazahkstanSumLines {
            get {
                return _UnpayedRequestKazahkstanSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestKazahkstanSumLines", ref _UnpayedRequestKazahkstanSumLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта Энергетика
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestEnergeticLines {
            get {
                return _UnpayedRequestEnergeticLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestEnergeticLines", ref _UnpayedRequestEnergeticLines, value);
            }
        }

        /// <summary>
        /// Итоги Энергетика
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestEnergeticSumLines {
            get {
                return _UnpayedRequestEnergeticSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestEnergeticSumLines", ref _UnpayedRequestEnergeticSumLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта ППР
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestPPRLines {
            get {
                return _UnpayedRequestPPRLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestPPRLines", ref _UnpayedRequestPPRLines, value);
            }
        }

        /// <summary>
        /// Итоги ППР
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestPPRSumLines {
            get {
                return _UnpayedRequestPPRSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestPPRSumLines", ref _UnpayedRequestPPRSumLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта По договору - ПТР
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestContractPTRLines {
            get {
                return _UnpayedRequestContractPTRLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestContractPTRLines", ref _UnpayedRequestContractPTRLines, value);
            }
        }

        /// <summary>
        /// Итоги По договору - ПТР
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestContractPTRSumLines {
            get {
                return _UnpayedRequestContractPTRSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestContractPTRSumLines", ref _UnpayedRequestContractPTRSumLines, value);
            }
        }

        /// <summary>
        /// Список строк отчёта Прочих счета - ПТР
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestNonContractPTRLines {
            get {
                return _UnpayedRequestNonContractPTRLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestNonContractPTRLines", ref _UnpayedRequestNonContractPTRLines, value);
            }
        }

        /// <summary>
        /// Итоги Прочих счета - ПТР
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestNonContractPTRSumLines {
            get {
                return _UnpayedRequestNonContractPTRSumLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestNonContractPTRSumLines", ref _UnpayedRequestNonContractPTRSumLines, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
