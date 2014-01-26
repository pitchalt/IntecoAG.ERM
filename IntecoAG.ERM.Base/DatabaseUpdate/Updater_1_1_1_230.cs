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
    public class Updater_1_1_1_230 : ModuleUpdater {
        public Updater_1_1_1_230(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        //public override void UpdateDatabaseBeforeUpdateSchema() {
        //    base.UpdateDatabaseBeforeUpdateSchema();
        //    if (this.CurrentDBVersion != new Version("1.1.1.230"))
        //        return;
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetValue\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetKey\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudget\";", true);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudget\" DROP CONSTRAINT \"FK_TrwBudget_TrwPeriod\";", false);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetValue\" DROP CONSTRAINT \"FK_TrwBudgetValue_PeriodValue\";", false);
        //}

        public void UpdateContractDeal(IObjectSpace os) {
            foreach (crmContractDeal deal in os.GetObjects<crmContractDeal>(
                    new UnaryOperator(UnaryOperatorType.IsNull, "Current.ContractDeal"))) {
                        if (deal.Current != null && deal.Current.ContractDeal == null) {
                            deal.Current.ContractDeal = deal;
                        }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.230"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                //
                UpdateContractDeal(os);
                os.CommitChanges();
            }
        }

    }
}
