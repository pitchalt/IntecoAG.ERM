using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
//
using FileHelpers;
//
using IntecoAG.ERM.FM;
using IntecoAG.ERM.Trw;

namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_228 : ModuleUpdater {
        public Updater_1_1_1_228(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.228"))
                return;
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetValue\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetKey\";", true);
            //ExecuteNonQueryCommand("DROP TABLE \"TrwBudget\";", true);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudget\" DROP CONSTRAINT \"FK_TrwBudget_TrwPeriod\";", false);
            //ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetValue\" DROP CONSTRAINT \"FK_TrwBudgetValue_PeriodValue\";", false);
        }

        [DelimitedRecord(";")]
        public class ReferecesRecord {
            public String IntCode;
            public String Code;
            public String Name;
        }

        public class ClassificatorImporter<Tr>
            where Tr : ReferecesRecord {

            public void Import(IObjectSpace os, String file_name) {
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(Tr));
                engine.Options.IgnoreFirstLines = 1;
                Tr[] records = (Tr[])engine.ReadFile(file_name);
                IList<fmCostItem> obj_refs = os.GetObjects<fmCostItem>();
                foreach (Tr rec in records) {
                    fmCostItem obj = obj_refs.FirstOrDefault(x => x.Code == rec.Code.Trim());
                    if (obj == null) {
                        obj = os.CreateObject<fmCostItem>();
                        obj_refs.Add(obj);
                        obj.Code = rec.Code.Trim();
                    }
                    //                    rec.Name = rec.Name.Trim();
                    //                    String new_name = rec.Name[0].ToString().ToUpper();
                    //                    new_name = new_name + rec.Name.Substring(1, rec.Name.Length - 1).ToLower();
                    obj.Name = rec.Name.Trim().ToLowerInvariant();
                    obj.IsSelectabled = true;
                }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.228"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                //
                FileInfo fi = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                String dir = fi.Directory.FullName + "\\Import\\";
                //
                new ClassificatorImporter<ReferecesRecord>().Import(os, dir + "fmCostItem.csv");
                //
                os.CommitChanges();
            }
        }

    }
}
