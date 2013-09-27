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
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Party;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_218 : ModuleUpdater {
        public Updater_1_1_1_218(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        private void UpdateOrders() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<fmCSubject> subjs = os.GetObjects<fmCSubject>();
                foreach (fmCSubject subj in subjs) {
                    if (subj.Direction != null)
                        subj.TrwCodeReNumber();
                    foreach (crmContractDeal deal in subj.Deals) {
                        subj.DealsAdd(deal);
                    }
                }
                IList<TrwOrder> trw_orders = os.GetObjects<TrwOrder>();
                foreach (TrwOrder trw_order in trw_orders) {
                    if (trw_order.TrwInternalNumber == 0) {
                        Int32 int_num = 0;
                        if (Int32.TryParse(trw_order.TrwCode.Split('/')[1], out int_num))
                            trw_order.TrwInternalNumberSet(int_num);
                    }
                    trw_order.UpdatePropertys();
                }
                os.CommitChanges();
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion == new Version("1.1.1.218"))
                UpdateOrders();
        }

    }
}
