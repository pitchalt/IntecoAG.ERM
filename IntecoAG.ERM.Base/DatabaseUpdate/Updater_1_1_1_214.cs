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
    public class Updater_1_1_1_214 : ModuleUpdater {
        public Updater_1_1_1_214(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        private void UpdateContractDeals() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                crmUserParty cfr = os.GetObjects<crmUserParty>().FirstOrDefault(x => x.Party.Code == "2518");
                cfr.TrwCode = "15";
                IList<crmContractDeal> deals = os.GetObjects<crmContractDeal>();
                foreach (crmContractDeal deal in deals) {
                    if (deal.Current != null) {
                        if (deal.Current.Customer != null) {
                            deal.Current.Customer.ContractDeal = deal;
                            deal.Current.Customer.TrwContractPartyType = TrwContractPartyType.PARTY_CUSTOMER;
                            if (cfr.Party == deal.Current.Customer.Party)
                                deal.Current.Customer.CfrUserParty = cfr;
                        }
                        if (deal.Current.Supplier != null) {
                            deal.Current.Supplier.ContractDeal = deal;
                            deal.Current.Supplier.TrwContractPartyType = TrwContractPartyType.PARTY_SUPPLIER;
                            if (cfr.Party == deal.Current.Supplier.Party)
                                deal.Current.Supplier.CfrUserParty = cfr;
                        }
                    }
                }
                os.CommitChanges();
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion == new Version("1.1.1.214"))
                UpdateContractDeals();
        }

        //void imp_party_type_ProcessRecordEvent(object sender, Updater_1_1_1_202.ClassificatorImporter<TrwPartyType, Updater_1_1_1_202.SimpleAnalyticRecord>.ProcessRecordEventArgs e) {
        //    e.CurrentObject.TrwType = (TrwPartyTypeType)Int32.Parse(e.CurrentObject.Code);
        //}
    }
}
