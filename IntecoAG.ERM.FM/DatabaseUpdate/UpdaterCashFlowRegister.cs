using System;
using System.Security.Principal;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;

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
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract.Analitic;

namespace IntecoAG.ERM.FM {

    /// <summary>
    /// Updater исправляет следующую неправильность. 
    /// 1. Факт автоматической привязки отображается с помощью галочки в платёжном документе.
    /// 2. Наличие этой галочки означает, что сумма по операционному журналу совпадает с суммой по журналу привязок.
    /// 3. Журнал првяок пока остался.
    /// 4. В то же время суммы в задаче привязки вычисляются по CashFlow, а для старых привязок их там нет.
    /// Данный updater призван исправить этот недочёт.
    /// </summary>
    public class UpdaterCashFlowRegister : ModuleUpdater {
        public UpdaterCashFlowRegister(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            // Disable version
            if (this.CurrentDBVersion != new Version("0.0.0.0"))
                return;
            //
            IObjectSpace os = ObjectSpace;
            Session ssn = ((ObjectSpace)os).Session;

            // "Наша" организация (она там одна)
            crmUserParty.CurrentUserParty = ValueManager.GetValueManager<crmUserParty>("UserParty");
            XPQuery<crmUserParty> userParties = new XPQuery<crmUserParty>(ssn);
            var queryUP = (from userParty in userParties
                           select userParty).ToList<crmUserParty>();
            foreach (var up in queryUP) {
                crmUserParty.CurrentUserParty.Value = (crmUserParty)up;
                break;
            }

            // Исправление CashFlowRgister
            CashFlowFormed(ssn);

            os.CommitChanges();

            // Исправление статусов заявок
            RepairRequestRestSum(ssn);

            os.CommitChanges();
        }

        private void CashFlowFormed(Session ssn) {
            XPQuery<crmCashFlowRegister> cashFlowDocs = new XPQuery<crmCashFlowRegister>(ssn);
            var queryCF = from cashFlowDoc in cashFlowDocs
                          select (csCDocRCB)(cashFlowDoc.PaymentDocument);

            XPQuery<csCDocRCB> paymentDocs = new XPQuery<csCDocRCB>(ssn);
            var queryPD = from paymentDoc in paymentDocs
                          select paymentDoc;

            XPQuery<fmCPRRepaymentJurnal> repaymentJournalDocs = new XPQuery<fmCPRRepaymentJurnal>(ssn);
            var queryRJ = from repaymentJournalDoc in repaymentJournalDocs
                          select (csCDocRCB)(repaymentJournalDoc.PaymentDocument);

            var queryDOC = (queryPD.Except<csCDocRCB>(queryCF)).Intersect(queryRJ).Distinct();

            foreach (var paymentDoc in queryDOC) {
                fmCDocRCB fmPaymentDoc = paymentDoc as fmCDocRCB;
                if (fmPaymentDoc == null) continue;
                var repaymentJournalQuery = from repaymentJournal in repaymentJournalDocs
                                            where repaymentJournal.PaymentDocument == fmPaymentDoc
                                            select repaymentJournal;
                foreach (var repaymentJournalRecord in repaymentJournalQuery) {
                    // Разброс суммы в CashFlow
                    DistributeSum(ssn, repaymentJournalRecord);
                }
            }
        }

        /// <summary>
        /// Исправление статусов заявок
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        private void RepairRequestRestSum(Session ssn) {

            Decimal _Accuracy = 0.01m;

            XPQuery<fmCPRPaymentRequest> paymentRequests = new XPQuery<fmCPRPaymentRequest>(ssn);
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(ssn);

            var queryPaymentRequests = from request in paymentRequests
                                       where (request.State == PaymentRequestStates.IN_PAYMENT || request.State == PaymentRequestStates.IN_BANK)
                                       select request;
            foreach (var request in queryPaymentRequests) {
                var queryRepaymentJournals = from repaymentJournal in repaymentJournals
                                             where repaymentJournal.PaymentRequest == request
                                             select repaymentJournal;

                // По меньшей мере, одна из сумм sumObligationIn или sumObligationOut равна нулю
                Decimal sumObligationIn = queryRepaymentJournals.Sum(r => r.SumObligationIn);
                Decimal sumObligationOut = queryRepaymentJournals.Sum(r => r.SumObligationOut);

                Decimal sumInObligationValuta = request.Summ - (sumObligationIn + sumObligationOut);   // В валюте обязательств
                if (Decimal.Compare(Math.Abs(sumInObligationValuta), _Accuracy) <= 0) {
                    request.State = PaymentRequestStates.PAYED;
                }
            }
        }

