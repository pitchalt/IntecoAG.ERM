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

    // �������� ������������ ������ ������ �� �������� ����

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

        #region ���� ������

        private DateTime _ReportDateStart;  // ����, ������� � ������� ���������� �����
        private DateTime _ReportDate;  // ����, � ������� ���������� �����

        List<fmCPRPaymentRequestObligation> usedObligationList = new List<fmCPRPaymentRequestObligation>();

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractLines; // ��������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionGZSumLines;
        
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractVEDLines; // ���
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionVEDSumLines;
        
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractOtherLines; // ������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionOtherSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestKazahkstanLines; // ���������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestKazahkstanSumLines; // ��������� �����
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestEnergeticLines; // ����������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestEnergeticSumLines; // ���������� �����
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestPPRLines; // ���
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestPPRSumLines; // ��� �����

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestContractPTRLines; // ��� � ������ �� ���������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractPTRSumLines; // ��� � ������ �� ��������� �����
        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractPTRLines; // ��� � ������ �����
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractPTRSumLines; // ��� � ������ ����� �����


        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractLines; // ���������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractMATSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractZPLines; // ��������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractZRSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractNLGLines; // ������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractNLGSumLines;

        private List<fmCRHUnpayedRequestContractList> _UnpayedRequestNonContractOtherLines; // ������
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractOtherSumLines;



        // ����� �����
        private List<fmCRHUnpayedRequestValutaCourseList> _ValutaCourses;
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestContractSectionLines;
        private List<fmCRHUnpayedRequestValutaCourseList> _UnpayedRequestNonContractSectionLines;

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

        /* ========= �� ��������� ========== */

        /// <summary>
        /// ������ ����� ������ �� ��������� (��������)
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
        /// ������ ����� ������ �� ��������� (���)
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
        /// ������ ����� ������ �� ��������� (������)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestContractOtherLines {
            get {
                return _UnpayedRequestContractOtherLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestContractOtherLines", ref _UnpayedRequestContractOtherLines, value);
            }
        }

        /* ========= �� �� ��������� ========== */

        /// <summary>
        /// ������ ����� ������ �� �� ��������� (���������)
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
        /// ������ ����� ������ �� �� ��������� (��������)
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
        /// ������ ����� ������ �� �� ��������� (������)
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
        /// ������ ����� ������ �� �� ��������� (������)
        /// </summary>
        public List<fmCRHUnpayedRequestContractList> UnpayedRequestNonContractOtherLines {
            get {
                return _UnpayedRequestNonContractOtherLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestContractList>>("UnpayedRequestNonContractOtherLines", ref _UnpayedRequestNonContractOtherLines, value);
            }
        }

        /* ======== ����� =========== */

        /// <summary>
        /// ������ ������ ����� � ������� �� ����� ������ (����� � ������� �����)
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
        /// ����� �� ������� ����� �� ��������� (����� � ������� �����)
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
        /// ����� �� ������� ������ ����� (����� � ������� �����)
        /// </summary>
        public List<fmCRHUnpayedRequestValutaCourseList> UnpayedRequestNonContractSectionLines {
            get {
                return _UnpayedRequestNonContractSectionLines;
            }
            set {
                SetPropertyValue<List<fmCRHUnpayedRequestValutaCourseList>>("UnpayedRequestNonContractSectionLines", ref _UnpayedRequestNonContractSectionLines, value);
            }
        }


        /* ======== ����� �� ��������� ����������� =========== */

        /// <summary>
        /// ����� �� ��������� - ��������
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
        /// ����� �� ��������� - ���
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
        /// ����� �� ��������� - ������
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
        /// ����� ������ - ���������
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
        /// ����� ������ - ��������
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
        /// ����� ������ - ������
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
        /// ����� ������ - ������
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
        /// ������ ����� ������ ���������
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
        /// ����� ���������
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
        /// ������ ����� ������ ����������
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
        /// ����� ����������
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
        /// ������ ����� ������ ���
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
        /// ����� ���
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
        /// ������ ����� ������ �� �������� - ���
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
        /// ����� �� �������� - ���
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
        /// ������ ����� ������ ������ ����� - ���
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
        /// ����� ������ ����� - ���
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

        #region ������

        #endregion

    }

}
