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
            if (this.CurrentDBVersion != new Version("0.0.0.0"))
                return;

            if (this.CurrentDBVersion > new Version("1.1.1.220"))   // Поправить на правильный номер!
                return;

            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                foreach (fmCOrderExt order in os.GetObjects<fmCOrderExt>(null, true)) {
                    if (order.DateEnd < gfmCAnalytic.DateMinValue)
                        order.IsPeriodUnlimited = true;
                    if (order.Status == fmIOrderStatus.Closed)
                        order.Status = fmIOrderStatus.FinClosed;
                    if (order.Status == fmIOrderStatus.Opened ||
                        order.Status == fmIOrderStatus.FinOpened ||
                        order.Status == fmIOrderStatus.Project)
                        order.Status = fmIOrderStatus.Loaded;
                    if (order.IsClosed)
                        order.Status = fmIOrderStatus.FinClosed;
                    if (order.Status == fmIOrderStatus.Loaded)
                        order.OverheadType = fmIOrderOverheadType.Standart;
                    else {
                        order.OverheadType = fmIOrderOverheadType.Individual;
                        order.FixKoeff = order.KoeffKB;
                        order.FixKoeffOZM = order.KoeffOZM;
                        if (order.FixKoeff == -1) {
                            order.PlanOverheadType = fmIOrderOverheadValueType.NO_OVERHEAD;
                            order.BuhOverheadType = fmIOrderOverheadValueType.NO_OVERHEAD;
                        }
                        else if (order.FixKoeff == 0 && order.FixKoeffOZM == 0) {
                            order.PlanOverheadType = fmIOrderOverheadValueType.VARIABLE;
                            order.BuhOverheadType = fmIOrderOverheadValueType.VARIABLE;
                        }
                        else {
                            order.PlanOverheadType = fmIOrderOverheadValueType.FIX_NPO;
                            order.BuhOverheadType = fmIOrderOverheadValueType.FIX_NPO;
                        }
                    }
                    if (order.Code.Length == 4)
                        order.Code = "00" + order.Code;
                    //order.BuhAccountCode = order.BuhAccount;
                    if (order.Subject != null &&
                        order.Subject.AnalitycAVT != null &&
                        order.AnalitycAVT != null &&
                        order.Subject.AnalitycAVT.Code == "Э" &&
                        order.AnalitycAVT.Code == "О") {
                        order.AnalitycAVT = order.Subject.AnalitycAVT;
                    }
                }
                os.CommitChanges();
            }
        }
    }

}
