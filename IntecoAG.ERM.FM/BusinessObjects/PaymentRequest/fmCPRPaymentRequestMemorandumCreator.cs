using System;
//
using DevExpress.ExpressApp;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.PaymentRequest {

    // Форма выбора параметров для создания Служебных записок

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

        #region ПОЛЯ КЛАССА

        private PaymentRequestMemorandumKinds _MemorandumKind; // Вид служебной записки
        private fmPaymentRequestMemorandum _RequestMemorandum; // Служебная записка-основание или шаблон для создания новой служебной записки
        private Boolean _CreatingTemplate; // Создать ли шаблон

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Вид служебной записки
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
        /// Служебная записка-основание или шаблон для создания новой служебной записки
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
        /// Создать ли шаблон
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

        #region МЕТОДЫ

        public fmPaymentRequestMemorandum CreateRequestMemorandum() {

            if ((int)MemorandumKind == 0) {
                throw new Exception("Необходимо указать тип служебной записки");
            }

            //fmPaymentRequestMemorandum rm = new fmPaymentRequestMemorandum(Session);
            fmPaymentRequestMemorandum rm = null;

            //CopyRequest(RequestMemorandum, rm);
            DevExpress.Persistent.Base.Cloner cloner = new Cloner();
        
            // Копирование полей из выбранного документа в новый
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
                throw new Exception("Необходимо указать тип служебной записки");
            }

            fmPaymentRequestMemorandum rm = null;

            //CopyRequest(RequestMemorandum, rm);
            DevExpress.Persistent.Base.Cloner cloner = new Cloner();

            Session ssn = ((ObjectSpace)os).Session;

            // Копирование полей из выбранного документа в новый
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
