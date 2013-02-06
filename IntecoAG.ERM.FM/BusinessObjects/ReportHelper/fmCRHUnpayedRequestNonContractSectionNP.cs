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

    // Перечень сумм на заданную дату в разрезе валют по всем НЕ договорным заявкам

    [VisibleInReports]
    [NonPersistent]
    public class fmCRHUnpayedRequestNonContractSectionNP : fmCRHUnpayedRequestResultByValuta
    {
        public fmCRHUnpayedRequestNonContractSectionNP(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCRHUnpayedRequestNonContractSectionNP);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private fmCRHUnpayedRequestNonPersistent _UnpayedRequest;
        
        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Заголовочная запись
        /// </summary>
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
