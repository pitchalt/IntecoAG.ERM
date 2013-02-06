using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CRM;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.StatementAccount;

using NUnit.Framework;

namespace IntecoAG.ERM.FM
{

    /// <summary>
    ///  Исследование автопривязки
    /// </summary>
    [TestFixture, Description("Исследование автопривязки")]
    public class AutoBindingStatCollection : AutoBindingBaseTest
    {
        public override void Init() {
            base.Init();

            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }

        #region Исследование автопривязки

        [Test, Description("Исследование автопривязки")]
        [Category("Debug")]
        public void AutoBindingResearch() {
            // Задача исследования. 
            // 1. Подсчёт количества загруженных документов в систему за определённый период (за 18-19 апреля, например).
            // 2. Подсчёт количества заявок в системе со статусом IN_PAY, да и вообще сколько заявок с какими статусами.
            // 3. Подсчёт актального количества связей платёжных документов и заявок.
            // 4. Подсчёт количества потенциально возможных связей платёжных документов и заявок.
            // 5. Если обнаружились непривязанные, но могшие быть привязанными автоматически заявками, выяснить, почему они не привязались.

            //Decimal _Accuracy = 0.01m;  // Сопоставление с точностью до копейки

            DateTime startDate = new DateTime(2012, 4, 1);
            DateTime endDate = new DateTime(2012, 4, 20);

            using (UnitOfWork uow = new UnitOfWork(Common.dataLayer)) {

                // Проверка уже привязавшихся заявок и документов
                XPQuery<fmCPRRepaymentJurnal> xpqBinding = new XPQuery<fmCPRRepaymentJurnal>(uow);
                // 3. Подсчёт актуального количества связей платёжных документов и заявок.
                Int32 BindingCount = (from record in xpqBinding
                                      //where record.State == PaymentRequestStates.IN_PAYMENT
                                      select new {
                                          record.PaymentDocument,
                                          record.PaymentRequest
                                      }).Distinct().Count();

                //var DocReqLink = from record in xpqBinding
                //                      select new {
                //                          record.PaymentDocument,
                //                          record.PaymentRequest
                //                      };
                //foreach (var record in DocReqLink) {
                //    //Assert.AreNotEqual(record.PaymentDocument.PaymentPayerRequisites.BankAccount, record.PaymentDocument.PaymentReceiverRequisites.BankAccount);
                //    if (record.PaymentRequest.Valuta != null || record.PaymentRequest.PaymentValuta != null) {
                //        Assert.AreEqual(record.PaymentRequest.Valuta, record.PaymentRequest.PaymentValuta);
                //        if (record.PaymentRequest.Valuta != null) {
                //            Assert.AreEqual(record.PaymentRequest.Valuta.Code, "RUB");
                //        }
                //        if (record.PaymentRequest.PaymentValuta != null) {
                //            Assert.AreEqual(record.PaymentRequest.PaymentValuta.Code, "RUB");
                //        }
                //    }
                //}





                // 1. Подсчёт количества загруженных документов в систему за определённый период (за 18-19 апреля, например).
                XPQuery<fmCDocRCB> paymentDocuments = new XPQuery<fmCDocRCB>(uow);
                var queryPaymentDocuments = from paymentDocument in paymentDocuments
                                            where paymentDocument.DocDate.Date >= startDate
                                               && paymentDocument.DocDate.Date < endDate
                                               //&& paymentDocument.
                                            select paymentDocument;
                Int32 countPaymentDocumentcount = queryPaymentDocuments.Count();

                // 2. Подсчёт количества заявок в системе со статусом IN_PAY, да и вообще сколько заявок с какими статусами.
                XPQuery<fmCPRPaymentRequest> paymentRequests = new XPQuery<fmCPRPaymentRequest>(uow);
                var queryPaymentRequestByStatus = from paymentRequest in paymentRequests
                                                  group paymentRequest by new {
                                                      paymentRequest.State,
                                                      paymentRequest.Valuta
                                                  }
                                                into gPR
                                                select new {
                                                    State = gPR.Key.State,
                                                    Valuta = gPR.Key.Valuta,
                                                    Count = gPR.Count()
                                                };

                // 4. Проверка состоит в том, чтобы поискать для каждого оставшегося непривязанным документа заявку по установленным правилам (статус и плательщик с получателем)
                // Поиск подходящей заявки (Статус, Плательщик, Получатель, остаточные суммы)
                crmCParty OurParty = GetOurPartyByCode(uow, "2518");
                Int32 FreeRequestCount = 0;
                Int32 CountAll =0, CountNoBind =0 , CountPotenAllBind =0 , CountPotenNoBind =0 ;
                foreach (var paymentDocument in queryPaymentDocuments) {
                    Int32 PaymentDocumentBusyCount = (from record in xpqBinding
                                                  where record.PaymentDocument == paymentDocument
                                                  select record).Count();

                    CountAll++;
                    if (PaymentDocumentBusyCount == 0)
                        CountNoBind++;
                    else
                        CountNoBind = CountNoBind;

                    //if (PaymentDocumentBusyCount == 0) { // т.е. документ не имеет никаких привязок (его остатоная сумма равна его начальной сумме)

                        //crmBankAccount bankAccount = null;
                        //if (paymentDocument.PaymentPayerRequisites.Party == OurParty) {
                        //    bankAccount = paymentDocument.PaymentPayerRequisites.BankAccount;
                        //} else {
                        //    bankAccount = paymentDocument.PaymentReceiverRequisites.BankAccount;
                        //}

                        //// Валюта платежа
                        //csValuta paymentValuta = crmBankAccount.GetValutaByBankAccount(uow, bankAccount);

                        // Поиск подходящей заявки
                        //XPQuery<fmCPRPaymentRequest> searchedPymentRequests = new XPQuery<fmCPRPaymentRequest>(uow);
                        var querySearchedPaymentRequests = (from pr in paymentRequests
                                                           where //pr.State == PaymentRequestStates.IN_PAYMENT
                                                                //&& 
                                                                pr.PartyPayReceiver.INN == paymentDocument.PaymentReceiverRequisites.INN
                                                                && pr.PartyPaySender.INN == paymentDocument.PaymentPayerRequisites.INN
                                                           select pr).ToList<fmCPRPaymentRequest>();
                        Debug.WriteLine(String.Format("{0} {1} {2} {3}", CountAll, querySearchedPaymentRequests.Count, paymentDocument.DocNumber, paymentDocument.DocDate));
                        foreach (var elem in querySearchedPaymentRequests) {
                            // Отбраковка: 
                            // (1) сумма заявки не должна быть исчерпана полностью, 
                            // (2) остаточная сумма должна равняться величине непокрытия Платёжного документа

                            Decimal operationPaymentDocSumIn = 0, operationPaymentDocSumOut = 0;
                            GetPaymentDocSumByOperationJournal(uow, paymentDocument, out operationPaymentDocSumIn, out operationPaymentDocSumOut);

                            if (elem.Summ == operationPaymentDocSumIn || elem.Summ == operationPaymentDocSumOut) {
                                FreeRequestCount++;
                                Debug.WriteLine(String.Format("Ok {0} Date {1}", paymentDocument.DocNumber, paymentDocument.DocDate));
                                CountPotenAllBind++;
                                if (PaymentDocumentBusyCount == 0)
                                    CountPotenNoBind++;
                            }

                            /*
                            Decimal paymentRequestSumIn = 0, paymentRequestSumOut = 0;   // Эти суммы в валюте платежа
                            Decimal paymentRequestSumObligationIn = 0, paymentRequestSumObligationOut = 0;   // Эти суммы в валюте обязательств
                            GetPaymentRequestSumByRepaymentJournal(
                                    uow,
                                    elem,
                                    out paymentRequestSumIn,
                                    out paymentRequestSumOut,
                                    out paymentRequestSumObligationIn,
                                    out paymentRequestSumObligationOut
                                );

                            //Decimal deltaRequestSum = GetRequestSumByCourse(uow, paymentDocument, elem, elem.Valuta);   // В валюте платежа
                            Decimal deltaRequestSum = GetRequestSumByCourse(uow, paymentDocument, elem, elem.Valuta);   // В валюте платежа
                            //if (paymentRequest.PartyPayReceiver.INN == OurParty.INN) {
                            if (elem.PartyPayReceiver == OurParty) {
                                deltaRequestSum -= paymentRequestSumIn;
                            } else {
                                deltaRequestSum -= paymentRequestSumOut;
                            }
                            if (Decimal.Compare(Math.Abs(deltaRequestSum), _Accuracy) <= 0)
                                continue;   // Переход к следующей заявке (тогда у заявки должен был бы быть статус PAYED и она не должна была попасть в рассмотрение - это предусловие контракта)

                            if (GetAnswer(uow, paymentDocument, elem)) {
                                FreeRequestCount += 1;
                            }
                            */
                        }
                    //}
                }

                //uow.CommitChanges();
            }
        }

