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
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.FM.ReportHelper {

    // �������� ������������ ������ ������ �� �������� ����

    [NavigationItem("Money")]
    [VisibleInReports]
    [Persistent("fmRHPayedRequest")]
    public class fmCRHPayedRequest : fmCRHPayedRequestNonPersistent
    {
        public fmCRHPayedRequest(Session session)
            : base(session) {
        }

        public fmCRHPayedRequest(Session session, DateTime reportDate)
            : base(session) {
                ReportDate = reportDate;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            //this.ComponentType = typeof(fmCRHPayedRequest);
            //this.CID = Guid.NewGuid();
}

        #region ���� ������

        #endregion

        #region �������� ������

        #endregion

        #region ������

        #endregion

    }

}