        private void DistributeSum(Session ssn, fmCPRRepaymentJurnal repaymentJournalRecord) {
            // Задача привязки
            fmCPRRepaymentTask repaymentTask = new fmCPRRepaymentTask(ssn);
            fmCDocRCB paymentDoc = repaymentJournalRecord.PaymentDocument;
            repaymentTask.BankAccount = repaymentJournalRecord.BankAccount;
            repaymentTask.PaymentDocument = paymentDoc;
            //repaymentTask.FillRepaymentTaskLines();
            //repaymentTask.FillRequestList();

            // Сумма, которую надо распределить между обязательствами заявки
            Decimal distributeSum = 0;   // repaymentJournalRecord.SumOut;   //GetSumRepaymentJournal(ssn, paymentDoc);
            if (paymentDoc.PaymentReceiverRequisites.BankAccount == repaymentJournalRecord.BankAccount) {   // && this.PaymentDocument.PaymentReceiverRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                distributeSum = repaymentJournalRecord.SumIn;
            }
            if (paymentDoc.PaymentPayerRequisites.BankAccount == repaymentJournalRecord.BankAccount) {   // && this.PaymentDocument.PaymentPayerRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                distributeSum = repaymentJournalRecord.SumOut;
            }

            if (distributeSum == 0)
                return;

            List<ForCashFlowRecord> cashFlowRecordList = new List<ForCashFlowRecord>();

            // Применяем механизм пропорциональной разбивки
            Decimal intermediateDistributeSum = 0;
            if (distributeSum != 0) {

                Decimal totalOblSum = 0;
                foreach (fmCPRPaymentRequestObligation pro in repaymentJournalRecord.PaymentRequest.PaySettlmentOfObligations) {
                    totalOblSum += pro.Summ;
                }


                foreach (fmCPRPaymentRequestObligation pro in repaymentJournalRecord.PaymentRequest.PaySettlmentOfObligations) {
                    Decimal proSum = Math.Round((pro.Summ / totalOblSum) * distributeSum, 4);

                    // Пишем в промежуточный объект ForCashFlowRecord
                    ForCashFlowRecord cashFlowRecord = new ForCashFlowRecord();
                    cashFlowRecord.repaymentJournalRecord = repaymentJournalRecord;
                    cashFlowRecord.paymentDoc = paymentDoc;
                    cashFlowRecord.repaymentTask = repaymentTask;
                    cashFlowRecord.paymentRequest = repaymentJournalRecord.PaymentRequest;
                    cashFlowRecord.paymentRequestObligation = pro;
                    cashFlowRecord.sum = proSum;
                    cashFlowRecord.date = paymentDoc.GetAccountDateChange();
                    cashFlowRecord.paymentRequestObligationGUID = pro.Oid;

                    cashFlowRecordList.Add(cashFlowRecord);

                    intermediateDistributeSum += proSum;
                }
            }

            // Разница между distributeSum и суммой по подуровню в результате ошибки округления
            Decimal diff = intermediateDistributeSum - distributeSum;
            if (diff != 0) {
                // Внесение поправки: нахождение самого большого по модулю значения и изменение этого значения
                // (без помощи linq)
                Decimal valueMax = 0;
                for (int i = 0; i < cashFlowRecordList.Count(); i++) {
                    if (Math.Abs(cashFlowRecordList[i].sum) > valueMax) {
                        valueMax = Math.Abs(cashFlowRecordList[i].sum);
                    }
                }

                for (int i = 0; i < cashFlowRecordList.Count(); i++) {
                    if (Math.Abs(cashFlowRecordList[i].sum) == valueMax) {
                        cashFlowRecordList[i].sum -= diff;
                        break;
                    }
                }

            }

            CreateCFRegisterRecords(ssn, cashFlowRecordList);   //, GetOurParty(ssn));
        }

        public Decimal GetSumRepaymentJournal(Session ssn, fmCDocRCB paymentDoc) {
            Decimal repaymentJournalSum = 0;

            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(ssn);
            var repaymentJournalQuery = from repaymentJournal in repaymentJournals
                                        where repaymentJournal.PaymentDocument == paymentDoc
                                        select repaymentJournal;
            foreach (var repaymentJournal in repaymentJournalQuery) {
                repaymentJournalSum += repaymentJournal.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(ssn, repaymentJournal.PaymentDate.Date, repaymentJournal.ValutaPayment, GetValutaByCode(ssn, "RUB"));
            }

            return repaymentJournalSum;
        }

