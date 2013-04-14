using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
//
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM.DatabaseUpdate {

    public class UpdaterFmAnalitic01 : ModuleUpdater {
        public UpdaterFmAnalitic01(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            if (this.CurrentDBVersion > new Version("1.1.1.218"))   // Поправить на правильный номер!
                return;

            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                foreach (fmCDirection dir in os.GetObjects<fmCDirection>()) {
                    if (dir.DateEnd < gfmCAnalytic.DateMinValue)
                        dir.IsPeriodUnlimited = true;
                }
                foreach (fmCSubjectExt subj in new List<fmCSubjectExt>(os.GetObjects<fmCSubjectExt>())) {
                    if (subj.DateEnd < gfmCAnalytic.DateMinValue)
                        subj.IsPeriodUnlimited = true;
                    if (subj.Name.ToLower() == "удалить")
                        subj.Status = fmISubjectStatus.DELETE;
                    if (subj.Status == fmISubjectStatus.DELETE &&
                        subj.OrderExts.Count == 0)
                        os.Delete(subj);
                }
                foreach (fmCOrderExt order in os.GetObjects<fmCOrderExt>(null, true)) {
                    if (order.DateEnd < gfmCAnalytic.DateMinValue)
                        order.IsPeriodUnlimited = true;
                }
                os.CommitChanges();
            }
       }
    }

}
