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
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_225 : ModuleUpdater {
        public Updater_1_1_1_225(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        [DelimitedRecord(";")]
        public class ReferecesRecord {
            public String Code;
            public String Name;
        }


        public class ClassificatorImporter<Tr> 
            where Tr : ReferecesRecord {

            public void Import(IObjectSpace os, String file_name) { 
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(Tr));
                engine.Options.IgnoreFirstLines = 1;
                Tr[] records = (Tr[])engine.ReadFile(file_name);
                IList<TrwRefCashFlow> obj_refs = os.GetObjects<TrwRefCashFlow>();
                foreach (Tr rec in records) {
                    TrwRefCashFlow obj = obj_refs.FirstOrDefault(x => x.Code == rec.Code.Trim());
                    if (obj == null) {
                        obj = os.CreateObject<TrwRefCashFlow>();
                        obj_refs.Add(obj);
                        obj.Code = rec.Code.Trim();
                    }
                    obj.Name = rec.Name.Trim();
                    obj.NameFull = rec.Name.Trim();
                }
                foreach (TrwRefCashFlow obj_ref in obj_refs) { 
                    String [] comp = obj_ref.Code.Split('.');
                    if (comp.Length > 1) {
                        String[] tops_comp = new String[comp.Length - 1];
                        for (int i = 0; i < tops_comp.Length; i++)
                            tops_comp[i] = comp[i];
                        String top_code = String.Join(".", tops_comp);
                        TrwRefCashFlow top_obj = obj_refs.FirstOrDefault(x => x.Code == top_code);
                        top_obj.Childs.Add(obj_ref);
                    }
                }
                foreach (TrwRefCashFlow obj_ref in obj_refs) {
                    if (obj_ref.Childs.Count == 0)
                        obj_ref.IsSelectabled = true;
                }
            }
        }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.225"))
                return;
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.225"))
                return;
            //
            FileInfo fi = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String dir = fi.Directory.FullName+"\\Import\\";
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                var references = new ClassificatorImporter<ReferecesRecord>();
                references.Import(os, dir + "TrwRefCashFlow.csv");
                os.CommitChanges();
            }
        }

    }
}
