using System;
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
using IntecoAG.ERM.FM.FinAccount;
//
namespace IntecoAG.ERM.FM {
    public class UpdaterFAAccount : ModuleUpdater {
        public UpdaterFAAccount(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

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
                    acc_sys.Name = "Бухгалтерия";
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
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
//            if (this.CurrentDBVersion == new Version("1.1.1.220"))
//                DropColumn("\"fmOrder\"", "\"BuhAccount\"");
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
//            ObjectSpace.CommitChanges();
        }
    }
}
