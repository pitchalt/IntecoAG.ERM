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
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.FinIndex;
//
namespace IntecoAG.ERM.FM {
    public class UpdaterFinIndexes : ModuleUpdater {
        public UpdaterFinIndexes(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

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

        [DelimitedRecord(";")]
        public class FinIndexItemImport {
            public String OrderCode;
            public String IndexCode;
            public String IndexName;
//            [FieldNullValue(0.0)]
            public Decimal? SummCKBM;
//            [FieldNullValue((Decimal)0)]
            public Decimal? SummOZM;
//            [FieldNullValue((Decimal)0)]
            public Decimal? SummOrion;
//            [FieldNullValue((Decimal)0)]
            public Decimal? SummPersonalContract;
//            [FieldNullValue((Decimal)0)]
            public Decimal? SummOther;
//            [FieldNullValue((Decimal)0)]
            public Decimal? SummAll;
        }

        public class FinIndexItemImporter {
            public void Import(IObjectSpace os, String file_name) {
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(FinIndexItemImport));
                FinIndexItemImport[] records = (FinIndexItemImport[])engine.ReadFile(file_name);
                foreach (FinIndexItemImport rec in records) {
                    fmCFinIndex index = os.GetObjects<fmCFinIndex>(new BinaryOperator("Code", rec.IndexCode),true).FirstOrDefault();
                    if (index == null)
                        throw new InvalidDataException("Unknow Index: " + rec.IndexCode);
                    fmCOrderExt order = os.GetObjects<fmCOrderExt>(new BinaryOperator("Code", rec.OrderCode),true).FirstOrDefault();
                    if (order == null)
                        throw new InvalidDataException("Unknow Order: " + rec.OrderCode);
                    fmIFinIndexStructureItem item = order.FinIndexes.FirstOrDefault(x => x.FinIndex == index);
                    if (item == null)
                        item = order.FinIndexesCreateItem(index);
                    if (rec.SummCKBM == null)
                        rec.SummCKBM = 0;
                    if (rec.SummOZM == null)
                        rec.SummOZM = 0;
                    if (rec.SummOrion == null)
                        rec.SummOrion = 0;
                    if (rec.SummPersonalContract == null)
                        rec.SummPersonalContract = 0;
                    if (rec.SummOther == null)
                        rec.SummOther = 0;
                    if (rec.SummAll == null)
                        rec.SummOther = 0;
                    item.SummKB = (Decimal)rec.SummCKBM;
                    item.SummOZM = (Decimal)rec.SummOZM;
                    item.SummOrion = (Decimal) rec.SummOrion;
                    item.SummPersonalContract = (Decimal) rec.SummPersonalContract;
                    item.SummOther = (Decimal) rec.SummOther;
                    if (item.SummKB + item.SummOZM + item.SummOrion + item.SummPersonalContract + item.SummOther != rec.SummAll)
                        throw new InvalidDataException("Order: " + order.Code + " Index: " + index.Code + " All summ" + rec.SummAll.ToString());

                }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.220"))
                return;
            //
            FileInfo fi = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String dir = fi.Directory.FullName+"\\Import\\";
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                var imp_ot = new SimpleAnalyticImporter<fmCFinIndex>();
                imp_ot.Import(os, dir + "fmCFinIndex.csv");
                var imp_fiv = new FinIndexItemImporter();
                imp_fiv.Import(os, dir + "fmCFinIndexItem.csv");
                os.CommitChanges();
            }
            ObjectSpace.CommitChanges();
        }
    }
}
