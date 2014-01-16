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
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Budget;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
using IntecoAG.ERM.Trw.Subject;
using IntecoAG.ERM.Trw.References;

namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_227 : ModuleUpdater {
        public Updater_1_1_1_227(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.227"))
                return;
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetValue\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetKey\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudget\";", true);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudget\" DROP CONSTRAINT \"FK_TrwBudget_TrwPeriod\";", false);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetValue\" DROP CONSTRAINT \"FK_TrwBudgetValue_PeriodValue\";", false);
        }


        public void UpdateTrwSubjects(IObjectSpace os) {
            foreach (TrwSubject trw_subj in os.GetObjects<TrwSubject>()) {
                if (trw_subj.SubjectType == TrwSubjectType.TRW_SUBJECT_TYPE_REAL) {
                    trw_subj.Subjects.Add(trw_subj.Subject);
                }
            }
        }

        public void UpdateTrwOrder(IObjectSpace os) {
            foreach (TrwOrder trw_order in os.GetObjects<TrwOrder>()) { 
                if (trw_order.Subject == null)
                    continue;
                if (trw_order.Deal != null) { 
                    if (trw_order.TrwDateFrom == default(DateTime)) {
                        trw_order.TrwDateFrom = trw_order.Deal.TrwDateValidFrom;
                        trw_order.TrwDateToPlan= trw_order.Deal.TrwDateValidToPlan;
                    }
                }
                foreach (fmCOrder order in trw_order.Subject.Orders) {
                    if (order.IsClosed)
                        continue;
                    foreach (TrwSaleNomenclature sale_nom in os.GetObjects<TrwSaleNomenclature>(new BinaryOperator("Order", order), true)) {
                        trw_order.TrwSaleNomenclatures.Add(sale_nom);
                    }
                }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.227"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                //
                UpdateTrwSubjects(os);
                UpdateTrwOrder(os);
                os.CommitChanges();
            }
        }

    }
}
