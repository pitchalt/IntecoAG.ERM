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
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.Trw.Party;
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_202 : ModuleUpdater {
        public Updater_1_1_1_202(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        [DelimitedRecord(";")]
        public class SimpleAnalyticRecord {
            public String Code;
            public String Name;
        }


        public class ClassificatorImporter<To, Tr> 
            where To : csCCodedComponent 
            where Tr : SimpleAnalyticRecord {
            public class ProcessRecordEventArgs {
                public ProcessRecordEventArgs(IObjectSpace os, To obj, Tr rec) {
                    ObjectSpace = os;
                    CurrentObject = obj;
                    CurrentRecord = rec;
                } 

                public IObjectSpace ObjectSpace;
                public To CurrentObject;
                public Tr CurrentRecord;
            }
            public delegate void ProcessRecordEventHandler(object sender, ProcessRecordEventArgs e);
            public event ProcessRecordEventHandler ProcessRecordEvent; 

            public void Import(IObjectSpace os, String file_name) { 
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(Tr));
                Tr[] records = (Tr[])engine.ReadFile(file_name);
                foreach (Tr rec in records) { 
                    To obj = os.GetObjects<To>(new BinaryOperator("Code", rec.Code)).FirstOrDefault();
                    if (obj == default(To))
                        obj = os.CreateObject<To>();
                    obj.Code = rec.Code;
                    obj.Name = rec.Name;
                    if (ProcessRecordEvent != null)
                        ProcessRecordEvent(this, new ProcessRecordEventArgs(os, obj, rec));
                }
            }
        }
        
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.202"))
                return;
            //
            FileInfo fi = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String dir = fi.Directory.FullName+"\\Import\\";
            //
            //using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
            //    var imp_party_type = new ClassificatorImporter<TrwPartyType, SimpleAnalyticRecord>();
            //    imp_party_type.ProcessRecordEvent += new ClassificatorImporter<TrwPartyType, SimpleAnalyticRecord>.ProcessRecordEventHandler(imp_party_type_ProcessRecordEvent);
            //    imp_party_type.Import(os, dir + "TrwPartyType.csv");
            //    os.CommitChanges();
            //    var imp_party_market = new ClassificatorImporter<TrwPartyMarket, SimpleAnalyticRecord>();
            //    imp_party_market.Import(os, dir + "TrwPartyMarket.csv");
            //    os.CommitChanges();
            //}
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<csCountry> countrys = os.GetObjects<csCountry>();
                foreach (csCountry country in countrys) {
                    if (country.NameRuShortLow == "Россия") {
                        country.IsUIG = false;
                        country.IsVED = false;
                    }
                    else {
                        if (country.NameRuShortLow == "Казахстан" ||
                            country.NameRuShortLow == "Украина" ||
                            country.NameRuShortLow == "Белоруссия") {
                                country.IsUIG = true;
                                country.IsVED = true;
                        }
                        else {
                            country.IsUIG = false;
                            country.IsVED = true;
                        }
                    }
                }
                os.CommitChanges();
            }
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<csAddress> address = os.GetObjects<csAddress>();
                foreach (csAddress addr in address) {
                    if (!addr.IsEmpty) {
                        addr.City = addr.City != null ? addr.City.Trim() : String.Empty;
                        if (addr.City.StartsWith("г.") ||
                            addr.City.StartsWith("Г.") ||
                            addr.City.StartsWith("п.") ||
                            addr.City.StartsWith("П.") ) {
                                addr.CityType = addr.City.Substring(0,2).ToLower();
                                addr.City = addr.City.Substring(2, addr.City.Length - 2).Trim();
                        }
                        else {
                            addr.CityType = "г.";
                        } 
                    }
                }
                os.CommitChanges();
            }
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<crmCPerson> pers_list = os.GetObjects<crmCPerson>();
                foreach (crmCPerson pers in pers_list) {
                    pers.UpdateCalcFields();
                }
                os.CommitChanges();
            }
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<crmBankAccount> account_list = os.GetObjects<crmBankAccount>();
                foreach (crmBankAccount account in account_list) {
                    account.TrwAccountType = TrwAccountType.ACCOUNT_CURRENT;
                }
                os.CommitChanges();
            }
        }

        //void imp_party_type_ProcessRecordEvent(object sender, Updater_1_1_1_202.ClassificatorImporter<TrwPartyType, Updater_1_1_1_202.SimpleAnalyticRecord>.ProcessRecordEventArgs e) {
        //    e.CurrentObject.TrwType = (TrwPartyTypeType)Int32.Parse(e.CurrentObject.Code);
        //}
    }
}
