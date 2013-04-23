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

    public class UpdaterFmAnalitic02 : ModuleUpdater {
        public UpdaterFmAnalitic02(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            if (this.CurrentDBVersion > new Version("1.1.1.220"))   // Поправить на правильный номер!
                return;

            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                foreach (fmCOrderExt order in os.GetObjects<fmCOrderExt>(null, true)) {
                    if (order.DateEnd < gfmCAnalytic.DateMinValue)
                        order.IsPeriodUnlimited = true;
                    if (order.Status == fmIOrderStatus.BuhClosed)
                        order.Status = fmIOrderStatus.FinClosed;
                    if (order.Status == fmIOrderStatus.BuhOpened ||
                        order.Status == fmIOrderStatus.FinOpened ||
                        order.Status == fmIOrderStatus.Project)
                        order.Status = fmIOrderStatus.Loaded;
                    if (order.Status == fmIOrderStatus.FinClosed)
                        order.IsClosed = true;
                    if (order.OverheadType != fmIOrderOverheadType.Standart) {
                        order.OverheadType = fmIOrderOverheadType.Individual;
                        order.FixKoeff = order.KoeffKB;
                        order.FixKoeffOZM = order.KoeffOZM;
                    }
                }
                os.CommitChanges();
            }
       }
    }

}
