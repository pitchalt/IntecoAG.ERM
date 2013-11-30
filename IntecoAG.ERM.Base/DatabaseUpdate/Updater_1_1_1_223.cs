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
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.References;

namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_223 : ModuleUpdater {
        public Updater_1_1_1_223(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }


        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.223"))
                return;
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<TrwPeriod> periods = os.GetObjects<TrwPeriod>();

                foreach (TrwPeriod period in periods) {
                    period.InitPeriodValues();
                }

                os.CommitChanges();
            }
        }

    }
}
