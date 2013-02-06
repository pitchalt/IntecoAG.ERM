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

    // �������� ���� �� �������� ���� � ������� ����� �� ���� ���������� �������

    [VisibleInReports]
    [NonPersistent]
    public class fmCRHUnpayedRequestContractSectionNP : fmCRHUnpayedRequestResultByValuta
    {
        public fmCRHUnpayedRequestContractSectionNP(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCRHUnpayedRequestContractSectionNP);
            this.CID = Guid.NewGuid();
        }

        #region ���� ������

        private fmCRHUnpayedRequestNonPersistent _UnpayedRequest;
        
        #endregion

        #region �������� ������

        /// <summary>
        /// ������������ ������
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

        #region ������

        #endregion

    }

}
