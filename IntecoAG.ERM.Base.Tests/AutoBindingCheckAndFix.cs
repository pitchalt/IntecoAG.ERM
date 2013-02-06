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
    /// Сопоставление с тем, что есть в системе после автопривязки в соотношении с тем, что идеально
    /// </summary>
    [TestFixture, Description("Сопоставление с тем, что есть в системе после автопривязки")]
    public class AutoBindingCheckAndFix : AutoBindingBaseTest
    {
        public override void Init() {
            base.Init();

            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }

        struct ConditionPair {
            public fmCPRPaymentRequest paymentRequest;
            public fmCDocRCB paymentDocument;
        };

        #region Исследование автопривязки

        [Test, Description("Сопоставление с тем, что есть в системе после автопривязки")]
        [Category("Debug")]
        public void AutoBindingResearch() {

            DateTime startDate = new DateTime(2012, 4, 1);
            DateTime endDate = new DateTime(2012, 5, 20);
            Debug.WriteLine(String.Format("{0} {1} - {2}", "Интервал дат: ", startDate, endDate));
            Debug.WriteLine("\n");

            using (UnitOfWork uow = new UnitOfWork(Common.dataLayer)) {

                // Анализ Платёжных документов. Общая таблица с аналитическими признаками в границах указаннных дат.
                Dictionary<String, csValuta> dictValuta = new Dictionary<string, csValuta>();
                XPQuery<csValuta> valutas = new XPQuery<csValuta>(uow);
                var queryValuta = (from valuta in valutas
                                    //where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                                    select valuta).Distinct<csValuta>();
                foreach (var valuta in queryValuta) {
                    dictValuta.Add(valuta.CodeCurrencyValue, valuta);
                }


                // Список счетов "нашей" организации
                //List<crmBankAccount> = new 
                XPQuery<crmBankAccount> ourBankAccounts = new XPQuery<crmBankAccount>(uow);
                var OurBankAccounts = (from bankAccount in ourBankAccounts
                                      where bankAccount.PrefferedParty.Code == "2518"
                                       select bankAccount).ToList<crmBankAccount>();
                Debug.WriteLine(String.Format("{0} {1}", "Всего счетов нашей организации", OurBankAccounts.Count()));
                Debug.WriteLine("\n");

                // --------------------- ПЛАТЁЖНЫЕ ДОКУМЕНТЫ -----------------------//
                Debug.WriteLine("\n");
                Debug.WriteLine("ПЛАТЁЖНЫЕ ДОКУМЕНТЫ");
                Debug.WriteLine("\n");


                String prmDocType = ""; //"Платежное поручение";
                XPQuery<fmCDocRCB> paymentDocuments = new XPQuery<fmCDocRCB>(uow);
                var queryPaymentDocuments = (from paymentDocument in paymentDocuments
                                            where paymentDocument.DocDate.Date >= startDate
                                               && paymentDocument.DocDate.Date < endDate
                                               && (prmDocType == "" || paymentDocument.DocType == prmDocType)
                                            select new {
                                                PaymentDocument = paymentDocument,
                                                DocType = paymentDocument.DocType,
                                                DocDate = paymentDocument.DocDate,
                                                IsDocDate = (paymentDocument.DocDate != DateTime.MinValue),

                                                IsSum = (paymentDocument.PaymentCost != 0m),
                                                SumIn = ((paymentDocument.PaymentReceiverRequisites.BankAccount.PrefferedParty.Code == "2518") ? paymentDocument.PaymentCost : 0m),
                                                SumOut = ((paymentDocument.PaymentPayerRequisites.BankAccount.PrefferedParty.Code == "2518") ? paymentDocument.PaymentCost : 0m),

                                                //PaymentValuta = crmBankAccount.GetValutaByBankAccount(uow, paymentDocument.PaymentPayerRequisites.BankAccount),
                                                PaymentValuta = paymentDocument.PaymentPayerRequisites.BankAccount.Number.Substring(5, 3),
                                                PaymentReceiverValuta = paymentDocument.PaymentReceiverRequisites.BankAccount.Number.Substring(5, 3),
                                                
                                                ReceiverAccount = (paymentDocument.PaymentReceiverRequisites.AccountParty != ""),
                                                PayerAccount = (paymentDocument.PaymentPayerRequisites.AccountParty != ""),
                                                ReceiverBankAccount = (paymentDocument.PaymentReceiverRequisites.BankAccount != null),
                                                PayerBankAccount = (paymentDocument.PaymentPayerRequisites.BankAccount != null),
                                                
                                                ReceiverBankRCBIC = (paymentDocument.PaymentReceiverRequisites.RCBIC != ""),
                                                ReceiverBank = (paymentDocument.PaymentReceiverRequisites.Bank != null),
                                                PayerBankRCBIC = (paymentDocument.PaymentPayerRequisites.RCBIC != ""),
                                                PayerBank = (paymentDocument.PaymentPayerRequisites.Bank != null),

                                                ReceiverINN = (paymentDocument.PaymentReceiverRequisites.INN != ""),
                                                PayerINN = (paymentDocument.PaymentPayerRequisites.INN != ""),

                                                ReceiverKPP = (paymentDocument.PaymentReceiverRequisites.KPP != ""),
                                                PayerKPP = (paymentDocument.PaymentPayerRequisites.KPP != ""),

                                                DateIn = (paymentDocument.ReceivedByPayerBankDate != DateTime.MinValue),
                                                DateOut = (paymentDocument.DeductedFromPayerAccount != DateTime.MinValue),
                                                DateAccountChanged = (paymentDocument.ReceivedByPayerBankDate != DateTime.MinValue) ? paymentDocument.ReceivedByPayerBankDate : ((paymentDocument.DeductedFromPayerAccount != DateTime.MinValue) ? paymentDocument.DeductedFromPayerAccount : DateTime.MinValue)
                                            }).ToList();

                // Статистика по queryPaymentDocuments
                Int32 TotalPaymentCount = queryPaymentDocuments.Count();
                Debug.WriteLine(String.Format("{0} {1}", "Всего платёжных документов в системе", TotalPaymentCount));
                Debug.WriteLine("\n");

                // Нашша сторона - плательщик (РАСХОДНЫЕ)
                List<fmCDocRCB> totalConditionPaymentDocument = new List<fmCDocRCB>();
                var queryOurDocPayer = (from doc in queryPaymentDocuments
                                       where OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                     select doc).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Наша сторона - плательщик (РАСХОДНЫЕ)", queryOurDocPayer.Count()));
                foreach (var doc in queryOurDocPayer) {
                    // БЕЗ ВЫДАЧИ СПИСКА Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                    if (!totalConditionPaymentDocument.Contains(doc.PaymentDocument)) {
                        totalConditionPaymentDocument.Add(doc.PaymentDocument);
                    }
                }
                Debug.WriteLine("\n");


                // --------------------- ЗАЯВКИ -----------------------//
                Debug.WriteLine("\n");
                Debug.WriteLine("ЗАЯВКИ");
                Debug.WriteLine("\n");


                // Анализ Платёжных документов. Общая таблица с аналитическими признаками в границах указаннных дат.
                //String prmReqType = ""; //"Платежное поручение";
                XPQuery<fmCPRPaymentRequest> paymentRequests = new XPQuery<fmCPRPaymentRequest>(uow);
                var queryPaymenRequests = (from paymentRequest in paymentRequests
                                             select new {
                                                 PaymentRequest = paymentRequest,
                                                 SumIn = 0m,
                                                 SumOut = paymentRequest.Summ,   // По факту все заявки - расходные
                                                 ObligationValuta = paymentRequest.Valuta
                                             }).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Всего заявок в системе", queryPaymenRequests.Count()));
                Debug.WriteLine("\n");


                // Заявки, прошедшие утверждение фин. отдела и сформированные правильно
                List<fmCPRPaymentRequest> totalConditionPaymentRequest = new List<fmCPRPaymentRequest>();
                var queryInPaymentRequestRightly = (from doc in queryPaymenRequests
                                          where doc.PaymentRequest.Date != null
                                              && doc.ObligationValuta != null
                                              && doc.PaymentRequest.Summ != 0m
                                              && doc.PaymentRequest.PartyPaySender != null
                                              && doc.PaymentRequest.PartyPayReceiver != null
                                              && (doc.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || doc.PaymentRequest.State == PaymentRequestStates.IN_BANK || doc.PaymentRequest.State == PaymentRequestStates.PAYED)
                                          select doc).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, прошедшие утверждение фин. отдела и сформированные правильно", queryInPaymentRequestRightly.Count()));
                foreach (var doc in queryInPaymentRequestRightly) {
                    // БЕЗ ВЫДАЧИ СПИСКА Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                    if (!totalConditionPaymentRequest.Contains(doc.PaymentRequest)) {
                        totalConditionPaymentRequest.Add(doc.PaymentRequest);
                    }
                }
                Debug.WriteLine("\n");




                // --------------------- ЗАЯВКИ и ПЛАТЁЖНЫЕ ДОКУМЕНТЫ -----------------------//
                Debug.WriteLine("\n");
                Debug.WriteLine("ЗАЯВКИ и ПЛАТЁЖНЫЕ ДОКУМЕНТЫ");
                Debug.WriteLine("\n");


                // Соединение по максимуму условий (абсолютно точное совпадение)
                List<ConditionPair> hardConditionPair = new List<ConditionPair>();
                List<fmCPRPaymentRequest> hardConditionPaymentRequest = new List<fmCPRPaymentRequest>();
                List<fmCDocRCB> hardConditionPaymentDocument = new List<fmCDocRCB>();
                // Полный набор максимальных условий:
                // Стороны попарно совпадают, суммы в пересчёте к сумме платежа совпадают (до 1 коп.), 
                // дата оплаты в платёжном документе отстоит от даты "к оплате" в заявке не более, чем на 2 дня
                var queryMaxConditions = from paymentRequest in queryInPaymentRequestRightly   //queryPaymenRequests
                                         join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                                           on new {
                                               P1 = paymentRequest.PaymentRequest.PartyPaySender,
                                               P2 = paymentRequest.PaymentRequest.PartyPayReceiver
                                           } equals new {
                                               P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.Party,
                                               P2 = paymentDocument.PaymentDocument.PaymentReceiverRequisites.Party
                                           }
                                            into RD
                                            from doc in RD
                                            where OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                               && doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.Date.Date //&& doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.Date.AddDays(3).Date
                                               && (paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_BANK || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                         select new {
                                             paymentRequest,
                                             paymentDocument = doc
                                         };
                Int32 queryMaxConditionsCount = 0;
                Int32 queryMaxConditionsValutaErrorCount = 0;
                foreach (var doc in queryMaxConditions) {
                    csValuta pValuta = (from valuta in valutas
                                        where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                                        select valuta).FirstOrDefault<csValuta>();
                    if (pValuta == null) {
                        queryMaxConditionsValutaErrorCount++;
                        continue;
                    }
                    Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.paymentDocument.DateAccountChanged, doc.paymentRequest.ObligationValuta, pValuta);
                    Decimal paymentSumOutPaymentValuta = doc.paymentRequest.SumOut * crossCource;
                    Boolean bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.paymentDocument.PaymentDocument.PaymentCost), 0.01m) <= 0);
                    if (bRes) {
                        // БЕЗ ВЫДАЧИ СПИСКА Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                        queryMaxConditionsCount++;
                        ConditionPair pair = new ConditionPair() { paymentDocument = doc.paymentDocument.PaymentDocument, paymentRequest = doc.paymentRequest.PaymentRequest };
                        //hardConditionPair.Add(pair);
                        if (!hardConditionPair.Contains(pair)) {
                            hardConditionPair.Add(pair);
                        }
                        if (!hardConditionPaymentRequest.Contains(doc.paymentRequest.PaymentRequest)) {
                            hardConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        }
                        if (!hardConditionPaymentDocument.Contains(doc.paymentDocument.PaymentDocument)) {
                            hardConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        }
                    }
                }
                Debug.WriteLine("\n");
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по максимуму условий (абсолютно точное совпадение), случай расходного документа", queryMaxConditionsCount));
                Debug.WriteLine(String.Format("{0} {1}", "Уникальных пар потенциальных привязок, количество ", hardConditionPair.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "Неопознана валюта в платёжном документе, количество ", queryMaxConditionsValutaErrorCount));
                Debug.WriteLine(String.Format("{0} {1}", "hard Заявок в слое, количество ", hardConditionPaymentRequest.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "hard Документов в слое, количество ", hardConditionPaymentDocument.Count()));
                Debug.WriteLine("\n");



                // -------------- Сколько имеется привяок в связи с отработкой имеющегося алгоритма автопривязки --------------------- //
                Int32 queryyRepaymentJurnalCount = 0;
                List<ConditionPair> ExistsConditionPairs = new List<ConditionPair>();
                List<fmCPRPaymentRequest> UsedPaymentRequestList = new List<fmCPRPaymentRequest>();
                List<fmCDocRCB> UsedPaymentDocumentList = new List<fmCDocRCB>();
                XPQuery<fmCPRRepaymentJurnal> repaymentJurnals = new XPQuery<fmCPRRepaymentJurnal>(uow);
                var queryRepaymentJurnals = (from record in repaymentJurnals
                                             select record).Distinct().ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Сколько имеется привязок в связи с отработкой имеющегося алгоритма автопривязки", queryRepaymentJurnals.Count()));
                //Debug.WriteLine("\n");
                foreach (var record in queryRepaymentJurnals) {
                    //Debug.Write(String.Format("({0}, {1}); ", UsedPaymentDocumentList.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                    queryyRepaymentJurnalCount++;
                    ConditionPair pair = new ConditionPair();
                    pair.paymentRequest = record.PaymentRequest;
                    pair.paymentDocument = record.PaymentDocument;
                    if (!ExistsConditionPairs.Contains(pair)) {
                        ExistsConditionPairs.Add(pair);
                    }
                    if (!UsedPaymentRequestList.Contains(record.PaymentRequest)) {
                        UsedPaymentRequestList.Add(record.PaymentRequest);
                    }
                    if (!UsedPaymentDocumentList.Contains(record.PaymentDocument)) {
                        UsedPaymentDocumentList.Add(record.PaymentDocument);
                    }
                }
                //Debug.WriteLine(String.Format("{0} {1}", "Всего  в журнале привязок, количество ", queryyRepaymentJurnalCount));
                Debug.WriteLine(String.Format("{0} {1}", "Уникальных пар в журнале привязок, количество ", ExistsConditionPairs.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "Заявок в журнале привязок, количество ", UsedPaymentRequestList.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "Документов в журнале привязок, количество ", UsedPaymentDocumentList.Count()));
                Debug.WriteLine("\n");


                // ОТВЕТЫ НА ВОПРОСЫ: КАКАЯ РАЗНИЦА МЕЖДУ СПИСКАМИ?
                Debug.WriteLine(String.Format("{0}", "ЗАЯВКИ, КОТОРЫЕ ЕСТЬ В ПОТЕНЦИАЛЬНОМ СПИСКЕ, НО ОТСУТСТВУЮТ В РЕАЛЬНОМ"));
                var requestInPotentialNoInReal = from req in hardConditionPaymentRequest
                                                 where !UsedPaymentRequestList.Contains(req)
                                                 orderby req.ExtDocNumber ascending
                                                 select req;
                foreach (var req in requestInPotentialNoInReal) {
                    Debug.WriteLine(String.Format("{0} \t\t {1} \t\t {2}", req.ExtDocNumber, req.Number, req.Date));
                }
                Debug.WriteLine("\n");

                Debug.WriteLine(String.Format("{0}", "ЗАЯВКИ, КОТОРЫЕ ЕСТЬ В РЕАЛЬНОМ СПИСКЕ, НО ОТСУТСТВУЮТ В ПОТЕНЦИАЛЬНОМ"));
                var requestInRealNoInPotential = from req in UsedPaymentRequestList
                                                 where !hardConditionPaymentRequest.Contains(req)
                                                 orderby req.ExtDocNumber ascending
                                                 select req;
                foreach (var req in requestInRealNoInPotential) {
                    Debug.WriteLine(String.Format("{0} \t\t {1} \t\t {2}", req.ExtDocNumber, req.Number, req.Date));
                }
                Debug.WriteLine("\n");

                Debug.WriteLine(String.Format("{0}", "ДОКУМЕНТЫ, КОТОРЫЕ ЕСТЬ В ПОТЕНЦИАЛЬНОМ СПИСКЕ, НО ОТСУТСТВУЮТ В РЕАЛЬНОМ"));
                var docInPotentialNoInReal = from doc in hardConditionPaymentDocument
                                             where !UsedPaymentDocumentList.Contains(doc)
                                             orderby doc.DocNumber ascending
                                             select doc;
                foreach (var doc in docInPotentialNoInReal) {
                    Debug.WriteLine(String.Format("{0} \t\t {1}", doc.DocNumber, doc.DocDate));
                }
                Debug.WriteLine("\n");

                Debug.WriteLine(String.Format("{0}", "ДОКУМЕНТЫ, КОТОРЫЕ ЕСТЬ В РЕАЛЬНОМ СПИСКЕ, НО ОТСУТСТВУЮТ В ПОТЕНЦИАЛЬНОМ"));
                var docInRealNoInPotential = from doc in UsedPaymentDocumentList
                                             where !hardConditionPaymentDocument.Contains(doc)
                                             orderby doc.DocNumber ascending
                                             select doc;
                foreach (var doc in docInRealNoInPotential) {
                    Debug.WriteLine(String.Format("{0} \t\t {1}", doc.DocNumber, doc.DocDate));
                }
                Debug.WriteLine("\n");

            }
        }

        #endregion

    }
}
