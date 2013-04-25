using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM {
    public class UpdaterFirstBase : ModuleUpdater {
        public UpdaterFirstBase(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("0.0.0.0"))  
                return;
            //
            try {
                fmCSAImporter1C.GetInstance(((ObjectSpace)ObjectSpace).Session);
            } catch {
            }
            try {
                fmCSAImporter1CGazPromBank.GetInstance(((ObjectSpace)ObjectSpace).Session);
            } catch {
            }

            // «аполнение кодов ценностей и курсов валют
            UpdaterVaultaCourse uvc = new UpdaterVaultaCourse(((ObjectSpace)this.ObjectSpace).Session);
            uvc.FixValutaCourseEtc();
            //
            //
            IList<fmCPRPaymentRequest> pays = ObjectSpace.GetObjects<fmCPRPaymentRequest>(new BinaryOperator("FBKManager.IsClosed", true), true);
            foreach (fmCPRPaymentRequest pay in pays) {
                String last_name = pay.FBKManager.LastName.Split(' ')[0];
                IList<hrmStaff> staff = ObjectSpace.GetObjects<hrmStaff>(CriteriaOperator.And(
                    new BinaryOperator("LastName", last_name),
                    new BinaryOperator("FirstName", pay.FBKManager.FirstName),
                    new BinaryOperator("Department.BuhCode", "560"),
                    CriteriaOperator.Or(
                        new BinaryOperator("IsClosed", false),
                        new UnaryOperator(UnaryOperatorType.IsNull, "IsClosed")
                    )), true);
                if (staff.Count == 1) {
                    pay.FBKManager = staff[0];
                }
                else
                    continue;
            }

        }
    }
}
