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
    public class Updater_1_1_1_222 : ModuleUpdater {
        public Updater_1_1_1_222(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }


        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.222"))
                return;
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<crmDealWithoutStage> deals = os.GetObjects<crmDealWithoutStage>();

                foreach (crmDealWithoutStage deal in deals) {
                    if (deal.Current == null || deal.Current.StageStructure == null)
                        continue;
                    deal.Current.StageStructure.FirstStage.Code = "бед";
                }

                os.CommitChanges();
            }
        }

    }
}
