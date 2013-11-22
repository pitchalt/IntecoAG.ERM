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
using IntecoAG.ERM.Trw.References;
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_220 : ModuleUpdater {
        public Updater_1_1_1_220(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        [DelimitedRecord(";")]
        public class ReferecesRecord {
            public String RefCode;
            public String Code;
            public String Name;
        }


        public class ClassificatorImporter<Tr> 
            where Tr : ReferecesRecord {

            public void Import(IObjectSpace os, String file_name) { 
                DelimitedFileEngine engine = new DelimitedFileEngine(typeof(Tr));
                engine.Options.IgnoreFirstLines = 1;
                Tr[] records = (Tr[])engine.ReadFile(file_name);
                foreach (Tr rec in records) {
                    if (String.IsNullOrEmpty(rec.RefCode)) continue;
                    TrwRefBase obj = os.GetObjects<TrwRefBase>(
                        new BinaryOperator("Code", rec.Code) &
                        new BinaryOperator("RefCode", rec.RefCode)
                        ).FirstOrDefault();
                    if (obj == null)
                        obj = TrwRefBase.Create(os, rec.RefCode);
                    obj.Code = rec.Code;
                    obj.Name = rec.Name;
                }
            }
        }

        public void DealWhithoutStageUpdate(IObjectSpace os) {
            var deals = os.GetObjects<crmDealWithoutStage>();
            foreach (crmDealWithoutStage deal in deals) {
                crmDealWithoutStageVersion deal_version = (crmDealWithoutStageVersion) deal.Current;
                deal_version.StageStructureCreate();
                deal_version.StageStructure.Customer = deal_version.Customer;
                deal_version.StageStructure.Supplier = deal_version.Supplier;
                deal_version.StageStructure.FirstStage.DateBegin = deal_version.DateBegin;
                deal_version.StageStructure.FirstStage.DateEnd = deal_version.DateEnd;
                deal_version.StageStructure.FirstStage.DateFinish = deal_version.DateFinish;
                deal_version.StageStructure.FirstStage.Valuta = deal_version.Valuta;
                deal_version.StageStructure.FirstStage.PaymentValuta = deal_version.PaymentValuta;
                deal_version.StageStructure.FirstStage.NDSRate = deal_version.NDSRate;
                deal_version.StageStructure.FirstStage.Order = deal_version.Order;
                var stage = deal_version.StageStructure.FirstStage.SubStagesCreate(deal_version.DeliveryPlan, deal_version.PaymentPlan);
//                stage.StageType = StageType.FINANCE;
                stage.Code = "1"; 
//                stage. = "Ведомость";
                deal_version.DeliveryPlan = null;
                deal_version.PaymentPlan = null;
                deal_version.Advance = null;
                deal_version.Settlement = null;
            }
        }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
//            var result = ExecuteNonQueryCommand(
//                "UPDATE \"XPObjectType\" "+
//                "SET \"AssemblyName\"= 'IntecoAG.ERM.Base' "+
//                "WHERE \"AssemblyName\" = 'IntecoAG.ERM.Base.Impl' ; "
//            , false);
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
                var references = new ClassificatorImporter<ReferecesRecord>();
                references.Import(os, dir + "TrwReferences.csv");
                os.CommitChanges();
            }
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                DealWhithoutStageUpdate(os);
                os.CommitChanges();
            }
        }

    }
}
