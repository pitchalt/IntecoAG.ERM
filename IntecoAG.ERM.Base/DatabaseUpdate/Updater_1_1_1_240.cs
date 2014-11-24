using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Data.Filtering;
//
using FileHelpers;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_240 : ModuleUpdater {
        public Updater_1_1_1_240(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.239"))
                return;
            DropTable("FmFinPlanDoc", false);
            DropTable("FmFinPlanPlan", false);
            DropTable("FmFinPlanOperation", false);
            DropTable("FmFinPlanJournal", false);
        }

        //public void UpdateContractDeal(IObjectSpace os) {
        //    foreach (crmContractDeal deal in os.GetObjects<crmContractDeal>(
        //            new UnaryOperator(UnaryOperatorType.IsNull, "Current.ContractDeal"))) {
        //                if (deal.Current != null && deal.Current.ContractDeal == null) {
        //                    deal.Current.ContractDeal = deal;
        //                }
        //    }
        //}

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.231"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                //
                os.CommitChanges();
            }
        }

    }
}
