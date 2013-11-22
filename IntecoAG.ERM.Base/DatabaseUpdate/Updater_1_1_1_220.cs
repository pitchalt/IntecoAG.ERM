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
                var stage = deal_version.StageStructure.FirstStage.SubStagesCreate();
                stage.StageType = StageType.FINANCE;
                stage.Code = "1"; 
//                stage. = "Ведомость";
                var delivery_plan = stage.DeliveryPlan;
                stage.DeliveryMethod = deal_version.DeliveryMethod;
                stage.DeliveryPlan = deal_version.DeliveryPlan;
                deal_version.DeliveryPlan.Stage = stage;
                deal_version.DeliveryPlan.CurrentCost.UpCol = delivery_plan.CurrentCost.UpCol;
                delivery_plan.Stage = null;
                delivery_plan.CurrentCost.UpCol = null;
                deal_version.DeliveryPlan = null;
                os.Delete(delivery_plan);
                //
                var payment_plan = stage.PaymentPlan;
                stage.PaymentMethod = deal_version.PaymentMethod;
                stage.PaymentPlan = deal_version.PaymentPlan;
                deal_version.PaymentPlan.Stage = stage;
                deal_version.PaymentPlan.CurrentCost.UpCol = payment_plan.CurrentCost.UpCol;
                payment_plan.Stage = null;
                payment_plan.CurrentCost.UpCol = null;
                deal_version.PaymentPlan = null;
                os.Delete(payment_plan);
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
