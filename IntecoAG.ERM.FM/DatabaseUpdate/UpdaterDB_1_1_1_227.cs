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
    public class UpdaterDB_1_1_1_227 : ModuleUpdater {
        public UpdaterDB_1_1_1_227(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.227"))
                return;
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                foreach (fmCOrderExt order in os.GetObjects<fmCOrderExt>(null, true)) {
                    if (order.IBSSystemsProtect == null)
                        order.IBSSystemsProtect = os.CreateObject<fmCOrderIBSSystemsProtect>();
                }
                os.CommitChanges();
            }
        }
    }
}
