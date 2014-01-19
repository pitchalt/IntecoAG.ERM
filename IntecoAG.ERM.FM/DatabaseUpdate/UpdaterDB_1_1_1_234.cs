using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.Trw;
//
namespace IntecoAG.ERM.FM {
    public class UpdaterDB_1_1_1_234 : ModuleUpdater {
        public UpdaterDB_1_1_1_234(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        void UpdatePaymentRequest(IObjectSpace os) {
            IList<fmCPRPaymentRequest> reqs = os.GetObjects<fmCPRPaymentRequest>( new BinaryOperator("Date", new DateTime(2014,1,1), BinaryOperatorType.GreaterOrEqual));
            foreach (fmCPRPaymentRequest req in reqs) {
                foreach (fmCPRPaymentRequestObligation line in req.PaySettlmentOfObligations) {
                    line.TrwRefCashFlow = TrwRefCashFlowLogic.AutoDetect(os, false, line.Order, line.PayType, line.CostItem);
                }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.234"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                UpdatePaymentRequest(os);
                os.CommitChanges();
            }
        }

    }
}
