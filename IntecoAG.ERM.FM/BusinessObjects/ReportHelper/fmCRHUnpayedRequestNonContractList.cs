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

    // �������� ������������ ������ ������ �� �������� ����

    [NonPersistent]
    public class fmCRHUnpayedRequestNonContractList : BaseObject
    {
        public fmCRHUnpayedRequestNonContractList(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        #region ���� ������


        private DateTime _ReportDateStart;  // ����, ������� � ������� ���������� �����
        private DateTime _ReportDate;  // ����, � ������� ���������� �����
        
        private Int32 _SectionNumber;   // ����� ������� (��� ����������?)
        private String _SectionName;   // �������� ������� (��� ����������?)

        private Int32 _GroupNumber;   // ����� ������ (��� ����������?)
        private String _GroupName;   // �������� ������ (��� ����������?)

        // �������� ������ (�������� ������, �����������, ������ ���,...) 
        private Guid _SourceGroupGUID;
        private Type _SourceGroupType;

        private fmCPRPaymentRequest _PaymentRequest;

        // ������� ������
        private crmCParty _Party;   // �������� �����������

        private decimal _SumRUB;   // ����� � ������
        private decimal _SumUSD;   // ����� � �������� ���
        private decimal _SumEUR;   // ����� � ����

        private String _Comment;   // ����������

        private decimal _CourseEUR;   // ���� ����
        private decimal _CourseUSD;   // ���� ������� ���

        private fmCRHUnpayedRequestNonPersistent _UnpayedRequest;   // ������������ ������

        private csValuta _ValutaPayment;   // ������ �������
        private csValuta _ValutaObligation;   // ������ �������������
        
        private fmCostItem _CostItem;   // ������ ������
        private fmCOrderExt _Order;

        private fmCPRPaymentRequestObligation _PaymentRequestObligation;   // �������������

        private decimal _SumObligation;   // ����� � ������ �������������
        private decimal _SumPayment;   // ����� � ������ ������� �� ���� ������

        private fmCSubject _Subject;   // ����
        private Guid _SubjectGuid;   // ���� - �������������
        private String _SubjectName;   // ���� - �����������

        private Int32 _SubSectionNumber;   // ����� ��������� (�������� - 1, ��� - 2, ������ - 3)
        private String _SubSectionName;   // ������������ ��������� (��������, ���, ������)

        #endregion

        #region �������� ������

        /// <summary>
        /// ����, ������� � ������� ���������� �����
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
        /// ����, � ������� ���������� �����
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
        /// ����� ������� (��� ����������?)
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
        /// �������� ������� (��� ����������?) - 1-� ������, 2-� ������ � �.�.
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
        /// ����� ��������� (�������� - 1, ��� - 2, ������ - 3)
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
        /// ������������ ��������� (��������, ���, ������)
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
        /// ����� ������ (��� ����������?)
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
        /// �������� ������ (��� ����������?) - 4202, 3�55, ������ � �.�.
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
        /// GUID �������-��������� ������ (�������� ������, �����������, ������ ���,...)
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
        /// ��� �������-��������� ������ (�������� ������, �����������, ������ ���,...)
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
        /// ������
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
        /// �����������
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
        /// ����� � ������
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
        /// ����� � �������� ���
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
        /// ����� � ����
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
        /// ����������
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
        /// ���� ����
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
        /// ���� ������� ���
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
        /// ������ �������
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
        /// ������ �������������
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
        /// ������
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
        /// �����
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
        /// �������������
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
        /// ����� � ������ �������������
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
        /// ����� � ������ ������� �� ���� ������
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
        /// ����
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
        /// ���� - �������������
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
        /// ���� - �����������
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
        /// ������������ ������
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

        #region ������

        #endregion

    }

}