        private csValuta GetValutaByCode(Session ssn, string code) {
            XPQuery<csValuta> valutas = new XPQuery<csValuta>(ssn);
            csValuta Valuta = (from valuta in valutas
                               where valuta.Code.ToUpper() == code.ToUpper()
                               select valuta).FirstOrDefault();
            return Valuta;
        }

        public Decimal GetSumRepaymentJournalOnDate(Session ssn, DateTime operationDate, fmCDocRCB paymentDoc, fmCPRPaymentRequest paymentRequest) {
            Decimal repaymentJournalSum = 0;

            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(ssn);
            var repaymentJournalQuery = from repaymentJournal in repaymentJournals
                                        where repaymentJournal.PaymentDocument == paymentDoc
                                           && repaymentJournal.PaymentDate == operationDate
                                        select repaymentJournal;
            foreach (var repaymentJournal in repaymentJournalQuery) {
                repaymentJournalSum += repaymentJournal.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(ssn, repaymentJournal.PaymentDate.Date, repaymentJournal.ValutaPayment, GetValutaByCode(ssn, "RUB"));
            }

            return repaymentJournalSum;
        }


        /*
        //public Decimal GetSumCashFlowJournalOnDate(Session ssn, DateTime operationDate, fmCDocRCB paymentDoc) {
        //    // Вычисляем разность по оборотному регистру и регистру привязок с учётом валют платежа и договора
        //    Decimal cashFlowSum = 0;

        //    XPQuery<crmCashFlowRegister> cashFlows = new XPQuery<crmCashFlowRegister>(ssn);
        //    var cashFlowQuery = from cashFlow in cashFlows
        //                        where cashFlow.PaymentDocument == paymentDoc
        //                           && cashFlow.OperationDate == operationDate
        //                        select cashFlow;
        //    foreach (var cashFlow in cashFlowQuery) {
        //        cashFlowSum += cashFlow.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(ssn, operationDate, cashFlow.PaymentValuta, GetValutaByCode(ssn, "RUB"));
        //    }

        //    return cashFlowSum;
        //}
        */

        private void CreateCFRegisterRecords(Session ssn, List<ForCashFlowRecord> cashFlowRecordList) {   //, crmCParty ourParty) {
            for (int i = 0; i < cashFlowRecordList.Count(); i++) {
                CreateCFRegisterRecord(ssn, cashFlowRecordList[i]);   //, ourParty);
            }
        }

