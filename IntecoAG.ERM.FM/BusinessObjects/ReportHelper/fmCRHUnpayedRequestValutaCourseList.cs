using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
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

    // �������� ������ ����� �� �������� ����

    [VisibleInReports]
    [NonPersistent]
    public class fmCRHUnpayedRequestValutaCourseList : fmCRHUnpayedRequestResultByValuta
    {
        public fmCRHUnpayedRequestValutaCourseList(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
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
    }

}
