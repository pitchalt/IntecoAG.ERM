using System;
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
using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class AVTInvoiceAnalyticUpdater : ModuleUpdater {
        public AVTInvoiceAnalyticUpdater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        [DelimitedRecord(";")]
        public class SimpleAnalyticImport {
            public String Code;
            public String Name;
        }

        public class SimpleAnalyticImporter<T> where T : csCCodedComponent {
            public void Import(IObjectSpace os, String file_name) { 
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(SimpleAnalyticImport));
                SimpleAnalyticImport[] records = (SimpleAnalyticImport[])engine.ReadFile(file_name);
                foreach (SimpleAnalyticImport rec in records) { 
                    T obj = os.GetObjects<T>(new BinaryOperator("Code", rec.Code)).FirstOrDefault();
                    if (obj == default(T))
                        obj = os.CreateObject<T>();
                    obj.Code = rec.Code;
                    obj.Name = rec.Name;
                }
            }
        }
        
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            FileInfo fi = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String dir = fi.Directory.FullName+"\\Import\\";
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                var imp_ot = new SimpleAnalyticImporter<fmCAVTInvoiceOperationType>();
                imp_ot.Import(os, dir + "fmCAVTInvoiceOperationType.csv");
                os.CommitChanges();
                var imp_tt = new SimpleAnalyticImporter<fmCAVTInvoiceTransferType>();
                imp_tt.Import(os, dir + "fmCAVTInvoiceTransferType.csv");
                os.CommitChanges();
            }
            ObjectSpace.CommitChanges();
        }
    }
}