        private Boolean GetAnswer(UnitOfWork uow, fmCDocRCB paymentDocument, fmCPRPaymentRequest paymentRequest) {
            Decimal _Accuracy = 0.01m;  // Сопоставление с точностью до копейки

            // Сумма Платёжного документа по OperationJournal (в валюте платежа)
            Decimal operationPaymentDocSumIn = 0, operationPaymentDocSumOut = 0;
            GetPaymentDocSumByOperationJournal(uow, paymentDocument, out operationPaymentDocSumIn, out operationPaymentDocSumOut);
            // Одна из сумм operationPaymentDocSumIn или operationPaymentDocSumOut обязательно равна 0

            // Сумма Платёжного документа по RepaymentJournal (в валюте платежа - это величины SumIn и SumOut)
            Decimal repaymentDocSumIn = 0, repaymentDocSumOut = 0;
            GetPaymentDocSumByRepaymentJournal(uow, paymentDocument, out repaymentDocSumIn, out repaymentDocSumOut);
            // Одна из сумм repaymentDocSumIn или repaymentDocSumOut также обязательно равна 0

            // Величина непокрытия Платёжного документа Заявками (все суммы в валюте платежа)
            Decimal deltaDocSumIn = operationPaymentDocSumIn - repaymentDocSumIn;
            Decimal deltaDocSumOut = operationPaymentDocSumOut - repaymentDocSumOut;

            if (Decimal.Compare(Math.Abs(deltaDocSumIn) + Math.Abs(deltaDocSumOut), _Accuracy) <= 0)
                return false;
            // Всё сопоставлено уже с точностью до _Accuracy - так условились!
            return true;
        }

