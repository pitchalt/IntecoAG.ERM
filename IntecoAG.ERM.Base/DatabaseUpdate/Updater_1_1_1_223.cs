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
    public class Updater_1_1_1_223 : ModuleUpdater {
        public Updater_1_1_1_223(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.223"))
                return;
            ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetBase\" RENAME TO \"TrwBudget\";", true);
            ExecuteNonQueryCommand("ALTER TABLE \"TrwPeriod\" RENAME TO \"TrwBudgetPeriod\";", true);
            ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetSubjectControl\";", true);
            ExecuteNonQueryCommand(
                "UPDATE \"XPObjectType\" "+
                "SET \"TypeName\"='IntecoAG.ERM.Trw.Budget.TrwBudgetPeriod' "+
                "WHERE \"TypeName\"='IntecoAG.ERM.Trw.TrwPeriod';", true);
            ExecuteNonQueryCommand(
                "UPDATE \"XPObjectType\" "+
                "SET \"TypeName\"='IntecoAG.ERM.Trw.Budget.TrwBudgetPeriodValue' "+
                "WHERE \"TypeName\"='IntecoAG.ERM.Trw.TrwBudgetPeriodValue';", true);

//            ExecuteNonQueryCommand("ALTER TABLE \"TrwBudget\" DROP CONSTRAINT \"FK_TrwBudget_TrwPeriod\";", false);
//            ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetValue\" DROP CONSTRAINT \"FK_TrwBudgetValue_PeriodValue\";", false);
        } 


        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.223"))
                return;
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<TrwBudgetPeriod> periods = os.GetObjects<TrwBudgetPeriod>();

                foreach (TrwBudgetPeriod period in periods) {
                    period.InitPeriodValues();
                }

                IList<TrwSubject> trw_subjects = os.GetObjects<TrwSubject>();
                foreach (TrwSubject trw_subj in trw_subjects) {
                    trw_subj.Subjects.Add(trw_subj.Subject);
                    foreach (TrwSubjectDealSale trw_deal_sale in trw_subj.DealsSale) {
                        trw_deal_sale.Subject = trw_subj.Subject;
                    }
                    foreach (TrwSubjectDealBay trw_deal_bay in trw_subj.DealsBay) {
                        trw_deal_bay.Subject = trw_subj.Subject;
                    }
                }
                os.CommitChanges();
            }
        }

    }
}
