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
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.FinAccount;
//
namespace IntecoAG.ERM.FM {
    public class UpdaterDB_1_1_1_220 : ModuleUpdater {
        public UpdaterDB_1_1_1_220(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        [DelimitedRecord(";")]
        public class fmOrderImport {
            public Int32 Oid;
            public Int16 OgCode;
            public String Code;
            public String IsClosed;
            public String AccType;
            public String AccBuhCode;
        }

        [DelimitedRecord(";")]
        public class FAAccountImport {
            public String BsCode;
            public String SsCode;
            public String BuhCode;
            public String NameShort;
            public String NameFull;
        }

        public class FAAccountImporter {
            public void Import(IObjectSpace os, String acc_sys_code, String file_name) {
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(FAAccountImport));
                FAAccountImport[] records = (FAAccountImport[])engine.ReadFile(file_name);
                fmCFAAccountSystem acc_sys = os.GetObjects<fmCFAAccountSystem>(new BinaryOperator("Code", acc_sys_code), true).FirstOrDefault();
                if (acc_sys == null) {
                    acc_sys = os.CreateObject<fmCFAAccountSystem>();
                    acc_sys.Code = "1000";
                    acc_sys.Name = "Áóõãàëòåðèÿ";
                }
                foreach (FAAccountImport rec in records) {
                    fmCFAAccount bs_acc = acc_sys.Accounts.Where(x => x.Code == rec.BsCode).FirstOrDefault();
                    if (bs_acc == null) {
                        bs_acc = os.CreateObject<fmCFAAccount>();
                        bs_acc.Code = rec.BsCode;
                        acc_sys.Accounts.Add(bs_acc);
                    }
                    if (rec.SsCode == "0000") {
                        bs_acc.Name = rec.NameShort;
                        bs_acc.NameFull = rec.NameFull;
                    }
                    else {
                        fmCFAAccount ss_acc = acc_sys.Accounts.Where(x => x.Code == rec.SsCode).FirstOrDefault();
                        if (ss_acc == null) {
                            ss_acc = os.CreateObject<fmCFAAccount>();
                            ss_acc.Code = rec.SsCode;
                            bs_acc.SubAccounts.Add(ss_acc);
                        }
                        ss_acc.BuhCode = rec.BuhCode;
                        if (rec.NameShort.Length > 60)
                            ss_acc.Name = rec.NameShort.Substring(0, 60);
                        else
                            ss_acc.Name = rec.NameShort;
                        ss_acc.NameFull = rec.NameFull;
                        ss_acc.IsSelectabled = true;
                    }
                }
            }
        }

