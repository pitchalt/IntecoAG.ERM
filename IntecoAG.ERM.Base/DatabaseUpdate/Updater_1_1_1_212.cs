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
using IntecoAG.ERM.CRM.Counters;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw.Party;
using IntecoAG.ERM.Trw.Contract;
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_212 : ModuleUpdater {
        public Updater_1_1_1_212(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }


        private void UpdateTrwOrders() {
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<fmCSubject> subjects = os.GetObjects<fmCSubject>();
                foreach (fmCSubject subject in subjects) {
                    subject.OrderNumberCurrent = subject.TrwOrders.Count;
                }
                os.CommitChanges();
            }

        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion == new Version("1.1.1.212"))
                UpdateTrwOrders();
        }

        //void imp_party_type_ProcessRecordEvent(object sender, Updater_1_1_1_202.ClassificatorImporter<TrwPartyType, Updater_1_1_1_202.SimpleAnalyticRecord>.ProcessRecordEventArgs e) {
        //    e.CurrentObject.TrwType = (TrwPartyTypeType)Int32.Parse(e.CurrentObject.Code);
        //}
    }
}
