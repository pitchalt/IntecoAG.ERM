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

    // �������� ���� �� �������� ���� � ������� �����

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

        #region ���� ������


        private DateTime _CourseDate;  // ����, � ������� ���������� �����
        private csValuta _Valuta;   // ������
        private String _ValutaName;   // ������������ ������
        private Decimal _Course;   // ����
        private Decimal _ConversionCount;   // ����� ���������� ������ ������

        private Decimal _ReportResultInPaymentValuta;   // ����� �� ����� ������ ��� �������� � Valuta ������ �������
        private Decimal _ReportResultInObligationValuta;   // ����� �� ����� ������ ��� �������� � Valuta ������ ������������
        
        #endregion

        #region �������� ������

        /// <summary>
        /// ����, � ������� ���������� �����
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
        /// ������
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
        /// ������������ ������
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
        /// ����
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
        /// ����� ���������� ������ ������
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
        /// ����� �� ����� ������ ��� �������� � Valuta ������ �������
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
        /// ����� �� ����� ������ ��� �������� � Valuta ������ ������������
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

        #region ������

        #endregion

    }

}