        private Decimal GetRequestSumByCourse(UnitOfWork uow, fmCDocRCB PaymentDocument, fmCPRPaymentRequest paymentRequest, csValuta valutaPayment) {
            // Поясение о вычислении. Сумма в заявке - это в в валюте обязательств, чтобы её сравнить
            // с суммой платежа, надо перевести по кросс-курсу к валюте платежа и уже полученную сумму сравнить. Вопрос: на
            // какой день брать курс? Предлагается брать непустую из двух дат: this.DeductedFromPayerAccount или this.ReceivedByPayerBankDate

            // Тривиальный случай: валюта платежа совпадает с валютой обязательство
            if (paymentRequest.Valuta == valutaPayment) {
                return paymentRequest.Summ;   //.PaymentSumm;
            }


            // Валюты платежа и обязательств не совпадают. Надо вычислять кросс-курс на дату courseDate (изменения счёта)
            DateTime courseDate = (PaymentDocument.DeductedFromPayerAccount != DateTime.MinValue) ? PaymentDocument.DeductedFromPayerAccount : PaymentDocument.ReceivedByPayerBankDate;  //paymentRequest.PayDate;
            Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, courseDate, paymentRequest.Valuta, valutaPayment);

            Decimal resultSum = Math.Round(paymentRequest.Summ * crossCource, 4);
            return resultSum;
        }

        private crmCParty GetOurPartyByCode(UnitOfWork uow, string BuhCode) {
            crmCParty partyRes = null;
            XPQuery<crmCParty> parties = new XPQuery<crmCParty>(uow);
            var queryParties = from party in parties
                               where party.Code == BuhCode
                               select party;
            foreach (var party in queryParties) {
                partyRes = party;
                break; 
            }
            return partyRes;
        }

        private void GetPaymentDocSumByOperationJournal(UnitOfWork uow, fmCDocRCB PaymentDocument, out Decimal SumIn, out Decimal SunOut) {
            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(uow);
            var queryOperationJournals = from operationJournal in operationJournals
                                         where operationJournal.PaymentDocument == PaymentDocument
                                         select operationJournal;

            // Как я понял, валюта выписки определяется по 3-м знакам счёта выписки, поэтому валюта везде одинаковая и суммирование ниже корректно.
            // В Валюте платежа
            SumIn = queryOperationJournals.Sum(x => x.SumIn);
            SunOut = queryOperationJournals.Sum(x => x.SumOut);
        }

        private void GetPaymentDocSumByRepaymentJournal(UnitOfWork uow, fmCDocRCB PaymentDocument, out Decimal SumIn, out Decimal SunOut) {
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(uow);
            var queryRepaymentJournals = from repaymentJournal in repaymentJournals
                                         where repaymentJournal.PaymentDocument == PaymentDocument
                                         select repaymentJournal;

            // В журнале привязок хранятся величины сумм для валют - платежа и обязательства. Суммы берём в валюте платежа.
            SumIn = queryRepaymentJournals.Sum(x => x.SumIn);
            SunOut = queryRepaymentJournals.Sum(x => x.SumOut);
        }

        private void GetPaymentRequestSumByRepaymentJournal(UnitOfWork uow, fmCPRPaymentRequest PaymentRequest, out Decimal SumIn, out Decimal SunOut, out Decimal SumObligationIn, out Decimal SunObligationOut) {
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(uow);
            var queryRepaymentJournals = from repaymentJournal in repaymentJournals
                                         where repaymentJournal.PaymentRequest == PaymentRequest
                                         select repaymentJournal;

            SumIn = queryRepaymentJournals.Sum(x => x.SumIn);
            SunOut = queryRepaymentJournals.Sum(x => x.SumOut);

            SumObligationIn = queryRepaymentJournals.Sum(x => x.SumObligationIn);
            SunObligationOut = queryRepaymentJournals.Sum(x => x.SumObligationOut);
        }

        #endregion

    }
}
