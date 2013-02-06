using System;
//
using DevExpress.ExpressApp;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.PaymentRequest {

    // ����� ������ ���������� ��� �������� ��������� �������

    //[VisibleInReports]
    [NonPersistent]
    public class fmCPRPaymentRequestMemorandumCreator : BaseObject   //csCComponent
    {
        public fmCPRPaymentRequestMemorandumCreator(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
}

        #region ���� ������

        private PaymentRequestMemorandumKinds _MemorandumKind; // ��� ��������� �������
        private fmPaymentRequestMemorandum _RequestMemorandum; // ��������� �������-��������� ��� ������ ��� �������� ����� ��������� �������
        private Boolean _CreatingTemplate; // ������� �� ������

        #endregion

        #region �������� ������

        /// <summary>
        /// ��� ��������� �������
        /// </summary>
        public PaymentRequestMemorandumKinds MemorandumKind {
            get {
                return _MemorandumKind;
            }
            set {
                PaymentRequestMemorandumKinds old = MemorandumKind;
                SetPropertyValue<PaymentRequestMemorandumKinds>("MemorandumKind", ref _MemorandumKind, value);
                if (!IsLoading && old != value) {
                    RequestMemorandum = null;
                    OnChanged("RequestMemorandum");
                }
            }
        }

        /// <summary>
        /// ��������� �������-��������� ��� ������ ��� �������� ����� ��������� �������
        /// </summary>
        //[DataSourceCriteria("MemorandumKind == @MemorandumKind")]
        [DataSourceCriteria("MemorandumKind == '@This.MemorandumKind'")]
        public fmPaymentRequestMemorandum RequestMemorandum {
            get {
                return _RequestMemorandum;
            }
            set {
                SetPropertyValue<fmPaymentRequestMemorandum>("RequestMemorandum", ref _RequestMemorandum, value);
            }
        }

        /// <summary>
        /// ������� �� ������
        /// </summary>
        public Boolean CreatingTemplate {
            get {
                return _CreatingTemplate;
            }
            set {
                SetPropertyValue<Boolean>("CreatingTemplate", ref _CreatingTemplate, value);
            }
        }

        #endregion

        #region ������

        public fmPaymentRequestMemorandum CreateRequestMemorandum() {

            if ((int)MemorandumKind == 0) {
                throw new Exception("���������� ������� ��� ��������� �������");
            }

            //fmPaymentRequestMemorandum rm = new fmPaymentRequestMemorandum(Session);
            fmPaymentRequestMemorandum rm = null;

            //CopyRequest(RequestMemorandum, rm);
            DevExpress.Persistent.Base.Cloner cloner = new Cloner();
        
            // ����������� ����� �� ���������� ��������� � �����
            if (RequestMemorandum != null) {
                //rm = cloner.CloneTo(RequestMemorandum, typeof(fmPaymentRequestMemorandum)) as fmPaymentRequestMemorandum;
                rm = RequestMemorandum.CloneRequest() as fmPaymentRequestMemorandum;
            } else {
                rm = cloner.CreateObject(Session, typeof(fmPaymentRequestMemorandum)) as fmPaymentRequestMemorandum;
            }
            rm.MemorandumKind = MemorandumKind;

            if (CreatingTemplate) {
                rm.State = PaymentRequestStates.TEMPLATE;
            }
            return rm;
        }

        public fmPaymentRequestMemorandum CreateRequestMemorandum(IObjectSpace os) {

            if ((int)MemorandumKind == 0) {
                throw new Exception("���������� ������� ��� ��������� �������");
            }

            fmPaymentRequestMemorandum rm = null;

            //CopyRequest(RequestMemorandum, rm);
            DevExpress.Persistent.Base.Cloner cloner = new Cloner();

            Session ssn = ((ObjectSpace)os).Session;

            // ����������� ����� �� ���������� ��������� � �����
            if (RequestMemorandum != null) {
                fmPaymentRequestMemorandum RequestMemorandum1 = SessionHelper.GetObjectInSession<fmPaymentRequestMemorandum>(RequestMemorandum, ssn);
                rm = RequestMemorandum1.CloneRequest() as fmPaymentRequestMemorandum;
            } else {
                rm = cloner.CreateObject(ssn, typeof(fmPaymentRequestMemorandum)) as fmPaymentRequestMemorandum;
            }
            rm.MemorandumKind = MemorandumKind;

            if (CreatingTemplate) {
                rm.State = PaymentRequestStates.TEMPLATE;
            }
            return rm;
        }

        #endregion

    }

}
