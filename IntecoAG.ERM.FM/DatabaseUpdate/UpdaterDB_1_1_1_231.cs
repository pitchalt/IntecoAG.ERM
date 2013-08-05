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
    public class UpdaterDB_1_1_1_231 : ModuleUpdater {
        public UpdaterDB_1_1_1_231(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.231"))
                return;
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<fmCDirection> directions = os.GetObjects<fmCDirection>(null, true);
                foreach (fmCDirection direction in directions) {
                    switch (direction.Code) {
                        case " –":
                            direction.TrwCode = "01";
                            break;
                        case "– —Õ":
                            direction.TrwCode = "02";
                            break;
                        case " —":
                            direction.TrwCode = "03";
                            break;
                        case "»“":
                            direction.TrwCode = "04";
                            break;
                        case "Ã–":
                            direction.TrwCode = "05";
                            break;
                        case "√œ":
                            direction.TrwCode = "06";
                            break;
                        case "Œ«":
                            direction.TrwCode = "07";
                            break;
                        case "”—À":
                            direction.TrwCode = "08";
                            break;
                        case "œ“–":
                            direction.TrwCode = "11";
                            break;
                        case "Õ–":
                            direction.TrwCode = "12";
                            break;
                        case "—œ":
                            direction.TrwCode = "13";
                            break;
                        default:
                            throw new InvalidDataException();
                    }
                    direction.TrwSubjectNumberCurrent = 0;
                    foreach (fmCSubjectExt subject in direction.Subjects) {
                        subject.TrwCodeNew();
                    }
                }
                os.CommitChanges();
            }
        }

    }
}
