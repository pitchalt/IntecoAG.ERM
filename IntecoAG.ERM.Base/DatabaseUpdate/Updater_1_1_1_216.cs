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
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw.Party;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_216 : ModuleUpdater {
        public Updater_1_1_1_216(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        private void UpdateContractDeals() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                crmUserParty cfr = os.GetObjects<crmUserParty>().FirstOrDefault(x => x.Party.Code == "2518");
                IList<TrwSaleNomenclature> sale_noms = os.GetObjects<TrwSaleNomenclature>();
                IList<crmContractDeal> deals = os.GetObjects<crmContractDeal>();
                foreach (crmContractDeal deal in deals) {
                    if (deal.Current != null) {
                        if (deal.Current.Customer != null && deal.Current.Supplier != null) {
                            if (deal.Current.Customer.CfrUserParty != null && deal.Current.Supplier.CfrUserParty == null) {
                                deal.Current.Customer.CfrUserParty = null;
                                deal.Current.Supplier.CfrUserParty = cfr;
                            }
                            else if (deal.Current.Customer.CfrUserParty == null && deal.Current.Supplier.CfrUserParty != null) {
                                deal.Current.Customer.CfrUserParty = cfr;
                                deal.Current.Supplier.CfrUserParty = null;
                            }
                        }
                    }
                    crmDealWithStage deal_ws = deal as crmDealWithStage;
                    if (deal_ws != null) {
                        crmDealWithStageVersion deal_ws_version = deal_ws.Current as crmDealWithStageVersion;
                        if (deal_ws_version == null) continue;
                        foreach (crmStage stage in deal_ws_version.StageStructure.Stages) {
                            if (stage.DeliveryUnits == null) continue;
                            foreach (crmDeliveryUnit unit in stage.DeliveryUnits) {
                                foreach (crmDeliveryItem item in unit.DeliveryItems) {
                                    if (item.Nomenclature == null) continue;
                                    if (item.Order == null || item.Nomenclature == null) continue;
                                    TrwSaleNomenclature nom = sale_noms.FirstOrDefault(x => x.Order == item.Order && x.Nomenclature == item.Nomenclature);
                                    if (nom == null) {
                                        nom = os.CreateObject<TrwSaleNomenclature>();
                                        nom.Order = item.Order;
                                        nom.Nomenclature = item.Nomenclature;
                                    }
                                    nom.TrwCodeSet(nom.Order.Code + ".9" + nom.Nomenclature.Code);
                                    foreach (TrwOrder trw_order in deal_ws.TrwOrders) {
                                        if (trw_order.Subject == nom.Order.Subject) {
                                            trw_order.TrwSaleNomenclatures.Add(nom);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                os.CommitChanges();
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion == new Version("1.1.1.216"))
                UpdateContractDeals();
        }

        //void imp_party_type_ProcessRecordEvent(object sender, Updater_1_1_1_202.ClassificatorImporter<TrwPartyType, Updater_1_1_1_202.SimpleAnalyticRecord>.ProcessRecordEventArgs e) {
        //    e.CurrentObject.TrwType = (TrwPartyTypeType)Int32.Parse(e.CurrentObject.Code);
        //}
    }
}
