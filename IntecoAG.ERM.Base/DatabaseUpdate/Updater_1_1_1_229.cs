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
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_229 : ModuleUpdater {
        public Updater_1_1_1_229(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.229"))
                return;
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetValue\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetKey\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudget\";", true);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudget\" DROP CONSTRAINT \"FK_TrwBudget_TrwPeriod\";", false);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetValue\" DROP CONSTRAINT \"FK_TrwBudgetValue_PeriodValue\";", false);
        }

        [DelimitedRecord(";")]
        public class ReferecesRecord {
            public String Code;
            public String OrderType;
        }

        public class ClassificatorImporter<Tr>
            where Tr : ReferecesRecord {

            public void Import(IObjectSpace os, String file_name) {
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(Tr));
                engine.Options.IgnoreFirstLines = 1;
                Tr[] records = (Tr[])engine.ReadFile(file_name);
                IList<fmÑOrderAnalitycFinanceSource> type_refs = os.GetObjects<fmÑOrderAnalitycFinanceSource>();
                foreach (Tr rec in records) {
                    fmCOrder order = os.GetObjects<fmCOrder>(new BinaryOperator("Code", rec.Code)).First();
                    if (rec.OrderType == "ÊÏ") {
                        order.AnalitycFinanceSource = type_refs.First(x => x.Code == "Ïð.êîììåð.");
                    }
                    if (rec.OrderType == "×Ï") {
                        order.AnalitycFinanceSource = type_refs.First(x => x.Code == "×èñò.Ïðèáûëü");
                    }
                }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.229"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                //
                FileInfo fi = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                String dir = fi.Directory.FullName + "\\Import\\";
                //
                new ClassificatorImporter<ReferecesRecord>().Import(os, dir + "fmOrderUpdate001.csv");
                //
                os.CommitChanges();
            }
        }

    }
}
