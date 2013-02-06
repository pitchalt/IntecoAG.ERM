using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.Data;
using DevExpress.Data.Filtering;
//using DevExpress.Persistent;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Npgsql;

using FileHelpers;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Imports {
    [DelimitedRecord(";")]
    public class ImportPartyRecord {
        public String Type;
        public String Code;
        public String INN;
        public String KPP;
        public String Name;
        public String Addr;
    }
    class ImportApplication : XafApplication {
        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            return null;
        }
    }
    class ImportParty {
        public void import(String filename) {
//            CsvEngine engine = new CsvEngine(new CsvOptions("ImportPartyRecord", ';',6));
//            engine.Options.IgnoreFirstLines = 1;
//            Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection("Server=npomash;User Id=pg_adm;Password='flesh*token=across';Database=ermdev;Encoding=UNICODE;");
//            IXpoDataStoreProvider dsp = new ConnectionDataStoreProvider(con);

//            IDbConnection con = PostgreSqlConnectionProvider.CreateConnection("Server=npomash;User Id=pg_adm;Password='flesh*token=across';Database=ermdev;Encoding=UNICODE;");
            IDbConnection con = PostgreSqlConnectionProvider.CreateConnection("Server=alt-dev;User Id=pg_adm;Password='flesh*token=across';Database=ermprod;Encoding=UNICODE;");
//
            IDataLayer data_layer = XpoDefault.GetDataLayer(con, AutoCreateOption.None);
            IObjectLayer object_layer = new SimpleObjectLayer(data_layer);
            Int32 count = 0;
//            ObjectSpaceProvider osp = new ObjectSpaceProvider(dsp);
            //
//            XafApplication application = new ImportApplication();
//            ModuleBase testModule = new ModuleBase();
//            testModule.AdditionalExportedTypes.Add(typeof(crmCParty));
//            testModule.AdditionalExportedTypes.Add(typeof(crmCBusinessman));
//            testModule.AdditionalExportedTypes.Add(typeof(crmCPhysicalParty));
//            testModule.AdditionalExportedTypes.Add(typeof(crmCLegalPerson));
//            application.Modules.Add(testModule);
//            application.Setup("ImportApplication", osp);
//            using (IObjectSpace os = application.CreateObjectSpace()) {
              using (UnitOfWork uow_base = new UnitOfWork(object_layer)) {
                DataTable tbl = (new CsvEngine("ImportPartyRecord", ';', 6)).ReadFileAsDT(filename);
                foreach (DataRow row in tbl.Rows) {
                    using (UnitOfWork uow = uow_base.BeginNestedUnitOfWork()) {
                        ImportPartyRecord rec = new ImportPartyRecord();
                        rec.Type = ((String)row.ItemArray[0]).Trim();
                        rec.Code = ((String)row.ItemArray[1]).Trim();
                        rec.INN = ((String)row.ItemArray[2]).Trim();
                        rec.KPP = ((String)row.ItemArray[3]).Trim();
                        rec.Name = ((String)row.ItemArray[4]).Trim();
                        rec.Addr = ((String)row.ItemArray[5]).Trim();
                        System.Console.WriteLine(rec.Type + "@" + rec.Code + "@" + rec.INN + "@" + rec.KPP + "@" + rec.Name + "@" + rec.Addr);
                        XPQuery<crmCParty> q_party = new XPQuery<crmCParty>(uow, true);
                        crmCParty party = q_party.FirstOrDefault(obj => obj.Code == rec.Code);
                        if (party != null) continue;
                        switch (rec.Type) {
                            case "ИП":
                                crmCBusinessman bm = new crmCBusinessman(uow);
                                bm.Code = rec.Code;
                                bm.Name = rec.Name;
                                bm.INN = rec.INN;
                                bm.AddressLegal.AddressHandmake = rec.Addr;
                                bm.AddressFact.AddressHandmake = rec.Addr;
                                break;
                            case "ФЛ":
                                crmCPhysicalParty phl = new crmCPhysicalParty(uow);
                                phl.Code = rec.Code;
                                phl.Name = rec.Name;
                                phl.INN = rec.INN;
                                phl.AddressLegal.AddressHandmake = rec.Addr;
                                phl.AddressFact.AddressHandmake = rec.Addr;
                                break;
                            default:
                                crmCLegalPerson lp = new crmCLegalPerson(uow);
                                lp.Code = rec.Code;
                                lp.Name = rec.Name;
                                lp.INN = rec.INN;
                                lp.KPP = rec.KPP;
                                lp.AddressLegal.AddressHandmake = rec.Addr;
                                lp.AddressFact.AddressHandmake = rec.Addr;
                                break;
                        }
                        uow.CommitChanges();
                        count++;
                    }
                }
                uow_base.CommitChanges();
                System.Console.WriteLine("All count: " + count);
            }
        }
    }
}
