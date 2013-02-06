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

    // �������� ������ ����� �� �������� ����

    //[NavigationItem("Money")]
    [VisibleInReports]
    [Persistent("fmRHUnpayedRequestValutaCourse")]
    //[DefaultProperty("OrganizationName")]
    public class fmCRHUnpayedRequestValutaCourse : fmCRHUnpayedRequestResultByValuta
    {
        public fmCRHUnpayedRequestValutaCourse(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCRHUnpayedRequestValutaCourse);
            this.CID = Guid.NewGuid();
        }

        #region ���� ������

        private fmCRHUnpayedRequest _UnpayedRequest;
        
        #endregion

        #region �������� ������

        /// <summary>
        /// ������������ ������
        /// </summary>
        public fmCRHUnpayedRequest UnpayedRequest {
            get {
                return _UnpayedRequest;
            }
            set {
                SetPropertyValue<fmCRHUnpayedRequest>("UnpayedRequest", ref _UnpayedRequest, value);
            }
        }

        #endregion

    }

}
