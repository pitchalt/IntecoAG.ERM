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
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.Trw.Party;
//
namespace IntecoAG.ERM.FM {
    public class UpdaterDB_1_1_1_228 : ModuleUpdater {
        public UpdaterDB_1_1_1_228(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("1.1.1.228"))
                return;
            //
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<TrwPartyParty> trw_partys = os.GetObjects<TrwPartyParty>(null, true);
                TrwPartyParty trw_party;
                IList<crmContractDeal> deals = os.GetObjects<crmContractDeal>(null, true);
                foreach (crmContractDeal deal in deals) {
//                    crmCPerson person = deal.Customer.Person;
                    if (deal.Customer != null) {
                        trw_party = GetTrwParty(os, trw_partys, deal.Customer);
                        if (trw_party != null)
                            trw_party.IsDeal = true;
                    }
                    if (deal.Supplier != null) {
                        trw_party = GetTrwParty(os, trw_partys, deal.Supplier);
                        if (trw_party != null)
                            trw_party.IsDeal = true;
                    }
                }
                IList<fmCPRPaymentRequest> pays = os.GetObjects<fmCPRPaymentRequest>(
                    new BinaryOperator("Date", new DateTime(2013,01,01), BinaryOperatorType.GreaterOrEqual), true);
                foreach (fmCPRPaymentRequest pay in pays) {
                    if (pay.PartyPayCreditor != null) {
                        trw_party = GetTrwParty(os, trw_partys, pay.PartyPayCreditor);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                    }
                    if (pay.PartyPayDebitor != null) {
                        trw_party = GetTrwParty(os, trw_partys, pay.PartyPayDebitor);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                    }
                    if (pay.PartyPaySender != null) {
                        trw_party = GetTrwParty(os, trw_partys, pay.PartyPaySender);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                    }
                    if (pay.PartyPayReceiver != null) {
                        trw_party = GetTrwParty(os, trw_partys, pay.PartyPayReceiver);
                        if (trw_party != null)
                            trw_party.IsPay = true;
                    }
                                        
                }
                os.CommitChanges();
            }
        }

        protected TrwPartyParty GetTrwParty(IObjectSpace os, IList<TrwPartyParty> trw_partys, crmCParty party) {
            if (party.Person == null)
                return null;
            TrwPartyParty trw_party = trw_partys.FirstOrDefault(x => x.Person == party.Person);
            if (trw_party == null) {
                trw_party = TrwPartyParty.LocateTrwParty(os, party);
                trw_partys.Add(trw_party);
            }
            return trw_party;
        }
    }
}
