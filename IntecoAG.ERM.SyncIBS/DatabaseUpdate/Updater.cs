using System;
using System.Security.Principal;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.FM.StatementAccount;

namespace IntecoAG.ERM.Sync.SyncIBS.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
        }
    }
}
