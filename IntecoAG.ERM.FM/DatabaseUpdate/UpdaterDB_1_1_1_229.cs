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
using IntecoAG.ERM.FM.Subject;
//
namespace IntecoAG.ERM.FM {
    public class UpdaterDB_1_1_1_229 : ModuleUpdater {
        public UpdaterDB_1_1_1_229(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.229"))
                return;
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<fmCDirection> directions = os.GetObjects<fmCDirection>(null, true);
                Int16 nextdir = 0;
                foreach (fmCDirection direction in directions) {
                    nextdir++;
                    direction.TrwCode = nextdir.ToString("D2");
                    foreach (fmCSubjectExt subject in direction.Subjects) {
                        subject.TrwCodeNew();
                    }
                }
                os.CommitChanges();
            }
        }

    }
}
