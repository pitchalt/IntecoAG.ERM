using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
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

    // Перечень неоплаченных счетов счетов на заданную дату

    [NonPersistent]
    public class fmCRHUnpayedRequestNonContractList : BaseObject
    {
        public fmCRHUnpayedRequestNonContractList(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        #region ПОЛЯ КЛАССА


        private DateTime _ReportDateStart;  // Дата, начиная с которой собирается отчёт
        private DateTime _ReportDate;  // Дата, к которой собирается отчёт
        
        private Int32 _SectionNumber;   // Номер раздела (для сортировки?)
        private String _SectionName;   // Название раздела (для сортировки?)

        private Int32 _GroupNumber;   // Номер группы (для сортировки?)
        private String _GroupName;   // Название группы (для сортировки?)

        // Источник группы (Источник заказа, Направление, Статья ДДС,...) 
        private Guid _SourceGroupGUID;
        private Type _SourceGroupType;

        private fmCPRPaymentRequest _PaymentRequest;

        // Столбцы отчёта
        private crmCParty _Party;   // Название организации

        private decimal _SumRUB;   // Сумма в рублях
        private decimal _SumUSD;   // Сумма в долларах США
        private decimal _SumEUR;   // Сумма в Евро

        private String _Comment;   // Примечание

        private decimal _CourseEUR;   // Курс ЕВРО
        private decimal _CourseUSD;   // Курс доллара США

        private fmCRHUnpayedRequestNonPersistent _UnpayedRequest;   // Заголовочная запись

        private csValuta _ValutaPayment;   // Валюта платежа
        private csValuta _ValutaObligation;   // Валюта обязательства
        
        private fmCostItem _CostItem;   // Статья затрат
        private fmCOrderExt _Order;

        private fmCPRPaymentRequestObligation _PaymentRequestObligation;   // Обязательство

        private decimal _SumObligation;   // Сумма в валюте обязательства
        private decimal _SumPayment;   // Сумма в валюте платежа на дату отчёта

        private fmCSubject _Subject;   // Тема
        private Guid _SubjectGuid;   // Тема - идентификатор
        private String _SubjectName;   // Тема - наиенование

        private Int32 _SubSectionNumber;   // Номер подсекции (Госзаказ - 1, ВЭД - 2, Прочие - 3)
        private String _SubSectionName;   // Наименование подсекции (Госзаказ, ВЭД, Прочие)

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
        /// Номер раздела (для сортировки?)
        /// </summary>
        public Int32 SectionNumber {
            get {
                return _SectionNumber;
            }
            set {
                SetPropertyValue<Int32>("SectionNumber", ref _SectionNumber, value);
            }
        }

        /// <summary>
        /// Название раздела (для сортировки?) - 1-й раздел, 2-й раздел и т.д.
        /// </summary>
        public String SectionName {
            get {
                return _SectionName;
            }
            set {
                SetPropertyValue<String>("SectionName", ref _SectionName, value);
            }
        }

        /// <summary>
        /// Номер подсекции (Госзаказ - 1, ВЭД - 2, Прочие - 3)
        /// </summary>
        public Int32 SubSectionNumber {
            get {
                return _SubSectionNumber;
            }
            set {
                SetPropertyValue<Int32>("SubSectionNumber", ref _SubSectionNumber, value);
            }
        }

        /// <summary>
        /// Наименование подсекции (Госзаказ, ВЭД, Прочие)
        /// </summary>
        public String SubSectionName {
            get {
                return _SubSectionName;
            }
            set {
                SetPropertyValue<String>("SubSectionName", ref _SubSectionName, value);
            }
        }

        /// <summary>
        /// Номер группы (для сортировки?)
        /// </summary>
        public Int32 GroupNumber {
            get {
                return _GroupNumber;
            }
            set {
                SetPropertyValue<Int32>("GroupNumber", ref _GroupNumber, value);
            }
        }

        /// <summary>
        /// Название группы (для сортировки?) - 4202, 3М55, Кондор и т.п.
        /// </summary>
        public String GroupName {
            get {
                return _GroupName;
            }
            set {
                SetPropertyValue<String>("GroupName", ref _GroupName, value);
            }
        }

        /// <summary>
        /// GUID объекта-источника группы (Источник заказа, Направление, Статья ДДС,...)
        /// </summary>
        [Browsable(false)]
        public virtual Guid SourceGroupGUID {
            get {
                return _SourceGroupGUID;
            }
            set {
                SetPropertyValue<Guid>("SourceGroupGUID", ref _SourceGroupGUID, value);
            }
        }

        /// <summary>
        /// Тип объекта-источника группы (Источник заказа, Направление, Статья ДДС,...)
        /// </summary>
        [ValueConverter(typeof(ConverterType2String))]
        public virtual Type SourceGroupType {
            get {
                return _SourceGroupType;
            }
            set {
                SetPropertyValue<Type>("SourceGroupType", ref _SourceGroupType, value);
            }
        }


        /// <summary>
        /// Заявка
        /// </summary>
        public fmCPRPaymentRequest PaymentRequest {
            get {
                return _PaymentRequest;
            }
            set {
                SetPropertyValue<fmCPRPaymentRequest>("PaymentRequest", ref _PaymentRequest, value);
                //if (!IsLoading && value != null) {
                //    this.InvoiceDate = value.ExtDocDate;
                //    this.InvoiceNumber = value.ExtDocNumber;
                //    this.Summ = value.Summ;
                //    this.Valuta = value.Valuta;
                //    this.PaymentValuta = value.PaymentValuta;
                //}
            }
        }

        /// <summary>
        /// Организация
        /// </summary>
        //[Browsable(false)]
        public crmCParty Party {
            get {
                return _Party;
            }
            set {
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
            }
        }

        /// <summary>
        /// Сумма в рублях
        /// </summary>
        public Decimal SumRUB {
            get {
                return _SumRUB;
            }
            set {
                SetPropertyValue<Decimal>("SumRUB", ref _SumRUB, value);
            }
        }

        /// <summary>
        /// Сумма в долларах США
        /// </summary>
        public Decimal SumUSD {
            get {
                return _SumUSD;
            }
            set {
                SetPropertyValue<Decimal>("SumUSD", ref _SumUSD, value);
            }
        }

        /// <summary>
        /// Сумма в Евро
        /// </summary>
        public Decimal SumEUR {
            get {
                return _SumEUR;
            }
            set {
                SetPropertyValue<Decimal>("SumEUR", ref _SumEUR, value);
            }
        }

        /// <summary>
        /// Примечание
        /// </summary>
        public String Comment {
            get {
                return _Comment;
            }
            set {
                SetPropertyValue<String>("Comment", ref _Comment, value);
            }
        }

        /// <summary>
        /// Курс ЕВРО
        /// </summary>
        public Decimal CourseEUR {
            get {
                return _CourseEUR;
            }
            set {
                SetPropertyValue<Decimal>("CourseEUR", ref _CourseEUR, value);
            }
        }

        /// <summary>
        /// Курс доллара США
        /// </summary>
        public Decimal CourseUSD {
            get {
                return _CourseUSD;
            }
            set {
                SetPropertyValue<Decimal>("CourseUSD", ref _CourseUSD, value);
            }
        }

        /// <summary>
        /// Валюта платежа
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
        /// Валюта обязательства
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

        /// <summary>
        /// Заказ
        /// </summary>
        public fmCOrderExt Order {
            get {
                return _Order;
            }
            set {
                SetPropertyValue<fmCOrderExt>("Order", ref _Order, value);
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
        /// Сумма в валюте обязательства
        /// </summary>
        public Decimal SumObligation {
            get {
                return _SumObligation;
            }
            set {
                SetPropertyValue<Decimal>("SumObligation", ref _SumObligation, value);
            }
        }

        /// <summary>
        /// Сумма в валюте платежа на дату отчёта
        /// </summary>
        public Decimal SumPayment {
            get {
                return _SumPayment;
            }
            set {
                SetPropertyValue<Decimal>("SumPayment", ref _SumPayment, value);
            }
        }

        /// <summary>
        /// Тема
        /// </summary>
        public fmCSubject Subject {
            get {
                return _Subject;
            }
            set {
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
            }
        }

        /// <summary>
        /// Тема - идентификатор
        /// </summary>
        public Guid SubjectGuid {
            get {
                return _SubjectGuid;
            }
            set {
                SetPropertyValue<Guid>("SubjectGuid", ref _SubjectGuid, value);
            }
        }

        /// <summary>
        /// Тема - наиенование
        /// </summary>
        public String SubjectName {
            get {
                return _SubjectName;
            }
            set {
                SetPropertyValue<String>("SubjectName", ref _SubjectName, value);
            }
        }

        /// <summary>
        /// Заголовочная запись
        /// </summary>
        //[Association("fmCRHUnpayedRequestNonPersistent-fmCRHUnpayedRequestNonContractList")]
        public fmCRHUnpayedRequestNonPersistent UnpayedRequest {
            get {
                return _UnpayedRequest;
            }
            set {
                SetPropertyValue<fmCRHUnpayedRequestNonPersistent>("UnpayedRequest", ref _UnpayedRequest, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
