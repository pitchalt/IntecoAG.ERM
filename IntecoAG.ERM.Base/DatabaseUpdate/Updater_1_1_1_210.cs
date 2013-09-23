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

using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Party;
using IntecoAG.ERM.Trw.Contract;
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_210 : ModuleUpdater {
        public Updater_1_1_1_210(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        private void UpdateTrwDeals() {
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<crmContractDeal> deals = os.GetObjects<crmContractDeal>();
                foreach (crmContractDeal deal in deals) {
                    if (deal.Contract == null)
                        throw new InvalidDataException("Deal.Contract is Null");
                    if (deal.ContractKind == ContractKind.CONTRACT && deal.Contract.ContractDocument != deal.ContractDocument)
                        throw new InvalidDataException("Invalid Contract Document");
                }
                //
                IList<crmContractRegistrationLog> log_recs = os.GetObjects<crmContractRegistrationLog>();
                foreach (crmContractRegistrationLog log_rec in log_recs) {
                    if (log_rec.ContractDeal.Contract == null) {
                        log_rec.ContractDeal.State = DealStates.DEAL_DELETED;
                        continue;
                    }
                    if (log_rec.ContractDeal.ContractKind == ContractKind.CONTRACT) {
                        if (log_rec.ContractDeal.Contract.IntNumber == 0)
                            log_rec.ContractDeal.Contract.IntNumber = log_rec.ISN;
                        else
                            throw new InvalidDataException("Contract already has int number");
                    }
                    else { 
                    }
                }
                //
                RegistrationLogISNCounter int_counter = os.GetObjects<RegistrationLogISNCounter>().FirstOrDefault();
                //
                IList<crmContract> contracts = os.GetObjects<crmContract>();
                foreach (crmContract contract in contracts) {
                    if (contract.IntNumber == 0) {
                        int_counter.ISN++;
                        contract.IntNumber = int_counter.ISN;
                    }
                    crmContractDeal contract_deal = null;
                    Int32 FailNumber = 1;
                    foreach (crmContractDeal deal in contract.ContractDeals) {
                        if (deal.State == DealStates.DEAL_DELETED) 
                            continue;
                        if (deal.ContractKind == ContractKind.CONTRACT) {
                            contract.IntCurDocNumber++;
                            deal.IntNumber = contract.IntCurDocNumber;
                            if (contract_deal != null)
                                contract_deal.FailNumber = FailNumber++;
                            //                                throw new InvalidDataException("Dublicate Contract Deal");
                            contract_deal = deal;
                        }
                    }
                    if (contract_deal == null)
                        contract.FailNumber = 1;
                    foreach (crmContractDeal deal in contract.ContractDeals) {
                        if (deal.State == DealStates.DEAL_DELETED || deal.FailNumber != 0)
                            continue;
                        if (deal.ContractKind == ContractKind.ADDENDUM) {
                            contract.IntCurDocNumber++;
                            deal.IntNumber = contract.IntCurDocNumber;
                            Int32 cur_fail_number = 1;
                            crmContractDeal cur_deal = deal;
                            foreach (crmContractDeal sub_deal in contract.ContractDeals) {
                                if (sub_deal.ContractKind != ContractKind.ADDENDUM || cur_deal == sub_deal || sub_deal.FailNumber != 0)
                                    continue;
                                if (cur_deal.ContractDocument.Number == sub_deal.ContractDocument.Number) {
                                    cur_deal.FailNumber = cur_fail_number++;
                                    cur_deal = sub_deal;
                                }
                            }
                        }
                    }
                    //                        throw new InvalidDataException("Unknow Contract Deal");
                    foreach (crmContractDeal deal in contract.ContractDeals) {
                        if (deal.State == DealStates.DEAL_DELETED || deal.FailNumber != 0)
                            continue;
                        deal.UpdateTrwNumbers();
                    }
                }
                os.CommitChanges();
            }
        }

        private void UpdateTrwOrders() {
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<fmCSubject> subjects = os.GetObjects<fmCSubject>();
                foreach (fmCSubject subject in subjects) {
                    foreach (fmCOrder order in subject.Orders) {
                        if (order.SourceDeal != null) {
                            subject.Deals.Add(order.SourceDeal);
                        }
                    }
                    foreach (crmContractDeal deal in subject.Deals) {
                        if (deal.TRVType == null || deal.TRVType.TrwContractSuperType != TrwContractSuperType.DEAL_SALE)
                            continue;
                        TrwOrder cur_order = null;
                        foreach (TrwOrder trw_order in subject.TrwOrders) {
                            if (trw_order.Deal == deal)
                                cur_order = trw_order;
                        }
                        if (cur_order == null) {
                            cur_order = os.CreateObject<TrwOrder>();
                            cur_order.Subject = subject;
                            cur_order.Deal = deal;
                        }
                    }
                }
                os.CommitChanges();
            }

        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion == new Version("1.1.1.210"))
                UpdateTrwDeals();
            if (this.CurrentDBVersion == new Version("1.1.1.210") ||
                this.CurrentDBVersion == new Version("1.1.1.211"))
                UpdateTrwOrders();
        }

        //void imp_party_type_ProcessRecordEvent(object sender, Updater_1_1_1_202.ClassificatorImporter<TrwPartyType, Updater_1_1_1_202.SimpleAnalyticRecord>.ProcessRecordEventArgs e) {
        //    e.CurrentObject.TrwType = (TrwPartyTypeType)Int32.Parse(e.CurrentObject.Code);
        //}
    }
}