        private void CreateCFRegisterRecord(Session ssn, ForCashFlowRecord cashFlowRecord) {   //, crmCParty ourParty) {

            crmCashFlowRegister cfr = new crmCashFlowRegister(ssn);

            fmCPRRepaymentTask repaymentTask = cashFlowRecord.repaymentTask;
            fmCDocRCB paymentDoc = cashFlowRecord.paymentDoc;
            fmCPRPaymentRequest paymentRequest = cashFlowRecord.paymentRequest;

            Guid token = paymentRequest.Oid;
            cfr.Token = token;
            cfr.Section = CashFlowRegisterSection.REPAYMENT_JOURNAL;

            cfr.SourceGUID = repaymentTask.Oid;   // Будет пустой GUID
            cfr.SourceType = repaymentTask.GetType();

            cfr.PaymentDocument = paymentDoc;
            cfr.BankAccount = repaymentTask.BankAccount;
            cfr.Bank = repaymentTask.BankAccount.Bank;
            cfr.OperationDate = cashFlowRecord.date;

            fmCPRPaymentRequestContract paymentRequestContract = paymentRequest as fmCPRPaymentRequestContract;
            //if (paymentRequestContract != null && paymentRequestContract.ContractDeal != null) {
            //    cfr.Contract = paymentRequestContract.ContractDeal.Contract;
            //}
            if (paymentRequestContract != null) {
                cfr.Contract = paymentRequestContract.Contract;
                cfr.ContractDeal = paymentRequestContract.ContractDeal;
            }

            cfr.fmOrder = cashFlowRecord.paymentRequestObligation.Order;
            cfr.CostItem = cashFlowRecord.paymentRequestObligation.CostItem;
            cfr.Subject = (cashFlowRecord.paymentRequestObligation.Order != null) ? cashFlowRecord.paymentRequestObligation.Order.Subject : null;

            cfr.PrimaryParty = paymentRequest.PartyPaySender;
            cfr.ContragentParty = paymentRequest.PartyPayReceiver;

            //cfr.ObligationUnit = 
            //cfr.PaymentItem = 

            cfr.ValutaPayment = paymentDoc.GetAccountValuta();
            cfr.ValutaObligation = cashFlowRecord.paymentRequestObligation.Valuta;

            // В валюте платежа
            //cfr.SumIn = this.PaymentDocument.GetSumIn(this.BankAccount);
            //cfr.SumOut = this.PaymentDocument.GetSumOut(this.BankAccount);
            /*
            if (paymentDoc.PaymentReceiverRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentReceiverRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                cfr.SumIn = paymentDoc.PaymentCost;
            }
            if (paymentDoc.PaymentPayerRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentPayerRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                cfr.SumOut = paymentDoc.PaymentCost;
            }
            */
            if (paymentDoc.PaymentReceiverRequisites.BankAccount == cashFlowRecord.repaymentJournalRecord.BankAccount) {   // && this.PaymentDocument.PaymentReceiverRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                //cfr.SumIn = cashFlowRecord.repaymentJournalRecord.SumIn;
                cfr.SumIn = cashFlowRecord.sum;
            }
            if (paymentDoc.PaymentPayerRequisites.BankAccount == cashFlowRecord.repaymentJournalRecord.BankAccount) {   // && this.PaymentDocument.PaymentPayerRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                //cfr.SumOut = cashFlowRecord.repaymentJournalRecord.SumOut;
                cfr.SumOut = cashFlowRecord.sum;
            }



            // В валюте обязательств
            /*
            if (paymentDoc.PaymentReceiverRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentReceiverRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                cfr.SumObligationIn = cashFlowRecord.sum;
            }
            if (paymentDoc.PaymentPayerRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentPayerRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                cfr.SumObligationOut = cashFlowRecord.sum;
            }
            */
            if (paymentDoc.PaymentReceiverRequisites.BankAccount == cashFlowRecord.repaymentJournalRecord.BankAccount) {   // && this.PaymentDocument.PaymentReceiverRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                //cfr.SumObligationIn = cashFlowRecord.sum;
                cfr.SumObligationIn = Math.Round(cfr.SumIn * csCNMValutaCourse.GetCrossCourceOnDate(ssn, paymentDoc.GetAccountDateChange(), cfr.ValutaPayment, cfr.ValutaObligation), 4);
            }
            if (paymentDoc.PaymentPayerRequisites.BankAccount == cashFlowRecord.repaymentJournalRecord.BankAccount) {   // && this.PaymentDocument.PaymentPayerRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                //cfr.SumObligationOut = cashFlowRecord.sum;
                cfr.SumObligationOut = Math.Round(cfr.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(ssn, paymentDoc.GetAccountDateChange(), cfr.ValutaPayment, cfr.ValutaObligation), 4);
            }

            // В рублях
            try {
                cfr.SumInAcc = Math.Round(cfr.SumIn * csCNMValutaCourse.GetCrossCourceOnDate(ssn, paymentDoc.GetAccountDateChange(), cfr.ValutaPayment, GetValutaByCode(ssn, "RUB")), 4);
                cfr.SumOutAcc = Math.Round(cfr.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(ssn, paymentDoc.GetAccountDateChange(), cfr.ValutaPayment, GetValutaByCode(ssn, "RUB")), 4);
            }
            catch {
            }

            cfr.PaymentRequestObligationGUID = cashFlowRecord.paymentRequestObligationGUID;

            //if (this.PaymentReceiverRequisites.INN == OurParty.INN && this.PaymentReceiverRequisites.StatementOfAccount.BankAccount == bankAccount) {
            //    Res = this.PaymentCost;
            //}
        }

        
        private crmCParty GetOurParty(Session ssn) {
            // Наша организация
            crmCParty pOurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    pOurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(ssn).Party;
                }
            }
            return pOurParty;
        }

        private class ForCashFlowRecord
        {
            public fmCPRRepaymentJurnal repaymentJournalRecord;
            public fmCDocRCB paymentDoc;
            public fmCPRRepaymentTask repaymentTask;
            public fmCPRPaymentRequest paymentRequest;
            public fmCPRPaymentRequestObligation paymentRequestObligation;
            public Decimal sum;
            public DateTime date;
            public Guid paymentRequestObligationGUID;
        }
    }
}
