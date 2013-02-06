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

    // �������� ������������ ������ ������ �� �������� ����

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

        #region ���� ������

        private DateTime _ReportDateStart;  // ����, ������� � ������� ���������� �����
        private DateTime _ReportDate;  // ����, � ������� ���������� �����

        private crmCashFlowRegister _CashFlowRecord;   // ������ CashFlow
        private fmCPRPaymentRequestObligation _PaymentRequestObligation;   // �������������
        private fmCPRRegistrator _PaymentRegistrator;   // ������ ������� �����������

        // �� CachFlow
        private csValuta _ValutaByObligation;   // ������ ������������
        private Decimal _SumByObligation;   // ����� �� �������������� �����
        private Decimal _SumByFact;   // ����� ����������� (�����������) �����

        private Decimal _SumObligationOut;   // ����� �� ��������������
        private Decimal _SumOut;   // ����� ����������� (�����������)

        private csCDocRCB _PaymentDocument;   // �������� ������
        private DateTime _OperationDate;   // ���� ��������� �����
        private crmBankAccount _BankAccount;   // ����
        private fmCOrder _fmOrder;   // �����
        private fmCostItem _CostItem;   // ������

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
        /// ������ CashFlow
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
        /// ������ ������� �����������
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
        /// ������ ������������
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
        /// ����� �� �������������� �����
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
        /// ����� ����������� (�����������) �����
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
        /// ����� �� ��������������
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
        /// ����� ����������� (�����������)
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
        /// �������� ������
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
        /// ���� ��������� �����
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
        /// ����
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
        /// �����
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

        #endregion

        #region ������

        #endregion

    }

}
