using System;
using System.Security.Principal;
using System.Linq;
using System.IO;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM {
    public class UpdaterRequestAutoBinding : ModuleUpdater {
        public UpdaterRequestAutoBinding(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("0.0.0.0"))
                return;
            //

            IObjectSpace os = ObjectSpace;
            Session ssn = ((ObjectSpace)os).Session;

            // Заполнение кодов ценностей и курсов валют
            UpdaterVaultaCourse uvc = new UpdaterVaultaCourse(ssn);
            uvc.FixValutaCourseEtc();
            os.CommitChanges();

            if (this.CurrentDBVersion.ToString() != "1.1.1.107")
                return;

            XPQuery<fmAutoBindingUpdater> autoBindingUpdaters = new XPQuery<fmAutoBindingUpdater>(ssn);
            fmAutoBindingUpdater ABU = (from autoBindingUpdater in autoBindingUpdaters
                                        select autoBindingUpdater).FirstOrDefault();
            if (ABU == null) {
                ABU = os.CreateObject<fmAutoBindingUpdater>();
            }
            if (ABU.AutoBindingUpdater)
                return; // Updater уже исполнялся

            // Зачистка fmCPRRepaymentJurnal результатов предыдущей автоматической привязки
            XPQuery<fmCPRRepaymentJurnal> paymentJurnals = new XPQuery<fmCPRRepaymentJurnal>(ssn);
            var queryRJ = (from paymentJurnal in paymentJurnals
                            select paymentJurnal).ToList<fmCPRRepaymentJurnal>();

            ssn.Delete(queryRJ);
            //ssn.PurgeDeletedObjects();
            //os.CommitChanges();
            //ssn.ExecuteQuery("UPDATE \"fmPRRepaymentJurnal\" SET \"GCRecord\"=null;");

            // Зачистка fmCPRRepaymentTask результатов предыдущей автоматической привязки
            XPQuery<fmCPRRepaymentTask> repaymentTasks = new XPQuery<fmCPRRepaymentTask>(ssn);
            var queryRT = (from repaymentTask in repaymentTasks
                           select repaymentTask).ToList<fmCPRRepaymentTask>();

            ssn.Delete(queryRT);
            ssn.PurgeDeletedObjects();
            os.CommitChanges();

            // Возвращение PAYED в IN_PAYMENT
            XPQuery<fmCPRPaymentRequest> paymentRequests = new XPQuery<fmCPRPaymentRequest>(ssn, true);
            var queryPR = (from paymentRequest in paymentRequests
                            where paymentRequest.State == PaymentRequestStates.PAYED
                            select paymentRequest).ToList<fmCPRPaymentRequest>();
            foreach (var pr in queryPR) {
                pr.State = PaymentRequestStates.IN_PAYMENT;
            }
            os.CommitChanges();

            // "Наша" организация (она там одна, отсев не нужен)
            crmUserParty.CurrentUserParty = ValueManager.GetValueManager<crmUserParty>("UserParty");
            XPQuery<crmUserParty> userParties = new XPQuery<crmUserParty>(ssn);
            var queryUP = (from userParty in userParties
                            select userParty).ToList<crmUserParty>();
            foreach (var up in queryUP) {
                crmUserParty.CurrentUserParty.Value = (crmUserParty)up;
                break;
            }
                
            // Новая автоматическая привязка
            DateTime startDate = new DateTime(2012, 4, 1);
            XPQuery<fmCSAImportResult> importResults = new XPQuery<fmCSAImportResult>(ssn);
            var queryIR = (from importResult in importResults
                            select importResult).ToList<fmCSAImportResult>();
            foreach (var ir in queryIR) {
                //ir.AutoBinding(null);

                foreach (fmCSAStatementAccount sa in ir.StatementOfAccounts) {
                    //ssa.AutoBinding(null);
                    foreach (fmCSAStatementAccountDoc sad in sa.PayInDocs) {
                        if (sad.DocDate >= startDate) {
                            fmCDocRCB paymentDoc = sad.PaymentDocument;
                            paymentDoc.AutoBinding(sa.BankAccount, null);
                        }
                    }
                    foreach (fmCSAStatementAccountDoc sad in sa.PayOutDocs) {
                        if (sad.DocDate >= startDate) {
                            fmCDocRCB paymentDoc = sad.PaymentDocument;
                            paymentDoc.AutoBinding(sa.BankAccount, null);
                        }
                    }
                }
                
                os.CommitChanges();
            }



            // Простановка признаков заявкам и документам, что они были привязаны, чтобы следующее выполнение этого updater
            // не изменяло ничего.
            ABU.AutoBindingUpdater = true;

            os.CommitChanges();
        }
    }
}
