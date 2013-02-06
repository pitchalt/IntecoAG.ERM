using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Security;

namespace IntecoAG.ERM.FM {
    public class RequestMemorandumUpdater : ModuleUpdater {
        public RequestMemorandumUpdater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            if (this.CurrentDBVersion.ToString() != "1.1.1.188")   // Поправить на правильный номер!
                return;

            IObjectSpace os = ObjectSpace;
            Session ssn = ((ObjectSpace)os).Session;

            // Назначение Creator для служебных записок
            XPQuery<fmPaymentRequestMemorandum> paymentRequestMemorandums = new XPQuery<fmPaymentRequestMemorandum>(ssn);
            var queryPRM = (from prm in paymentRequestMemorandums
                            where prm.Creator == null
                            select prm).ToList<fmPaymentRequestMemorandum>();
            foreach (var prm in queryPRM) {
                XPQuery<AuditDataItemPersistent> auditData = new XPQuery<AuditDataItemPersistent>(ssn);
                var queryAuditData = (from audit in auditData
                                      where audit.OperationType == "ObjectCreated"
                                        && audit.AuditedObject.GuidId == prm.Oid
                                      select audit).ToList<AuditDataItemPersistent>();
                if (queryAuditData.Count() > 0) {
                    XPQuery<csCSecurityUser> users = new XPQuery<csCSecurityUser>(ssn);
                    csCSecurityUser user = (from usr in users
                                            where usr.UserName == queryAuditData[0].UserName
                                          select usr).FirstOrDefault();
                    prm.Creator = user;
                }
            }
            os.CommitChanges();
        }
    }
}
