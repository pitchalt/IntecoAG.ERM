using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.Module
{
    public class Updater : ModuleUpdater
    {
        public Updater(ObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            #region ƒ¿ÕÕ€≈ ƒÀﬂ Œ¡⁄≈ “¿ "TimeUnit"

            CS.Measurement.TimeUnit TimeUnit1 = ObjectSpace.FindObject<CS.Measurement.TimeUnit>(new BinaryOperator("Name", "day"));
            if (TimeUnit1 == null) {
                TimeUnit1 = ObjectSpace.CreateObject<CS.Measurement.TimeUnit>();
                TimeUnit1.Name = "day";
                TimeUnit1.Save();
            }

            CS.Measurement.TimeUnit TimeUnit2 = ObjectSpace.FindObject<CS.Measurement.TimeUnit>(new BinaryOperator("Name", "month"));
            if (TimeUnit2 == null) {
                TimeUnit2 = ObjectSpace.CreateObject<CS.Measurement.TimeUnit>();
                TimeUnit2.Name = "month";
                TimeUnit2.Save();
            }

            CS.Measurement.TimeUnit TimeUnit3 = ObjectSpace.FindObject<CS.Measurement.TimeUnit>(new BinaryOperator("Name", "year"));
            if (TimeUnit3 == null) {
                TimeUnit3 = ObjectSpace.CreateObject<CS.Measurement.TimeUnit>();
                TimeUnit3.Name = "year";
                TimeUnit3.Save();
            }

            #endregion

            #region ƒ¿ÕÕ€≈ ƒÀﬂ Œ¡⁄≈ “¿ "Stage"
            /*
            CRM.Contract.DateTimeExt dteStd1 = ObjectSpace.CreateObject<CRM.Contract.DateTimeExt>();
            dteStd1.DateTime = System.DateTime.Now;
            CRM.Contract.Time tm1 = ObjectSpace.CreateObject<CRM.Contract.Time>();
            tm1.AbsoluteDateTime = dteStd1;


            CRM.Contract.Stage Stage1 = ObjectSpace.FindObject<CRM.Contract.Stage>(new BinaryOperator("Oid", 1));
            if (Stage1 == null) {
                Stage1 = ObjectSpace.CreateObject<CRM.Contract.Stage>();
                Stage1.DateBegin = tm1;
                Stage1.Save();
            }

            CRM.Contract.DateTimeExt dteStd2 = ObjectSpace.CreateObject<CRM.Contract.DateTimeExt>();
            dteStd1.TimeSingularity = CS.TimeSingularity.NegativeInfinity;
            CRM.Contract.Time tm2 = ObjectSpace.CreateObject<CRM.Contract.Time>();
            tm1.AbsoluteDateTime = dteStd2;

            CRM.Contract.Stage Stage2 = ObjectSpace.FindObject<CRM.Contract.Stage>(new BinaryOperator("Oid", 2));
            if (Stage2 == null) {
                Stage2 = ObjectSpace.CreateObject<CRM.Contract.Stage>();
                Stage2.DateBegin = tm2;
                Stage2.Save();
            }
*/
            #endregion

        }
    }
}
