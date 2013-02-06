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

    // Перечень неоплаченных счетов счетов на заданную дату

    [NavigationItem("Money")]
    [VisibleInReports]
    [Persistent("fmRHUnpayedRequest")]
    //[DefaultProperty("OrganizationName")]
    public class fmCRHUnpayedRequest : fmCRHUnpayedRequestNonPersistent
    {
        public fmCRHUnpayedRequest(Session session)
            : base(session) {
        }

        public fmCRHUnpayedRequest(Session session, DateTime reportDate)
            : base(session) {
                ReportDate = reportDate;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCRHUnpayedRequest);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        List<fmCPRPaymentRequestObligation> usedObligationList = new List<fmCPRPaymentRequestObligation>();

        #endregion

        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
