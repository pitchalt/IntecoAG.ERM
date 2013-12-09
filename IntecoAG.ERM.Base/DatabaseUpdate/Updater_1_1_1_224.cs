using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System.IO;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Budget;
using IntecoAG.ERM.Trw.Subject;
using IntecoAG.ERM.Trw.References;

namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_224 : ModuleUpdater {
        public Updater_1_1_1_224(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.224"))
                return;
            ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetValue\";", true);
            ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetKey\";", true);
            ExecuteNonQueryCommand("DROP TABLE \"TrwBudget\";", true);
            //            ExecuteNonQueryCommand("ALTER TABLE \"TrwBudget\" DROP CONSTRAINT \"FK_TrwBudget_TrwPeriod\";", false);
//            ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetValue\" DROP CONSTRAINT \"FK_TrwBudgetValue_PeriodValue\";", false);
        } 


        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
        }

    }
}
