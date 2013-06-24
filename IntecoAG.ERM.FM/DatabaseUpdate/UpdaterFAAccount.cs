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
//            ObjectSpace.CommitChanges();
        }
    }
}
