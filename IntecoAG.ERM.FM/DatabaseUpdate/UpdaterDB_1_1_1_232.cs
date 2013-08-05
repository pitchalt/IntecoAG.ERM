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
//
namespace IntecoAG.ERM.FM {
    public class UpdaterDB_1_1_1_232 : ModuleUpdater {
        public UpdaterDB_1_1_1_232(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        [DelimitedRecord(";")]
        public class SimpleAnalyticRecord {
            public String Code;
            public String Name;
        }


        public class NomenclatureImporter<Tr>
            where Tr : SimpleAnalyticRecord {
            public class ProcessRecordEventArgs {
                public ProcessRecordEventArgs(IObjectSpace os, csService obj, Tr rec) {
                    ObjectSpace = os;
                    CurrentObject = obj;
                    CurrentRecord = rec;
                }

                public IObjectSpace ObjectSpace;
                public csService CurrentObject;
                public Tr CurrentRecord;
            }
            public delegate void ProcessRecordEventHandler(object sender, ProcessRecordEventArgs e);
            public event ProcessRecordEventHandler ProcessRecordEvent;

            public void Import(IObjectSpace os, String file_name) {
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(Tr));
                Tr[] records = (Tr[])engine.ReadFile(file_name);
                foreach (Tr rec in records) {
                    csService obj = os.GetObjects<csService>(new BinaryOperator("Code", rec.Code)).FirstOrDefault();
                    if (obj == default(csService))
                        obj = os.CreateObject<csService>();
                    obj.Code = rec.Code;
                    obj.NameShort = rec.Name;
                    if (ProcessRecordEvent != null)
                        ProcessRecordEvent(this, new ProcessRecordEventArgs(os, obj, rec));
                }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.232"))
                return;
            FileInfo fi = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String dir = fi.Directory.FullName + "\\Import\\";
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                var service_importer = new NomenclatureImporter<SimpleAnalyticRecord>();
                service_importer.Import(os, dir + "csNomWork.csv");

                //
                IList<fmCSubject> subjects = os.GetObjects<fmCSubject>();
                foreach (fmCSubject subj in subjects) { 
                    if (subj.SourceDeal != null)
                        subj.Deals.Add(subj.SourceDeal);
                }
                os.CommitChanges();
            }
        }

    }
}