        public class fmOrderImporter {
            public void Import(IObjectSpace os, String file_name) {
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(fmOrderImport));
                fmOrderImport[] records = (fmOrderImport[])engine.ReadFile(file_name);
                IList<fmCOrderExt> orders = os.GetObjects<fmCOrderExt>(null, true);
                fmCFAAccountSystem acc_sys = os.GetObjects<fmCFAAccountSystem>(new BinaryOperator("Code", "1000"), true).FirstOrDefault();
                IList<fmÑOrderAnalitycAccouterType> acc_types = os.GetObjects<fmÑOrderAnalitycAccouterType>();
                foreach (fmOrderImport rec in records) {
                    fmCOrderExt order = orders.FirstOrDefault(item => item.Code == rec.Code);
                    if (order == null) {
                        Trace.TraceWarning("Updater_1.1.1.220_OrderImport: Order >" + rec.Code + "< not found");
                        order = os.CreateObject<fmCOrderExt>();
                        order.Code = rec.Code;
                        if (rec.IsClosed != "X") 
                            order.Status = fmIOrderStatus.Loaded;
                        else
                            order.Status = fmIOrderStatus.FinClosed;
                        orders.Add(order);
                    }
                    order.BuhIntNum = rec.Oid;
                    if (rec.IsClosed != "X") {
                        if (order.Status == fmIOrderStatus.Project || order.Status == fmIOrderStatus.FinOpened) {
                            order.Status = fmIOrderStatus.Opened;
                            order.IsClosed = false;
                        }
                    }
                    else {
                        if (order.Status == fmIOrderStatus.Project || order.Status == fmIOrderStatus.FinClosed) {
                            order.Status = fmIOrderStatus.Closed;
                            order.IsClosed = true;
                        }
                    }
                    if (!String.IsNullOrEmpty(rec.AccType) && rec.AccType != "0") {
                        order.AnalitycAccouterType = acc_types.FirstOrDefault(x => x.Code == rec.AccType);
                    }
                    if (!String.IsNullOrEmpty(rec.AccBuhCode) && rec.AccBuhCode != "0") {
                        order.BuhAccount = acc_sys.Accounts.FirstOrDefault(x => x.BuhCode == rec.AccBuhCode);
                        if (order.BuhAccount == null)
                        Trace.TraceWarning("Updater_1.1.1.220_OrderImport: Order >" + rec.Code + "< Account: >" + rec.AccBuhCode + "< not found");
                    }
                }
                foreach (fmCOrderExt order in orders) {
                    fmOrderImport order_record = records.FirstOrDefault(x => x.Code == order.Code);
                    if (order.Status != fmIOrderStatus.Project && 
                        order.Status != fmIOrderStatus.FinOpened && order_record == null) {
                        order.Status = fmIOrderStatus.Deleting;
                        Trace.TraceWarning("Updater_1.1.1.220_OrderImport: Delete order >" + order.Code + "< Status: " + order.Status.ToString()  );
                    }
                    if (order.Status != fmIOrderStatus.Deleting && order.Status != fmIOrderStatus.Project &&
                        order.Status != fmIOrderStatus.FinOpened && order.Status != fmIOrderStatus.FinClosed &&
                        order.Status != fmIOrderStatus.Opened && order.Status != fmIOrderStatus.Closed) {
                            order.Status = fmIOrderStatus.Loaded;
                    }
                }
            }
        }

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
            [FieldConverter(ConverterKind.Decimal,",")]
            public Decimal? SummCKBM;
//            [FieldNullValue((Decimal)0)]
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? SummOZM;
//            [FieldNullValue((Decimal)0)]
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? SummOrion;
//            [FieldNullValue((Decimal)0)]
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? SummPersonalContract;
//            [FieldNullValue((Decimal)0)]
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? SummOther;
//            [FieldNullValue((Decimal)0)]
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? SummAll;
        }

        public class FinIndexItemImporter {
            public void Import(IObjectSpace os, String file_name) {
//               CsvOptions options = new CsvOptions(typeof(FinIndexItemImport).Name, ';', 6);
//               options.DecimalSeparator = ",";
//               CsvEngine engine = new CsvEngine(options);
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(FinIndexItemImport));
                FinIndexItemImport[] records = (FinIndexItemImport[])engine.ReadFile(file_name);
                IList<fmCFinIndex> indexes = os.GetObjects<fmCFinIndex>(null, true);
                fmCOrderExt order = null;
                foreach (FinIndexItemImport rec in records) {
                    fmCFinIndex index = indexes.FirstOrDefault(x => x.Code == rec.IndexCode);
//                        os.GetObjects<fmCFinIndex>(new BinaryOperator("Code", rec.IndexCode),true).FirstOrDefault();
                    if (index == null)
                        throw new InvalidDataException("Unknow Index: " + rec.IndexCode);
                    if (order == null || order.Code != rec.OrderCode)
                        order = os.GetObjects<fmCOrderExt>(new BinaryOperator("Code", rec.OrderCode),true).FirstOrDefault();
                    if (order == null) {
//                        DialogResult result = MessageBox.Show("Unknow Order: " + rec.OrderCode);
//                        throw new InvalidDataException("Unknow Order: " + rec.OrderCode);
                        continue;
                    }
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
                    item.SummKB = (Decimal)rec.SummCKBM * 1000;
                    item.SummOZM = (Decimal)rec.SummOZM * 1000;
                    item.SummOrion = (Decimal) rec.SummOrion * 1000;
                    item.SummPersonalContract = (Decimal) rec.SummPersonalContract * 1000;
                    item.SummOther = (Decimal) rec.SummOther * 1000;
                    if (item.SummKB + item.SummOZM + item.SummOrion + item.SummPersonalContract + item.SummOther != rec.SummAll * 1000)
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
                var imp_fiv = new FAAccountImporter();
                imp_fiv.Import(os, "1000", dir + "fmCFAAccount.csv");
                os.CommitChanges();
            }
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                var imp_fiv = new fmOrderImporter();
                imp_fiv.Import(os, dir + "fmCOrderList.csv");
                os.CommitChanges();
            }
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                var imp_ot = new SimpleAnalyticImporter<fmCFinIndex>();
                imp_ot.Import(os, dir + "fmCFinIndex.csv");
                var imp_fiv = new FinIndexItemImporter();
                imp_fiv.Import(os, dir + "fmCFinIndexItem.csv");
                os.CommitChanges();
            }

            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                foreach (fmCOrderExt order in os.GetObjects<fmCOrderExt>(null, true)) {
                    order.IsSyncRequired = false;
                }
                os.CommitChanges();
            }
            //ObjectSpace.CommitChanges();
        }
    }
}
